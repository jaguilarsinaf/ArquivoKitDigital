using Projeto.Domain.ClassesInsert;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Infra.Mapping
{
    public class LogControle2ViaMap : EntityTypeConfiguration<LogControle2Via>
    {
        public LogControle2ViaMap()
        {
            ToTable("LogControle2Via");
            HasKey(x => x.idLogControle2Via);
        }
    }
}
