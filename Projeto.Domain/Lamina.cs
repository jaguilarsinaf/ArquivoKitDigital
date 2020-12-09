using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain
{
    public class Lamina
    {
        public int parcela { get; set; }
        public string vencimento { get; set; }
        public string agencia { get; set; }
        public string codigoBeneficiario { get; set; }
        public string valor { get; set; }
        public string nossoNumero { get; set; }
        public string linhaDigitavel { get; set; }
        public string codigoBarras { get; set; }
        public string numeroDocumento { get; set; }
        public string dataDocumento { get; set; }
        public string dataProcessamento { get; set; }
        public string dataLimitePagamento { get; set; }
        public string carencia { get; set; }
    }
}
