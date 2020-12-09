using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain
{
    public class Seguro
    {
        public string codigoRamo { get; set; }
        public string ramo { get; set; }
        public string produto { get; set; }
        public string sucursal { get; set; }
        public string inicioVigencia { get; set; }
        public string fimVigencia { get; set; }
        public string ingressoApolice { get; set; }
        public string apolice { get; set; }
        public string proLabore { get; set; }
        public string tipoPagamento { get; set; }
        public string corretor { get; set; }
        public string codigoSUSEP { get; set; }
        public string periodicidade { get; set; }
        public string vencimento { get; set; }
        public string valorPremioLiquido { get; set; }
        public string adicionalFracionamento { get; set; }
        public string custoApolice { get; set; }        
        public string valorIOF { get; set; }
        public string valorPremioTotal { get; set; }
        public List<DPS> dps { get; set; }
        public Estipulante estipulante { get; set; }
    }
}
