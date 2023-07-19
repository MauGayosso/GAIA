using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer
{
	public class ItemAtts
	{
		public string Name { get; set; }
		public string no_parte { get; set; }
		public string Revision { get; set; }
		public string Materia_Prima { get; set; }
		public string lote { get; set; }
		public string proyecto { get; set; }
		public string acabado_s { get; set; }
		public string fecha_e { get; set; }
		public string tipo_acero { get; set; }
		public bool p_corte { get; set; }
		public bool p_doblez { get; set; }
		public bool p_maquinado { get; set; }
		public bool p_pintura { get; set; }
		public bool p_detallado { get; set; }
		public bool p_soldadura { get; set; }

	}
}