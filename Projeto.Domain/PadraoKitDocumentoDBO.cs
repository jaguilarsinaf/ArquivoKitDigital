using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain
{
    public class PadraoKitDocumentoDBO
    {
        public int idPadraoKitDocumento { get; set; }
        public int idPadraoTipoKit { get; set; }
        public int idPadraoTipoDocumento { get; set; }
        public Nullable<System.DateTime> dataVigencia { get; set; }
        public Nullable<System.DateTime> dataInclusao { get; set; }
        public string usuario { get; set; }
        public short codigoProduto { get; set; }
        public string clienteDigital { get; set; }
    }
}
