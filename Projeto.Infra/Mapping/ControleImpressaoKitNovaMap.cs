using Projeto.Domain.ClassesInsert;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Infra.Mapping
{
    public class ControleImpressaoKitNovaMap : EntityTypeConfiguration<ControleImpressaoKitNova>
    {
        public ControleImpressaoKitNovaMap()
        {
            ToTable("ControleImpressaoKitNova");
            HasKey(x => x.idControleImpressaoKitNova);
        }
    }
}
