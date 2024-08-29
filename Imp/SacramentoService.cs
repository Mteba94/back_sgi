﻿using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApi_SGI_T.Models;
using WebApi_SGI_T.Models.Commons.Helpers;
using WebApi_SGI_T.Models.Commons.Request;
using WebApi_SGI_T.Models.Commons.Response;
using System.Linq.Dynamic.Core;
using WebApi_SGI_T.Static;
using Microsoft.Data.SqlClient;
using Azure.Core;

namespace WebApi_SGI_T.Imp
{
    public class SacramentoService
    {
        private readonly SgiSacramentosContext _context;
        public SacramentoService(SgiSacramentosContext context)
        {
            _context = context;
        }

        public async Task<BaseResponse<BaseEntityResponse<SacramentoResponse>>> ListSacramentos(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<BaseEntityResponse<SacramentoResponse>>()
            {
                Data = new BaseEntityResponse<SacramentoResponse>()
            };

            try
            {

                var query = _context.TblSacramentos.Where(x => x.ScDeleteUser == null && x.ScDeleteDate == null)
                    .AsNoTracking();

                if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
                {
                    switch (filters.NumFilter)
                    {
                        case 1:
                            query = query.Where(x => x.ScIdpersonaNavigation.PeNombre.Contains(filters.TextFilter));
                            break;
                        case 2:
                            query = query.Where(x => x.ScIdpersonaNavigation.PeNumeroDocumento.Contains(filters.TextFilter));
                            break;
                        case 3:
                            query = query.Where(x => x.ScNumeroPartida!.Contains(filters.TextFilter));
                            break;
                        case 4:
                            query = query.Where(x => x.ScIdSacramentoNavigation.TsNombre.Contains(filters.TextFilter));
                            break;
                    }
                }

                if (filters.StateFilter is not null)
                {
                    query = query.Where(x => x.ScIdSacramentoNavigation.TsIdTipoSacramento == filters.StateFilter);
                }

                if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
                {
                    var filteredData = response.Data.Items.Where(x => x.ScCreateDate >= Convert.ToDateTime(filters.StartDate)
                                             && x.ScCreateDate <= Convert.ToDateTime(filters.EndDate).AddDays(1));
                    response.Data.Items = filteredData.ToList();
                }

                var totalRecords = await query.CountAsync();
                var mappedData = query.Select(s => new SacramentoResponse
                {
                    ScIdSacramento = s.ScIdSacramento,
                    ScNumeroPartida = s.ScNumeroPartida,
                    ScTipoSacramento = s.ScIdSacramentoNavigation.TsNombre,
                    PeNombre = s.ScIdpersonaNavigation.PeNombre,
                    PeNumeroDocumento = s.ScIdpersonaNavigation.PeNumeroDocumento,
                    ScFechaSacramento = s.ScFechaSacramento,
                    ScObservaciones = s.ScObservaciones,
                    ScCreateDate = s.ScCreateDate,
                    
                }).ToList();

                if (filters.Sort is null) filters.Sort = "ScIdSacramento";

                var orderedList = OrderingHelper.Ordering(filters, mappedData.AsQueryable(), !(bool)filters.Download!);
                response.Data.Items = orderedList.ToList();
                response.Data.TotalRecords = totalRecords;

                response.IsSuccess = response.Data.Items.Any();
                response.Message = response.IsSuccess ? ReplyMessage.MESSAGE_QUERY : ReplyMessage.MESSAGE_QUERY_EMPTY;

            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<BaseResponse<SacramentoResponse>> GetSacramentoById(int id)
        {
            var response = new BaseResponse<SacramentoResponse>();

            try
            {
                // Obtén el Sacramento con el ID especificado
                var query = await _context.TblSacramentos
                    .Include(x => x.ScIdSacramentoNavigation)
                    .Include(x => x.ScIdpersonaNavigation)
                    .Include(x => x.ScIdpersonaNavigation.PeIdTipoDocumentoNavigation)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ScIdSacramento == id);
                    

                // Verifica si se encontró el Sacramento
                if (query != null)
                {
                    // Mapea el resultado a SacramentoResponse
                    var mappedData = new SacramentoResponse
                    {
                        ScIdSacramento = query.ScIdSacramento,
                        ScNumeroPartida = query.ScNumeroPartida,
                        scIdTipoSacramento = query.ScIdTipoSacramento,
                        ScTipoSacramento = query.ScIdSacramentoNavigation.TsNombre,
                        PeNombre = query.ScIdpersonaNavigation.PeNombre,
                        PeFechaNacimiento = query.ScIdpersonaNavigation.PeFechaNacimiento,
                        PeNumeroDocumento = query.ScIdpersonaNavigation.PeNumeroDocumento,
                        PeIdTipoDocumento = query.ScIdpersonaNavigation.PeIdTipoDocumento,
                        PeTipoDocumento = query.ScIdpersonaNavigation.PeIdTipoDocumentoNavigation.TdAbreviacion,
                        PeDireccion = query.ScIdpersonaNavigation.PeDireccion,
                        ScNombrePadre = query.ScPadre,
                        ScNombreMadre = query.ScMadre,
                        ScNombrePadrino = query.ScPadrino,
                        ScNombreMadrina = query.ScMadrina,
                        ScFechaSacramento = query.ScFechaSacramento,
                        ScParroco = query.ScParroco,
                        ScObservaciones = query.ScObservaciones,
                        ScCreateDate = query.ScCreateDate
                    };

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

        public async Task<BaseResponse<bool>> RegisterSacramento(SacramentoRequestDto request)
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
                cmd.CommandText = "sp_control_sacramento";

                cmd.Parameters.Add(new SqlParameter("@i_operacion", "RNS"));
                cmd.Parameters.Add(new SqlParameter("@i_id_sacramento", request.ScIdTipoSacramento));
                cmd.Parameters.Add(new SqlParameter("@i_numDocto", request.PeNumeroDocumento));
                cmd.Parameters.Add(new SqlParameter("@i_fechaSacramento", request.ScFechaSacramento));
                cmd.Parameters.Add(new SqlParameter("@i_numpartida", request.ScNumeroPartida));
                cmd.Parameters.Add(new SqlParameter("@i_padre", request.ScPadre));
                cmd.Parameters.Add(new SqlParameter("@i_madre", request.ScMadre));
                cmd.Parameters.Add(new SqlParameter("@i_padrino", request.ScPadrino));
                cmd.Parameters.Add(new SqlParameter("@i_madrina", request.ScMadrina));
                cmd.Parameters.Add(new SqlParameter("@i_parroco", request.ScParroco));
                cmd.Parameters.Add(new SqlParameter("@i_nombre", request.PeNombre));
                cmd.Parameters.Add(new SqlParameter("@i_fechaNacimiento", request.PeFechaNacimiento));
                cmd.Parameters.Add(new SqlParameter("@i_tipoDoc", request.PeIdTipoDocumento));
                cmd.Parameters.Add(new SqlParameter("@i_direccion", request.PeDireccion));
                cmd.Parameters.Add(new SqlParameter("@i_observacion", request.ScObservaciones));
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
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<BaseResponse<bool>> RegisterMatrimonio(MatrimonioRequest request)
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
                cmd.CommandText = "sp_control_sacramento_matrimonio";

                cmd.Parameters.Add(new SqlParameter("@i_operacion", "RNM"));
                cmd.Parameters.Add(new SqlParameter("@i_id_sacramento", request.ScIdTipoSacramento));
                cmd.Parameters.Add(new SqlParameter("@i_numDocto_esposo", request.PeNumeroDocumentoEsposo));
                cmd.Parameters.Add(new SqlParameter("@i_numDocto_esposa", request.PeNumeroDocumentoEsposa));
                cmd.Parameters.Add(new SqlParameter("@i_fechaSacramento", request.ScFechaSacramento));
                cmd.Parameters.Add(new SqlParameter("@i_numpartida", request.ScNumeroPartida));
                cmd.Parameters.Add(new SqlParameter("@i_padre_esposo", request.ScPadreEsposo));
                cmd.Parameters.Add(new SqlParameter("@i_padre_esposa", request.ScPadreEsposa));
                cmd.Parameters.Add(new SqlParameter("@i_madre_esposo", request.ScMadreEsposo));
                cmd.Parameters.Add(new SqlParameter("@i_madre_esposa", request.ScMadreEsposa));
                cmd.Parameters.Add(new SqlParameter("@i_testigo1", request.ScTestigo1));
                cmd.Parameters.Add(new SqlParameter("@i_testigo2", request.ScTestigo2));
                cmd.Parameters.Add(new SqlParameter("@i_parroco", request.ScParroco));
                cmd.Parameters.Add(new SqlParameter("@i_nombre_esposo", request.PeNombreEsposo));
                cmd.Parameters.Add(new SqlParameter("@i_nombre_esposa", request.PeNombreEsposa));
                cmd.Parameters.Add(new SqlParameter("@i_fechaNacimiento_esposo", request.PeFechaNacimientoEsposo));
                cmd.Parameters.Add(new SqlParameter("@i_fechaNacimiento_esposa", request.PeFechaNacimientoEsposa));
                cmd.Parameters.Add(new SqlParameter("@i_tipoDoc_esposo", request.PeIdTipoDocumentoEsposo));
                cmd.Parameters.Add(new SqlParameter("@i_tipoDoc_esposa", request.PeIdTipoDocumentoEsposa));
                cmd.Parameters.Add(new SqlParameter("@i_direccion_esposo", request.PeDireccionEsposo));
                cmd.Parameters.Add(new SqlParameter("@i_direccion_esposa", request.PeDireccionEsposa));
                cmd.Parameters.Add(new SqlParameter("@i_observacion", request.ScObservaciones));
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
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;

        }

        public async Task<BaseResponse<bool>> UpdateSacramento(int sacramentoId, SacramentoRequestDto request)
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
                cmd.CommandText = "sp_control_sacramento";

                cmd.Parameters.Add(new SqlParameter("@i_operacion", "ASE"));
                cmd.Parameters.Add(new SqlParameter("@i_id", sacramentoId));
                cmd.Parameters.Add(new SqlParameter("@i_id_sacramento", request.ScIdTipoSacramento));
                cmd.Parameters.Add(new SqlParameter("@i_numDocto", request.PeNumeroDocumento));
                cmd.Parameters.Add(new SqlParameter("@i_fechaSacramento", request.ScFechaSacramento));
                cmd.Parameters.Add(new SqlParameter("@i_numpartida", request.ScNumeroPartida));
                cmd.Parameters.Add(new SqlParameter("@i_padre", request.ScPadre));
                cmd.Parameters.Add(new SqlParameter("@i_madre", request.ScMadre));
                cmd.Parameters.Add(new SqlParameter("@i_padrino", request.ScPadrino));
                cmd.Parameters.Add(new SqlParameter("@i_madrina", request.ScMadrina));
                cmd.Parameters.Add(new SqlParameter("@i_parroco", request.ScParroco));
                cmd.Parameters.Add(new SqlParameter("@i_nombre", request.PeNombre));
                cmd.Parameters.Add(new SqlParameter("@i_fechaNacimiento", request.PeFechaNacimiento));
                cmd.Parameters.Add(new SqlParameter("@i_tipoDoc", request.PeIdTipoDocumento));
                cmd.Parameters.Add(new SqlParameter("@i_observacion", request.ScObservaciones));
                cmd.Parameters.Add(new SqlParameter("@i_direccion", request.PeDireccion));
                cmd.Parameters.Add(new SqlParameter("@i_User", createUser));
                cmd.Parameters.Add(new SqlParameter("@i_Date", createDate));

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
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
