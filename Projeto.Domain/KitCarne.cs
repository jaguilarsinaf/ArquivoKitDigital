using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain
{
    public class KitCarne
    {
        public int idKitCarne { get; set; }
        public int idControle2Via { get; set; }
        public int codigoContrato { get; set; }
        public int codigoEmissao { get; set; }
        public short numeroParcela { get; set; }
    }
}
