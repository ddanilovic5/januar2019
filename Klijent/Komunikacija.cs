using Domen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Klijent
{
	 public class Komunikacija
	{
		NetworkStream tok;
		TcpClient klijent;
		BinaryFormatter formater;

		public TransferKlasa PoveziSeNaServer(TransferKlasa transfer)
		{
			try
			{
				klijent = new TcpClient("localhost", 20000);
				tok = klijent.GetStream();
				formater = new BinaryFormatter();
				formater.Serialize(tok, transfer);

				return formater.Deserialize(tok) as TransferKlasa;
			}
			catch (Exception)
			{
				transfer.Poruka = "Nesto ne valja";
				transfer.Ulogovan = false;
				return transfer;
			}
			
		}
		public void Kraj()
		{
			TransferKlasa transfer = new TransferKlasa();
			transfer.Operacije = Operacije.Kraj;
			formater.Serialize(tok, transfer);
		}

		public TransferKlasa PrimiPoruku()
		{
			 return formater.Deserialize(tok) as TransferKlasa;
		}

		internal void PosaljiPoruku(TransferKlasa transfer)
		{
			formater.Serialize(tok, transfer);
		}
	}
}
