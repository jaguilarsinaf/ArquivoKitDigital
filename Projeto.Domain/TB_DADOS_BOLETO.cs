using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain
{
    public class TB_DADOS_BOLETO
    {
        public int cd_seq { get; set; }
        public int cd_ctt { get; set; }
        public int cd_ems { get; set; }
        public int nr_pclpmo { get; set; }
        public string cd_lindig { get; set; }
        public string cd_bar { get; set; }
        public decimal nr_nosnum { get; set; }
        public string nr_dignosnum { get; set; }
        public decimal vl_pretot { get; set; }

    }
}
