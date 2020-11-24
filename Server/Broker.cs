using Domen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
	public class Broker
	{
		SqlConnection konekcija;
		SqlCommand komanda;
		SqlTransaction transakcija;
		

		void KonektujSe()
		{
			konekcija = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Predok2019;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
			komanda = konekcija.CreateCommand();
		}
		public Broker()
		{
			KonektujSe();
		}

		public static Broker instanca;

		public static Broker DajSesiju()
		{
			if (instanca == null) instanca = new Broker();
			return instanca;
		}

		public List<Igra> VratiSveIgre()
		{
			List<Igra> listaIgara = new List<Igra>();
			try
			{
				konekcija.Open();
				komanda.CommandText = "SELECT * FROM Igra";
				SqlDataReader citac = komanda.ExecuteReader();

				while (citac.Read())
				{
					Igra i = new Igra();
					i.IgraId = citac.GetInt32(0);
					i.Naziv = citac.GetString(1);

					listaIgara.Add(i);
				}
				citac.Close();
				return listaIgara;
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				if (konekcija != null)
					konekcija.Close();
			}
		}

		internal void AzurirajIgru(Igra igra)
		{
			try
			{
				konekcija.Open();
				komanda.CommandText = $"UPDATE Igra SET DatumVremePocetka = '{DateTime.Now.ToString("")}' WHERE IgraId = {igra.IgraId}";
				komanda.ExecuteNonQuery();
			}
			catch (Exception)
			{

				throw;
			}
			finally
			{
				if (konekcija != null)
					konekcija.Close();
			}
		}

		internal void AzurirajKrajIgre(Igra igra)
		{
			try
			{
				konekcija.Open();
				komanda.CommandText = $"UPDATE Igra SET DatumVremeKraja = '{DateTime.Now.ToString("")}', Pobednik = '{igra.Pobednik}' WHERE IgraId = {igra.IgraId}";
				komanda.ExecuteNonQuery();
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				if (konekcija != null)
					konekcija.Close();
			}
		}


		public void VratiPitanja(Igra i)
		{
			i.ListaPitanja = new BindingList<Pitanje>();
			try
			{
				konekcija.Open();
				komanda.CommandText = $"SELECT * FROM Pitanje Where IgraId = {i.IgraId}";
				SqlDataReader citac = komanda.ExecuteReader();

				while (citac.Read())
				{
					Pitanje p = new Pitanje();
					p.IgraId = citac.GetInt32(0);
					p.PitanjeId = citac.GetInt32(1);
					p.PitanjeTekst = citac.GetString(2);
					p.TacanOdgovor = citac.GetString(3);
					p.BrojPoena = citac.GetInt32(4);

					i.ListaPitanja.Add(p);

				}
				citac.Close();
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				if (konekcija != null)
					konekcija.Close();
			}
		}
	}
}
