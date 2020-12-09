using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain
{
    public class CertificadoDBO
    {
        public int cdconseg { get; set; }
        public int cdemi { get; set; }
        public int nrcer { get; set; }
        public System.DateTime dtemipxmpar { get; set; }
        public int cditeseg { get; set; }
        public Nullable<short> qtparcar { get; set; }
        public short tpfreqpl { get; set; }
        public string indcoris { get; set; }
        public string idperiodocar { get; set; }
        public Nullable<System.DateTime> dtgerpxmpar { get; set; }
        public Nullable<System.DateTime> dtatugerpxmpar { get; set; }
        public string cdusugerpxmpar { get; set; }
        public Nullable<short> qtmesrda { get; set; }
        public string idsitpep { get; set; }
        public string indpep { get; set; }
        public string idvenadm { get; set; }
        public string id_ictserasa { get; set; }
        public string idmigdep { get; set; }
        public string id_ppncjg { get; set; }
    }
}
