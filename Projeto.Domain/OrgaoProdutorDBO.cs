using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain
{
    public class OrgaoProdutorDBO
    {
        public int cdorgprt { get; set; }
        public short tporgprt { get; set; }
        public string nmorgprt { get; set; }
        public short cdrgisus { get; set; }
        public short storgprt { get; set; }
        public Nullable<int> cdorgprtvin { get; set; }
        public Nullable<short> tporgprtvin { get; set; }
        public Nullable<int> cdpes { get; set; }
        public Nullable<short> nrseqend { get; set; }
        public Nullable<int> cdpescol { get; set; }
        public Nullable<System.DateTime> dtcad { get; set; }
        public Nullable<decimal> nrcgcorgprt { get; set; }
        public Nullable<decimal> pcisspj { get; set; }
        public Nullable<int> cdcencus { get; set; }
        public Nullable<decimal> pcisspf { get; set; }
    }
}
