using Domen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Klijent
{
	public delegate void Delegat(TransferKlasa transfer);
	public partial class FormaKlijent : Form
	{
		Komunikacija k;
		public FormaKlijent()
		{
			InitializeComponent();
		}

		void PrikaziPoruku(TransferKlasa transfer)
		{
			txtxPorukaServ.Text = transfer.Poruka;
			textBox5.Text = transfer.UkBrPoena.ToString();

			if (transfer.KrajIgre)
			{
				button2.Enabled = false;
			}
			else
			{
				textBox2.Text = transfer.Pitanje;
				button2.Enabled = true;
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			k = new Komunikacija();

			TransferKlasa transfer = new TransferKlasa();
			transfer.Igrac = new Igrac();
			transfer.Igrac.Username = textBox1.Text;

			transfer = k.PoveziSeNaServer(transfer);
			if (transfer.Ulogovan == true)
			{
				this.Text = "Povezan";
				txtxPorukaServ.Text = transfer.Poruka;
				button1.Enabled = false;

				ThreadStart ts = PrimiPoruku;
				new Thread(ts).Start();
			}
			else
			{
				MessageBox.Show(transfer.Poruka);
			}

		}

		private void PrimiPoruku()
		{
			while (true)
			{
				TransferKlasa tk = k.PrimiPoruku();
				Delegat d = new Delegat(PrikaziPoruku);
				Invoke(d, tk);
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			TransferKlasa transfer = new TransferKlasa();
			transfer.Odgovor = textBox3.Text;
			transfer.Operacije = Operacije.Pogadjaj;

			k.PosaljiPoruku(transfer);
		}

		private void label2_Click(object sender, EventArgs e)
		{

		}

		private void label3_Click(object sender, EventArgs e)
		{

		}

		private void label4_Click(object sender, EventArgs e)
		{

		}

		private void label5_Click(object sender, EventArgs e)
		{

		}

		private void label6_Click(object sender, EventArgs e)
		{

		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{

		}

		private void textBox2_TextChanged(object sender, EventArgs e)
		{

		}

		private void textBox3_TextChanged(object sender, EventArgs e)
		{

		}

		private void txtxPorukaServ_TextChanged(object sender, EventArgs e)
		{

		}

		private void textBox5_TextChanged(object sender, EventArgs e)
		{

		}

		private void label1_Click(object sender, EventArgs e)
		{

		}

		private void FormaKlijent_Load(object sender, EventArgs e)
		{

		}
	}
}
