using Projeto.Domain;
using Projeto.Domain.ClassesInsert;
using Projeto.Infra.Mapping;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Infra.DataContext
{
    public class Conexao : DbContext
    {
        public Conexao()
            : base(ConfigurationManager.ConnectionStrings["kitDigitalJson"].ConnectionString)
        {
            //this.Configuration.LazyLoadingEnabled = false;
            //this.Database.CommandTimeout = 380;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new ControleImpKitItemMap());
            modelBuilder.Configurations.Add(new ControleImpressaoKitNovaMap());
            modelBuilder.Configurations.Add(new TB_DETALHE_REGISTROMap());
            modelBuilder.Configurations.Add(new TB_ERRO_REGISTROMap());
            modelBuilder.Configurations.Add(new TB_LOG_PROCESSAMENTOMap());
            modelBuilder.Configurations.Add(new TB_REGISTRO_PROCESSAMENTOMap());
            modelBuilder.Configurations.Add(new Controle2ViaMap());
            modelBuilder.Configurations.Add(new LogControle2ViaMap());
        }

        public DbSet<TB_CAMPO> TB_CAMPO { get; set; }
        public DbSet<Controle2Via> Controle2Via { get; set; }
        public DbSet<ControleImpKitItem> ControleImpKitItem { get; set; }
        public DbSet<ControleImpressaoKitNova> ControleImpressaoKitNova { get; set; }
        public DbSet<TB_DETALHE_REGISTRO> TB_DETALHE_REGISTRO { get; set; }
        public DbSet<TB_ERRO_REGISTRO> TB_ERRO_REGISTRO { get; set; }
        public DbSet<TB_LOG_PROCESSAMENTO> TB_LOG_PROCESSAMENTO { get; set; }
        public DbSet<TB_REGISTRO_PROCESSAMENTO> TB_REGISTRO_PROCESSAMENTO { get; set; }
        public DbSet<LogControle2Via> LogControle2Via { get; set; }

    }
}
