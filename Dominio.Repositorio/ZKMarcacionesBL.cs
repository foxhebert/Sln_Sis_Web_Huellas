using Dominio.Entidades;
using Dominio.Entidades.Personalizado;
using Infraestructura.Data.SqlServer;
using System;
using System.Data.SqlClient;
using System.Transactions;

namespace Dominio.Repositorio
{
    public class ZKMarcacionesBL : IDisposable
    {
        private ZKMarcacionesDAO zMarcacionesDao = new ZKMarcacionesDAO();

        #region MÉTODOS NO TRANSACCIONALES
        public ListItemAsistencia ListarMarcaciones(DateTime x_feIni, DateTime x_feFin, string x_criterio, string x_filtro, int x_estado, int x_intIdSede)
        {
            string funcion = "ListarMarcaciones";
            ListItemAsistencia result = new ListItemAsistencia();
            try
            {
                using (TransactionScope tscTrans = new TransactionScope())
                {
                    result = zMarcacionesDao.ListarMarcaciones(x_feIni, x_feFin, x_criterio, x_filtro, x_estado, x_intIdSede);
                    tscTrans.Complete();
                }
            }
            catch (SqlException ex)
            {
                UtilitarioBL.AlmacenarLogError(ex);
                Exception exc = new Exception("Ocurrió un error en BD (" + funcion + ")");
                throw exc;
            }
            catch (Exception ex)
            {
                UtilitarioBL.AlmacenarLogError(ex);
                Exception exc = new Exception("Ocurrió una excepción (" + funcion + ")");
                throw exc;
            }

            return result;
        }

        public bool RegistrarMarca(string x_strHuella, string x_sSerie, int x_iIdSede, ref int x_numDedo, ref string x_mensaje)
        {
            ZKUsuarios usuario = new ZKUsuariosBL().IdentificarHuella(x_strHuella, ref x_numDedo);

            if (usuario == null || usuario.iIdUsuario == 0)
            {
                x_mensaje = "2La huella no se encuentra registrada";
                return false;
            }


            string funcion = "RegistrarMarca";
            bool result = false;
            try
            {
                using (TransactionScope tscTrans = new TransactionScope())
                {
                    result = zMarcacionesDao.RegistrarMarca(usuario.iIdUsuario, x_sSerie, x_numDedo, x_iIdSede, ref x_mensaje);
                    x_mensaje = Convert.ToInt32(result).ToString() + "" + usuario.sNombre + "<br>" + x_mensaje;
                    tscTrans.Complete();
                }
            }
            catch (SqlException ex)
            {
                UtilitarioBL.AlmacenarLogError(ex);
                Exception exc = new Exception("Ocurrió un error en BD (" + funcion + ")");
                throw exc;
            }
            catch (TransactionManagerCommunicationException ex)
            {
                UtilitarioBL.AlmacenarLogError(ex);
                Exception exc = new Exception("Ocurrió un error con el administrador de comunicaciones (" + funcion + ")");
                throw exc;
            }
            catch (TransactionInDoubtException ex)
            {
                UtilitarioBL.AlmacenarLogError(ex);
                Exception exc = new Exception("Ocurrió un error al confirmar una transacción dudosa (" + funcion + ")");
                throw exc;

            }
            catch (TransactionAbortedException ex)
            {
                UtilitarioBL.AlmacenarLogError(ex);
                Exception exc = new Exception("Ocurrió un error al intentar confirmar una transacción inexistente (" + funcion + ")");
                throw exc;
            }
            catch (TransactionException ex)
            {
                UtilitarioBL.AlmacenarLogError(ex);
                Exception exc = new Exception("Ocurrió un error al intentar confirmar una transacción inexistente (" + funcion + ")");
                throw exc;
            }
            catch (Exception ex)
            {
                UtilitarioBL.AlmacenarLogError(ex);
                Exception exc = new Exception("Ocurrió una excepción (" + funcion + ")");
                throw exc;
            }
            return result;
        }
        //añadida 16.06.2021
        public bool RegistrarMarca_(string x_strHuella, string x_sSerie, int x_iIdSede, ref int x_numDedo, DateTime feHor, ref string x_mensaje)
        {
            UtilitarioBL.AlmacenarLogMensaje(x_strHuella, "Marca - Huella 10 | RegistrarMarca_"); //comentar para liberación

            ZKUsuarios usuario = new ZKUsuariosBL().IdentificarHuella(x_strHuella, ref x_numDedo);

            if (usuario == null || usuario.iIdUsuario == 0)
            {
                x_mensaje = "La huella no se encuentra registrada";
                return false;
            }


            string funcion = "RegistrarMarca";
            bool result = false;
            try
            {
                using (TransactionScope tscTrans = new TransactionScope())
                {
                    result = zMarcacionesDao.RegistrarMarca_(usuario.iIdUsuario, x_sSerie, x_numDedo, x_iIdSede, feHor, ref x_mensaje);
                    x_mensaje = Convert.ToInt32(result).ToString() + "" + usuario.sNombre + "<br>" + x_mensaje;
                    tscTrans.Complete();
                }
            }
            catch (SqlException ex)
            {
                UtilitarioBL.AlmacenarLogError(ex);
                Exception exc = new Exception("Ocurrió un error en BD (" + funcion + ")");
                throw exc;
            }
            catch (TransactionManagerCommunicationException ex)
            {
                UtilitarioBL.AlmacenarLogError(ex);
                Exception exc = new Exception("Ocurrió un error con el administrador de comunicaciones (" + funcion + ")");
                throw exc;
            }
            catch (TransactionInDoubtException ex)
            {
                UtilitarioBL.AlmacenarLogError(ex);
                Exception exc = new Exception("Ocurrió un error al confirmar una transacción dudosa (" + funcion + ")");
                throw exc;

            }
            catch (TransactionAbortedException ex)
            {
                UtilitarioBL.AlmacenarLogError(ex);
                Exception exc = new Exception("Ocurrió un error al intentar confirmar una transacción inexistente (" + funcion + ")");
                throw exc;
            }
            catch (TransactionException ex)
            {
                UtilitarioBL.AlmacenarLogError(ex);
                Exception exc = new Exception("Ocurrió un error al intentar confirmar una transacción inexistente (" + funcion + ")");
                throw exc;
            }
            catch (Exception ex)
            {
                UtilitarioBL.AlmacenarLogError(ex);
                Exception exc = new Exception("Ocurrió una excepción (" + funcion + ")");
                throw exc;
            }
            return result;
        }


