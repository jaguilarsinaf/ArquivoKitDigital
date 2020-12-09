using AutoMapper;
using Dapper;
using Projeto.Domain;
using Projeto.Infra.Dao;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Projeto.Infra
{
    public class ProdutoDao
    {
        public IEnumerable<Kitdigital> BuscarCertificado(List<int> lstKit, string usuario, string nomeArquivo)
        {
            try
            {
                var dao = new ListaKitIndividualListaLogListaTotal();
                var retornoLogProcessamento = dao.AddLogProcessamento(usuario);
                var lista = new ConcurrentBag<Kitdigital>();
                var listaDto = new ConcurrentBag<Kitdigital>();
                Mapeamento.Instance();
                foreach (var tipoKit in lstKit)
                {
                    Console.WriteLine();
                    Console.Write("Processando KIT ");
                    Console.WriteLine(tipoKit);


                    using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["kitDigitalJson"].ConnectionString))
                    {
                        db.Open();
                        var range = db.Query<Controle2Via>("PR_BuscarControle2ViaArquivoDigital", new { tipoKit = tipoKit }, commandType: CommandType.StoredProcedure).ToList();
                        db.Close();
                        if (range.Count() > 0)
                        {
                            Console.WriteLine();
                            Console.Write("Quantidade de Certificados a Processar: ");
                            Console.WriteLine(range.Count());
                            try
                            {
                                Parallel.For(0, range.Count(), item =>
                                {
                                    Kitdigital kitDigital = new Kitdigital();
                                    kitDigital.externalKey = range[item].cdc2v;
                                    kitDigital.tipoKit = tipoKit;
                                    kitDigital.digital = range[item].iddig.TrimEnd();
                                    ItemOutRiscPess itemOutRiscPess = BuscarItemOutRiscPess(range[item].cdconseg, (int)range[item].cditeseg);
                                    Pessoa pessoa = BuscarPessoaKit((int)range[item].nrcer, range[item].cdconseg);
                                    EnderecoPessoa enderecoPessoa = BuscarEnderecoPessoaKit(pessoa.cdpes);
                                    Produto produto = BuscarProduto(range[item].cdconseg);
                                    Ramo ramo = BuscarRamo(produto.cdramosg);
                                    Emissao emissao = BuscarEmissao(range[item].cdconseg, range[item].cdemi);
                                    string orgaoProdutor = BuscarOrgaoProdutor((int)emissao.cdorgprtsuc).TrimEnd();
                                    var certificados = BuscarCertificados(tipoKit, range[item].cdconseg, range[item].iddig);
                                    if (certificados.Count() > 0)
                                    {
                                        foreach (var i in certificados)
                                        {
                                            switch (i.nomeDocumento)
                                            {
                                                case "CERTIFICADO":
                                                    kitDigital.certificado = new Certificado();
                                                    kitDigital.certificado.seguro = BuscarSeguro(ramo, emissao, produto, pessoa, orgaoProdutor, range[item].cdemi, (int)range[item].nrcer, range[item].cdconseg, (int)range[item].cditeseg);
                                                    kitDigital.certificado.titular = BuscarPrincipal(enderecoPessoa, emissao, produto, pessoa, itemOutRiscPess, range[item].cdconseg, (int)range[item].nrcer, (int)range[item].cditeseg);
                                                    kitDigital.certificado.dependentes = BuscarDependentes(range[item].cdconseg, (int)range[item].cditeseg);
                                                    kitDigital.certificado.beneficiarios = BuscarBeneficiarios(range[item].cdconseg, (int)range[item].cditeseg);
                                                    kitDigital.certificado.dataEmissao = BuscarDataEmissao().ToString();
                                                    kitDigital.certificado.processoSUSEP = BuscarProcessoSusep(produto.cdprodut);


                                                    kitDigital.certificado.titular.dadosComplementares.email = enderecoPessoa.nmemail;
                                                    kitDigital.certificado.titular.dadosComplementares.endereco = Mapper.Map<EnderecoPessoa, Endereco>(enderecoPessoa);
                                                    kitDigital.certificado.titular.dadosComplementares.telefone = Mapper.Map<IEnumerable<TelefonePessoa>, IEnumerable<Telefone>>(BuscarTelefone(enderecoPessoa.cdpes));
                                                    kitDigital.certificado.titular.dadosComplementares.tipoPagamento = RecuperarTipoPagamento(emissao.cdconseg, emissao.cdemi, pessoa.cdpes);


                                                    kitDigital.certificado.observacaoCarencia = "A carência para efeitos deste seguro, a contar da data de início de vigência, será de 120 (cento e vinte) dias. Não haverá carência para acidentes pessoais. A carência para Assistência Desemprego, quando inclusa no seguro, será de 180 (cento e oitenta) dias. Em caso de inclusão do cônjuge após o início de vigência do seguro deverá ser cumprida a carência estabelecida, a contar da data de inclusão do mesmo. A carência para Dependentes Agregados será contada a partir da data de início de vigência da cobertura do dependente.";
                                                    break;
                                                case "CARNÊ":
                                                    kitDigital.boleto = new Boleto();

                                                    kitDigital.boleto.dadosComplementares.email = enderecoPessoa.nmemail;
                                                    kitDigital.boleto.dadosComplementares.endereco = Mapper.Map<EnderecoPessoa, Endereco>(enderecoPessoa);
                                                    kitDigital.boleto.dadosComplementares.telefone = Mapper.Map<IEnumerable<TelefonePessoa>, IEnumerable<Telefone>>(BuscarTelefone(enderecoPessoa.cdpes));

                                                    if (kitDigital.tipoKit != 123)
                                                    {
                                                        kitDigital.boleto.dadosComplementares.tipoPagamento = RecuperarTipoPagamento(emissao.cdconseg, emissao.cdemi, pessoa.cdpes);
                                                    }

                                                    kitDigital.boleto.laminas = new List<Lamina>();
                                                    var lstkitCarne = BuscarKitCarne(range[item].cdc2v);

                                                    if (lstkitCarne.Count() > 0)
                                                    {
                                                        //Inclusão para Recuperar o Meio de Pagamento da Parcela da Teimosinha
                                                        if (kitDigital.tipoKit == 123)
                                                        {
                                                            kitDigital.boleto.dadosComplementares.tipoPagamento = RecuperarTipoPagamentoParcela(lstkitCarne[0].codigoContrato, lstkitCarne[0].codigoEmissao, lstkitCarne[0].numeroParcela);

                                                            //Caso não encontre o valor da parcela anterior, recupera do certificado
                                                            if (kitDigital.boleto.dadosComplementares.tipoPagamento == null)
                                                                kitDigital.boleto.dadosComplementares.tipoPagamento = RecuperarTipoPagamento(emissao.cdconseg, emissao.cdemi, pessoa.cdpes);
                                                        }

                                                        foreach (var kit in lstkitCarne)
                                                        {
                                                            ParcelaPremio pp = BuscarParcelaPremio(kit.codigoContrato, kit.codigoEmissao, kit.numeroParcela);

                                                            if (pp.tplqdparpre == 0) //Ficha de Compensação
                                                            {
                                                                if ((pp.stparpre != 4) && (pp.stparpre != 5) && (pp.stparpre != 12))
                                                                {
                                                                    Lamina lamina = Mapper.Map<Lamina>(BuscarTBDadosBoleto(kit.codigoContrato, kit.codigoEmissao, kit.numeroParcela));

                                                                    if (lamina != null)
                                                                    {
                                                                        lamina.nossoNumero = lamina.nossoNumero.Replace("-", "").Insert(lamina.nossoNumero.Count() - 2, "-");
                                                                        lamina = BuscarLamina(kit.codigoContrato, kit.codigoEmissao, kit.numeroParcela, (int)range[item].nrcer, emissao, lamina);
                                                                        lamina.vencimento = pp.dtven > pp.dtvenprg ? pp.dtven.ToString() : pp.dtvenprg.ToString();
                                                                        lamina.dataProcessamento = BuscarDataEmissao().ToString();
                                                                        lamina.dataDocumento = lamina.dataProcessamento;
                                                                        lamina.dataLimitePagamento = kitDigital.tipoKit != 123 ? Convert.ToDateTime(lamina.vencimento).AddDays(15).ToString() : Convert.ToDateTime(lamina.vencimento).ToString();
                                                                        lamina.carencia = BuscarCarencia(kit.codigoContrato, kit.codigoEmissao, kit.numeroParcela);
                                                                        kitDigital.boleto.laminas.Add(lamina);
                                                                    }
                                                                    else
                                                                    {
                                                                        Lamina laminaInvalida = new Lamina();
                                                                        laminaInvalida.parcela = -1;
                                                                        kitDigital.boleto.laminas.Add(laminaInvalida);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }

                                                    if (kitDigital.boleto.laminas.Count > 0)
                                                    {
                                                        kitDigital.boleto.certificado = (int)range[item].nrcer;
                                                        kitDigital.boleto.nome = pessoa.nmpes.TrimEnd();
                                                        kitDigital.boleto.cpf = pessoa.nrcgccpf.ToString().Count() > 11 ? Convert.ToUInt64(pessoa.nrcgccpf).ToString(@"00\.000\.000\/0000\-00").TrimEnd() : Convert.ToUInt64(pessoa.nrcgccpf).ToString(@"000\.000\.000\-00").TrimEnd();
                                                        kitDigital.boleto.apolice = emissao.cdorgprtsuc + "/" + ramo.cdramosg + "/" + emissao.nrapo;
                                                        kitDigital.boleto.dadosComplementares.endereco = Mapper.Map<Endereco>(enderecoPessoa);
                                                    }
                                                    else
                                                    {
                                                        //Não houve recuperação de nenhuma lâmina
                                                        kitDigital.boleto = null;
                                                    }
                                                    break;
                                                case "CARTEIRINHA":
                                                    kitDigital.carteirinha = new Carteirinha()
                                                    {
                                                        certificado = (int)range[item].nrcer,
                                                        contrato = range[item].cdconseg,
                                                        nome = pessoa.nmpes.TrimEnd()
                                                    };

                                                    kitDigital.carteirinha.dadosComplementares.email = enderecoPessoa.nmemail;
                                                    kitDigital.carteirinha.dadosComplementares.endereco = Mapper.Map<EnderecoPessoa, Endereco>(enderecoPessoa);
                                                    kitDigital.carteirinha.dadosComplementares.telefone = Mapper.Map<IEnumerable<TelefonePessoa>, IEnumerable<Telefone>>(BuscarTelefone(enderecoPessoa.cdpes));

                                                    break;
                                            }
                                        }
                                    }
                                    lista.Add(kitDigital);
                                });
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }
                }
                using (TransactionScope scope = new TransactionScope())
                {
                    listaDto = dao.AddControleImpKitItem(lstKit, usuario, lista, retornoLogProcessamento, nomeArquivo);
                    scope.Complete();
                    scope.Dispose();
                }
                return listaDto;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private Lamina BuscarLamina(int codigoContrato, int codigoEmissao, int numeroParcela, int nrcer, Emissao emissao, Lamina lamina)
        {
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["kitDigitalJson"].ConnectionString))
            {
                var item = db.Query<LaminaBoleto>("PR_BuscarLaminaKitDigital", new { codigoContrato = codigoContrato, nrcer = nrcer, numeroParcela = numeroParcela }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                if (item != null)
                {
                    lamina.agencia = item.agencia;
                    lamina.codigoBeneficiario = item.codBeneficiario.Insert(item.codBeneficiario.Count() - 1, "-");
                    item.banco = item.banco.TrimEnd();
                    if (item.banco.Equals("BRADESCO"))
                    {
                        lamina.numeroDocumento = emissao.cdorgprtsuc.ToString() + "/" + emissao.nrapo.ToString("00000");
                    }
                    else if (item.banco.Equals("CAIXA"))
                    {
                        lamina.numeroDocumento = nrcer.ToString();
                    }
                    else
                    {
                        lamina.numeroDocumento = emissao.cdorgprtsuc.ToString() + "/" + emissao.cdrmoseg.ToString() + "/" + emissao.nrapo.ToString("00000");
                    }
                }
                return lamina;
            }
        }

        private string BuscarProcessoSusep(int cdprodut)
        {
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["kitDigitalJson"].ConnectionString))
            {
                var dados = db.Query<string>("select a.cdprcsusep from VigCaractProduto a where a.cdprodut = @cdprodut and dtinivig = (select max(x.dtinivig) from VigCaractProduto x where a.cdprodut = x.cdprodut)", new { cdprodut = cdprodut }).FirstOrDefault();
                return dados.TrimEnd();
            }
        }

        private ItemOutRiscPess BuscarItemOutRiscPess(int cdconseg, int cditeseg)
        {
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["kitDigitalJson"].ConnectionString))
            {
                var dados = db.Query<ItemOutRiscPess>("select * from ItemOutRiscPess where cdconseg = @cdconseg and cditeseg = @cditeseg", new { cdconseg = cdconseg, cditeseg = cditeseg }).FirstOrDefault();
                return dados;
            }
        }

        private DateTime BuscarDataEmissao()
        {
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["kitDigitalJson"].ConnectionString))
            {
                var dados = db.Query<DateTime>("select dtdiariaatual from ControleDataSistema").FirstOrDefault();
                return dados;
            }
        }

        private string BuscarCarencia(int cdconseg, int cdemi, int cdparpre)
        {
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["kitDigitalJson"].ConnectionString))
            {
                var parcelaReal = db.Query<int>("select cdparprereal FROM ParcelaPremioReal WHERE cdconseg = @cdconseg and cdemi = @cdemi and cdparpre = @cdparpre", new { cdconseg = cdconseg, cdemi = cdemi, cdparpre = cdparpre }).FirstOrDefault();

                if ((parcelaReal == 2) || (parcelaReal == 3) || (parcelaReal == 4))
                    return "Sim"; //Em Carência
                return "Não"; //Fora de Carência
            }
        }


        private List<Dependente> BuscarDependentes(int cdconseg, int cditeseg)
        {
            var lista = new List<Dependente>();
            var lstItemOut = new List<ItemOutRiscPess>();
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["kitDigitalJson"].ConnectionString))
            {
                lstItemOut = db.Query<ItemOutRiscPess>("select * from ItemOutRiscPess where cdconsegagr = @cdconsegagr and cditesegagr = @cditesegagr and stsgr = @stsgr", new { cdconsegagr = cdconseg, cditesegagr = cditeseg, stsgr = 0 }).ToList();
            }
            if (lstItemOut.Count() > 0)
            {
                foreach (var item in lstItemOut)
                {
                    Dependente dp = new Dependente();
                    int cdemi = 0;
                    var lstMov = new List<MovCoberturaVidaDTO>();
                    using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["kitDigitalJson"].ConnectionString))
                    {
                        var dados = db.Query("PR_BuscarDepentendesKitDigital", new { cdconseg = cdconseg, cditeseg = item.cditeseg }, commandType: CommandType.StoredProcedure).FirstOrDefault();

                        cdemi = db.Query<int>("select cdemi from Certificado where cdconseg = @cdconseg and cditeseg = @cditeseg", new { cdconseg = cdconseg, cditeseg = item.cditeseg }).FirstOrDefault();

                        lstMov = db.Query<MovCoberturaVidaDTO>("PR_BuscarMovCoberturaVidaKitDigital", new { cdconseg = cdconseg, cditeseg = item.cditeseg }, commandType: CommandType.StoredProcedure).ToList();

                        string parentesco = Convert.ToString(dados.ds_sgncam);
                        string nome = Convert.ToString(dados.nmpes);
                        string dataNascimento = Convert.ToString(dados.dtnas);
                        string plano = Convert.ToString(dados.dsplnind);
                        dp.parentesco = parentesco.TrimEnd();
                        dp.nome = nome.TrimEnd();
                        dp.dataNascimento = dataNascimento.TrimEnd();
                        dp.plano = plano.TrimEnd();
                    }
                    if (lstMov.Count() > 0)
                    {
                        dp.coberturasContratadas = new List<CoberturasContratada>();
                        foreach (var i in lstMov)
                        {
                            dp.coberturasContratadas.Add(Mapper.Map<CoberturasContratada>(i));
                        }
                    }

                    var emissao = BuscarEmissao(cdconseg, cdemi);
                    dp.dataInclusao = emissao.dtinivig.ToString();
                    dp.inicioVigencia = emissao.dtinivig > emissao.dtfimvig.AddYears(-1) ? emissao.dtinivig.ToString() : emissao.dtfimvig.AddYears(-1).ToString();
                    dp.fimVigencia = emissao.dtfimvig.ToString();
                    lista.Add(dp);
                }
            }
            return lista;
        }

        private List<Beneficiario> BuscarBeneficiarios(int cdconseg, int cditeseg)
        {
            var lista = new List<Beneficiario>();
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["kitDigitalJson"].ConnectionString))
            {
                lista = db.Query<Beneficiario>("PR_BuscarBeneficiariosKitDigital", new { cdconseg = cdconseg, cditeseg = cditeseg }, commandType: CommandType.StoredProcedure).ToList();
            }
            return lista;
        }

        private Titular BuscarPrincipal(EnderecoPessoa enderecoPessoa, Emissao emissao, Produto produto, Pessoa pessoa, ItemOutRiscPess itemOutRiscPess, int cdconseg, int nrcer, int cditeseg)
        {
            string plano = null;
            var lstMov = new List<MovCoberturaVidaDTO>();
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["kitDigitalJson"].ConnectionString))
            {
                plano = db.Query<string>("select p.dsplnind from PlanoIndGrupo p inner join ItemOutRiscPess i on p.cdprodut = i.cdprodut and p.nrseqplnind = i.nrseqplnind and p.dtinivig = i.dtinivig " +
                                            "where i.cdconseg = @cdconseg and i.cditeseg = @cditeseg", new { cdconseg = cdconseg, cditeseg = cditeseg }).FirstOrDefault();

                lstMov = db.Query<MovCoberturaVidaDTO>("PR_BuscarMovCoberturaVida_DTO_KitDigital", new { cdconseg = cdconseg, cditeseg = cditeseg }, commandType: CommandType.StoredProcedure).ToList();
            }
            Titular principal = new Titular();
            principal.certificado = nrcer;
            principal.contrato = cdconseg;
            principal.proposta = emissao.nrppscor;
            principal.emissao = emissao.cdemi;
            principal.item = cditeseg;
            principal.nome = pessoa.nmpes.TrimEnd();
            principal.dataNascimento = pessoa.dtnas.ToString();
            principal.cpf = pessoa.nrcgccpf.ToString().Count() > 11 ? Convert.ToUInt64(pessoa.nrcgccpf).ToString(@"00\.000\.000\/0000\-00").TrimEnd() : Convert.ToUInt64(pessoa.nrcgccpf).ToString(@"000\.000\.000\-00").TrimEnd();
            principal.numeroSegurado = cdconseg + " " + emissao.nrppscor + " " + nrcer;
            principal.plano = string.IsNullOrEmpty(plano) ? null : plano.TrimEnd();
            principal.dadosComplementares.endereco = Mapper.Map<Endereco>(enderecoPessoa);
            principal.email = BuscarEmail(enderecoPessoa.cdpes, enderecoPessoa.nrseqend);
            principal.dadosComplementares.telefone = Mapper.Map<List<Telefone>>(BuscarTelefone(pessoa.cdpes));

            if (lstMov.Count() > 0)
            {
                principal.coberturasContratadas = new List<CoberturasContratada>();
                foreach (var item in lstMov)
                {
                    principal.coberturasContratadas.Add(Mapper.Map<CoberturasContratada>(item));
                }
            }

            //principal.dps = BuscarDPS(produto.cdprodut, cdconseg, emissao);
            return principal;
        }

        private List<TelefonePessoa> BuscarTelefone(int cdpes)
        {
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["kitDigitalJson"].ConnectionString))
            {
                List<TelefonePessoa> listaTelefone = db.Query<TelefonePessoa>("SELECT ISNULL (cdddd, 0) AS cdddd, ISNULL (nrtel, 0) AS nrtel, CASE WHEN idenvsms = 'S' THEN 'Sim' ELSE 'Não' END AS idenvsms, CASE WHEN idwpp = 'S' THEN 'Sim' ELSE 'Não' END AS idwpp FROM TelefonePessoa WHERE cdpes = @cdpes AND tptel = 2 AND (idValido = 'S' OR idValido IS NULL OR idValido = '') ORDER BY idenvsms DESC", new { cdpes = cdpes }).ToList();

                if (listaTelefone.Count() == 0)
                {
                    TelefonePessoa telefonePessoa = new TelefonePessoa
                    {
                        idenvsms = "Não",
                        idwpp = "Não"
                    };

                    listaTelefone.Add(telefonePessoa);
                }

                return listaTelefone;
            }
        }

        private string BuscarEmail(int cdpes, int nrseqend)
        {
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["kitDigitalJson"].ConnectionString))
            {
                string email = db.Query<string>("select nmemail from EnderecoPessoa where cdpes = @cdpes and nrseqend = @nrseqend and (idEmailValido='S' OR idEmailValido IS NULL)", new { cdpes = cdpes, nrseqend = nrseqend }).FirstOrDefault();
                return string.IsNullOrEmpty(email) ? null : email.TrimEnd();
            }
        }

        private int VerificaExisteConjuge(int cdconseg, int cdemi)
        {
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["kitDigitalJson"].ConnectionString))
            {
                return db.Query<int>("SELECT COUNT(1) FROM Certificado c INNER JOIN Emissao e ON c.cdconseg = e.cdconseg AND c.cdemi = e.cdemi INNER JOIN ItemOutRiscPess iorp   ON iorp.cdconsegagr = c.cdconseg AND iorp.cditesegagr = c.cditeseg   WHERE iorp.stsgr = 0 AND iorp.tpsegagr = 1 AND e.cdconseg = @cdconseg AND e.cdemi = @cdemi", new { cdconseg = cdconseg, cdemi = cdemi }).FirstOrDefault();                
            }
        }


        private List<DPS> BuscarDPS(int codigoProduto, int cdconseg, Emissao emissao)
        {
            var lista = new List<DPS>();
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["kitDigitalJson"].ConnectionString))
            {
                var listaOrdemPerguntaDps = db.Query<OrdemPerguntaDps>("PR_BuscarOrdemPerguntaDpsKitDigital", new { codigoProduto = codigoProduto, dtcotpps = emissao.dtcotpps }, commandType: CommandType.StoredProcedure).ToList();

                if (listaOrdemPerguntaDps.Count() > 0)
                {
                    var listaResposta = db.Query<PerguntaRespostaDPS>("PR_BuscarPerguntaRespostaDpsKitDigital", new { codigoProduto = codigoProduto, dtcotpps = emissao.dtcotpps, cdconseg = cdconseg, cdemi = emissao.cdemi }, commandType: CommandType.StoredProcedure).ToList();
                    var listaPrincipal = listaOrdemPerguntaDps.Where(x => x.tipoSegurado.Equals("P")).OrderBy(x => x.numeroPergunta).ToList();
                    var listaConjuge = listaOrdemPerguntaDps.Where(x => x.tipoSegurado.Equals("C")).OrderBy(x => x.numeroPergunta).ToList();

                    if (listaPrincipal.Count() > 0)
                    {
                        DPS dps = new DPS();
                        if (listaResposta.Count() > 0)
                        {
                            dps.perguntaRespostaDPS = new List<PerguntaRespostaDPS>();
                            foreach (var i in listaPrincipal)
                            {
                                PerguntaRespostaDPS perguntaRespostaDPS = listaResposta.Where(x => x.codigoPergunta == i.codigoPergunta).Select(x => new PerguntaRespostaDPS() { resposta = x.resposta.TrimEnd(), codigoPergunta = i.numeroPergunta }).FirstOrDefault();
                                if (perguntaRespostaDPS != null)
                                {
                                    dps.tipoSegurado = "Principal";
                                    dps.perguntaRespostaDPS.Add(perguntaRespostaDPS);
                                }
                            }
                        }

                        lista.Add(dps);
                    }

                    //Verifica se o Titular tem Conjuge, senão não vem DPS
                    if (VerificaExisteConjuge(cdconseg, emissao.cdemi) > 0)
                    {
                        if (listaConjuge.Count() > 0)
                        {
                            DPS dps = new DPS();
                            if (listaResposta.Count() > 0)
                            {
                                dps.perguntaRespostaDPS = new List<PerguntaRespostaDPS>();
                                foreach (var i in listaConjuge)
                                {
                                    PerguntaRespostaDPS perguntaRespostaDPS = listaResposta.Where(x => x.codigoPergunta == i.codigoPergunta).Select(x => new PerguntaRespostaDPS() { resposta = x.resposta.TrimEnd(), codigoPergunta = i.numeroPergunta }).FirstOrDefault();
                                    if (perguntaRespostaDPS != null)
                                    {
                                        dps.tipoSegurado = "Cônjuge";
                                        dps.perguntaRespostaDPS.Add(perguntaRespostaDPS);
                                    }
                                }
                            }

                            lista.Add(dps);
                        }
                    }
                }
            }
            return lista;
        }

        private decimal BuscarValorPremioLiquido(int cdconseg, int cditeseg)
        {
            try
            {
                var lstMov = new List<MovCoberturaVidaDTO>();
                using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["kitDigitalJson"].ConnectionString))
                {
                    lstMov = db.Query<MovCoberturaVidaDTO>("PR_BuscarMovCoberturaVida_DTO_KitDigital", new { cdconseg = cdconseg, cditeseg = cditeseg }, commandType: CommandType.StoredProcedure).ToList();
                }

                float listaDependentesSoma = 0;

                var buscarDependente = BuscarDependentes(cdconseg, cditeseg);

                if (buscarDependente != null)
                {
                    if (buscarDependente.Any(x => x.coberturasContratadas == null))
                    {
                        return 0;
                    }
                    else
                    {
                        listaDependentesSoma = buscarDependente
                            .Sum(x => x.coberturasContratadas.Sum(y => (float)Convert.ToDouble(y.valorPremio)));
                    }
                }

                if (lstMov.Count() > 0)
                    return lstMov.Sum(x => x.vlsldpreliq) + (decimal)listaDependentesSoma;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private decimal BuscarValorIOF(int cdconseg, int cditeseg)
        {
            try
            {
                decimal valorIOF = 0;

                using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["kitDigitalJson"].ConnectionString))
                {
                    valorIOF = db.Query<decimal>("PR_BuscarValorIOF", new { cdconseg = cdconseg, cditeseg = cditeseg }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }

                return valorIOF;
            }
            catch (Exception e)
            {
                throw e;
            }
           
        }

        private string RecuperarTipoPagamento(int contrato, int emissao, int codigoPessoa)
        {
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["kitDigitalJson"].ConnectionString))
            {
                return db.Query<string>("select t.ds_sgncam from CorrentDevedor c " +
                                        "inner join TB_DOMINIO_CAMPO t on t.nm_cam = 'tpmeicba' " +
                                        "and t.nm_tab = 'CorrentDevedor' and t.ds_vlrdmn = c.tpmeicba " +
                                        "where c.cdconseg = @cdconseg and c.cdemi = @cdemi and c.cdpes = @cdpes", 
                                        new {
                                                 cdconseg = contrato,
                                                 cdemi = emissao,
                                                 cdpes = codigoPessoa
                                             }).FirstOrDefault();
            }
        }

        private string RecuperarTipoPagamentoParcela(int contrato, int emissao, int numeroParcela)
        {
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["kitDigitalJson"].ConnectionString))
            {
                return db.Query<string>(" select top 1 t.ds_sgncam from HistDadosBancariosParcela a " +
                                        " inner join TB_DOMINIO_CAMPO t on t.nm_cam = 'tpmeicba' and t.nm_tab = 'CorrentDevedor' and t.ds_vlrdmn = a.tpmeicba " +
                                        " where a.cdconseg = @cdconseg and a.cdemi = @cdemi and a.cdparpre = @cdparpre " +
                                        " and a.nrseq < (select max(b.nrseq) from HistDadosBancariosParcela b where a.cdconseg = b.cdconseg and a.cdemi = b.cdemi and b.tpreg = 4) " +
                                        " order by a.nrseq desc ", 
                                        new
                                        {
                                            cdconseg = contrato,
                                            cdemi = emissao,
                                            cdparpre = numeroParcela
                                        }).FirstOrDefault();
            }
        }

        private Seguro BuscarSeguro(Ramo ramo, Emissao emissao, Produto produto, Pessoa pessoa, string orgaoProdutor, int cdemi, int nrcer, int cdconseg, int cditeseg)
        {
            Pessoa pes = new Pessoa();
            List<TelefonePessoa> telefonePessoa = null;
            string tipoPagamento = null;
            var corretor = new Corretor();
            string periodicidade = null;

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["kitDigitalJson"].ConnectionString))
            {
                pes = db.Query<Pessoa>("select * from Pessoa where cdpes = (select cdpes from ContratoSeguro where cdconseg = @cdconseg)", new { cdconseg = emissao.cdconseg }).FirstOrDefault();

                telefonePessoa = db.Query<TelefonePessoa>("select * from TelefonePessoa where cdpes = @cdpes and (idValido='S' OR idValido IS NULL OR idValido = '')", new { cdpes = pes.cdpes }).ToList();

                tipoPagamento = RecuperarTipoPagamento(emissao.cdconseg, emissao.cdemi, pessoa.cdpes);

                corretor = db.Query<Corretor>("PR_BuscarCorretorKitDigital", new { cdconseg = emissao.cdconseg , cdemi = emissao.cdemi }, commandType: CommandType.StoredProcedure).FirstOrDefault();

                periodicidade = db.Query<string>("select ds_sgncam from TB_DOMINIO_CAMPO where nm_cam = 'tpfreqpl' and nm_tab = 'Certificado' and ds_vlrdmn = (select tpfreqpl from Certificado where cdconseg = @cdconseg and cdemi = @cdemi and nrcer = @nrcer)", new { cdconseg = cdconseg, cdemi = cdemi, nrcer = nrcer }).FirstOrDefault();
            }

            Seguro seguro = new Seguro();
            seguro.codigoRamo = ramo.cdramosg.ToString().PadLeft(4, '0');
            seguro.ramo = ramo.nmramseg.TrimEnd();
            seguro.sucursal = orgaoProdutor.TrimEnd();
            seguro.produto = produto.nmprodut.TrimEnd();
            seguro.inicioVigencia = emissao.dtinivig > emissao.dtfimvig.AddYears(-1) ? emissao.dtinivig.ToString() : emissao.dtfimvig.AddYears(-1).ToString();
            seguro.fimVigencia = emissao.dtfimvig.ToString();
            seguro.ingressoApolice = emissao.dtbasecalc.ToString();
            seguro.apolice = emissao.cdorgprtsuc + "/" + ramo.cdramosg + "/" + emissao.nrapo;
            seguro.proLabore = "0,00";
            seguro.tipoPagamento = string.IsNullOrEmpty(tipoPagamento) ? null : tipoPagamento.TrimEnd();
            if (corretor != null)
            {
                seguro.corretor = string.IsNullOrEmpty(corretor.nmpes) ? null : corretor.nmpes.TrimEnd();
                seguro.codigoSUSEP = string.IsNullOrEmpty(corretor.cdregsus) ? null : corretor.cdregsus.TrimEnd();
            }
            seguro.periodicidade = string.IsNullOrEmpty(periodicidade) ? null : periodicidade.TrimEnd();
            seguro.vencimento = emissao.dtinivig.Day.ToString();

            decimal valorPremioLiquido = BuscarValorPremioLiquido(cdconseg, cditeseg);
            seguro.valorPremioLiquido = valorPremioLiquido.ToString("N");
            seguro.adicionalFracionamento = "0,00";
            seguro.custoApolice = "0,00";

            decimal valorIOF = BuscarValorIOF(cdconseg, cditeseg);
            seguro.valorIOF = valorIOF.ToString("N");

            decimal valorPremioTotal = valorPremioLiquido + valorIOF;
            seguro.valorPremioTotal = valorPremioTotal.ToString("N");

            seguro.estipulante = new Estipulante();
            seguro.estipulante.nome = pes.nmpes.TrimEnd();
            seguro.estipulante.cnpj = pes.nrcgccpf.ToString().Count() > 11 ? Convert.ToUInt64(pes.nrcgccpf).ToString(@"00\.000\.000\/0000\-00").TrimEnd() : Convert.ToUInt64(pes.nrcgccpf).ToString(@"000\.000\.000\-00").TrimEnd();

            seguro.estipulante.dadosComplementares.email = BuscarEnderecoPessoaKit((int)pes.cdpes).nmemail;
            //seguro.estipulante.dadosComplementares.telefone =  Mapper.Map<List<TelefonePessoa>, List<Telefone>>(telefonePessoa);
            seguro.estipulante.dadosComplementares.telefone = Mapper.Map<List<Telefone>>(BuscarTelefone(pes.cdpes));
            seguro.estipulante.dadosComplementares.endereco = Mapper.Map<EnderecoPessoa, Endereco>(BuscarEnderecoPessoaKit((int)pes.cdpes));

            seguro.dps = BuscarDPS(produto.cdprodut, cdconseg, emissao);

            return seguro;
        }

        private TB_DADOS_BOLETO BuscarTBDadosBoleto(int codigoContrato, int codigoEmissao, int numeroParcela)
        {
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["kitDigitalJson"].ConnectionString))
            {
                var dados = db.Query<TB_DADOS_BOLETO>("select top 1 * from TB_DADOS_BOLETO where cd_ctt = @cdconseg and cd_ems = @cdemi and nr_pclpmo = @cdparpre order by cd_seq desc", new { cdconseg = codigoContrato, cdemi = codigoEmissao, cdparpre = numeroParcela }).FirstOrDefault();
                return dados;
            }
        }

        private ParcelaPremio BuscarParcelaPremio(int codigoContrato, int codigoEmissao, int numeroParcela)
        {
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["kitDigitalJson"].ConnectionString))
            {
                var parcelaPremio = db.Query<ParcelaPremio>("select * from ParcelaPremio where cdconseg = @cdconseg and cdemi = @cdemi and cdparpre = @cdparpre", new { cdconseg = codigoContrato, cdemi = codigoEmissao, cdparpre = numeroParcela }).FirstOrDefault();
                return parcelaPremio;
            }
        }

        private List<KitCarne> BuscarKitCarne(int cdc2v)
        {
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["kitDigitalJson"].ConnectionString))
            {
                var lstKitCarne = db.Query<KitCarne>("select * from KitCarne where idControle2Via = @cdc2v", new { cdc2v = cdc2v }).ToList();
                return lstKitCarne;
            }
        }

        private string BuscarOrgaoProdutor(int cdorgprtsuc)
        {
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["kitDigitalJson"].ConnectionString))
            {
                var orgaoProdutor = db.Query<string>("select nmorgprt from OrgaoProdutor where cdorgprt = @cdorgprt", new { cdorgprt = cdorgprtsuc }).FirstOrDefault();
                return orgaoProdutor;
            }
        }

        private Emissao BuscarEmissao(int cdconseg, int cdemi)
        {
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["kitDigitalJson"].ConnectionString))
            {
                var emissao = db.Query<Emissao>("select * from Emissao where cdconseg = @cdconseg and cdemi = @cdemi", new { cdconseg = cdconseg, cdemi = cdemi }).FirstOrDefault();
                return emissao;
            }
        }

        private Produto BuscarProduto(int cdconseg)
        {
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["kitDigitalJson"].ConnectionString))
            {
                var produto = db.Query<Produto>("select * from Produto where cdprodut = (select cdpro from ContratoSeguro where cdconseg = @cdconseg)", new { cdconseg = cdconseg }).FirstOrDefault();
                return produto;
            }
        }

        private Ramo BuscarRamo(int cdramosg)
        {
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["kitDigitalJson"].ConnectionString))
            {
                var ramo = db.Query<Ramo>("select * from Ramo where cdramosg = @cdramosg", new { cdramosg = cdramosg }).FirstOrDefault();
                return ramo;
            }
        }

        private IEnumerable<dynamic> BuscarCertificados(int tipoKit, int cdconseg, string digital)
        {
            try
            {
                using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["kitDigitalJson"].ConnectionString))
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@digital", digital);
                    queryParameters.Add("@tipoKit", tipoKit);
                    queryParameters.Add("@cdconseg", cdconseg);
                    var controle2Via = db.Query("PR_BuscarNomeDocumentoKitDigital", queryParameters, commandType: CommandType.StoredProcedure);
                    return controle2Via;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private Pessoa BuscarPessoaKit(int nrcer, int cdconseg)
        {
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["kitDigitalJson"].ConnectionString))
            {
                var pessoa = db.Query<Pessoa>("PR_BuscarPessoaKitDigital", new { @nrcer = nrcer, cdconseg = cdconseg }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                return pessoa;
            }
        }

        private EnderecoPessoa BuscarEnderecoPessoaKit(int pn_cdpes)
        {
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["kitDigitalJson"].ConnectionString))
            {
                var enderecoPessoa = db.Query<EnderecoPessoa>("PR_BuscarEnderecoPessoaKitDigital", new { @pn_cdpes = pn_cdpes }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                return enderecoPessoa;
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

    }
}
