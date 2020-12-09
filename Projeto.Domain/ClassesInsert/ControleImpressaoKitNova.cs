using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain.ClassesInsert
{
    public class ControleImpressaoKitNova
    {
        public int idControleImpressaoKitNova { get; set; }
        public int idPadraoTipoKit { get; set; }
        public DateTime dtimpressao { get; set; }
        public string cdusuimp { get; set; }
        public string nmarqimp { get; set; }
        public int qtdereg { get; set; }

        public ControleImpressaoKitNova()
        {

        }

        public ControleImpressaoKitNova(int idPadraoTipoKit, DateTime dtimpressao, string cdusuimp, string nmarqimp, int qtdereg)
        {
            this.idPadraoTipoKit = idPadraoTipoKit;
            this.dtimpressao = dtimpressao;
            this.cdusuimp = cdusuimp;
            this.nmarqimp = nmarqimp;
            this.qtdereg = qtdereg;
        }
    }
}
