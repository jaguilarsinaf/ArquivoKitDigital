using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain
{
    public class DadosComplementares
    {


        public string email { get; set; }
        public Endereco endereco { get; set; }
        public IEnumerable<Telefone> telefone { get; set; }
        public string tipoPagamento { get; set; }
    }
}
