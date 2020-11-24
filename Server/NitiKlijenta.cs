using Domen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
	public class NitiKlijenta
	{
		NetworkStream tok;
		BinaryFormatter formater;
		int trenutnoPitanje = 0;
		private Igrac igrac;
		public NitiKlijenta(NetworkStream tok, Igrac igrac)
		{
			this.tok = tok;
			this.igrac = igrac;
			formater = new BinaryFormatter();

			ThreadStart ts = Obradi;
			new Thread(ts).Start();
		}

		void Obradi()
		{
			try
			{
				int operacija = 0;
				while (operacija != (int)Operacije.Kraj)
				{
					TransferKlasa transferKlasa = formater.Deserialize(tok) as TransferKlasa;

					switch (transferKlasa.Operacije)
					{
						case Operacije.Kraj:
							operacija = 1;
							break;
						case Operacije.Pogadjaj:
							double brPoena = Server.Igra.ListaPitanja[trenutnoPitanje].BrojPoena;
							if (transferKlasa.Odgovor == Server.Igra
								.ListaPitanja[trenutnoPitanje].TacanOdgovor)
							{
								
								if(Server.listaPogodjenih.Count == trenutnoPitanje)
								{
									igrac.Ukupno += brPoena;
									igrac.BrTacnih++;
									transferKlasa.Poruka = "Tacan odgovor!";
									Server.listaPogodjenih.Add(igrac.Username);
								}
								else
								{
									transferKlasa.Poruka = "Tacan odg ali niste prvi odg!";
								}
							}
							else
							{
								igrac.Ukupno -= brPoena*0.1;
								igrac.BrNetacnih++;
								transferKlasa.Poruka = "Netacan odg";
							}

							trenutnoPitanje++;

							if(trenutnoPitanje == Server.Igra.ListaPitanja.Count)
							{
								// nema vise pitanja
								transferKlasa.KrajIgre = true;
								transferKlasa.Poruka += "Sacekajte proglasenje pobednika!";
								Server.brojZavrsenih++;
							}
							else
							{
								transferKlasa.Pitanje = Server.Igra.ListaPitanja[trenutnoPitanje].PitanjeTekst;

							}
							transferKlasa.UkBrPoena = igrac.Ukupno;
							formater.Serialize(tok, transferKlasa);
							break;
						default:
							break;
					}
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}
