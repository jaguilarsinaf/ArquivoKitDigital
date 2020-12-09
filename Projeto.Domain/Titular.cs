using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain
{
    public class Titular
    {
        public int contrato { get; set; }
        public int certificado { get; set; }
        public int proposta { get; set; }
        public int emissao { get; set; }
        public int item { get; set; }
        public string nome { get; set; }
        public string dataNascimento { get; set; }
        public string numeroSegurado { get; set; }
        public string cpf { get; set; }
        public string email { get; set; }
        public DadosComplementares dadosComplementares { get; set; }
        public string plano { get; set; }
        public List<CoberturasContratada> coberturasContratadas { get; set; }   
        public Titular()
        {
            dadosComplementares = new DadosComplementares();
        }
    }
}
