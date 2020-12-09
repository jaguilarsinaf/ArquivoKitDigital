using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain
{
    public class Pessoa
    {
        public int cdpes { get; set; }
        public short tppes { get; set; }
        public Nullable<decimal> nrcgccpf { get; set; }
        public string nmpes { get; set; }
        public decimal cdadmcrocre { get; set; }
        public decimal cdcrocre { get; set; }
        public System.DateTime dtvldcrocre { get; set; }
        public short tppgt { get; set; }
        public int cditfful { get; set; }
        public Nullable<decimal> nremp { get; set; }
        public short tpcli { get; set; }
        public decimal nrinsiap { get; set; }
        public short tpclaisp { get; set; }
        public string iniseiss { get; set; }
        public decimal pciss { get; set; }
        public decimal nriscmun { get; set; }
        public string iniseir { get; set; }
        public System.DateTime dtcad { get; set; }
        public string inmcremp { get; set; }
        public string nmcot { get; set; }
        public string nmfanpes { get; set; }
        public decimal nrinsest { get; set; }
        public string iniseiof { get; set; }
        public System.DateTime dtnas { get; set; }
        public short tpsex { get; set; }
        public short stestciv { get; set; }
        public short nrdepirf { get; set; }
        public string inemintf { get; set; }
        public string inretinss { get; set; }
        public string incpf { get; set; }
        public Nullable<short> cdativpes { get; set; }
        public Nullable<decimal> vlrendapes { get; set; }
        public Nullable<decimal> nrrg { get; set; }
        public string orgaoexprg { get; set; }
        public Nullable<System.DateTime> dtexprg { get; set; }
        public string idpep { get; set; }
        public Nullable<bool> fl_vlacpf { get; set; }
    }
}
