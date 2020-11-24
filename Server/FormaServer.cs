using Domen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
	public partial class FormaServer : Form
	{
		public FormaServer()
		{
			InitializeComponent();
		}

		Server s;
		Timer t;
		private void FormaServer_Load(object sender, EventArgs e)
		{
			comboBox1.DataSource = Broker.DajSesiju().VratiSveIgre();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Server.Igra = comboBox1.SelectedItem as Igra;
			Broker.DajSesiju().VratiPitanja(Server.Igra);

			try
			{
				Server.BrIgraca = Convert.ToInt32(txtBrIgraca.Text);
			}
			catch (Exception)
			{

				MessageBox.Show("Niste uneli igrace");
				return;
			}

			Broker.DajSesiju().AzurirajIgru(Server.Igra);

			s = new Server();
			if (s.PokreniServer())
			{
				t = new Timer();
				t.Interval = 1000;
				t.Tick += Osvezi;
				t.Start();
			}
		}

		bool posalji = true;
		private void Osvezi(object sender, EventArgs e)
		{
			List<Igrac> lista = new List<Igrac>();

			foreach (Igrac igrac in Server.listaIgraca)
			{
				lista.Add(igrac);
			}
			dataGridView1.DataSource = lista;

			TransferKlasa transfer = new TransferKlasa();
			BinaryFormatter formater = new BinaryFormatter();

			if (Server.listaIgraca.Count == Server.BrIgraca && posalji)
			{
				transfer.Pitanje = Server.Igra.ListaPitanja[0].PitanjeTekst;

				foreach (NetworkStream tok in Server.listaTokova)
				{
					formater.Serialize(tok, transfer);
				}
				posalji = false;
			}

			if(Server.brojZavrsenih == Server.BrIgraca)
			{
				// proglasenje
				Igrac pobednik = null;
				double maxPoena = 0;

				foreach (Igrac igrac in Server.listaIgraca)
				{
					if (igrac.Ukupno > maxPoena)
					{
						maxPoena = igrac.Ukupno;
						pobednik = igrac;
					}
				}

				Server.Igra.Pobednik = "";
				Server.Igra.Pobednik = pobednik.Username;
				Broker.DajSesiju().AzurirajKrajIgre(Server.Igra);

				transfer.Poruka = $"Igra je gotova, pobednik je {pobednik.Username}";
				transfer.KrajIgre = true;

				foreach (NetworkStream tok in Server.listaTokova)
				{
					formater.Serialize(tok, transfer);
				}
				t.Stop();

			}
		}
	}
}
