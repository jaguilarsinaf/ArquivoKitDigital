using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain
{
    public class Kitdigital
    {       
        public int tipoKit { get; set; }
        public string digital { get; set; }
        public int externalKey { get; set; }
        public Certificado certificado { get; set; }
        public Boleto boleto { get; set; }
        public Carteirinha carteirinha { get; set; }

    }
}
