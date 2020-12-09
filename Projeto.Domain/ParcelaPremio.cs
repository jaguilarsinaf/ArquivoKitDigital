using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain
{
    public class ParcelaPremio
    {
        public int cdconseg { get; set; }
        public int cdemi { get; set; }
        public short cdparpre { get; set; }
        public System.DateTime dtven { get; set; }
        //public System.DateTime dtrcd { get; set; }        
        public System.DateTime dtvenprg { get; set; }
        public int stparpre { get; set; }
        public int tplqdparpre { get; set; }
    }
}
