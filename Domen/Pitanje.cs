using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domen
{
	[Serializable]
	public class Pitanje
	{
		public int IgraId { get; set; }
		public int PitanjeId { get; set; }
		public string PitanjeTekst { get; set; }
		public string TacanOdgovor { get; set; }
		public int BrojPoena { get; set; }
	}
}
