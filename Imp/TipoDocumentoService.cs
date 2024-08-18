using WebApi_SGI_T.Models;
using WebApi_SGI_T.Models.Commons.Response;
using WebApi_SGI_T.Static;

namespace WebApi_SGI_T.Imp
{
    public class TipoDocumentoService
    {
        readonly SgiSacramentosContext _context;
        public TipoDocumentoService(SgiSacramentosContext context)
        {
            _context = context;
        }

        public BaseResponse<List<TipoDocumentoResponse>> ListSelectTipoDocumento()
        {
            var response = new BaseResponse<List<TipoDocumentoResponse>>
            {
                Data = new List<TipoDocumentoResponse>()
            };

            try
            {
                var list = _context.TblTipoDocumentos
                    .Where(x => x.TdEstado == 1)
                    .Select(x => new TipoDocumentoResponse
                    {
                        TdIdTipoDocumento = x.TdIdTipoDocumento,
                        TdAbreviacion = x.TdAbreviacion
                    })
                    .ToList();

                response.Data = list;
                response.Message = ReplyMessage.MESSAGE_QUERY;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                response.IsSuccess = false;
            }

            return response;
        }
    }
}
