using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain
{
    public class Beneficiario
    {
        public string nome { get; set; }
        public string cpf { get; set; }
        public string percentualParticipacao { get; set; }
        public string parentesco { get; set; }
    }
}
