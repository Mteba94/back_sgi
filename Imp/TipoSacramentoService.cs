using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApi_SGI_T.Models;
using WebApi_SGI_T.Models.Commons.Helpers;
using WebApi_SGI_T.Models.Commons.Request;
using WebApi_SGI_T.Models.Commons.Response;
using WebApi_SGI_T.Static;
using System.Linq.Dynamic.Core;


namespace WebApi_SGI_T.Imp
{
    public class TipoSacramentoService
    {
        private readonly SgiSacramentosContext _context;
        public TipoSacramentoService(SgiSacramentosContext context)
        {
            _context = context;
        }

        public BaseResponse<BaseEntityResponse<TipoSacramentoResponse>> ListTipoSacramento(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<BaseEntityResponse<TipoSacramentoResponse>>()
            {
                Data = new BaseEntityResponse<TipoSacramentoResponse>()
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
                cmd.CommandText = "SELECT * FROM Tbl_Tipo_Sacramentos where ts_delete_user is null and ts_delete_date is null";

                con.Open();

                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    var sacramento = new TipoSacramentoResponse
                    {
                        TsIdTipoSacramento = dr.GetInt32(0),
                        TsNombre = dr.GetString(1),
                        TsDescripcion = dr.GetString(2),
                        TsRequerimiento = dr.GetString(3),
                        TsCreateDate = dr.GetDateTime(6),
                        TsEstado = dr.GetInt32(4),
                        EstadoDescripcion = dr.GetInt32(4) == 1 ? "Activo" : "Inactivo",
                        
                    };

                    response.Data.Items.Add(sacramento);
                }

                if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
                {
                    switch (filters.NumFilter)
                    {
                        case 1:
                            response.Data.Items = response.Data.Items.Where(x => x.TsNombre.Contains(filters.TextFilter)).ToList();
                            break;
                        case 2:
                            response.Data.Items = response.Data.Items.Where(x => x.TsDescripcion.Contains(filters.TextFilter)).ToList();
                            break;
                    }
                }

                if (filters.StateFilter is not null)
                {
                    response.Data.Items = response.Data.Items.Where(x => x.TsEstado == filters.StateFilter).ToList();
                }

                if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
                {
                    var filteredData = response.Data.Items.Where(x => x.TsCreateDate >= Convert.ToDateTime(filters.StartDate)
                                             && x.TsCreateDate <= Convert.ToDateTime(filters.EndDate).AddDays(1));
                    response.Data.Items = filteredData.ToList();
                }

                if (filters.Sort is null) filters.Sort = "TsIdTipoSacramento";

                var queryableData = response.Data.Items.AsQueryable();

                response.Data.TotalRecords = queryableData.Count();

                response.Data.Items = OrderingHelper.Ordering(filters, queryableData, !(bool)filters.Download!).ToList();
                
                if (response.Data.Items is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                }

                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_QUERY;

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        public BaseResponse<List<TipoSacramentoSelectResponse>> ListSelectTipoSacramento()
        {
            var response = new BaseResponse<List<TipoSacramentoSelectResponse>>
            {
                Data = new List<TipoSacramentoSelectResponse>()
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
                cmd.CommandText = "SELECT * FROM Tbl_Tipo_Sacramentos where ts_delete_user is null and ts_delete_date is null";

                con.Open();

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        var sacramento = new TipoSacramentoSelectResponse
                        {
                            TsIdTipoSacramento = dr.GetInt32(0),
                            TsNombre = dr.GetString(1)
                        };

                        response.Data.Add(sacramento);
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
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        public BaseResponse<TipoSacramentoResponse> GetTipoSacramentoById(int id)
        {
            var response = new BaseResponse<TipoSacramentoResponse>
            {
                Data = new TipoSacramentoResponse()
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
                cmd.CommandText = "SELECT * FROM Tbl_Tipo_Sacramentos where ts_idTipoSacramento = @id";

                param.ParameterName = "@id";
                param.Value = id;
                cmd.Parameters.Add(param);

                con.Open();

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        response.Data.TsIdTipoSacramento = dr.GetInt32(0);
                        response.Data.TsNombre = dr.GetString(1);
                        response.Data.TsDescripcion = dr.GetString(2);
                        response.Data.TsRequerimiento = dr.GetString(3);
                        response.Data.TsCreateDate = dr.GetDateTime(6);
                        response.Data.TsEstado = dr.GetInt32(4);
                        response.Data.EstadoDescripcion = dr.GetInt32(4) == 1 ? "Activo" : "Inactivo";
                        
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
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<BaseResponse<bool>> RegisterTipoSacramento(TipoSacramentoRequest request)
        {
            var response = new BaseResponse<bool>();

            SqlConnection con = new SqlConnection();
            SqlCommand cmd;
            SqlParameter param = new SqlParameter();
            DataSet ds = new DataSet();
            SqlDataReader dr;

            try
            {
                var createUser = 1;
                var createDate = DateTime.Now;

                con.ConnectionString = _context.Database.GetDbConnection().ConnectionString;
                cmd = new SqlCommand();
                cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_agrega_tipo_sacramento";

                cmd.Parameters.Add(new SqlParameter("@i_operacion", "RNS"));
                cmd.Parameters.Add(new SqlParameter("@i_nombre", request.TsNombre));
                cmd.Parameters.Add(new SqlParameter("@i_descripcion", request.TsDescripcion));
                cmd.Parameters.Add(new SqlParameter("@i_requerimiento", request.TsRequerimiento));
                cmd.Parameters.Add(new SqlParameter("@i_User", createUser));
                cmd.Parameters.Add(new SqlParameter("@i_Date", createDate));

                con.Open();

                var rowsAffected = await cmd.ExecuteNonQueryAsync();
                
                if (rowsAffected > 0)
                {
                    response.IsSuccess = true;
                    response.Data = rowsAffected > 0;
                    response.Message = ReplyMessage.MESSAGE_SAVE;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_FAILED;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
            }

            return response;
        }

        public async Task<BaseResponse<bool>> UpdateTipoSacramento(int tipoSacramentoId, TipoSacramentoRequest request)
        {
            var response = new BaseResponse<bool>();

            SqlConnection con = new SqlConnection();
            SqlCommand cmd;
            SqlParameter param = new SqlParameter();
            DataSet ds = new DataSet();
            SqlDataReader dr;

            try
            {
                var updateUser = 1;
                var updateDate = DateTime.Now;

                con.ConnectionString = _context.Database.GetDbConnection().ConnectionString;
                cmd = new SqlCommand();
                cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_agrega_tipo_sacramento";

                cmd.Parameters.Add(new SqlParameter("@i_operacion", "ASE"));
                cmd.Parameters.Add(new SqlParameter("@i_nombre", request.TsNombre));
                cmd.Parameters.Add(new SqlParameter("@i_descripcion", request.TsDescripcion));
                cmd.Parameters.Add(new SqlParameter("@i_requerimiento", request.TsRequerimiento));
                cmd.Parameters.Add(new SqlParameter("@i_User", updateUser));
                cmd.Parameters.Add(new SqlParameter("@i_Date", updateDate));
                cmd.Parameters.Add(new SqlParameter("@i_id", tipoSacramentoId));

                con.Open();

                var rowsAffected = await cmd.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                {
                    response.IsSuccess = true;
                    response.Data = rowsAffected > 0;
                    response.Message = ReplyMessage.MESSAGE_UPDATE;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_FAILED;
                }

            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
            }

            return response;
        }

        public async Task<BaseResponse<bool>> DeleteTipoSacramento(int tipoSacramentoId)
        {
            var response = new BaseResponse<bool>();

            SqlConnection con = new SqlConnection();
            SqlCommand cmd;
            SqlParameter param = new SqlParameter();
            DataSet ds = new DataSet();
            SqlDataReader dr;

            try
            {
                var deleteUser = 1;
                var deleteDate = DateTime.Now;

                con.ConnectionString = _context.Database.GetDbConnection().ConnectionString;
                cmd = new SqlCommand();
                cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_agrega_tipo_sacramento";

                cmd.Parameters.Add(new SqlParameter("@i_operacion", "ESE"));
                cmd.Parameters.Add(new SqlParameter("@i_User", deleteUser));
                cmd.Parameters.Add(new SqlParameter("@i_Date", deleteDate));
                cmd.Parameters.Add(new SqlParameter("@i_id", tipoSacramentoId));

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
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
            }

            return response;
        }
    }
}
