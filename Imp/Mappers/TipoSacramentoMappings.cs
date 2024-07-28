using AutoMapper;
using WebApi_SGI_T.Models;
using WebApi_SGI_T.Models.Commons.Request;

namespace WebApi_SGI_T.Imp.Mappers
{
    public class TipoSacramentoMappings : Profile
    {
        public TipoSacramentoMappings()
        {
            //CreateMap<TblTipoSacramento, TipoSacramentoRequest>();

            CreateMap<TipoSacramentoRequest, TblTipoSacramento>()
                .ReverseMap();

        }
    }
}
