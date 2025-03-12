using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApi_SGI_T.Models;
using WebApi_SGI_T.Models.Commons.Request;
using WebApi_SGI_T.Models.Commons.Response;
using WebApi_SGI_T.Static;
using WebApi_SGI_T.Imp;
using System.Security.Claims;

namespace WebApi_SGI_T.Imp.Matrimonio
{
    public class matrimonioDeleteService
    {
        private readonly SgiSacramentosContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MatrimonioById _matrimonioById;
        public matrimonioDeleteService(SgiSacramentosContext context, IHttpContextAccessor httpContextAccessor, MatrimonioById matrimonioById)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _matrimonioById = matrimonioById;
        }

        public async Task<BaseResponse<bool>> DeleteMatrimonio(int sacramentoId, MatrimonioRequest request)
        {
            var response = new BaseResponse<bool>();

            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = _context.Database.GetDbConnection().ConnectionString;

                response = await DeleteMatrimonio(sacramentoId, request, con);
                con.Close();
            }catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_FAILED;
            }

            return response;
        }

        private async Task<BaseResponse<bool>> DeleteMatrimonio(int sacramentoId, MatrimonioRequest request, SqlConnection con)
        {
            var response = new BaseResponse<bool>();

            SqlCommand cmd;
            SqlParameter param = new SqlParameter();
            DataSet ds = new DataSet();
            SqlDataReader dr;
            try
            {
                cmd = new SqlCommand();
                cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_control_sacramento_matrimonio";

                //var matrimonio = await GetMatrimonioById(sacramentoId);
                var matrimonio = await _matrimonioById.GetMatrimonioById(sacramentoId);

                if (matrimonio.Data != null)
                {
                    var deleteUser = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                    var deleteDate = DateTime.Now;

                    cmd.Parameters.Add(new SqlParameter("@i_operacion", "EME"));
                    cmd.Parameters.Add(new SqlParameter("@i_id_sacramento", request.ScIdTipoSacramento));
                    cmd.Parameters.Add(new SqlParameter("@i_numDocto_esposo", request.PeNumeroDocumentoEsposo));
                    cmd.Parameters.Add(new SqlParameter("@i_numDocto_esposa", request.PeNumeroDocumentoEsposa));
                    cmd.Parameters.Add(new SqlParameter("@i_fechaSacramento", request.ScFechaSacramento));
                    cmd.Parameters.Add(new SqlParameter("@i_numpartida", request.ScNumeroPartida));
                    cmd.Parameters.Add(new SqlParameter("@i_padre_esposo", request.ScPadreEsposo));
                    cmd.Parameters.Add(new SqlParameter("@i_padre_esposa", request.ScPadreEsposa));
                    cmd.Parameters.Add(new SqlParameter("@i_madre_esposo", request.ScMadreEsposo));
                    cmd.Parameters.Add(new SqlParameter("@i_madre_esposa", request.ScMadreEsposa));
                    cmd.Parameters.Add(new SqlParameter("@i_sexo_esposo", request.PeSexoIdEsposo));
                    cmd.Parameters.Add(new SqlParameter("@i_sexo_esposa", request.PeSexoIdEsposa));
                    cmd.Parameters.Add(new SqlParameter("@i_testigo1", request.ScTestigo1));
                    cmd.Parameters.Add(new SqlParameter("@i_testigo2", request.ScTestigo2));
                    cmd.Parameters.Add(new SqlParameter("@i_parroco", request.ScParroco));
                    cmd.Parameters.Add(new SqlParameter("@i_nombre_esposo", request.PeNombreEsposo));
                    cmd.Parameters.Add(new SqlParameter("@i_nombre_esposa", request.PeNombreEsposa));
                    cmd.Parameters.Add(new SqlParameter("@i_edad_esposo", request.PeEdadEsposo));
                    cmd.Parameters.Add(new SqlParameter("@i_edad_esposa", request.PeEdadEsposa));
                    cmd.Parameters.Add(new SqlParameter("@i_fechaNacimiento_esposo", request.PeFechaNacimientoEsposo));
                    cmd.Parameters.Add(new SqlParameter("@i_fechaNacimiento_esposa", request.PeFechaNacimientoEsposa));
                    cmd.Parameters.Add(new SqlParameter("@i_tipoDoc_esposo", request.PeIdTipoDocumentoEsposo));
                    cmd.Parameters.Add(new SqlParameter("@i_tipoDoc_esposa", request.PeIdTipoDocumentoEsposa));
                    cmd.Parameters.Add(new SqlParameter("@i_direccion_esposo", request.PeDireccionEsposo));
                    cmd.Parameters.Add(new SqlParameter("@i_direccion_esposa", request.PeDireccionEsposa));
                    cmd.Parameters.Add(new SqlParameter("@i_observacion", request.ScObservaciones));
                    cmd.Parameters.Add(new SqlParameter("@i_User", deleteUser));
                    cmd.Parameters.Add(new SqlParameter("@i_Date", deleteDate));
                    cmd.Parameters.Add(new SqlParameter("@i_id_esposo", matrimonio.Data?.ma_esposo));
                    cmd.Parameters.Add(new SqlParameter("@i_id_esposa", matrimonio.Data?.ma_esposa));
                    cmd.Parameters.Add(new SqlParameter("@i_sacramento_esposo", matrimonio.Data?.scIdSacramentoEsposo));
                    cmd.Parameters.Add(new SqlParameter("@i_sacramento_esposa", matrimonio.Data?.scIdSacramentoEsposa));

                    con.Open();

                    var rowsAffected = await cmd.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        response.IsSuccess = true;
                        response.Data = rowsAffected > 0;
                        response.Message = ReplyMessage.MESSAGE_DELETE;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = ReplyMessage.MESSAGE_FAILED;
                    }
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
