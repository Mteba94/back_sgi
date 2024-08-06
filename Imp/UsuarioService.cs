using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApi_SGI_T.Models;
using WebApi_SGI_T.Models.Commons.Response;
using WebApi_SGI_T.Static;

namespace WebApi_SGI_T.Imp
{
    public class UsuarioService
    {
        private readonly SgiSacramentosContext _context;

        public UsuarioService(SgiSacramentosContext context)
        {
            _context = context;
        }

        public BaseResponse<UsuarioResponse> GetUsuarioByUserName(string userName)
        {
            var response = new BaseResponse<UsuarioResponse>
            {
                Data = new UsuarioResponse()
            };

            SqlConnection con = new SqlConnection();
            SqlCommand cmd;
            SqlParameter param = new SqlParameter();
            DataSet ds = new DataSet();
            SqlDataReader dr;

            try
            {
                con.ConnectionString = _context.Database.GetDbConnection().ConnectionString;
                cmd = new SqlCommand();
                cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM tbl_usuarios WHERE us_userName = @UserName";

                param.ParameterName = "@UserName";
                param.Value = userName;
                cmd.Parameters.Add(param);

                con.Open();

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        response.Data.UsIdUsuario = dr.GetInt32(0);
                        response.Data.UsUserName = dr.GetString(1);
                        response.Data.UsPass = dr.GetString(2);
                        response.Data.UsImage = dr.IsDBNull(3) ? null : dr.GetString(3);
                        response.Data.UsNombre = dr.GetString(4);
                        response.Data.UsFechaNacimiento = dr.GetDateTime(5);
                        response.Data.UsDireccion = dr.GetString(8);
                        response.Data.UsCreateDate = dr.GetDateTime(11);
                        response.Data.UsEstado = dr.GetByte(9);
                        response.Data.EstadoDescripcion = dr.GetByte(9) == 1 ? "Activo" : "Inactivo";
                    }
                    response.IsSuccess = true;
                    response.Message = ReplyMessage.MESSAGE_QUERY;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                }

            }
            catch (SqlException ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