        public ZKUsuarios RegistrarMarcaWinService(string x_strHuella, string x_sSerie, int x_iIdSede, ref int x_numDedo, ref bool x_result, ref string x_mensaje)
        {
            ZKUsuarios usuario = new ZKUsuariosBL().IdentificarHuella(x_strHuella, ref x_numDedo);

            if (usuario == null || usuario.iIdUsuario == 0)
            {
                usuario = new ZKUsuarios();
                x_mensaje = "La huella no se encuentra registrada";
                x_result = false;
                return usuario;
            }


            string funcion = "RegistrarMarcaWinService";
            try
            {
                using (TransactionScope tscTrans = new TransactionScope())
                {
                    x_result = zMarcacionesDao.RegistrarMarca(usuario.iIdUsuario, x_sSerie, x_numDedo, x_iIdSede, ref x_mensaje);
                    tscTrans.Complete();
                }
            }
            catch (SqlException ex)
            {
                UtilitarioBL.AlmacenarLogError(ex);
                Exception exc = new Exception("Ocurrió un error en BD (" + funcion + ")");
                throw exc;
            }
            catch (TransactionManagerCommunicationException ex)
            {
                UtilitarioBL.AlmacenarLogError(ex);
                Exception exc = new Exception("Ocurrió un error con el administrador de comunicaciones (" + funcion + ")");
                throw exc;
            }
            catch (TransactionInDoubtException ex)
            {
                UtilitarioBL.AlmacenarLogError(ex);
                Exception exc = new Exception("Ocurrió un error al confirmar una transacción dudosa (" + funcion + ")");
                throw exc;

            }
            catch (TransactionAbortedException ex)
            {
                UtilitarioBL.AlmacenarLogError(ex);
                Exception exc = new Exception("Ocurrió un error al intentar confirmar una transacción inexistente (" + funcion + ")");
                throw exc;
            }
            catch (TransactionException ex)
            {
                UtilitarioBL.AlmacenarLogError(ex);
                Exception exc = new Exception("Ocurrió un error al intentar confirmar una transacción inexistente (" + funcion + ")");
                throw exc;
            }
            catch (Exception ex)
            {
                UtilitarioBL.AlmacenarLogError(ex);
                Exception exc = new Exception("Ocurrió una excepción (" + funcion + ")");
                throw exc;
            }
            return usuario;
        }
        #endregion

        #region IDisposable
        // Para detectar llamadas redundantes
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    // TODO: eliminar estado administrado (objetos administrados).
                }
                // TODO: liberar recursos no administrados (objetos no administrados) e invalidar Finalize() below.
                // TODO: Establecer campos grandes como Null.
            }
            this.disposedValue = true;
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}