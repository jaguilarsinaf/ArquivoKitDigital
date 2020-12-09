using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain
{
    public class EnderecoPessoa
    {
        public int cdpes { get; set; }
        public short nrseqend { get; set; }
        public string nmendpes { get; set; }
        public string nmcidpes { get; set; }
        public int cdceppes { get; set; }
        public string sguf { get; set; }
        public Nullable<short> cdddd { get; set; }
        public Nullable<int> nrtel { get; set; }
        public Nullable<short> nrram { get; set; }
        public Nullable<int> nrfax { get; set; }
        public Nullable<int> nrtex { get; set; }
        public string inendres { get; set; }
        public string inendcol { get; set; }
        public string inendcob { get; set; }
        public string inendcor { get; set; }
        public Nullable<int> nrendpes { get; set; }
        public string nmcompl { get; set; }
        public string nmbairro { get; set; }
        public string nmemail { get; set; }
        public string nmreferencia { get; set; }
        public Nullable<int> nrcel { get; set; }
        public Nullable<short> cddddcom { get; set; }
        public Nullable<int> nrtelcom { get; set; }
        public Nullable<short> nrramalcom { get; set; }
        public string idenvsms { get; set; }
    }
}
