using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Transactions;
using Dominio.Entidades;
using Infraestructura.Data.SqlServer;
using System.Linq;

using Dominio.Entidades.Personalizado;
namespace Dominio.Repositorio
{
    public class RefComm
    {
        [DllImport("Matchdll.dll", EntryPoint = "process", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int process(string a, string b);
    }

    public class ZKUsuariosBL : IDisposable
    {
        private ZKUsuariosDAO zkUsuariosDao = new ZKUsuariosDAO();

        #region MÉTODOS NO TRANSACCIONALES
        public ZKUsuarios ListarUsuarioByCod(int x_iCodUsuario)
        {
            string funcion = "ListarUsuarioByID";
            ZKUsuarios result = new ZKUsuarios();
            try
            {
                using (TransactionScope tscTrans = new TransactionScope())
                {
                    result = zkUsuariosDao.ListarUsuarioByCod(x_iCodUsuario);
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
            catch (Exception ex)
            {
                UtilitarioBL.AlmacenarLogError(ex);
                Exception exc = new Exception("Ocurrió una excepción (" + funcion + ")");
                throw exc;
            }
            return result;
        }
        public ZKUsuarios ListarUsuarioByID(int x_iIdUsuario)
        {
            string funcion = "ListarUsuarioByID";
            ZKUsuarios result = new ZKUsuarios();
            try
            {
                using (TransactionScope tscTrans = new TransactionScope())
                {
                    result = zkUsuariosDao.ListarUsuarioByID(x_iIdUsuario);
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
        public ZKUsuarios IdentificarHuella(string x_template, ref int x_numDedo)
        {

            ZKUsuarios result = new ZKUsuarios();

            try
            {

                ZKHuellasDAO zkHuellaDao = new ZKHuellasDAO();

                int total = 0;
                List<ZKHuellas> lstTodasHuellas = zkHuellaDao.ListarHuellasAlg10Todo(1, 10000, out total);
                bool encontrado = false;
                int idUsuario = 0;
                int verificado = 0;
                x_numDedo = 0;
                for (int j = 0; j < lstTodasHuellas.Count; j++)
                {
                    //Sgt. Linea de verificación comentada para pruebas 17.06.2021 (descomentar antes de liberar)
                    //verificado = RefComm.process(lstTodasHuellas[j].Huella10, x_template); //指纹模板的对比 
                    verificado = RefComm.process(lstTodasHuellas[j].Huella10, x_template); //指纹模板的对比 --de prueba 21.06.2021
                    //// <inicio añadido para reemplazar VERIFICACION REFCOMM 17.06.2021 ES (borrar todo bloque)
                    //if (lstTodasHuellas[j].Huella10 == x_template)
                    //{
                    //    verificado = 1;
                    //}
                    //// <fin añadido para reemplazar VERIFICACION REFCOMM 17.06.2021 ES
                    if (verificado == 1)
                    {
                        x_numDedo = lstTodasHuellas[j].iFingerNumber;
                        idUsuario = lstTodasHuellas[j].iIdUsuario;
                        encontrado = true;
                        break;
                    }
                }

                if (encontrado)
                {
                    result = zkUsuariosDao.ListarUsuarioByID(idUsuario);
                }

            }
            catch (Exception ex)
            {
                UtilitarioBL.AlmacenarLogError(ex);
                //throw ex;
            }
            return result;
        }

        public List<ZKUsuarios> ListarUsuariosTodo(int x_idSede, int x_estadoActivo, int x_pagina, int x_tamanio, ref int x_total)
        {
            string funcion = "ListarUsuariosTodo";
            List<ZKUsuarios> result = new List<ZKUsuarios>();
            try
            {
                using (TransactionScope tscTrans = new TransactionScope())
                {
                    result = zkUsuariosDao.ListarUsuariosTodo(x_idSede, x_estadoActivo, x_pagina, x_tamanio, ref x_total);
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

        public List<ItemCombo> ListarMenuUsuarioCombo(int x_idSesion)
        {
            string funcion = "ListarMenuCombo";
            List<ItemCombo> result = new List<ItemCombo>();
            try
            {
                //ListZKMenu lstMenu = new ZKMenuDAO().ListarMenuLogin(x_idSesion);//modificado 22.06.2021
                ListZKMenuOff lstMenu = new ZKMenuDAO().ListarMenuLogin(x_idSesion);
                lstMenu.ToList().ForEach(x => {
                    result.Add(new ItemCombo() { Texto = x.StrDeMenus, Valor = string.Format("{0}-{1}", x.IntIdMenus, x.IntAsing) });
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

        #region MÉTODOS TRANSACCIONALES
        public bool InsertarUsuario(string x_dedosAntes, ref ZKUsuarios x_usuario, ref string x_mensaje)
        {
            string funcion = "InsertarUsuario";
            bool result = false;
            try
            {
                //validar las huellas registradas
                if (x_usuario.Huellas != null)
                {
                    bool repite = false;
                    int verificado = 0;
                    int i = 0;
                    //<INICIO COMENTARIO ES 11.06.2021>
                    while (i < x_usuario.Huellas.Count && !repite)
                    {
                        string huella = x_usuario.Huellas[i].Huella10;
                        for (int j = i + 1; j < x_usuario.Huellas.Count; j++)
                        {
                            //Sgt. Linea de verificación comentada para pruebas 17.06.2021 (descomentar antes de liberar)
                            verificado = RefComm.process(x_usuario.Huellas[j].Huella10, huella); //指纹模板的对比
                            //// <inicio añadido para reemplazar VERIFICACION REFCOMM 17.06.2021 ES (borrar todo bloque)
                            //if (x_usuario.Huellas[j].Huella10== huella)
                            //{
                            //    verificado = 1;
                            //}
                            //// <fin añadido para reemplazar VERIFICACION REFCOMM 17.06.2021 ES
                            if (verificado == 1)
                            {
                                repite = true;
                                continue;
                            }
                        }
                        i++;
                    }

                    if (repite)
                    {
                        x_mensaje = "Está registrando la misma huella para diferentes dedos.";
                        return false;
                    }


                    //Validar que la huella no pertenezca a otro usuario:
                    int numdedo = 0;
                    i = 0;
                    while (i < x_usuario.Huellas.Count)
                    {
                        string huella = x_usuario.Huellas[i].Huella10;
                        ZKUsuarios usuario = IdentificarHuella(huella, ref numdedo);

                        if (usuario != null && usuario.iIdUsuario > 0)
                            x_usuario.Huellas[i].ifingernumberzk5000 = 1;

                        i++;
                    }


                    string detalleMsg = "";
                    var repetidos = x_usuario.Huellas.FindAll(x => x.ifingernumberzk5000 > 0);
                    for (i = 0; i < repetidos.Count; i++)
                    {
                        if (i == 0)
                            detalleMsg = repetidos[i].iFingerNumber.ToString();
                        else if (i == repetidos.Count - 1)
                            detalleMsg = detalleMsg + " y " + repetidos[i].iFingerNumber.ToString();
                        else
                            detalleMsg = detalleMsg + ", " + repetidos[i].iFingerNumber.ToString();
                    }


                    if (detalleMsg != "")
                    {
                        if (repetidos.Count == 1)
                            x_mensaje = string.Format("La huella #{0} ya está registrada.", detalleMsg);
                        else
                            x_mensaje = string.Format("Las huellas {0} ya están registradas.", detalleMsg);
                        return false;
                    }
                    //<FIN COMENTARIO ES 11.06.2021>
                }
                using (TransactionScope tscTrans = new TransactionScope())
                {
                    result = zkUsuariosDao.InsertarUsuario(x_dedosAntes, ref x_usuario);
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
        public bool EliminarUsuario(int x_idUsuario, int x_idSesion, ref string x_mensaje)
        {
            string funcion = "EliminarUsuario";
            bool result = false;
            try
            {
                using (TransactionScope tscTrans = new TransactionScope())
                {
                    result = zkUsuariosDao.EliminarUsuario(x_idUsuario, x_idSesion);
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
        public bool CambiarEstadoUsuario(int x_idUsuario, int x_estado, int x_idSesion, ref ZKUsuarios x_usuario, ref string x_mensaje)
        {
            string funcion = "CambiarEstadoUsuario";
            bool result = false;
            try
            {
                using (TransactionScope tscTrans = new TransactionScope())
                {
                    result = zkUsuariosDao.CambiarEstadoUsuario(x_idUsuario, x_estado, x_idSesion);
                    x_usuario = zkUsuariosDao.ListarUsuarioByID(x_idUsuario);
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
        #endregion

        #region SINCRONIZACIÓN

        //public ListItemPersonaExport ListarPersonalExport(string x_strCodUnico, int x_idSede, int x_pagina, int x_numReg, ref int x_intTotalReg, ref string x_mensaje)
        //{
        //    string funcion = "ListarPersonalExport";
        //    ListItemPersonaExport result = new ListItemPersonaExport();
        //    try
        //    {
        //        result = new ZKUsuariosDAO().ListarPersonalExport(x_strCodUnico, x_idSede, x_pagina, x_numReg, ref x_intTotalReg, ref x_mensaje);
        //    }
        //    catch (SqlException ex)
        //    {
        //        UtilitarioBL.AlmacenarLogError(ex);
        //        Exception exc = new Exception("Ocurrió un error en BD (" + funcion + ")");
        //        throw exc;
        //    }
        //    catch (Exception ex)
        //    {
        //        UtilitarioBL.AlmacenarLogError(ex);
        //        Exception exc = new Exception("Ocurrió una excepción (" + funcion + ")");
        //        throw exc;
        //    }

        //    return result;
        //}

        //public string ListarPersonalInactivoExport(string x_strCodUnico, int x_idSede, ref string x_mensaje)
        //{
        //    string funcion = "ListarPersonalInactivoExport";
        //    string result = "";
        //    try
        //    {
        //        result = new ZKUsuariosDAO().ListarPersonalInactivoExport(x_strCodUnico, x_idSede, ref x_mensaje);
        //    }
        //    catch (SqlException ex)
        //    {
        //        UtilitarioBL.AlmacenarLogError(ex);
        //        Exception exc = new Exception("Ocurrió un error en BD (" + funcion + ")");
        //        throw exc;
        //    }
        //    catch (Exception ex)
        //    {
        //        UtilitarioBL.AlmacenarLogError(ex);
        //        Exception exc = new Exception("Ocurrió una excepción (" + funcion + ")");
        //        throw exc;
        //    }

        //    return result;
        //}

        //public ListItemHuellaExport ListarHuellasExport(string x_strCodUnico, int x_idSede, int x_pagina, int x_numReg, ref int x_intTotalReg, ref string x_mensaje)
        //{
        //    string funcion = "ListarHuellasExport";
        //    ListItemHuellaExport result = new ListItemHuellaExport();
        //    try
        //    {
        //        result = new ZKUsuariosDAO().ListarHuellasExport(x_strCodUnico, x_idSede, x_pagina, x_numReg, ref x_intTotalReg, ref x_mensaje);
        //    }
        //    catch (SqlException ex)
        //    {
        //        UtilitarioBL.AlmacenarLogError(ex);
        //        Exception exc = new Exception("Ocurrió un error en BD (" + funcion + ")");
        //        throw exc;
        //    }
        //    catch (Exception ex)
        //    {
        //        UtilitarioBL.AlmacenarLogError(ex);
        //        Exception exc = new Exception("Ocurrió una excepción (" + funcion + ")");
        //        throw exc;
        //    }

        //    return result;
        //}

        //public bool InsertarMarcasImport(ListItemMarcaImport x_lst, ref string x_mensaje)
        //{
        //    string funcion = "InsertarMarcasImport";
        //    bool result = false;
        //    try
        //    {
        //        result = new ZKUsuariosDAO().InsertarMarcasImport(x_lst, ref x_mensaje);
        //    }
        //    catch (SqlException ex)
        //    {
        //        UtilitarioBL.AlmacenarLogError(ex);
        //        Exception exc = new Exception("Ocurrió un error en BD (" + funcion + ")");
        //        throw exc;
        //    }
        //    catch (Exception ex)
        //    {
        //        UtilitarioBL.AlmacenarLogError(ex);
        //        Exception exc = new Exception("Ocurrió una excepción (" + funcion + ")");
        //        throw exc;
        //    }

        //    return result;
        //}

        //public void ActualizaFechaSincro(string x_codUnico)
        //{
        //    string funcion = "ActualizaFechaSincro";
        //    try
        //    {
        //        new ZKUsuariosDAO().ActualizaFechaSincro(x_codUnico);
        //    }
        //    catch (SqlException ex)
        //    {
        //        UtilitarioBL.AlmacenarLogError(ex);
        //        Exception exc = new Exception("Ocurrió un error en BD (" + funcion + ")");
        //        throw exc;
        //    }
        //    catch (Exception ex)
        //    {
        //        UtilitarioBL.AlmacenarLogError(ex);
        //        Exception exc = new Exception("Ocurrió una excepción (" + funcion + ")");
        //        throw exc;
        //    }
        //}
        //public int ObtieneConfigValores(ref int x_tiempoSincro)
        //{
        //    string funcion = "ObtieneConfigValores";
        //    try
        //    {
        //        return new ZKUsuariosDAO().ObtieneConfigValores(ref x_tiempoSincro);
        //    }
        //    catch (SqlException ex)
        //    {
        //        UtilitarioBL.AlmacenarLogError(ex);
        //        Exception exc = new Exception("Ocurrió un error en BD (" + funcion + ")");
        //        throw exc;
        //    }
        //    catch (Exception ex)
        //    {
        //        UtilitarioBL.AlmacenarLogError(ex);
        //        Exception exc = new Exception("Ocurrió una excepción (" + funcion + ")");
        //        throw exc;
        //    }
        //}

        //public ListItemErrorCarga InsertMasivo(ListItemPersonaImport x_lst, ref int x_intInsertados, ref int x_intErrores, ref string x_mensaje)
        //{
        //    string funcion = "InsertMasivo";
        //    ListItemErrorCarga result = new ListItemErrorCarga();
        //    try
        //    {
        //        using (TransactionScope tscTrans = new TransactionScope())
        //        {
        //            result = zkUsuariosDao.InsertMasivo(x_lst, ref x_intInsertados, ref x_intErrores);
        //            tscTrans.Complete();
        //        }

        //    }
        //    catch (SqlException ex)
        //    {
        //        x_mensaje = ex.Message;
        //        UtilitarioBL.AlmacenarLogError(ex);
        //    }
        //    catch (TransactionManagerCommunicationException ex)
        //    {
        //        UtilitarioBL.AlmacenarLogError(ex);
        //        Exception exc = new Exception("Ocurrió un error con el administrador de comunicaciones (" + funcion + ")");
        //        throw exc;
        //    }
        //    catch (TransactionInDoubtException ex)
        //    {
        //        UtilitarioBL.AlmacenarLogError(ex);
        //        Exception exc = new Exception("Ocurrió un error al confirmar una transacción dudosa (" + funcion + ")");
        //        throw exc;

        //    }
        //    catch (TransactionAbortedException ex)
        //    {
        //        UtilitarioBL.AlmacenarLogError(ex);
        //        Exception exc = new Exception("Ocurrió un error al intentar confirmar una transacción inexistente (" + funcion + ")");
        //        throw exc;
        //    }
        //    catch (TransactionException ex)
        //    {
        //        UtilitarioBL.AlmacenarLogError(ex);
        //        Exception exc = new Exception("Ocurrió un error al intentar confirmar una transacción inexistente (" + funcion + ")");
        //        throw exc;
        //    }
        //    catch (Exception ex)
        //    {
        //        UtilitarioBL.AlmacenarLogError(ex);
        //        Exception exc = new Exception("Ocurrió una excepción (" + funcion + ")");
        //        throw exc;
        //    }
        //    return result;
        //}
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