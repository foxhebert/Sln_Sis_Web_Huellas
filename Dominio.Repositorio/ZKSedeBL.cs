using Dominio.Entidades;
using Dominio.Entidades.Personalizado;
using Infraestructura.Data.SqlServer;
using System;
using System.Data.SqlClient;
using System.Transactions;
using System.Linq;
using System.Collections.Generic;

namespace Dominio.Repositorio
{
    public class ZKSedeBL : IDisposable
    {
        private ZKSedeDAO zkSedeDao = new ZKSedeDAO();

        #region MÉTODOS NO TRANSACCIONALES
        public ListZKSede ListarSedes(int x_intEstadoActivo)
        {
            string funcion = "ListarSedes";
            ListZKSede result = new ListZKSede();
            try
            {
                using (TransactionScope tscTrans = new TransactionScope())
                {
                    result = zkSedeDao.ListarSedes(x_intEstadoActivo);
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

        public List<ItemCombo> ListarSedeCombo()
        {
            string funcion = "ListarSedes";
            List<ItemCombo> result = new List<ItemCombo>();
            ListZKSede lstSede = new ListZKSede();
            try
            {
                using (TransactionScope tscTrans = new TransactionScope())
                {
                    lstSede = zkSedeDao.ListarSedes(1);
                    tscTrans.Complete();
                }
                lstSede.ToList().ForEach(x => {
                    result.Add(new ItemCombo() { Texto = x.strDeLocal, Valor = x.intIdSede });
                });
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

        #endregion


        public bool MantenimientoSede(ZKSede x_sede, ref string x_mensaje)
        {
            string funcion = "InsertarSede";
            bool result = false;
            try
            {
                using (TransactionScope tscTrans = new TransactionScope())
                {
                    result = zkSedeDao.MantenimientoSede(ref x_sede);
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
