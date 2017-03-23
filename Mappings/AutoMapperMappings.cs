
using AutoMapper;

namespace fangraph_priceguide_generator
{
    public class AutoMapperMappings
    {
        public static void Setup() {
            Mapper.Initialize(cfg =>
                cfg.CreateMap<MasterConversionRecord, ConversionRecord>()
                    .ForMember(dest => dest.mlbamID, opt => opt.MapFrom(src => src.mlb_id))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.mlb_name))
                    .ForMember(dest => dest.BDB, opt => opt.MapFrom(src => src.bref_id))
                    .ForMember(dest => dest.statsID, opt => opt.MapFrom(src => src.nfbc_id))
                    .ForMember(dest => dest.cbsID, opt => opt.MapFrom(src => src.cbs_id))
                    .ForMember(dest => dest.espnID, opt => opt.MapFrom(src => src.espn_id))
                    .ForMember(dest => dest.retroID, opt => opt.MapFrom(src => src.retro_id))
                    .ForMember(dest => dest.Pos_Class, opt => opt.MapFrom(src => src.yahoo_pos.Replace("/", "|")))
                    .ForMember(dest => dest.bisID, opt => opt.MapFrom(src => src.fg_id))
                    .ForMember(dest => dest.team, opt => opt.MapFrom(src => src.mlb_team))
            );            
        }
    }
}