using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Transactions;
using Dominio.Entidades;
using Infraestructura.Data.SqlServer;

namespace Dominio.Repositorio
{
    public class ZKLectorBL : IDisposable
    {
        private ZKTerminalDAO zkTerminalDao = new ZKTerminalDAO();

        public ListZKTerminal ListarLectores(string x_filtro, int x_estado, int x_IdTerminal)
        {
            string funcion = "ListarLectores";
            ListZKTerminal result = new ListZKTerminal();//modificado ListZKLogin
            try
            {

                using (TransactionScope tscTrans = new TransactionScope())
                {
                    result = zkTerminalDao.ListarLectores(x_filtro, x_estado, x_IdTerminal);
                    tscTrans.Complete();
                }

            }
            catch (SqlException ex)
            {
                UtilitarioBL.AlmacenarLogError(ex);
                Exception exc = new Exception("Ocurrió un error en BD (" + funcion + ")");
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

        public bool InsertarLector( ref ZKTerminal x_Terminal, ref string x_mensaje)
        {
            string funcion = "InsertarLector";
            bool result = false;
            try
            {
                using (TransactionScope tscTrans = new TransactionScope())
                {
                    result = zkTerminalDao.InsertarLector(ref x_Terminal, ref x_mensaje);
                    tscTrans.Complete();
                }
            }
            catch (SqlException ex)
            {
                x_mensaje = ex.Message;
                UtilitarioBL.AlmacenarLogError(ex);
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
        public bool EliminarLector(int x_idTerminal, ref string x_mensaje)
        {
            string funcion = "EliminarLector";
            bool result = false;
            try
            {
                using (TransactionScope tscTrans = new TransactionScope())
                {
                    result = zkTerminalDao.EliminarLector(x_idTerminal);
                    tscTrans.Complete();
                }
            }
            catch (SqlException ex)
            {
                x_mensaje = ex.Message;
                UtilitarioBL.AlmacenarLogError(ex);
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
        public bool CambiarEstadoLector(int x_idTerminal, int x_estado, ref string x_mensaje)
        {
            string funcion = "CambiarEstadoLector";
            bool result = false;
            try
            {
                using (TransactionScope tscTrans = new TransactionScope())
                {
                    result = zkTerminalDao.CambiarEstadoLector(x_idTerminal, x_estado);
                    tscTrans.Complete();
                }

            }
            catch (SqlException ex)
            {
                x_mensaje = ex.Message;
                UtilitarioBL.AlmacenarLogError(ex);
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
