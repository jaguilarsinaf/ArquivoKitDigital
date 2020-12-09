using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain
{
    public class Dependente
    {
        public string parentesco { get; set; }
        public string nome { get; set; }
        public string dataNascimento { get; set; }
        public string dataInclusao { get; set; }
        public string inicioVigencia { get; set; }
        public string fimVigencia { get; set; }
        public string plano { get; set; }
        public List<CoberturasContratada> coberturasContratadas { get; set; }
    }
}
