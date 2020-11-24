using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domen
{
	[Serializable]
	public class Igrac
	{
		public string Username { get; set; }
		public int BrTacnih { get; set; }
		public int BrNetacnih { get; set; }
		public double Ukupno { get; set; }
	}
}
