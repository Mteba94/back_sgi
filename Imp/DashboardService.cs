using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApi_SGI_T.Models;
using WebApi_SGI_T.Models.Commons.Request;
using WebApi_SGI_T.Models.Commons.Response;
using WebApi_SGI_T.Models.Commons.Response.Dashboard;
using WebApi_SGI_T.Static;

namespace WebApi_SGI_T.Imp
{
    public class DashboardService
    {
        private readonly SgiSacramentosContext _context;

        public DashboardService(SgiSacramentosContext context)
        {
            _context = context;
        }

        public async Task<BaseResponse<BaseEntityResponse<IndicadorSacramentosResponse>>> IndicadorSacramentos(BaseFiltersRequest filters)
        {

            var response = new BaseResponse<BaseEntityResponse<IndicadorSacramentosResponse>>()
            {
                Data = new BaseEntityResponse<IndicadorSacramentosResponse>()
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
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_cantidad_sacramentos";

                cmd.Parameters.Add(new SqlParameter("@i_operacion", "CS"));

                con.Open();

                dr = cmd.ExecuteReader();

                int totalRecords = 0;

                while (dr.Read())
                {
                    var data = new IndicadorSacramentosResponse
                    {
                        sacramentos = dr["sacramentos"].ToString(),
                        total = Convert.ToInt32(dr["total"]),
                        anio = Convert.ToInt32(dr["anio"])
                    };

                    response.Data.Items.Add(data);
                    totalRecords++;
                };

                response.Data.TotalRecords = totalRecords;
                response.Message = ReplyMessage.MESSAGE_QUERY;
                response.IsSuccess = true;
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
