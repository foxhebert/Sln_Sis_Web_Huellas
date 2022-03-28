using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Transactions;
using Dominio.Entidades;
using Infraestructura.Data.SqlServer;

namespace Dominio.Repositorio
{
    public class ZKHuellasBL : IDisposable
    {
        private ZKHuellasDAO zkHuellasDao = new ZKHuellasDAO();

        #region MÉTODOS NO TRANSACCIONALES
        public List<ZKHuellas> ListarHuellasAlg10Usuario(int x_iIdUsuario)
        {
            string funcion = "ListarHuellasAlg10Usuario";
            List<ZKHuellas> result = new List<ZKHuellas>();
            try
            {
                using (TransactionScope tscTrans = new TransactionScope())
                {
                    result = zkHuellasDao.ListarHuellasAlg10Usuario(x_iIdUsuario);
                    //if (result)
                    //{
                    //    //Grabando movimiento de usuario.
                    //    p_TSUsuarMovimEn.intIdEntid = result;
                    //    p_TSUsuarMovimEn.strDeTabla = "TMAlmac";
                    //    objTSUsuarMovimDA.Insertar(p_TSUsuarMovimEn);
                    //}
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
        public List<ZKHuellas> ListarHuellasAlg10Todo(int x_pagina, int x_tamanio, out int x_totalregistros)
        {
            string funcion = "ListarHuellasAlg10Todo";
            List<ZKHuellas> result = new List<ZKHuellas>();
            x_totalregistros = 0;
            try
            {
                using (TransactionScope tscTrans = new TransactionScope())
                {
                    result = zkHuellasDao.ListarHuellasAlg10Todo(x_pagina, x_tamanio, out x_totalregistros);
                    //if (result)
                    //{
                    //    //Grabando movimiento de usuario.
                    //    p_TSUsuarMovimEn.intIdEntid = result;
                    //    p_TSUsuarMovimEn.strDeTabla = "TMAlmac";
                    //    objTSUsuarMovimDA.Insertar(p_TSUsuarMovimEn);
                    //}
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
        public string ListarHuellasAlg10UsuarioCadena(int x_iIdUsuario)
        {
            string funcion = "ListarHuellasAlg10UsuarioCadena";
            List<string> result = new List<string>();
            for (int i = 0; i < 10; i++)
                result.Add("0");
            try
            {
                List<ZKHuellas> lista = new List<ZKHuellas>();
                using (TransactionScope tscTrans = new TransactionScope())
                {
                    lista = zkHuellasDao.ListarHuellasAlg10Usuario(x_iIdUsuario);
                    tscTrans.Complete();
                }

                lista.ForEach(x =>
                {
                    if (x.iFingerNumber <= 10 && x.iFingerNumber > 0)
                        result[x.iFingerNumber - 1] = "1";
                });
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
            return string.Join("", result.ToArray());
        }
        #endregion

        #region MÉTODOS TRANSACCIONALES
        public bool InsertarHuella(int x_iIdUsuario, List<ZKHuellas> x_lstHuellas)
        {
            string funcion = "InsertarHuella";
            bool result = false;
            try
            {
                using (TransactionScope tscTrans = new TransactionScope())
                {
                    result = zkHuellasDao.InsertarHuella(x_iIdUsuario, x_lstHuellas);
                    //if (result)
                    //{
                    //    //Grabando movimiento de usuario.
                    //    p_TSUsuarMovimEn.intIdEntid = result;
                    //    p_TSUsuarMovimEn.strDeTabla = "TMAlmac";
                    //    objTSUsuarMovimDA.Insertar(p_TSUsuarMovimEn);
                    //}
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


        public bool EliminarHuella(int x_iIdHuella, int x_iIdUsuario, int x_iFingerNumber)
        {
            string funcion = "EliminarHuella";
            bool result = false;
            try
            {
                using (TransactionScope tscTrans = new TransactionScope())
                {
                    result = zkHuellasDao.EliminarHuella(x_iIdHuella, x_iIdUsuario, x_iFingerNumber);
                    //if (result)
                    //{
                    //    //Grabando movimiento de usuario.
                    //    p_TSUsuarMovimEn.intIdEntid = result;
                    //    p_TSUsuarMovimEn.strDeTabla = "TMAlmac";
                    //    objTSUsuarMovimDA.Insertar(p_TSUsuarMovimEn);
                    //}
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