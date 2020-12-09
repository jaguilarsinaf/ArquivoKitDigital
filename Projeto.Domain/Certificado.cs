using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain
{
    public class Certificado
    {
        public Seguro seguro { get; set; }
        public Titular titular { get; set; }
        public List<Dependente> dependentes { get; set; }
        public List<Beneficiario> beneficiarios { get; set; }
        public string processoSUSEP { get; set; }
        public string dataEmissao { get; set; }
        public string observacaoCarencia { get; set; }
    }
}
