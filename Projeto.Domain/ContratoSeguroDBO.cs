using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain
{
    public class ContratoSeguroDBO
    {
        public int cdconseg { get; set; }
        public short tpconseg { get; set; }
        public short tpapo { get; set; }
        public string insegmss { get; set; }
        public string inemicer { get; set; }
        public short qtviaapo { get; set; }
        public short qtviacer { get; set; }
        public short qtviaeds { get; set; }
        public decimal pcdptiniapoa { get; set; }
        public decimal vldptiniapoa { get; set; }
        public decimal pcajupremena { get; set; }
        public decimal vlpreminapo { get; set; }
        public decimal vlpreini { get; set; }
        public string inapofat { get; set; }
        public short tpblocan { get; set; }
        public int cdpescor { get; set; }
        public short cdpro { get; set; }
        public int cdpes { get; set; }
        public Nullable<short> nrseqend { get; set; }
        public short cdrmoseg { get; set; }
        public short cdsmo { get; set; }
        public Nullable<short> cdrefmonprem { get; set; }
        public Nullable<short> cdrefmonprei { get; set; }
        public Nullable<short> cdrefmondpsi { get; set; }
        public decimal cdconreu { get; set; }
        public decimal cdconrnv { get; set; }
        public short tpiteseg { get; set; }
        public short tpnivctt { get; set; }
        public short tpsgr { get; set; }
        public short tpnivben { get; set; }
        public Nullable<int> cdpessegrnv { get; set; }
        public Nullable<decimal> nrapornv { get; set; }
        public Nullable<int> cdcanal { get; set; }
        public Nullable<short> id_vidindgru { get; set; }
    }
}
