using Domen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
	public class Server
	{
		Socket soket;
		public static int BrIgraca = 0;
		public static Igra Igra;
		public static List<Igrac> listaIgraca = new List<Igrac>();
		public static List<NetworkStream> listaTokova = new List<NetworkStream>();
		public static List<string> listaPogodjenih = new List<string>();
		public static int brojZavrsenih = 0;
		public bool PokreniServer()
		{
			try
			{
				soket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				IPEndPoint ep = new IPEndPoint(IPAddress.Any, 20000);
				soket.Bind(ep);

				ThreadStart ts = Osluskuj;
				new Thread(ts).Start();

				return true;
			}
			catch (Exception)
			{

				return false;
			}
		}

		void Osluskuj()
		{
			try
			{
				while (true)
				{
					soket.Listen(8);
					Socket klijent = soket.Accept();
					NetworkStream tok = new NetworkStream(klijent);
					BinaryFormatter formater = new BinaryFormatter();
					TransferKlasa transfer = formater.Deserialize(tok) as TransferKlasa;
					transfer.Ulogovan = true;

					if(Server.listaIgraca.Contains(transfer.Igrac))
					{
						transfer.Ulogovan = false;
						transfer.Poruka = "Vec je ulogovan";
					}

					if(transfer.Ulogovan && Server.listaIgraca.Count == BrIgraca)
					{
						transfer.Ulogovan = false;
						transfer.Poruka = "Max br ulogovanih";
					}

					if(transfer.Ulogovan)
					{
						listaIgraca.Add(transfer.Igrac);
						listaTokova.Add(tok);

						new NitiKlijenta(tok, transfer.Igrac);
						transfer.Poruka = "Sacekajte pitanja!";
					}
					
					formater.Serialize(tok, transfer);
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		public bool ZaustaviServer()
		{
			try
			{
				soket.Close();
				return true;
			}
			catch (Exception)
			{

				return false;
			}
		}
	}
}
