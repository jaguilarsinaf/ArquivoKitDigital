﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain
{
    public class Emissao
    {
        public int cdconseg { get; set; }
        public int cdemi { get; set; }
        public int nrpps { get; set; }
        public int nrppscor { get; set; }
        public short cdrmoseg { get; set; }
        public short cdsmo { get; set; }
        public int nrapo { get; set; }
        public int nreds { get; set; }
        public short tpemi { get; set; }
        public short tpestemi { get; set; }
        public short tpope { get; set; }
        public short tpeds { get; set; }
        public System.DateTime dtcotpps { get; set; }
        public System.DateTime dtefepps { get; set; }
        public System.DateTime dtemi { get; set; }
        public System.DateTime dtinivig { get; set; }
        public System.DateTime dtfimvig { get; set; }
        public decimal pcnospte { get; set; }
        public decimal pcmdicom { get; set; }
        public int nratasor { get; set; }
        public decimal nrcmnsegvul { get; set; }
        public int nrordcosact { get; set; }
        public short tpnivcom { get; set; }
        public short tprcrrtt { get; set; }
        public string incalparpre { get; set; }
        public string incalparcom { get; set; }
        public string incalparces { get; set; }
        public string inrcliof { get; set; }
        public string incbajur { get; set; }
        public string incbacstapo { get; set; }
        public decimal txpdapre { get; set; }
        public short cdrefmonpre { get; set; }
        public short cdrefmonis { get; set; }
        public System.DateTime dtcvspre { get; set; }
        public System.DateTime dtcvsis { get; set; }
        public decimal vlistotinf { get; set; }
        public System.DateTime dtatasor { get; set; }
        public decimal vlpretotemi { get; set; }
        public short tpprt { get; set; }
        public short stitf { get; set; }
        public Nullable<int> cdorgprtsuc { get; set; }
        public Nullable<short> tporgprtsuc { get; set; }
        public Nullable<int> cdorgprtemp { get; set; }
        public Nullable<short> tporgprtemp { get; set; }
        public Nullable<System.DateTime> dtbasecalc { get; set; }
        public Nullable<System.DateTime> dtresseguro { get; set; }
        public string incancemi { get; set; }
        public Nullable<short> tpmotrecusa { get; set; }
        public Nullable<decimal> vllimind { get; set; }
        public string idrecusared { get; set; }
        public string cdmotrecusared { get; set; }
    }
}
