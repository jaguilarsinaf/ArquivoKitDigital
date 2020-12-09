using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain
{
    public class DPS
    {
        public string tipoSegurado { get; set; }
        public List<PerguntaRespostaDPS> perguntaRespostaDPS { get; set; }
    }
}
