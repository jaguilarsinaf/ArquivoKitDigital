using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain
{
    public class PadraoTipoKitDBO
    {
        public int idPadraoTipoKit { get; set; }
        public Nullable<int> tipoKit { get; set; }
        public string nomeTipoKit { get; set; }
        public Nullable<System.DateTime> dataInclusao { get; set; }
        public string usuario { get; set; }
    }
}
