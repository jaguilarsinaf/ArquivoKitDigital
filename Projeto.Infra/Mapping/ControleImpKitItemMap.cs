using Projeto.Domain.ClassesInsert;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Infra.Mapping
{
    public class ControleImpKitItemMap : EntityTypeConfiguration<ControleImpKitItem>
    {
        public ControleImpKitItemMap()
        {
            ToTable("ControleImpKitItem");
            HasKey(x => new { x.idControleImpKitItem });
        }
    }
}
