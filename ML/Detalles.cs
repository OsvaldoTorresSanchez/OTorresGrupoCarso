using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Detalles
    {

        public double Altura { get; set; }
        public double Peso { get; set; }
        public string  Categoria { get; set; }

        public string Habilidad { get; set; }
        public Habilidad Habilidades { get; set; }
        public Tipo Tipo { get; set; }
    }
}
