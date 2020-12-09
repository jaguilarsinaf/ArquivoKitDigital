using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain
{
    public class LaminaBoleto
    {
        public int parcela { get; set; }
        public string agencia { get; set; }
        public string codBeneficiario { get; set; }
        public string banco { get; set; }
    }
}
