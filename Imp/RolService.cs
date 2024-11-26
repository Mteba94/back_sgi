using Microsoft.EntityFrameworkCore;
using WebApi_SGI_T.Models;
using WebApi_SGI_T.Models.Commons.Request;
using WebApi_SGI_T.Models.Commons.Response;
using WebApi_SGI_T.Static;

namespace WebApi_SGI_T.Imp
{
    public class RolService
    {
        readonly SgiSacramentosContext _context;

        public RolService(SgiSacramentosContext context)
        {
            _context = context;
        }

        public BaseResponse<List<RolResponse>> ListSelectRol()
        {
            var response = new BaseResponse<List<RolResponse>>
            {
                Data = new List<RolResponse>()
            };

            try
            {
                var list = _context.TblRols
                    .Where(x => x.RoEstado == 1)
                    .Select(TblRol => new RolResponse
                    {
                        RoIdRol = TblRol.RoIdRol,
                        RoNombre = TblRol.RoNombre,
                        RoDescripcion = TblRol.RoDescripcion,
                        RoEstado = TblRol.RoEstado
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

        public async Task<BaseResponse<RolResponse>> GetRolById(int id)
        {
            var response = new BaseResponse<RolResponse>();

            try
            {
                var rol = await _context.TblRols
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.RoIdRol == id);

                if (rol != null)
                {
                    var mappedData = new RolResponse
                    {
                        RoIdRol = rol.RoIdRol,
                        RoNombre = rol.RoNombre,
                        RoDescripcion = rol.RoDescripcion,
                        RoEstado = rol.RoEstado
                    };

                    response.Data = mappedData;
                    response.Message = ReplyMessage.MESSAGE_QUERY;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                response.IsSuccess = false;
            }

            return response;
        }

        public async Task<BaseResponse<bool>> RegisterRol(RolRequest request)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var maxId = 0;

                try
                {
                    maxId = _context.TblRols.Max(x => x.RoIdRol) + 1;
                }
                catch (Exception)
                {
                    maxId = 1;
                }


                byte newId = byte.Parse(maxId.ToString());

                var newRol = new TblRol
                {
                    RoIdRol = newId,
                    RoNombre = request.RoNombre!,
                    RoDescripcion = request.RoDescripcion!,
                    RoEstado = 1
                };

                _context.TblRols.Add(newRol);
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

        public async Task<BaseResponse<bool>> UpdateRol(int RoId, RolRequest request)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var rol = await _context.TblRols
                    .FirstOrDefaultAsync(x => x.RoIdRol == RoId);

                if (rol != null)
                {
                    rol.RoNombre = request.RoNombre!;
                    rol.RoDescripcion = request.RoDescripcion!;

                    _context.TblRols.Update(rol);
                    await _context.SaveChangesAsync();

                    response.Data = true;
                    response.Message = ReplyMessage.MESSAGE_UPDATE;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                response.Message = ReplyMessage.MESSAGE_FAILED;
                response.IsSuccess = false;
            }

            return response;
        }

        public async Task<BaseResponse<bool>> DeleteRol(int RoId)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var rol = await _context.TblRols
                    .FirstOrDefaultAsync(x => x.RoIdRol == RoId);

                if (rol != null)
                {
                    rol.RoEstado = 0;

                    _context.TblRols.Update(rol);
                    await _context.SaveChangesAsync();

                    response.Data = true;
                    response.Message = ReplyMessage.MESSAGE_DELETE;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    response.IsSuccess = false;
                }
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
