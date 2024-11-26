using Microsoft.EntityFrameworkCore;
using WebApi_SGI_T.Models;
using WebApi_SGI_T.Models.Commons.Response;
using WebApi_SGI_T.Static;

namespace WebApi_SGI_T.Imp
{
    public class UserRolService
    {
        readonly SgiSacramentosContext _context;

        public UserRolService(SgiSacramentosContext context)
        {
            _context = context;
        }

        public async Task<List<TblRol>> GetRolesByUserIdAsync(int userId)
        {
            var userRoles = await (from ur in _context.TblUserRols
                                   join r in _context.TblRols on ur.UrIdRol equals r.RoIdRol
                                   where ur.UrIdUsuario == userId
                                   select r).ToListAsync();

            return userRoles;
        }

        public async Task<BaseResponse<TblUserRol>> GetUserRolById(int userId)
        {
            var response = new BaseResponse<TblUserRol>
            {
                Data = new TblUserRol()
            };

            try
            {
                
            }
            catch(Exception ex)
            {

            }

            return response;

        }

        public async Task<BaseResponse<bool>> AssignRoleToUser(int userId, int roleId)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var maxId = 0;

                try
                {
                    maxId = _context.TblUserRols.Max(x => x.UrIdUserRol) + 1;
                }
                catch (Exception)
                {
                    maxId = 1;
                }

                byte newId = byte.Parse(maxId.ToString());

                var userRol = new TblUserRol
                {
                    UrIdUserRol = newId,
                    UrIdUsuario = userId,
                    UrIdRol = roleId,
                    UrEstado = 1
                };

                _context.TblUserRols.Add(userRol);
                await _context.SaveChangesAsync();

                response.Data = true;
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

        public async Task<BaseResponse<bool>> DeleteUserRol(int id)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var userRol = await _context.TblUserRols.FindAsync(id);

                if (userRol == null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                _context.TblUserRols.Remove(userRol);
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Message = ReplyMessage.MESSAGE_DELETE;
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
