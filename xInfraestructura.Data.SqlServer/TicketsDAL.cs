using Dominio.Entidades;
using Infraestructura.Data.SqlServer.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infraestructura.Data.SqlServer
{
    public class TicketsDAL:Conexion
    {
        public List<TicketEN> ListarTicketsGenerados(int nroTck, DateTime fecInicio, DateTime fecFin, int usu, int intEstado,int idArea)
        {
            List<TicketEN> lista = new List<TicketEN>();
            try
            {
                SqlDataReader lector = SqlHelper.ExecuteReader(cnx, "ListaTicketsGenerados", nroTck, fecInicio.ToString("yyyyMMdd"), fecFin.ToString("yyyyMMdd"), usu, intEstado,idArea);
                while (lector.Read())
                {
                    TicketEN objTicket = new TicketEN();
                    DetalleTicketEN objDetalle = new DetalleTicketEN();
                    MotivoEN objMotivo = new MotivoEN();
                    EstadoEN objEstado = new EstadoEN();
                    EncargadoEN objEncargado = new EncargadoEN();
                    PrioridadEN objPrio = new PrioridadEN();

                    objTicket.Nro = lector.GetInt32(0);
                    objTicket.empresa = lector.GetString(1);
                    objTicket.fechaInicio = lector.GetDateTime(2);
                    if (!lector.IsDBNull(3))
                        objTicket.fechaFin = lector.GetDateTime(3);
                    objTicket.TiempoDuracion = lector.GetString(4);
                    objMotivo.DescMotivo = lector.GetString(5);
                    objDetalle.descripcion = lector.GetString(6);
                    objEstado.IdEstado = lector.GetInt32(7);
                    objEstado.DesEstado = lector.GetString(8);
                    objDetalle.progreso = lector.GetInt32(9);
                    objEncargado.Nombre = lector.GetString(10);
                    objPrio.IdPrio = lector.GetInt32(11);
                    objTicket.cantSoporte = lector.GetInt32(12);
                    objDetalle.prioridad = objPrio;
                    objDetalle.encargado = objEncargado;
                    objDetalle.estado = objEstado;
                    objTicket.motivo = objMotivo;
                    objTicket.detalle = objDetalle;
                    
                    lista.Add(objTicket);
                }
                lector.Close();
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<TicketEN> BuscarTicket(int nroTck, bool add, int usu)
        {
            List<TicketEN> lista = new List<TicketEN>();
            try
            {
                SqlDataReader lector = SqlHelper.ExecuteReader(cnx, "BuscarTicket", nroTck, add, usu);
                while (lector.Read())
                {
                    TicketEN objTicket = new TicketEN();
                    DetalleTicketEN objDetalle = new DetalleTicketEN();
                    MotivoEN objMotivo = new MotivoEN();
                    PrioridadEN objPrioridad = new PrioridadEN();
                    EncargadoEN objEncargado = new EncargadoEN();
                    EstadoEN objEstado = new EstadoEN();

                    objTicket.Nro = lector.GetInt32(0);
                    objTicket.contacto = lector.GetString(1);
                    objTicket.empresa = lector.GetString(2);
                    objMotivo.IdMotivo = lector.GetInt32(3);
                    objDetalle.Id = lector.GetInt32(4);
                    objDetalle.FechaRegistro = lector.GetDateTime(5);
                    objDetalle.horaRegistro = lector.GetDateTime(5).ToString("HH:mm:ss");
                    objPrioridad.IdPrio = lector.GetInt32(6);
                    objEncargado.IdEnc = lector.GetInt32(7);
                    objEstado.IdEstado = lector.GetInt32(8);
                    objDetalle.descripcion = lector.GetString(9);
                    if (!lector.IsDBNull(10))
                    {
                        objDetalle.adjunto = lector.GetString(10);
                    }
                    objDetalle.progreso = lector.GetInt32(11);
                    if (!lector.IsDBNull(12))
                    {
                        objDetalle.respuesta = lector.GetString(12);
                    }
                    if (!lector.IsDBNull(13))
                    {
                        objDetalle.GuiaServicio = lector.GetString(13);
                    }
                    if (!lector.IsDBNull(14))
                    {
                        objDetalle.CartaConf = lector.GetBoolean(14);
                    }
                    objDetalle.flgSoporte = lector.GetInt32(15);
                    objDetalle.prioridad = objPrioridad;
                    objDetalle.encargado = objEncargado;
                    objDetalle.estado = objEstado;
                    objTicket.motivo = objMotivo;
                    objTicket.detalle = objDetalle;
                    lista.Add(objTicket);
                }
                lector.Close();
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int InsertarNuevoTicket(TicketEN objTicket, int IdUsuCrea, ref int nroRegistrado)
        {
            int insert = 0;
            try
            {
                SqlParameter Nro = new SqlParameter("Nro", SqlDbType.Int);
                SqlParameter contacto = new SqlParameter("contacto", SqlDbType.VarChar);
                SqlParameter empresa = new SqlParameter("empresa", SqlDbType.VarChar);
                SqlParameter IdMotivo = new SqlParameter("IdMotivo", SqlDbType.Int);
                SqlParameter IdUsuario = new SqlParameter("IdUsuCrea", SqlDbType.Int);
                SqlParameter IdDet = new SqlParameter("IdDet", SqlDbType.Int);
                SqlParameter fechaReg = new SqlParameter("fechaReg", SqlDbType.DateTime);
                SqlParameter IdPrio = new SqlParameter("IdPrio", SqlDbType.Int);
                SqlParameter IdEnc = new SqlParameter("IdEnc", SqlDbType.Int);
                SqlParameter IdEst = new SqlParameter("IdEstado", SqlDbType.Int);
                SqlParameter desc = new SqlParameter("desc", SqlDbType.VarChar);
                SqlParameter adjunto = new SqlParameter("adjunto", SqlDbType.VarChar);
                SqlParameter nroOutput = new SqlParameter("nroOutput", SqlDbType.Int);
              
                Nro.Direction = ParameterDirection.Input;
                contacto.Direction = ParameterDirection.Input;
                empresa.Direction = ParameterDirection.Input;
                IdMotivo.Direction = ParameterDirection.Input;
                IdUsuario.Direction = ParameterDirection.Input;
                IdDet.Direction = ParameterDirection.Input;
                fechaReg.Direction = ParameterDirection.Input;
                IdPrio.Direction = ParameterDirection.Input;
                IdEnc.Direction = ParameterDirection.Input;
                IdEst.Direction = ParameterDirection.Input;
                desc.Direction = ParameterDirection.Input;
                adjunto.Direction = ParameterDirection.Input;
                nroOutput.Direction = ParameterDirection.Output;

                Nro.Value = objTicket.Nro;
                contacto.Value = objTicket.contacto.Trim();
                empresa.Value = objTicket.empresa.Trim();
                IdMotivo.Value = objTicket.motivo.IdMotivo;
                IdUsuario.Value = IdUsuCrea;
                IdDet.Value = objTicket.detalle.Id;
                fechaReg.Value = objTicket.detalle.FechaRegistro;
                IdPrio.Value = objTicket.detalle.prioridad.IdPrio;
                IdEnc.Value = objTicket.detalle.encargado.IdEnc;
                IdEst.Value = objTicket.detalle.estado.IdEstado;
                desc.Value = objTicket.detalle.descripcion;
                adjunto.Value = objTicket.detalle.adjunto;

                SqlParameter[] param = new SqlParameter[13]{
                                                            Nro,
                                                            contacto,
                                                            empresa,
                                                            IdMotivo,
                                                            IdUsuario,
                                                            IdDet,
                                                            fechaReg,
                                                            IdPrio,
                                                            IdEnc,
                                                            IdEst,
                                                            desc,
                                                            adjunto,
                                                            nroOutput
                                                            };

                SqlHelper.ExecuteNonQuery(cnx, CommandType.StoredProcedure, "InsertarNuevoTicket", param);
                if (param[12].Value != null)
                {
                    nroRegistrado = Convert.ToInt32(param[12].Value.ToString());
                }
               insert= 1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return insert;
        }

        public int InsertarDetalleTicket(TicketEN objTicket, ref string fechaRegistrada)
        {
            int insert = 0;
            try
            {
                SqlParameter IdDet = new SqlParameter("IdDet", SqlDbType.Int);
                SqlParameter Nro = new SqlParameter("Nro", SqlDbType.Int);
                SqlParameter IdPrio = new SqlParameter("IdPrio", SqlDbType.Int);
                SqlParameter IdEnc = new SqlParameter("IdEnc", SqlDbType.Int);
                SqlParameter IdEst = new SqlParameter("IdEst", SqlDbType.Int);
                SqlParameter Progreso = new SqlParameter("Progreso", SqlDbType.Int);
                SqlParameter adjunto = new SqlParameter("adjunto", SqlDbType.VarChar);
                SqlParameter desc = new SqlParameter("desc", SqlDbType.VarChar);
                SqlParameter resp = new SqlParameter("resp", SqlDbType.VarChar);
                SqlParameter pdf = new SqlParameter("pdf", SqlDbType.VarChar);
                SqlParameter cartaConf = new SqlParameter("cartaConf", SqlDbType.Bit);
                SqlParameter fechaReg = new SqlParameter("fechaReg", SqlDbType.DateTime);

                IdDet.Direction = ParameterDirection.Input;
                Nro.Direction = ParameterDirection.Input;
                IdPrio.Direction = ParameterDirection.Input;
                IdEnc.Direction = ParameterDirection.Input;
                IdEst.Direction = ParameterDirection.Input;
                Progreso.Direction = ParameterDirection.Input;
                adjunto.Direction = ParameterDirection.Input;
                desc.Direction = ParameterDirection.Input;
                resp.Direction = ParameterDirection.Input;
                pdf.Direction = ParameterDirection.Input;
                cartaConf.Direction = ParameterDirection.Input;
                fechaReg.Direction = ParameterDirection.Output;

                IdDet.Value = objTicket.detalle.Id;
                Nro.Value = objTicket.Nro;
                IdPrio.Value = objTicket.detalle.prioridad.IdPrio;
                IdEnc.Value = objTicket.detalle.encargado.IdEnc;
                IdEst.Value = objTicket.detalle.estado.IdEstado;
                Progreso.Value = objTicket.detalle.progreso;
                adjunto.Value = objTicket.detalle.adjunto;
                desc.Value = objTicket.detalle.descripcion;
                resp.Value = objTicket.detalle.respuesta;
                pdf.Value = objTicket.detalle.GuiaServicio;
                cartaConf.Value = objTicket.detalle.CartaConf;

                SqlParameter[] param = new SqlParameter[12]{
                                                            IdDet,
                                                            Nro,
                                                            IdPrio,
                                                            IdEnc,
                                                            IdEst,
                                                            Progreso,
                                                            adjunto,
                                                            desc,
                                                            resp,
                                                            pdf,
                                                            cartaConf,
                                                            fechaReg
                                                            };

                SqlHelper.ExecuteNonQuery(cnx, CommandType.StoredProcedure, "InsertarDetalleTicket", param);
                if (param[11].Value != null)
                {
                    fechaRegistrada = param[11].Value.ToString();
                }
                insert= 1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return insert;
        }

        public int ConsultarCorrelativo()
        {
            int correlativo = 0;
            try
            {
                SqlDataReader lector = SqlHelper.ExecuteReader(cnx, "ConsultarCorrelativo");
                if (lector.Read())
                {
                    correlativo = lector.GetInt32(0);
                }
                lector.Close();
                return correlativo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int CambiarEstadoTicket(int nro,string obser)
        {
            int update = 0;
            try
            {
                SqlHelper.ExecuteNonQuery(cnx, "CambiarEstadoTicket", nro, obser);
                update= 1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return update;
        }

        public TicketEN datosEmail(int nro)
        {
            TicketEN objT = null;
            try
            {
                SqlDataReader lector = SqlHelper.ExecuteReader(cnx, "DatosEnvioCorreo", nro);
                if (lector.Read())
                {
                    objT = new TicketEN();
                    DetalleTicketEN objD = new DetalleTicketEN();
                    EncargadoEN objE = new EncargadoEN();
                    MotivoEN objM = new MotivoEN();
                    PrioridadEN objP = new PrioridadEN();
                    objT.Nro = lector.GetInt32(0);
                    objE.Nombre = lector.GetString(1);
                    objE.correo = lector.GetString(2);
                    objT.contacto = lector.GetString(3);
                    objT.empresa = lector.GetString(4);
                    objM.DescMotivo = lector.GetString(5);
                    objP.DesPrio = lector.GetString(6);
                    objD.descripcion = lector.GetString(7);
                    objD.encargado = objE;
                    objD.prioridad = objP;
                    objT.motivo = objM;
                    objT.detalle = objD;
                }
                lector.Close();
                return objT;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

             
               
    }
}
