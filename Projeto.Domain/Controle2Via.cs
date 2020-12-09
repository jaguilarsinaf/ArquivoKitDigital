using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain
{
    public class Controle2Via
    {
        public int cdc2v { get; set; }
        public int cdconseg { get; set; }
      
        public int cdemi { get; set; }
        public Nullable<int> cditeseg { get; set; }
        public Nullable<int> nrcer { get; set; }
        public short tpkit { get; set; }
      
        public System.DateTime dtsolicita { get; set; }
        public Nullable<System.DateTime> dtimpressao { get; set; }
        public string indimp { get; set; }
        public string cdusuari { get; set; }

        public int idKitSolicitacao { get; set; }
        public string iddig { get; set; }


    }
}
