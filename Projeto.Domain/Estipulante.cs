using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain
{
    public class Estipulante
    {
        public Estipulante()
        {
            dadosComplementares = new DadosComplementares();
        }
        public string nome { get; set; }
        public string cnpj { get; set; }       
        //public Endereco endereco { get; set; }
        public DadosComplementares dadosComplementares { get; set; }
    }
}
