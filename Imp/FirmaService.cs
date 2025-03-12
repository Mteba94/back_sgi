using WebApi_SGI_T.Models;
using WebApi_SGI_T.Models.Commons.Response;
using WebApi_SGI_T.Static;

namespace WebApi_SGI_T.Imp
{
    public class FirmaService
    {
        readonly SgiSacramentosContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FirmaService(SgiSacramentosContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public BaseResponse<List<SacerdoteResponse>> ListSelectFirma()
        {
            var response = new BaseResponse<List<SacerdoteResponse>>
            {
                Data = new List<SacerdoteResponse>()
            };

            try
            {
                var list = _context.TblSacerdotes
                    .Where(x => x.ScFirma == "Si")
                    .Select(x => new SacerdoteResponse
                    {
                        SacerdoteId = x.ScId,
                        SacerdoteNombre = x.ScNombre,
                        SacerdoteIdCategoria = x.ScIdCategoria,
                        SacerdoteCategoria = x.ScIdCategoriaNavigation.CsAbreviacion,
                        SacerdoteFirma = x.ScFirma,
                        SacerdoteEstado = x.ScEstado,
                        SacerdoteEstadoDesc = (x.ScEstado == 1 ? "Activo" : "Inactivo"),
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

        public async Task<BaseResponse<bool>> RegisterFirma(int id)
        {
            var response = new BaseResponse<bool>();

            try
            {
                // Eliminar todas las firmas existentes
                var firmasExistentes = _context.TblSacerdotes.Where(x => x.ScFirma == "Si").ToList();
                foreach (var firma in firmasExistentes)
                {
                    firma.ScFirma = "No";
                    _context.TblSacerdotes.Update(firma);
                }

                var nuevaFirma = _context.TblSacerdotes.FirstOrDefault(x => x.ScId == id);

                if (nuevaFirma == null)
                {
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    response.IsSuccess = false;
                    return response;
                }

                // Registrar la nueva firma
                nuevaFirma.ScFirma = "Si";
                _context.TblSacerdotes.Update(nuevaFirma);
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Message = ReplyMessage.MESSAGE_UPDATE;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Message = ReplyMessage.MESSAGE_FAILED;
                response.IsSuccess = false;
            }

            return response;
        }

        public async Task<BaseResponse<bool>> DeleteFirma(int id)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var firma = _context.TblSacerdotes.FirstOrDefault(x => x.ScId == id);

                if (firma == null)
                {
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    response.IsSuccess = false;
                    return response;
                }

                firma.ScFirma = "No";

                _context.TblSacerdotes.Update(firma);
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Message = ReplyMessage.MESSAGE_DELETE;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Message = ReplyMessage.MESSAGE_FAILED;
                response.IsSuccess = false;
                return response;
            }

            return response;
        }
    }
}
