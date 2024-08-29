using WebApi_SGI_T.Models;
using WebApi_SGI_T.Models.Commons.Request;
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

        public async Task<BaseResponse<bool>> RegisterTipoDocumento(TipoDocumentoRequest request)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var maxId = 0;

                try
                {
                    maxId = _context.TblTipoDocumentos.Max(x => x.TdIdTipoDocumento) + 1;
                }
                catch (Exception)
                {
                    maxId = 1;
                }


                byte newId = byte.Parse(maxId.ToString());

                var tipoDocumento = new TblTipoDocumento
                {
                    TdIdTipoDocumento = newId,
                    TdNombre = request.TdNombre,
                    TdAbreviacion = request.TdAbreviacion,
                    TdEstado = 1
                };

                _context.TblTipoDocumentos.Add(tipoDocumento);
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Message = ReplyMessage.MESSAGE_SAVE;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Message = ReplyMessage.MESSAGE_FAILED;
                response.IsSuccess = false;
            }

            return response;
        }
    }
}
