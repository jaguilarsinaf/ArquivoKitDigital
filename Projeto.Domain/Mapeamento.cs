using AutoMapper;
using Projeto.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain
{
    public sealed class Mapeamento
    {
        private static Mapeamento instance;

        public static Mapeamento Instance()
        {
            lock (typeof(Mapeamento))
                if (instance == null) instance = new Mapeamento();

            return instance;
        }

        public Mapeamento()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<EnderecoPessoa, Endereco>()
                 .ForMember(dest => dest.bairro, opt => opt.MapFrom(src => src.nmbairro.TrimEnd()))
                 .ForMember(dest => dest.cep, opt => opt.MapFrom(src => src.cdceppes))
                 .ForMember(dest => dest.complemento, opt => opt.MapFrom(src => src.nmcompl.TrimEnd()))
                 .ForMember(dest => dest.logradouro, opt => opt.MapFrom(src => src.nmendpes.TrimEnd()))
                 .ForMember(dest => dest.municipio, opt => opt.MapFrom(src => src.nmcidpes.TrimEnd()))
                 .ForMember(dest => dest.numero, opt => opt.MapFrom(src => src.nrendpes))
                 .ForMember(dest => dest.uf, opt => opt.MapFrom(src => src.sguf.TrimEnd()));
                 

                config.CreateMap<EnderecoPessoa, DadosComplementares>()
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.nmemail));

                config.CreateMap<TelefonePessoa, DadosComplementares>()
                .ForMember(dest => dest.telefone, opt => opt.MapFrom(src => src.cdddd.Trim() + src.nrtel.TrimEnd()));

                config.CreateMap<TB_DADOS_BOLETO, Lamina>()
                 .ForMember(dest => dest.parcela, opt => opt.MapFrom(src => src.nr_pclpmo))
                 .ForMember(dest => dest.linhaDigitavel, opt => opt.MapFrom(src => src.cd_lindig.TrimEnd()))
                 .ForMember(dest => dest.codigoBarras, opt => opt.MapFrom(src => src.cd_bar.TrimEnd()))
                 .ForMember(dest => dest.valor, opt => opt.MapFrom(src => src.vl_pretot.ToString("N")))
                 .ForMember(dest => dest.nossoNumero, opt => opt.MapFrom(src => src.nr_nosnum.ToString("000000000000000").Insert(0, "14/").Insert(src.nr_nosnum.ToString().Count() - 1, "-")));

                config.CreateMap<MovCoberturaVidaDTO, CoberturasContratada>()
                 .ForMember(dest => dest.cobertura, opt => opt.MapFrom(src => src.nmcobert.TrimEnd()))
                 .ForMember(dest => dest.valorCapital, opt => opt.MapFrom(src => src.vlsldis.ToString("N")))
                 .ForMember(dest => dest.valorPremio, opt => opt.MapFrom(src => src.vlsldpreliq.ToString("N")));

                config.CreateMap<Kitdigital, KitDigitalDto>();
                config.CreateMap<KitDigitalDto, Kitdigital>();

                config.CreateMap<Controle2Via, ControleImpKitItemDto>();

                config.CreateMap<TelefonePessoa, Telefone>()
                //.ForMember(dest => dest.ddd, opt => opt.MapFrom(src => src.cdddd.TrimEnd()))
                .ForMember(dest => dest.telefone, opt => opt.MapFrom(src => src.cdddd.Trim() + src.nrtel.TrimEnd()))
                .ForMember(dest => dest.envioSMS, opt => opt.MapFrom(src => src.idenvsms.TrimEnd()))
                .ForMember(dest => dest.envioWhattsApp, opt => opt.MapFrom(src => src.idwpp.TrimEnd()));

                //config.CreateMap<Controle2Via, KitDigitalDto>()
                //.ForMember(dest => dest.certificado.principal.contrato, opt => opt.MapFrom(src => src.cdconseg))
                //.ForMember(dest => dest.certificado.principal.emissao, opt => opt.MapFrom(src => src.cdemi))
                //.ForMember(dest => dest.certificado.principal.item, opt => opt.MapFrom(src => src.cditeseg))
                //.ForMember(dest => dest.certificado.principal.certificado, opt => opt.MapFrom(src => src.nrcer))
                //.ForMember(dest => dest.codigoControle, opt => opt.MapFrom(src => src.cdc2v));

            });
        }
        
    }
}
