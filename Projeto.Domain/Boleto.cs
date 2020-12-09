using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain
{
    public class Boleto
    {
        
        public string apolice { get; set; }
        public int certificado { get; set; }
        public string nome { get; set; }
        public string cpf { get; set; }
        public DadosComplementares dadosComplementares { get; set; }
        //public Endereco endereco { get; set; }
        public List<Lamina> laminas { get; set; }

        public Boleto()
        {
            dadosComplementares = new DadosComplementares();
        }
    }
}
