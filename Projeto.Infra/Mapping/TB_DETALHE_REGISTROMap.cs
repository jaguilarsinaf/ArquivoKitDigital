using Projeto.Domain.ClassesInsert;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Infra.Mapping
{
    public class TB_DETALHE_REGISTROMap : EntityTypeConfiguration<TB_DETALHE_REGISTRO>
    {
        public TB_DETALHE_REGISTROMap()
        {
            ToTable("TB_DETALHE_REGISTRO");
            HasKey(x => x.cd_dtreg);
        }
    }
}
