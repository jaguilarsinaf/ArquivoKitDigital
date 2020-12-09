using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain.ClassesInsert
{
    public class ControleImpKitItem
    {
        public int idControleImpKitItem { get; set; }
        public int cdconseg { get; set; }
        public int cdemi { get; set; }
        public int cditeseg { get; set; }
        public int nrcer { get; set; }
        public int cdc2v { get; set; }
        public int idControleImpressaoKitNova { get; set; }
        public string jsonClasse { get; set; }

        public ControleImpKitItem(int cdconseg, int cdemi, int cditeseg, int nrcer, int cdc2v, int idControleImpressaoKitNova, string jsonClasse)
        {
            this.cdconseg = cdconseg;
            this.cdemi = cdemi;
            this.cditeseg = cditeseg;
            this.nrcer = nrcer;
            this.cdc2v = cdc2v;
            this.idControleImpressaoKitNova = idControleImpressaoKitNova;
            this.jsonClasse = jsonClasse;
        }

        public ControleImpKitItem()
        {

        }
    }
}
