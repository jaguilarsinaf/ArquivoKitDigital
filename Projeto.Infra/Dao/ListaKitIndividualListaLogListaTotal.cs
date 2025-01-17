﻿using AutoMapper;
using Dapper;
using Newtonsoft.Json;
using Projeto.Domain;
using Projeto.Domain.ClassesInsert;
using Projeto.Domain.DTO;
using Projeto.Infra.DataContext;
using Projeto.Infra.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Projeto.Infra.Dao
{
    public class ListaKitIndividualListaLogListaTotal
    {
        public RetornoLogProcessamento AddLogProcessamento(string usuario)
        {
            TB_LOG_PROCESSAMENTO tbLogProcessamento = new TB_LOG_PROCESSAMENTO()
            {
                cd_pss = "KIT DIGITAL - Json",
                cd_usu = usuario,
                dt_inipss = DateTime.Now,
                qt_totreg = null,
                dt_fimpss = null
            };
            using (var context = new Conexao())
            {
                try
                {
                    context.Entry(tbLogProcessamento).State = EntityState.Added;
                    context.SaveChanges();
                    return new RetornoLogProcessamento(tbLogProcessamento.cd_logpss);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        public ConcurrentBag<Kitdigital> AddControleImpKitItem(List<int> lstKit, string usuario, ConcurrentBag<Kitdigital> lstKitDigital, RetornoLogProcessamento retornoLogProcessamento, string nomeArquivo)
        {
            Mapeamento.Instance();
            var lista = new ConcurrentBag<Kitdigital>();
            var context = new Conexao();

            try
            {
                context.Configuration.AutoDetectChangesEnabled = false;

                try
                {
                    var listaDto = new ConcurrentBag<ControleImpKitItemDto>();
                    int cont = 0;
                    foreach (var kit in lstKit)
                    {
                        var controleImpressaoKitNova = new ControleImpressaoKitNova(BuscarIdPadraoTipoKit(kit), DateTime.Now, usuario, nomeArquivo, lstKitDigital.Where(x => x.tipoKit == kit).Count());

                        using (TransactionScope scope2 = new TransactionScope())
                        {
                            context.Entry(controleImpressaoKitNova).State = EntityState.Added;
                            context.SaveChanges();
                            scope2.Complete();
                        }

                        try
                        {
                            foreach (var item in lstKitDigital.Where(x => x.tipoKit == kit))
                            {

                                TB_REGISTRO_PROCESSAMENTO tb_registro_processamento = new TB_REGISTRO_PROCESSAMENTO(retornoLogProcessamento.idLogProcessamento, "1", null);
                                using (TransactionScope scope5 = new TransactionScope())
                                {
                                    context.Entry(tb_registro_processamento).State = EntityState.Added;
                                    context.SaveChanges();
                                    scope5.Complete();
                                }
                                TB_DETALHE_REGISTRO tb_detalhe_registro = new TB_DETALHE_REGISTRO(tb_registro_processamento.cd_regpss, new Repository<TB_CAMPO>().Get(x => x.nm_cam == "cdc2v").FirstOrDefault().cd_cam, item.externalKey.ToString());
                                using (TransactionScope scope6 = new TransactionScope())
                                {
                                    context.Entry(tb_detalhe_registro).State = EntityState.Added;
                                    context.SaveChanges();
                                    scope6.Complete();
                                }
                                var controle2Via = context.Controle2Via.Find(item.externalKey);
                                string verificarErrosKit = verificarErros(item, controle2Via.cdconseg, kit);
                                if (verificarErrosKit.Equals("ok"))
                                {
                                    var dto = Mapper.Map<ControleImpKitItemDto>(controle2Via);
                                    dto.KitDigitalDto = Mapper.Map<KitDigitalDto>(item);
                                    dto.KitDigitalDto.idControleImpressaoKitNova = controleImpressaoKitNova.idControleImpressaoKitNova;
                                    listaDto.Add(dto);
                                    using (TransactionScope scope7 = new TransactionScope())
                                    {
                                        tb_registro_processamento.st_regpss = "2";
                                        tb_registro_processamento.dt_sitreg = DateTime.Now;
                                        context.Entry(tb_registro_processamento).State = EntityState.Modified;

                                        //var controle2Via = context.Controle2Via.Find(item.codigoControle);
                                        controle2Via.dtimpressao = DateTime.Now;
                                        controle2Via.indimp = "S";
                                        context.Entry(controle2Via).State = EntityState.Modified;
                                        context.SaveChanges();
                                        scope7.Complete();
                                    }
                                    lista.Add(item);
                                }
                                else
                                {
                                    using (TransactionScope scope8 = new TransactionScope())
                                    {
                                        tb_registro_processamento.st_regpss = "3";
                                        tb_registro_processamento.dt_sitreg = DateTime.Now;
                                        context.Entry(tb_registro_processamento).State = EntityState.Modified;
                                        context.SaveChanges();

                                        //Alteração na Controle2Via para esta solicitação ser cancelada por erro na geração.
                                        LogControle2Via logControle2Via = new LogControle2Via(controle2Via.cdc2v, controle2Via.cdusuari);
                                        context.Entry(logControle2Via).State = EntityState.Added;
                                        context.SaveChanges();

                                        controle2Via.dtimpressao = DateTime.Now;
                                        controle2Via.indimp = "C";
                                        controle2Via.cdusuari = usuario;
                                        context.Entry(controle2Via).State = EntityState.Modified;
                                        context.SaveChanges();
                                        //Fim Alteração

                                        TB_ERRO_REGISTRO tb_erro_registro = new TB_ERRO_REGISTRO(tb_registro_processamento.cd_regpss, 9999, verificarErrosKit);
                                        context.Entry(tb_erro_registro).State = EntityState.Added;
                                        context.SaveChanges();
                                        scope8.Complete();
                                    }
                                }
                                ++cont;
                                if ((cont % 1000 == 0) || (cont == lstKitDigital.Count()))
                                {
                                    if (listaDto.Count() > 0)
                                    {
                                        context = AddToContext(context, listaDto, controleImpressaoKitNova);
                                        listaDto = null;
                                        listaDto = new ConcurrentBag<ControleImpKitItemDto>();
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                        using (TransactionScope scope9 = new TransactionScope())
                        {
                            controleImpressaoKitNova.qtdereg = lista.Where(x => x.tipoKit == kit).Count();
                            context.Entry(controleImpressaoKitNova).State = EntityState.Modified;

                            var tbLogProcessamento = context.TB_LOG_PROCESSAMENTO.Find(retornoLogProcessamento.idLogProcessamento);
                            tbLogProcessamento.dt_fimpss = DateTime.Now;
                            tbLogProcessamento.qt_totreg = lstKitDigital.Count();
                            context.Entry(tbLogProcessamento).State = EntityState.Modified;
                            context.SaveChanges();
                            scope9.Complete();
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                return lista;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private int BuscarIdPadraoTipoKit(int tipoKit)
        {
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["kitDigitalJson"].ConnectionString))
            {
                var id = db.Query<int>("select idPadraoTipoKit from PadraoTipoKit where tipoKit = @tipoKit", new { tipoKit = tipoKit }).FirstOrDefault();
                return id;
            }
        }

        private void AddTransacao(SqlTransaction transaction, Conexao context, ConcurrentBag<ControleImpKitItemDto> lista, ControleImpressaoKitNova controleImpressaoKitNova)
        {
            using (context)
            {
                using (TransactionScope scope4 = new TransactionScope(TransactionScopeOption.Required))
                {
                    using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["kitDigitalJson"].ConnectionString))
                    {
                        connection.Open();
                        using (transaction = connection.BeginTransaction(System.Data.IsolationLevel.Serializable))
                        {
                            try
                            {
                                //foreach (var item in lista)
                                Parallel.ForEach(lista, item =>
                                {
                                    SqlCommand cmd2 = new SqlCommand("PR_InserirControleImpKitItem", connection, transaction);
                                    cmd2.CommandType = System.Data.CommandType.StoredProcedure;
                                    cmd2.Parameters.AddWithValue("@cdconseg", item.cdconseg);
                                    cmd2.Parameters.AddWithValue("@cdemi", item.cdemi);
                                    cmd2.Parameters.AddWithValue("@cditeseg", item.cditeseg);
                                    cmd2.Parameters.AddWithValue("@nrcer", item.nrcer);
                                    cmd2.Parameters.AddWithValue("@cdc2v", item.cdc2v);
                                    cmd2.Parameters.AddWithValue("@idControleImpressaoKitNova", item.KitDigitalDto.idControleImpressaoKitNova);
                                    cmd2.Parameters.AddWithValue("@json", JsonConvert.SerializeObject(Mapper.Map<Kitdigital>(item.KitDigitalDto)));
                                    cmd2.ExecuteNonQuery();
                                    cmd2.Dispose();
                                });
                                transaction.Commit();
                            }
                            catch (Exception e)
                            {
                                transaction.Rollback();
                                throw new Exception(e.Message);
                            }
                        }
                    }
                    scope4.Complete();
                }
            }
            transaction.Dispose();
        }

        private Conexao AddToContext(Conexao context, ConcurrentBag<ControleImpKitItemDto> lista, ControleImpressaoKitNova controleImpressaoKitNova)
        {
            SqlTransaction transaction = null;
            AddTransacao(transaction, context, lista, controleImpressaoKitNova);
            context.Dispose();
            context = new Conexao();
            context.Configuration.AutoDetectChangesEnabled = false;
            return context;
        }

        private IEnumerable<dynamic> BuscarCertificados(int id, int cdconseg, string digital)
        {
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["kitDigitalJson"].ConnectionString))
            {
                db.Open();
                using (var tr = db.BeginTransaction())
                {
                    try
                    {
                        var queryParameters = new DynamicParameters();
                        queryParameters.Add("@digital", digital);
                        queryParameters.Add("@tipoKit", id);
                        queryParameters.Add("@cdconseg", cdconseg);
                        var controle2Via = db.Query("PR_BuscarNomeDocumentoKitDigital", queryParameters, commandType: CommandType.StoredProcedure);
                        tr.Commit();
                        return controle2Via;
                    }
                    catch (Exception)
                    {
                        tr.Rollback();
                        throw;
                    }

                }
            }
        }

        private bool VerificaClienteCancelado(int cdc2v)
        {
            try
            {
                using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["kitDigitalJson"].ConnectionString))
                {
                    int tpestemi = db.Query<int>("select tpestemi from Controle2Via c2v inner join Emissao emi on c2v.cdconseg = emi.cdconseg and c2v.cdemi = emi.cdemi where c2v.cdc2v = @cdc2v", new { cdc2v = cdc2v }).FirstOrDefault();
                
                    if (tpestemi == 3)
                        return true;

                    return false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }           
        }

        private string verificarErros(Kitdigital kit, int cdconseg, int id)
        {
            try
            {
                //Verifica se o certificado está cancelado                
                if (VerificaClienteCancelado(kit.externalKey))
                    return "kit: Cliente está cancelado.";

                var documentos = BuscarCertificados(id, cdconseg, kit.digital);
                if (documentos.Count() == 0)
                    return "Nenhum documento parametrizado para este Produto.";

                foreach (var i in documentos)
                {
                    switch (i.nomeDocumento)
                    {
                        case "CERTIFICADO":
                            if (kit.certificado == null)
                                return "certificado: Nenhum dado do documento foi recuperado.";
                            if (kit.certificado.seguro == null)
                                return "certificado: Nenhum dado de seguro foi recuperado.";
                            if (kit.certificado.seguro.estipulante == null)
                                return "certificado: Erro ao recuperar Estipulante.";
                            if (kit.certificado.seguro.valorPremioLiquido.Equals("0,00"))
                                return "certificado: Prêmio liquido não encontrado/igual a 0.";
                            if (kit.certificado.seguro.estipulante.dadosComplementares.endereco == null)
                                return "certificado: Erro ao recuperar endereço do Estipulante";
                            if (kit.certificado.titular == null)
                                return "certificado: Nenhum dado do principal foi recuperado.";
                            if (kit.certificado.titular.coberturasContratadas == null)
                                return "certificado: Não foram encontradas coberturas do principal.";
                            if (kit.certificado.titular.dadosComplementares.endereco == null)
                                return "certificado: Erro ao recuperar endereço do principal.";
                            if (kit.certificado.seguro.dps.Count() == 0)
                                return "certificado: Erro ao recuperar DPS do principal.";

                            //Tasker 33110
                            if (kit.digital == "S")
                            {
                                //Se o certificado for digital e não tiver telefone e nem e-mail não deixar gerar json
                                if ((kit.certificado.titular.dadosComplementares.telefone == null) && (kit.certificado.titular.dadosComplementares.email == null))
                                    return "certificado: Erro ao recuperar telefone e e-mail";
                            }
                            //Tasker 33110 fim

                            if (kit.certificado.dependentes != null)
                            {
                                foreach (var item in kit.certificado.dependentes)
                                {
                                    if (item.coberturasContratadas == null)
                                        return "certificado: Erro ao recuperar coberturas para os dependente";
                                }
                            }
                            break;
                        case "CARNÊ":
                            if (kit.boleto == null) 
                            {
                                //KITS que saem somente Carnê
                                if (kit.tipoKit == 48 || kit.tipoKit == 123 || kit.tipoKit == 100 || kit.tipoKit == 124)
                                    return "boleto: Não foi encontrado boleto para o cliente.";

                                //Tasker 36408                                
                                //Se for Kit Renovação, certificado não digital e forma de pagamento = Ficha de Compensação 
                                if ((kit.tipoKit == 51) && (kit.digital == "N"))
                                    if (kit.certificado == null)
                                        return "boleto: Nenhum dado do documento foi recuperado.";
                                    else
                                        if (kit.certificado.titular.dadosComplementares.tipoPagamento.Equals("Ficha de Compensação"))
                                        return "boleto: Não foi encontrado boleto para o cliente.";
                                //Tasker 36408 - fim
                            }
                            else
                            {
                                if (kit.boleto.laminas.Where(x => x.parcela == -1).Count() > 0)
                                    return "boleto: Não foi encontrado linha digitável para os boletos.";
                                if (kit.boleto.dadosComplementares.endereco == null)
                                    return "boleto: Erro ao recuperar endereço.";

                                //Tasker 33110
                                if (kit.digital == "S")
                                {
                                    //Se o certificado for digital e não tiver telefone e nem e-mail não deixar gerar json                                    
                                    if ((kit.boleto.dadosComplementares.telefone == null) && (kit.boleto.dadosComplementares.email == null))
                                        return "boleto: Erro ao recuperar telefone e e-mail";
                                }
                                //Tasker 33110 fim
                            }
                            break;
                        case "CARTEIRINHA":
                            if (kit.carteirinha == null)
                                return "carteirinha: Nenhum dado do documento foi recuperado.";
                            if (kit.carteirinha.dadosComplementares.endereco == null)
                                return "carteirinha: Erro ao recuperar endereço.";

                            //Tasker 33110
                            if (kit.digital == "S")
                            {
                                //Se o certificado for digital e não tiver telefone e nem e-mail não deixar gerar json                                
                                if ((kit.carteirinha.dadosComplementares.telefone == null) && (kit.carteirinha.dadosComplementares.email == null))
                                    return "carteirinha: Erro ao recuperar telefone e e-mail";
                            }
                            //Tasker 33110 fim

                            break;
                        default:
                            return "Nenhum dado foi encontrado";
                    }
                }
                return "ok";
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void AddTB_ERRO_REGISTRO(int cd_regpss, int cd_err, string erro)
        {
            using (var context = new Conexao())
            {
                try
                {
                    var tbErroRegistro = new TB_ERRO_REGISTRO(cd_regpss, cd_err, erro);
                    context.Entry(tbErroRegistro).State = EntityState.Added;
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }
    }
}
