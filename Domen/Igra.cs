using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domen
{
	[Serializable]
	public class Igra
	{
		public override string ToString()
		{
			return Naziv;
		}
		public int IgraId { get; set; }
		public string Naziv { get; set; }
		public DateTime DatumVremePocetka { get; set; }
		public DateTime DatumVremeKraja { get; set; }
		public string Pobednik { get; set; }
		public BindingList<Pitanje> ListaPitanja { get; set; }
	}
}
