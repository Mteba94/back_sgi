using WebApi_SGI_T.Models;
using WebApi_SGI_T.Models.Commons.Request;
using WebApi_SGI_T.Models.Commons.Response;
using WebApi_SGI_T.Static;

namespace WebApi_SGI_T.Imp
{
    public class TipoSexoService
    {
        readonly SgiSacramentosContext _context;

        public TipoSexoService(SgiSacramentosContext context)
        {
            _context = context;
        }

        public BaseResponse<List<TipoSexoResponse>> ListSelectTipoSexo()
        {
            var response = new BaseResponse<List<TipoSexoResponse>>
            {
                Data = new List<TipoSexoResponse>()
            };

            try
            {
                var list = _context.TblSexos
                    .Where(x => x.SexoEstado == 1)
                    .Select(x => new TipoSexoResponse
                    {
                        SexoId = x.SexoId,
                        SxAbreviacion = x.SexoAbreviacion
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

        public async Task<BaseResponse<bool>> RegisterTipoSexo(TipoSexoRequest request)
        {
            var response = new BaseResponse<bool>();

            try
            {

                var maxId = 0;

                try
                {
                    maxId = _context.TblSexos.Max(x => x.SexoId) + 1;
                }
                catch (Exception)
                {
                    maxId = 1;
                }

                byte newId = byte.Parse(maxId.ToString());

                var tipoSexo = new TblSexo
                {
                    SexoId = newId,
                    SexoNombre = request.SexoNombre,
                    SexoAbreviacion = request.SexoAbreviacion,
                    SexoEstado = 1
                };

                _context.TblSexos.Add(tipoSexo);
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Message = ReplyMessage.MESSAGE_SAVE;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Message = ReplyMessage.MESSAGE_FAILED;
                response.IsSuccess = false;
            }

            return response;
        }
    }
}
