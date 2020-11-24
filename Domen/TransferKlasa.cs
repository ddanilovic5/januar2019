using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domen
{
	public enum Operacije { Kraj = 1,
		Pogadjaj = 2
	}
	[Serializable]
	public class TransferKlasa
	{
		public Operacije Operacije;
		public string Poruka;
		public Igrac Igrac;
		public bool Ulogovan;

		public string Pitanje;
		public string Odgovor;
		public double UkBrPoena;
		public bool KrajIgre;
	}
}
