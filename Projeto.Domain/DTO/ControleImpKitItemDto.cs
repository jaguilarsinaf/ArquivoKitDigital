using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain.DTO
{
    public class ControleImpKitItemDto
    {
        public int cdc2v { get; set; }
        public int cdconseg { get; set; }
        public int cdemi { get; set; }
        public Nullable<int> cditeseg { get; set; }
        public Nullable<int> nrcer { get; set; }
        public KitDigitalDto KitDigitalDto { get; set; }
    }
}
