using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain
{
    public class Carteirinha
    {
        public int contrato { get; set; }
        public int certificado { get; set; }
        public string nome { get; set; }
        public DadosComplementares dadosComplementares { get; set; }
        //public Endereco endereco { get; set; }

        public Carteirinha()
        {
            dadosComplementares = new DadosComplementares();
        }
    }
}
