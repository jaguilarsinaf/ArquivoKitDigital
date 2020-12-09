using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain
{
    public class PadraoTipoDocumentoDBO
    {
        public int idPadraoTipoDocumento { get; set; }
        public string nomeDocumento { get; set; }
        public Nullable<System.DateTime> dataInclusao { get; set; }
        public string usuario { get; set; }
    }
}
