using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using WebApi_SGI_T.Models;
using WebApi_SGI_T.Models.Commons.Response;
using WebApi_SGI_T.Static;

namespace WebApi_SGI_T.Imp.Matrimonio
{
    public class MatrimonioById
    {
        private readonly SgiSacramentosContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public MatrimonioById(SgiSacramentosContext context, IHttpContextAccessor httpContextAccessor = null)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<MatrimonioResponse>> GetMatrimonioById(int sacramentoId)
        {
            var response = new BaseResponse<MatrimonioResponse>();
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = _context.Database.GetDbConnection().ConnectionString;

                response = await GetMatrimonioById(sacramentoId, con);
                con.Close();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
            }

            return response;
        }

        public async Task<BaseResponse<MatrimonioResponse>> GetMatrimonioById(int sacramentoId, SqlConnection con)
        {
            var response = new BaseResponse<MatrimonioResponse>();

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

                var User = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var Date = DateTime.Now;

                cmd.Parameters.Add(new SqlParameter("@i_operacion", "OM"));
                cmd.Parameters.Add(new SqlParameter("@i_id_sacramento", 4));
                cmd.Parameters.Add(new SqlParameter("@i_numDocto_esposo", null));
                cmd.Parameters.Add(new SqlParameter("@i_numDocto_esposa", null));
                cmd.Parameters.Add(new SqlParameter("@i_fechaSacramento", null));
                cmd.Parameters.Add(new SqlParameter("@i_numpartida", null));
                cmd.Parameters.Add(new SqlParameter("@i_padre_esposo", null));
                cmd.Parameters.Add(new SqlParameter("@i_padre_esposa", null));
                cmd.Parameters.Add(new SqlParameter("@i_madre_esposo", null));
                cmd.Parameters.Add(new SqlParameter("@i_madre_esposa", null));
                cmd.Parameters.Add(new SqlParameter("@i_sexo_esposo", null));
                cmd.Parameters.Add(new SqlParameter("@i_sexo_esposa", null));
                cmd.Parameters.Add(new SqlParameter("@i_testigo1", null));
                cmd.Parameters.Add(new SqlParameter("@i_testigo2", null));
                cmd.Parameters.Add(new SqlParameter("@i_parroco", null));
                cmd.Parameters.Add(new SqlParameter("@i_nombre_esposo", null));
                cmd.Parameters.Add(new SqlParameter("@i_nombre_esposa", null));
                cmd.Parameters.Add(new SqlParameter("@i_edad_esposo", null));
                cmd.Parameters.Add(new SqlParameter("@i_edad_esposa", null));
                cmd.Parameters.Add(new SqlParameter("@i_fechaNacimiento_esposo", null));
                cmd.Parameters.Add(new SqlParameter("@i_fechaNacimiento_esposa", null));
                cmd.Parameters.Add(new SqlParameter("@i_tipoDoc_esposo", null));
                cmd.Parameters.Add(new SqlParameter("@i_tipoDoc_esposa", null));
                cmd.Parameters.Add(new SqlParameter("@i_direccion_esposo", null));
                cmd.Parameters.Add(new SqlParameter("@i_direccion_esposa", null));
                cmd.Parameters.Add(new SqlParameter("@i_observacion", null));
                cmd.Parameters.Add(new SqlParameter("@i_User", User));
                cmd.Parameters.Add(new SqlParameter("@i_Date", Date));
                cmd.Parameters.Add(new SqlParameter("@i_id_esposo", null));
                cmd.Parameters.Add(new SqlParameter("@i_id_esposa", null));
                cmd.Parameters.Add(new SqlParameter("@i_sacramento_esposo", sacramentoId));
                cmd.Parameters.Add(new SqlParameter("@i_sacramento_esposa", null));

                con.Open();

                var reader = await cmd.ExecuteReaderAsync();

                var datos = new List<DatosMatrimonio>();

                while (await reader.ReadAsync())
                {
                    var matrimonioData = new DatosMatrimonio
                    {
                        ma_IdMatrimonio = Convert.ToInt32(reader["ma_IdMatrimonio"]),
                        ma_esposo = Convert.ToInt32(reader["ma_esposo"]),
                        ma_esposa = Convert.ToInt32(reader["ma_esposa"]),
                        sc_idSacramento = Convert.ToInt32(reader["sc_idSacramento"]),
                        MatrimonioEstado = Convert.ToInt32(reader["MatrimonioEstado"]),
                        sc_id_TipoSacramento = Convert.ToInt32(reader["sc_idTipoSacramento"]),
                        sc_numeroPartida = reader["sc_numeroPartida"].ToString(),
                        pe_nombre = reader["pe_nombre"].ToString(),
                        PeEdad = reader["PeEdad"] as int? ?? 0,
                        pe_fechaNamcimiento = Convert.ToDateTime(reader["pe_fechaNacimiento"]),
                        PeTipoDocumento = Convert.ToByte(reader["pe_idTipoDocumento"]),
                        PeNumeroDocumento = reader["Pe_numeroDocumento"].ToString(),
                        PeSexoId = Convert.ToByte(reader["PeSexoId"]),
                        PeDireccion = reader["Pe_Direccion"].ToString(),
                        ScNombrePadre = reader["Sc_Padre"].ToString(),
                        ScNombreMadre = reader["Sc_Madre"].ToString(),
                        ScNombrePadrino = reader["Sc_Padrino"].ToString(),
                        ScNombreMadrina = reader["Sc_Madrina"].ToString(),
                        ScFechaSacramento = Convert.ToDateTime(reader["sc_fechaSacramento"]),
                        ScParrocoId = Convert.ToInt32(reader["sc_parrocoId"]),
                        ScObservaciones = reader["sc_observaciones"].ToString()
                    };
                    datos.Add(matrimonioData);
                }

                if (datos.Count > 0)
                {
                    var mappedData = new MatrimonioResponse();

                    foreach (var dato in datos)
                    {
                        if (dato.PeSexoId == 1) // Mujer
                        {
                            mappedData.ma_esposa = dato.ma_esposa;
                            mappedData.scIdSacramentoEsposa = dato.sc_idSacramento;
                            mappedData.PeNombreEsposa = dato.pe_nombre;
                            mappedData.PeEdadEsposa = dato.PeEdad;
                            mappedData.PeFechaNacimientoEsposa = dato.pe_fechaNamcimiento;
                            mappedData.PeIdTipoDocumentoEsposa = dato.PeTipoDocumento;
                            mappedData.PeNumeroDocumentoEsposa = dato.PeNumeroDocumento!;
                            mappedData.PeSexoIdEsposa = dato.PeSexoId;
                            mappedData.PeDireccionEsposa = dato.PeDireccion;
                            mappedData.ScPadreEsposa = dato.ScNombrePadre;
                            mappedData.ScMadreEsposa = dato.ScNombreMadre;
                        }

                        if (dato.PeSexoId == 2) // Hombre
                        {
                            mappedData.ma_esposo = dato.ma_esposo;
                            mappedData.scIdSacramentoEsposo = dato.sc_idSacramento;
                            mappedData.PeNombreEsposo = dato.pe_nombre;
                            mappedData.PeEdadEsposo = dato.PeEdad;
                            mappedData.PeFechaNacimientoEsposo = dato.pe_fechaNamcimiento;
                            mappedData.PeIdTipoDocumentoEsposo = dato.PeTipoDocumento;
                            mappedData.PeNumeroDocumentoEsposo = dato.PeNumeroDocumento!;
                            mappedData.PeSexoIdEsposo = dato.PeSexoId;
                            mappedData.PeDireccionEsposo = dato.PeDireccion;
                            mappedData.ScPadreEsposo = dato.ScNombrePadre;
                            mappedData.ScMadreEsposo = dato.ScNombreMadre;
                        }

                        // Campos comunes
                        mappedData.ma_IdMatrimonio = dato.ma_IdMatrimonio;
                        mappedData.MatrimonioEstado = dato.MatrimonioEstado;
                        mappedData.ScIdTipoSacramento = dato.sc_id_TipoSacramento;
                        mappedData.ScNumeroPartida = dato.sc_numeroPartida;
                        mappedData.ScTestigo1 = dato.ScNombrePadrino;
                        mappedData.ScTestigo2 = dato.ScNombreMadrina;
                        mappedData.ScParrocoId = dato.ScParrocoId;
                        mappedData.ScFechaSacramento = dato.ScFechaSacramento;
                        mappedData.ScObservaciones = dato.ScObservaciones;
                    }

                    response.IsSuccess = true;
                    response.Data = mappedData;
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
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public class DatosMatrimonio
        {
            public int ma_IdMatrimonio { get; set; }
            public int ma_esposo { get; set; }
            public int ma_esposa { get; set; }
            public int sc_idSacramento { get; set; }
            public int MatrimonioEstado { get; set; }
            public int sc_id_TipoSacramento { get; set; }
            public string? sc_numeroPartida { get; set; }
            public string? pe_nombre { get; set; }
            public int PeEdad { get; set; }
            public DateTime pe_fechaNamcimiento { get; set; }
            public byte PeTipoDocumento { get; set; }
            public string? PeNumeroDocumento { get; set; } = null!;
            public byte PeSexoId { get; set; }
            public string? PeDireccion { get; set; }
            public string? ScNombrePadre { get; set; }
            public string? ScNombreMadre { get; set; }
            public string? ScNombrePadrino { get; set; }
            public string? ScNombreMadrina { get; set; }
            public DateTime ScFechaSacramento { get; set; }
            public int? ScParrocoId { get; set; }
            public string? ScObservaciones { get; set; }

        }

    }
}
