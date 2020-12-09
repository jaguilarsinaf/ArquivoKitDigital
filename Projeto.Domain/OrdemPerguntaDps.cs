using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain
{
    public class OrdemPerguntaDps
    {
        public int idOrdemPerguntaDps { get; set; }
        public int codigoProduto { get; set; }
        public int numeroPergunta { get; set; }
        public int codigoPergunta { get; set; }
        public string tipoSegurado { get; set; }
        public DateTime dataVigencia { get; set; }
        public DateTime dataCadastro { get; set; }
        public string usuario { get; set; }
    }
}
