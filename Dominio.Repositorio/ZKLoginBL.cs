using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Transactions;
using Dominio.Entidades;
using Infraestructura.Data.SqlServer;

//Para Registrar Licencia
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;
using System.Net.Mail;
using System.Text;


namespace Dominio.Repositorio
{
    public class ZKLoginBL : IDisposable
    {
        private ZKLoginDAO zkLoginDao = new ZKLoginDAO();
        //private TSConfiDAO objDao = new TSConfiDAO();

        #region "Métodos NO Transaccionales"
        public ZKLoginOff ListarLoginByCod(string x_NombreSesion, string x_Password, ref string x_mensaje)
        //public ZKLogin ListarLoginByCod(string x_NombreSesion, string x_Password, ref string x_mensaje)
        {
            string funcion = "ListarLoginByCod";
            ZKLoginOff result = new ZKLoginOff();
            //ZKLogin result = new ZKLogin();
            try
            {
                using (TransactionScope tscTrans = new TransactionScope())
                {
                    result = zkLoginDao.ListarLoginByCod(x_NombreSesion, x_Password, ref x_mensaje);
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
        public ListZKLoginOff ListarLogin(int x_idSesion)//modificado  ListZKLogin
        {
            string funcion = "ListarLogin";
            ListZKLoginOff result = new ListZKLoginOff();//modificado ListZKLogin
            try
            {

                using (TransactionScope tscTrans = new TransactionScope())
                {
                    result = zkLoginDao.ListarLogin(x_idSesion);
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
        //añadido 22.06.2021
        public ZKLoginOff ListarLoginID(int x_idSesion)
        {
            using (ZKLoginBL objBL = new ZKLoginBL())
            {
                ListZKLoginOff lst = objBL.ListarLogin(x_idSesion);
                if (lst.Count > 0)
                    return lst[0];
                return new ZKLoginOff();
            }
        }

        //añadido 15.06
        public ListZKLoginOff ListarUsers_(int x_idSesion)//añadido 15.06
                                                          //public ListZKLogin ListarUsers_(int x_idSesion)//añadido 15.06
        {
            string funcion = "ListarUsers_";
            ListZKLoginOff result = new ListZKLoginOff();
            //ListZKLogin result = new ListZKLogin();
            try
            {
                using (TransactionScope tscTrans = new TransactionScope())
                {
                    result = zkLoginDao.ListarUsers_(x_idSesion);
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
        //añadido 16.06
        public ListZKMenuOff ListarMenuUsers_(int x_idSesion)//añadido 15.06
                                                             //public ListZKLogin ListarUsers_(int x_idSesion)//añadido 15.06
        {
            string funcion = "ListarMenuUsers_";
            ListZKMenuOff result = new ListZKMenuOff();
            //ListZKLogin result = new ListZKLogin();
            try
            {
                using (TransactionScope tscTrans = new TransactionScope())
                {
                    result = new ZKMenuDAO().ListarMenuLogin_(x_idSesion);
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
        #endregion

        #region Metodos ZK
        public bool InsertarLogin(ref ZKLoginOff x_login, string x_menus, ref string x_mensaje)//modificado ZkLogin
        {
            string funcion = "InsertarLogin";
            bool result = false;
            try
            {
                using (TransactionScope tscTrans = new TransactionScope())
                {
                    result = zkLoginDao.InsertarLogin(ref x_login, x_menus);
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
        public bool EliminarLogin(int x_idSesion, ref string x_mensaje)
        {
            ZKLoginOff login = new ZKLoginOff();//modificado ZkLogin
            login.iIdSesion = x_idSesion;
            login.bitFlActivo = false;

            login.intEsAdmin = 0;
            login.intIdSede = 0;
            login.iPermiso = 0;
            login.sNombreSesion = "";
            login.sPasswordSesion = "";
            login.sPermisoMenu = "";
            login.strSede = "";

            return InsertarLogin(ref login, "", ref x_mensaje);
        }
        public bool CambiarSede(int x_idSesion, int x_idSede, string x_menus, ref string x_mensaje)
        {
            ZKLoginOff login = new ZKLoginOff();//modificado ZkLogin
            login.iIdSesion = x_idSesion;
            login.intIdSede = x_idSede;
            login.bitFlActivo = true;

            login.intEsAdmin = 0;
            login.iPermiso = 0;
            login.sNombreSesion = "";
            login.sPasswordSesion = "";
            login.sPermisoMenu = "";
            login.strSede = "";

            return InsertarLogin(ref login, x_menus, ref x_mensaje);

        }
        public bool CambiarContraseña(int x_idSesion, string x_sPasswordSesion, ref string x_mensaje)
        {
            ZKLoginOff login = new ZKLoginOff();//modificado ZkLogin
            login.iIdSesion = x_idSesion;
            login.sPasswordSesion = x_sPasswordSesion;
            login.bitFlActivo = true;

            login.intEsAdmin = 0;
            login.intIdSede = 0;
            login.iPermiso = 0;
            login.sNombreSesion = "";
            login.sPermisoMenu = "";
            login.strSede = "";

            return InsertarLogin(ref login, "", ref x_mensaje);
        }
        //añadido 28.06.2021: Eduardo solicita llevar registro por IP de quien ya descargó Driver (No hay forma de saber si se instaló o no).
        public bool DescargaDriver(int x_idSesion,string strIpHost, ref string x_mensaje, ref int x_rpta)
        {
            string funcion = "DescargaDriver";
            bool result = false;
            try
            {
                using (TransactionScope tscTrans = new TransactionScope())
                {
                    result = zkLoginDao.DescargaDriver(x_idSesion, strIpHost, ref x_mensaje, ref x_rpta);
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

        //añadido 02.11.2021 HGM
        public bool ConsultarDescargaDriver(int x_idSesion, string strIpHost, ref string x_mensaje, ref int x_rpta)
        {
            string funcion = "DescargaDriver";
            bool result = false;
            try
            {
                using (TransactionScope tscTrans = new TransactionScope())
                {
                    result = zkLoginDao.ConsultarDescargaDriver(x_idSesion, strIpHost, ref x_mensaje, ref x_rpta);
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

        #region registrar licencia HGM Añadido 20.10.2021 

        //private ZKLoginDAO objZkLoginDao = new ZKLoginDAO();
        ///////////////////////////////////////////////////////////////
        // 3.25 VALIDAR EL SERVER /7https://www.base64encode.net/
        ///////////////////////////////////////////////////////////////
        public string ValidaServer(string Cadena, ref int Oper)//string Cadena, int Oper: 0 = Validación LOGIN | 1 = Registrar Token
        {



            ////try
            ////{
            ////    throw new DivideByZeroException();
            ////}
            ////catch (DivideByZeroException ex)
            ////{
            ////    UtilitarioBL.AlmacenarLogError(ex);
            ////    UtilitarioBL.AlmacenarLogMensaje("prueba", "mensaje 02.11.2021");
            ////}




            string strMsjUsuario = "";
            //obtener datos de configuraciónBD
            Session_Movi objSession = new Session_Movi();           
            //UsuarioDAO objUser = new UsuarioDAO();  //UsuarioDAO objUser = new UsuarioDAO();

            objSession.intIdSesion = 1;
            objSession.intIdSoft = 6;// 1;
            objSession.intIdMenu = 1;

            //Variables para:
            string IDEncrypt = "";
            string IP = "";
            string MAC = "";
            bool EsQA = false; //añadido 05.07.2021
            bool ValidaQA = false; //añadido 05.07.2021
            string strMsjValidaQA = ""; //añadido 05.07.2021
            int ipPcVersusIpRegistado = 1 ; //añadido 29.10.2021 HGM  ----> 0 : IPs no coincide con el ip registrado en el serial encriptado, 1: Si coinciden

            try
            {
                #region Encapsulado_

                ////OBTENER IP ACTIVA
                #endregion Encapsulado_
                //Metodo Encapsulado fuera de este metodo
                //Para conultar si este servidor(esta pc) tiene Serial
                ObtServer(ref IP, ref MAC);//  Denuelve el IP y la MAC de mi pc

                if (IP != null && IP != "")
                {
                    string IPDesencrypt = "";
                    string MACDesencrypt = "";
                    string Sufi_ = "";
                    int Sum_ = 0;

                    if (Oper == 0)
                    {
                        //obtener valor de ID desde la tabla Configuraciones para verificar si ya esta registrado.
                        List<TSConfi> lista = new List<TSConfi>();
                        lista = ListarConfig(objSession, "%_SERVICE", ref strMsjUsuario);

                        for (int i = 0; i < lista.Count(); i++)

                        {
                            if (lista[i].strCoConfi == "ID_SERVICE")
                            {
                                IDEncrypt = lista[i].strValorConfi;
                                // SERIAL QA BETSABE  : 192X168X001X158X000C2951FE3C519 || MTkyWDE2OFgwMDFYMTU4WDAwMEMyOTUxRkUzQzUxOQ== (31 caracteres)
                                // SERIAL HGM mi dell : 192X168X001X103XF8DA0C05735D464 || MTkyWDE2OFgwMDFYMTAzWEY4REEwQzA1NzM1RDQ2NA==
                            }
                        }
                    }
                    else
                    {//Obtener valor de input de la ventana
                        IDEncrypt = Cadena;
                    }



                    /////////////////////////////////////////////////////////////////////////////////////////
                    //LO SIGUIENTE VALIDA A FECHA DE PERIODO (DE PRUEBA) QUE DEPENDE DE LO QUE ESTA DENTRO 
                    //DE LA CADENA ENCRIPTADA 
                    //PREGUNGTAR SI IRA EL PERIODO DE PRUEBA A ELI ---> 21.10.2021 Queda comentado


                    //CUANDO LA CONSULTA AL LISTADO SI CONTIENE ALGUNA CADENA
                    if (IDEncrypt != "" && IDEncrypt != null)
                    {
                       
                        string IDDesencrypt = DesencriptarPassword(IDEncrypt);//string IDDesencrypt = objUser.DesencriptarPassword(IDEncrypt);

                        //Token de Licencia estándar:29-32 [ 16 + 12 + (1-4) e iniciando con números.
                        //Token de Licencia Prueba:41-44 [ 12 + 16 + 12 + (1-4) e iniciando con Q__A, donde los 2 primeros digitos son la cantidad de dias seguido de DDMMAAAA de prueba están en medio.

                        if (IDDesencrypt.Substring(0, 1) == "Q") //Deberia llegar como "OTJYMTY4WDAwMVgxMDNYRjhEQTBDMDU3MzVENDY0"???
                        {//Token de Licencia Prueba:41-44 [ 12 + 16 + 12 + (1-4) e iniciando con Q__A, donde los 2 primeros digitos son la cantidad de dias seguido de DDMMAAAA de prueba están en medio.
                            EsQA = true;
                            int Dias = Convert.ToInt32(IDDesencrypt.Substring(1, 2)) + 1;//extrayendo la cantidad de días
                            string FecIni = IDDesencrypt.Substring(3, 8);//extrayendo la fechaInicio
                            string Fec = FecIni.Substring(0, 2) + "/" + FecIni.Substring(2, 2) + "/" + FecIni.Substring(4, 4);
                            DateTime DateIni = DateTime.ParseExact(Fec, "dd/MM/yyyy", null);
                            DateTime DateFin = DateIni.AddDays(Dias);
                            DateTime DateNow = DateTime.Now;

                            //Comparar la fecha para ver si aun esta entre los dias habiles para usar la licencia de prueba
                            if (DateNow <= DateFin && DateNow >= DateIni)
                            {
                                ValidaQA = true;//Dependiendo de la fecha devuelve un true 
                            }
                            else
                            {
                                ValidaQA = false;
                                strMsjValidaQA = "El Periodo de Prueba caducó el " + DateFin.ToString();
                            }

                            IDDesencrypt = IDDesencrypt.Substring(12, (IDDesencrypt.Length - 12));//quitandole los primeros 12 digitos
                        }
                        else
                        {//Token de Licencia estándar:29-32 [ 16 + 12 + (1-4) e iniciando con números.
         
                            ipPcVersusIpRegistado = 0 ; //----> "El serial registrado no coincide con el Servidor.";
                        }


                        if (IDDesencrypt.Length >= 29)
                        {
                            string[] IDs = IDDesencrypt.Substring(0, 15).Split('X');
                            MACDesencrypt = IDDesencrypt.Substring(16, 12);
                            Sufi_ = IDDesencrypt.Substring(28, (IDDesencrypt.Length - 28));

                            foreach (string Id in IDs)
                            {
                                IPDesencrypt = IPDesencrypt + Convert.ToInt32(Id).ToString() + '.'; //192.168.1.158.
                                Sum_ = Sum_ + Convert.ToInt32(Id);
                            }
                            IPDesencrypt = IPDesencrypt.Substring(0, (IPDesencrypt.Length - 1));  ///OK
                        }

                        if (MACDesencrypt != "" && IPDesencrypt != "" && Sufi_ != "")
                        {
                            int Sufi_int = Int16.Parse(Sufi_);

                            if (MAC == MACDesencrypt && IP == IPDesencrypt && Sufi_int == Sum_)//Sufi_ == Sum_.ToString()
                            {
                                strMsjUsuario = "";
                            }
                            else
                            {
                                Oper = 4; //Valor solo para mostrar el mensaje em el tipo de control del "Login" y no del modal


                                if (ipPcVersusIpRegistado == 0) { //IF AÑADIDO PARA HUELLASWEB HGM 29.10.2021
                                    strMsjUsuario = "El serial de la Licencia registrada es incorrecta.";
                                }
                                else { 

                                strMsjUsuario = "Servidor no Autorizado.";

                                }



                            }
                        }
                        else
                        {
                            strMsjUsuario = "Token erróneo";
                        }
                        /**********************************************************************
                       ***********************************************************************/


                    }

                    else
                    {
                        if (Oper == 0) //Para el Login = 0 y Para CTRL + X = 1 
                        {
                            //Se enviará al correo de Tecflex para Soporte. 
                            //Ya que al consultar en el listado de configuraciones no se encontro grabado en el 
                            //campo  "sCadena" ---> 192X168X001X103XF8DA0C05735D464 (mi pc dell) que debe estar 
                            //encriptado de  como base64  ---> MTkyWDE2OFgwMDFYMTAzWEY4REEwQzA1NzM1RDQ2NA==   

                            string strMsjDB      = "";
                            string strTipo       = "REGSERVER"; //Registrar
                            int    intResult     = 0;
                            string CorreoDestino = "";
                            string Cliente       = "";
                            List<TSConfi> lista = new List<TSConfi>();
                            //Consulta a la tabla Configuraciones de la bd
                            lista = ListarConfig(objSession, "%_CBX", ref strMsjUsuario);

                            for (int i = 0; i < lista.Count(); i++)
                            {
                                if (lista[i].strCoConfi == "EMAIL_CBX")
                                {
                                    CorreoDestino = lista[i].strValorConfi;
                                }
                                if (lista[i].strCoConfi == "OC_RUCCLI_CBX")
                                {
                                    Cliente = lista[i].strValorConfi;//Espera una cadena encriptada
                                }
                            }

                            /*
                            string CliDesencriptado = DesencriptarPassword(Cliente);//string CliDesencriptado = objUser.DesencriptarPassword(Cliente);
                            string[] subs = CliDesencriptado.Split('|');
                            */

                            string[] subs = Cliente.Split('|');
                            CorreoEmp obj = new CorreoEmp();
                            obj.intIdPersonal = 0;
                            obj.strOC         = subs[0];       //ORDEN COMPRA
                            obj.strRUC        = subs[1];       //RUC
                            obj.strDestinos   = CorreoDestino; //destinatarios
                            string[] subsIP   = IP.Split('.');
                            string sCadena    = "";
                            int Sum = 0;
                            foreach (string subIP in subsIP)
                            {
                                sCadena = sCadena + (Convert.ToInt32(subIP) + 1000).ToString().Substring(1, 3) + "X";
                                Sum = Sum + Convert.ToInt32(subIP);
                            }
                            sCadena = sCadena + MAC + Sum; // IP:15 dígitos + MAC: 12 dígitos + Sufijo (n dígitos)
                            obj.strCadena = sCadena;

                            if (sCadena != null && sCadena != "")
                            {
                                //enviarCorreo(objSession, obj, strTipo, ref intResult, ref strMsjDB, ref strMsjUsuario);
                                enviarCorreoObtenerServerLogin_SAF(objSession, obj, strTipo, ref intResult, ref strMsjDB, ref strMsjUsuario);
                            }

                            if (intResult == 1)
                            {
                                Oper = 4; //solo para devolder un valor al Controlador cuando pase por este punto
                                strMsjUsuario = "Servidor sin Licencia registrada (se envió correo a Tecflex SAC)";
                            }

                            else
                            {
                                Oper = 4;
                                strMsjUsuario = "Servidor sin Licencia registrada.";
                            }


                        }
                    }
                }
                else
                {
                    strMsjUsuario = "Servidor no tiene Serial";
                }


                /*************************************************************************
                 * NOTA: Esta parte en el sitema Huellas Web no irá
                 * ***********************************************************************
                **************************************************************************/
                //Validación Complementaria de QA: Periodo de Prueba ---- DE PRUEBA
                if (EsQA)
                {
                    if (strMsjUsuario == "" && ValidaQA == false)
                    {
                        Oper = 4; //solo para devolder un valor al Controlador cuando pase por este punto
                        strMsjUsuario = strMsjValidaQA;
                    }
                }


                return strMsjUsuario;
            }
            catch (SqlException ex)
            {
                //Log.AlmacenarLogError(ex, "TSConfiBL.cs: SqlException"); //C_HGM
                UtilitarioBL.AlmacenarLogError(ex); // Añadido HGM log 02.11.2021
                throw new Exception("Ocurrió un error en BD (ObtenerServer)");
            }
            catch (Exception ex)
            {
                //Log.AlmacenarLogError(ex, "TSConfiBL.cs: Exception");//C_HGM
                UtilitarioBL.AlmacenarLogError(ex); // Añadido HGM log 02.11.2021
                throw new Exception("Error General (ObtenerServer)");
            }
        }

        ///////////////////////////////////////////////////////////////
        // 3.25 ENCAPSULADO --> Para obtener valores de hardware la pc 
        ///////////////////////////////////////////////////////////////
        public void ObtServer(ref string IP, ref string MAC)
        {
            try
            {
                //OBTENER IP ACTIVA
                //-------------------------------------------------
                // Get the list of network interfaces for the local computer.
                var adapters = NetworkInterface.GetAllNetworkInterfaces();
                for (int i = 0; i < adapters.Count(); i++)
                {
                    //Log.AlmacenarLogMensaje("Adapter " + i.ToString() + ": " + adapters[i].Description.ToString() + "| " + adapters[i].NetworkInterfaceType.ToString() + "| " + adapters[i].OperationalStatus.ToString() + "| " + adapters[i].SupportsMulticast.ToString()); C_HGM

                }

                string Host = Dns.GetHostName();
                //Log.AlmacenarLogMensaje("HostName: " + Host); C_HGM
                if (adapters != null)
                {
                    //Log.AlmacenarLogMensaje("Adapter no es nulo");

                    var A = (from adapter in adapters
                             let properties = adapter.GetIPProperties()
                             from address in properties.UnicastAddresses
                             where adapter.OperationalStatus == OperationalStatus.Up
                             //&& adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet //añadido manualmente 02.07.2021
                             && address.Address.AddressFamily == AddressFamily.InterNetwork
                             && !address.Equals(IPAddress.Loopback)
                             && address.DuplicateAddressDetectionState == DuplicateAddressDetectionState.Preferred
                             && properties.GatewayAddresses.Count >= 1 //añadido 06.07.2021
                             //&& address.AddressPreferredLifetime != UInt32.MaxValue //se comenta porque no funciona en la 45 
                             select address.Address);
                    var B = (from adapter in adapters
                             let properties = adapter.GetIPProperties()
                             from address in properties.UnicastAddresses
                             where adapter.OperationalStatus == OperationalStatus.Up
                             //&& adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet //añadido manualmente 02.07.2021
                             && address.Address.AddressFamily == AddressFamily.InterNetwork
                             && !address.Equals(IPAddress.Loopback)
                             && address.DuplicateAddressDetectionState == DuplicateAddressDetectionState.Preferred
                             && properties.GatewayAddresses.Count >= 1 //añadido 06.07.2021
                             //&& address.AddressPreferredLifetime != UInt32.MaxValue //se comenta porque no funciona en la 45 
                             select adapter.GetPhysicalAddress());

                    if (A != null && A.Count() > 0)
                    {
                        //Log.AlmacenarLogMensaje("Objeto Adapter si tiene redes");

                        for (int i = 0; i < A.Count(); i++)
                        {
                            //Log.AlmacenarLogMensaje("A: " + A.ElementAt(i).ToString() + "| Total Redes: " + A.Count());

                            if (i == 0)
                            {
                                //Nota: si en caso el adapters tiene más de una coincidencia se toma la primera diferente de 127.0.0.1
                                IP = A.ElementAt(i).ToString();
                                MAC = B.ElementAt(i).ToString();
                                //NameRed = C.ElementAt(i).ToString();
                                //Log.AlmacenarLogMensaje(MAC + " | " + IP); //C_HGM
                            }

                        }
                    }
                    else
                    {
                        //Log.AlmacenarLogMensaje("Objeto Adapter no tiene redes" + " | Total Redes: " + A.Count()); C_HGM
                    }

                }

            }
            catch (SqlException ex)
            {
                UtilitarioBL.AlmacenarLogError(ex); // Añadido HGM log 02.11.2021
                throw new Exception("Ocurrió un error en BD (ObtServer)");
            }
            catch (Exception ex)
            {
                UtilitarioBL.AlmacenarLogError(ex); // Añadido HGM log 02.11.2021
                throw new Exception("Error General (ObtServer)");
            }
        }

        ///////////////////////////////////////////////////////////////
        // LISTAR LOS VALORES DE LA CONFIGURACIONES
        ///////////////////////////////////////////////////////////////
        //1.16 y 3.22 hgm
        public List<TSConfi> ListarConfig(Session_Movi objSession, string strCoConfi, ref string strMsjUsuario)
        {
            List<TSConfi> lista = new List<TSConfi>();
            try
            {
                int intResult = 0;
                string strMsjDB = "";

                lista = zkLoginDao.ListarConfig(objSession, strCoConfi, ref intResult, ref strMsjDB, ref strMsjUsuario);
                if (intResult == 0)
                {
                    if (!strMsjDB.Equals(""))
                    {
                        //Log.AlmacenarLogMensaje("[ListarConfig] => Respuesta del Procedimiento : " + strMsjDB); //CHMG
                        if (strMsjUsuario.Equals(""))
                            strMsjUsuario = strMsjDB;
                    }
                }
            }
            catch (SqlException ex)
            {
                UtilitarioBL.AlmacenarLogError(ex); // Añadido HGM log 02.11.2021
                throw new Exception("Ocurrió un error en BD (ListarConfig)");
            }
            catch (Exception ex)
            {
                UtilitarioBL.AlmacenarLogError(ex); // Añadido HGM log 02.11.2021
                throw new Exception("Error General (ListarConfig)");
            }
            return lista;
        }


        ///////////////////////////////////////////////////////////////
        // METODO DESENCRIPTAR
        ///////////////////////////////////////////////////////////////
        //3.21 Desencriptar el String que se ingreso en el input "Registrar Token" 
        //ejemplo:"OTJYMTY4WDAwMVgxMDNYRjhEQTBDMDU3MzVENDY0"
        public string DesencriptarPassword(string strCoPassw)
        {
            string strcontraseña = "";
            byte[] b = Convert.FromBase64String(strCoPassw);
            strcontraseña = System.Text.Encoding.UTF8.GetString(b);
            return strcontraseña;
        }

        ///////////////////////////////////////////////////////////////
        // METODO DESENCRIPTAR
        ///////////////////////////////////////////////////////////////
        //3.20
        public string EncriptarPassword(string strCoPassw)
        {
            //Codificar contraseña actual para validación
            string strcontraseña = "";
            byte[] byt = System.Text.Encoding.UTF8.GetBytes(strCoPassw);
            strcontraseña = Convert.ToBase64String(byt);
            return strcontraseña;
        }

        ///////////////////////////////////////////////////////////////
        // 3.22.2.- ENVIO DE CORREO A SOPORTE TECNICO ---> Copiado del //5.43
        ///////////////////////////////////////////////////////////////
        //private ImportarExcelDAO objDao_ = new ImportarExcelDAO();
        private void enviarCorreoObtenerServerLogin_SAF(Session_Movi objSession, CorreoEmp obj, string strTipo, ref int intResult, ref string strMsjDB, ref string strMsjUsuario)
        {
            //Datos del Servidor de Correo
            List<EnCorreo> lsCorreoDatos = zkLoginDao.obtenerDatosCorreo(objSession, ref intResult, ref strMsjDB, ref strMsjUsuario);

            string host    = lsCorreoDatos[0].strhost;
            string puerto  = lsCorreoDatos[0].strpuerto;
            string ccorreo = lsCorreoDatos[0].strccorreo;
            string cpass   = lsCorreoDatos[0].strcpass;
            string cde     = lsCorreoDatos[0].strremitente;
            bool auth      = lsCorreoDatos[0].bitAutentificacion;

            MailMessage msg = new MailMessage();

            try
            {

                //ObtenerServer rutas
                //string root = Server.MapPath("~" + strRutaArchivos);//

                // Create file attachment
                Attachment ImageAttachment = new Attachment(AppDomain.CurrentDomain.BaseDirectory + "App_Data\\DirLogos\\logo.png");
                // Set the ContentId of the attachment, used in body HTML
                ImageAttachment.ContentId = "logo";
                msg.Attachments.Add(ImageAttachment);
                //UsuarioBL UserBL = new UsuarioBL();
                //StringBuilder datos = UserBL.htmlMessageBodyObtenerServerLogin_SAF(obj, strTipo);
                StringBuilder datos = htmlMessageBodyObtenerServerLogin_SAF(obj, strTipo);
                //receptores Tecflex
                string[] subs = obj.strDestinos.Split(';');
                foreach (string sub in subs)
                {
                    msg.To.Add(new MailAddress(sub));
                }
                msg.CC.Add(new MailAddress("sisactivofijo@gmail.com"));//cambiar correo  -->  siscopweb@gmail.com

                //Remitente
                msg.From = new MailAddress(ccorreo);

                //Titulo
                msg.Subject = "Generar Licencia";

                //Cuerpo de correo
                msg.Body = datos.ToString();

                msg.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = host;
                smtp.Port = Int32.Parse(puerto);
                smtp.EnableSsl = auth;
                smtp.UseDefaultCredentials = true;
                //usuario y clave
                smtp.Credentials = new NetworkCredential(ccorreo, cpass);

                smtp.Send(msg);

                intResult = 1;
                strMsjUsuario = "El correo con la solicitud fue enviado correctamente.";
            }

            catch (Exception ex)
            {
                intResult = 3;
                strMsjUsuario = "Ocurrió un error al enviar el correo";
                UtilitarioBL.AlmacenarLogError(ex); // Añadido HGM log 02.11.2021
            }
        }

        ///////////////////////////////////////////////////////////////
        // 3.22.3.- OBTENER EL CUERPO DEL CORREO ---> Copiado del//3.17
        ///////////////////////////////////////////////////////////////
        //private ImportarExcelDAO objDaoImpExc = new ImportarExcelDAO();
        public StringBuilder htmlMessageBodyObtenerServerLogin_SAF(CorreoEmp obj, string filtro)//private cambiado a public 01.07.2021
        {
            StringBuilder strB = new StringBuilder();

            int intResult = 0;
            string strMsjDB = "";
            string strMsjUsuario = "";
            TEXTOCORREO objTexto = new TEXTOCORREO();

            //Obtener el texto del correo como objeto
            objTexto = zkLoginDao.GetTextoCorreoObj(filtro, obj, 0, false, "", ref intResult, ref strMsjDB, ref strMsjUsuario);

            strB.AppendLine("<html>");
            strB.AppendLine("<body>");
            strB.AppendLine("<img src=cid:logo />");
            strB.AppendLine("<br>");
            strB.AppendLine("<div>");
            strB.AppendLine("<span style='font-size: large;'>" + objTexto.saludo + "</span>");
            strB.AppendLine("</div>");
            strB.AppendLine("<br>");
            strB.AppendLine("<div>");
            if (!objTexto.texto1.Equals(""))
            {
                strB.AppendLine("<span>");
                strB.AppendLine(objTexto.texto1);
                strB.AppendLine("</span>");
                strB.AppendLine("<br>");
            }
            if (!objTexto.texto2.Equals(""))
            {
                strB.AppendLine("<span>");
                strB.AppendLine(objTexto.texto2);
                strB.AppendLine("</span>");
                strB.AppendLine("<br>");
            }
            if (!objTexto.texto3.Equals(""))
            {
                strB.AppendLine("<span>");
                strB.AppendLine(objTexto.texto3);
                strB.AppendLine("</span>");
                strB.AppendLine("<br>");
            }
            if (!objTexto.texto4.Equals(""))
            {
                strB.AppendLine("<span>");
                strB.AppendLine(objTexto.texto4);
                strB.AppendLine("</span>");
                strB.AppendLine("<br>");
            }
            if (!objTexto.texto5.Equals(""))
            {
                strB.AppendLine("<span>");
                strB.AppendLine(objTexto.texto5);
                strB.AppendLine("</span>");
                strB.AppendLine("<br>");
            }
            strB.AppendLine("</div>");
            strB.AppendLine("<br>");
            strB.AppendLine("<div>");
            strB.AppendLine(objTexto.despedida);
            strB.AppendLine("</div>");

            strB.AppendLine("<br>");
            strB.AppendLine("<br>");
            strB.AppendLine("<br>");
            strB.AppendLine("</div>");
            if (!objTexto.pie1.Equals(""))
            {
                strB.AppendLine("<span>");
                strB.AppendLine(objTexto.pie1);
                strB.AppendLine("</span>");
                strB.AppendLine("<br>");
            }
            if (!objTexto.pie2.Equals(""))
            {
                strB.AppendLine("<span>");
                strB.AppendLine(objTexto.pie2);
                strB.AppendLine("</span>");
                strB.AppendLine("<br>");
            }
            if (!objTexto.pie3.Equals(""))
            {
                strB.AppendLine("<span>");
                strB.AppendLine(objTexto.pie3);
                strB.AppendLine("</span>");
                strB.AppendLine("<br>");
            }
            strB.AppendLine("</div>");
            strB.AppendLine("</body>");
            strB.AppendLine("</html>");

            return strB;
        }

        ///////////////////////////////////////////////////////////////
        // 3.23.1.- ----> 04.07.21
        ///////////////////////////////////////////////////////////////
        // 13:07   CTRL + 1 "Generar Token de Prueba"  int Oper = 3   --OPER = 3 
        public string GenerarServerEncriptado(Session_Movi objSesion, ref int intRpta, string sCadena, int Oper)//1: Encriptar // 2: Registrar //3: ?? Toquen de prueba
        {
            string strMsjUsuario = "";
            try
            {
                Session_Movi objSession = new Session_Movi();
                //UsuarioDAO objUser = new UsuarioDAO();

                objSession.intIdSesion = 1;
                objSession.intIdSoft = 6;// 1;   6 para sisactivofijo
                objSession.intIdMenu = 1;
                string IDServidor = "";
                string Suma_ = "";
                bool Valida = false;
                bool EsQA = false;//añadido 06.07.2021

                /*********************************************************************
                         "Oper == 2" Registra con el encriptado desde lo ingresado en  
                          el html para grabarlo en la tabla de configuraciones.
                **********************************************************************/
                if (Oper == 2) //Registrar Licencia/Token encriptado que ya se ha generado  (Primero se obtenedrá el valor "Suma_")
                {
                    strMsjUsuario = ValidaServer(sCadena, ref Oper);//Validando si es igual a la del Servidor Actual.

                    if (strMsjUsuario != "")
                    {
                        strMsjUsuario = "Token Incorrecto";
                        Valida = false;
                    }
                    else
                    {
                        //--------------------------------------------------------------------------------------------------
                        IDServidor = sCadena;
                        sCadena = DesencriptarPassword(sCadena); //objUser.
                        //Token de Licencia estándar:29-32 [ 16 + 12 + (1-4) e iniciando con números.
                        //Token de Licencia Prueba:41-44 [ 12 + 16 + 12 + (1-4) e iniciando con Q__A, donde los 2 primeros digitos son la cantidad de dias seguido de DDMMAAAA de prueba están en medio.

                        if (sCadena.Substring(0, 1) == "Q")
                        {//Token de Licencia Prueba:41-44 [ 12 + 16 + 12 + (1-4) e iniciando con Q__A, donde los 2 primeros digitos son la cantidad de dias seguido de DDMMAAAA de prueba están en medio.
                            EsQA = true;
                            sCadena = sCadena.Substring(12, (sCadena.Length - 12));//quitandole los primeros 12 digitos
                        }
                        //------------------------------------------
                        //sCadena = objUser.DesencriptarPassword(sCadena);
                        Suma_ = sCadena.Substring(28, sCadena.Length - 28);
                        Valida = true;
                    }
                }

                /*********************************************************************
                         "Oper == 1" (generar seaial) 
                         "Oper == 3" (generar seaial de prueba o token) 
                **********************************************************************/
                else  //Solo Encriptar
                {
                    List<TSConfi> lista = new List<TSConfi>();
                    Suma_ = sCadena.Substring(28, sCadena.Length - 28);

                    /*
                    if (Oper == 3)//QA ---_>  DE PRUEBA
                    {
                        string D = "07"; //Duracion
                        //--------------------------------------------------------------------------

                        lista = ListarConfig(objSession, "%_SERVICE", ref strMsjUsuario);

                        for (int i = 0; i < lista.Count(); i++)
                        {
                            if (lista[i].strCoConfi == "DUR_SERVICE")
                            {
                                //D = (Convert.ToInt32(objUser.DesencriptarPassword(lista[i].strValorConfi))+100).ToString().Substring(1,2);
                                D = (Convert.ToInt32(lista[i].strValorConfi) + 100).ToString().Substring(1, 2);
                                break;
                            }
                        }
                        //--------------------------------------------------------------------------

                        string[] DT = DateTime.Now.ToString().Split(' ');//Dato en duro
                        string[] DT_ = DT[0].Split('/');
                        D = D + (Convert.ToUInt32(DT_[0]) + 100).ToString().Substring(1, 2) + (Convert.ToUInt32(DT_[1]) + 100).ToString().Substring(1, 2) + DT_[2];
                        IDServidor = EncriptarPassword("Q" + D + "A" + sCadena);//le sumamos a la cadena los dias de prueba
                    }
                    else
                    {
                        IDServidor = EncriptarPassword(sCadena);
                    }
                    */

                    IDServidor = EncriptarPassword(sCadena);
                    Valida = true;

                    //--------------------------------------------------------------------------------------------------------
                    //Enviar correo a Tecflex para soporte 06.07.2021
                    string strMsjDB      = "";
                    string strTipo       = "TOKEN";
                    int intResult        = 0;
                    string CorreoDestino = "";
                    string Cliente       = "";
                    //List<TSConfi> lista = new List<TSConfi>();
                    lista = ListarConfig(objSession, "%_CBX", ref strMsjUsuario);

                    for (int i = 0; i < lista.Count(); i++)
                    {
                        if (lista[i].strCoConfi == "EMAIL_CBX")//El/los emails a donde se enviarán
                        {
                            CorreoDestino = lista[i].strValorConfi; //"sisactivofijo@gmail.com";
                        }
                        if (lista[i].strCoConfi == "OC_RUCCLI_CBX")//Ruc del cliente encriptado
                        {
                            Cliente = lista[i].strValorConfi;
                        }
                    }

                    /* Se comenta ya que en ese campos en la base de datos ya no ira como encriptado HGM 22.10.2021
                    string CliDesencriptado = DesencriptarPassword(Cliente);
                    string[] subs = CliDesencriptado.Split('|');   
                    */

                    string[] subs = Cliente.Split('|');
                    CorreoEmp obj = new CorreoEmp();
                    obj.intIdPersonal = 0;
                    obj.strOC         = subs[0];      //OC
                    obj.strRUC        = subs[1];      //RUC
                    obj.strDestinos   = CorreoDestino;//destinatarios
                    obj.strCadena     = sCadena;      // ID;
                    obj.strCadena2    = IDServidor;   // token;

                    if (sCadena != null && sCadena != "")
                    {
                        //En Marcar Huellas No va
                        //enviarCorreo(objSession, obj, strTipo, ref intResult, ref strMsjDB, ref strMsjUsuario);
                        ////////enviarCorreoTOKEN(objSession, obj, strTipo, ref intResult, ref strMsjDB, ref strMsjUsuario); 
                    }

                    if (intResult == 1)
                    {
                        strMsjUsuario = "Token generado (se envió correo a Tecflex SAC)";
                    }
                    else
                    {
                        strMsjUsuario = "No se pudo enviar correo del nuevo token";
                    }
                    //--------------------------------------------------------------------------------------------------------
                }


                ////--------------------------------------------------------------
                bool rpta = false;
                if (Valida)
                {
                    if (Oper == 2) // Registrar: Oper = 2  --> Licencia Normal o Token de Prueba
                    {
                        List<TSConfi> detalleConfig = new List<TSConfi>();
                        TSConfi o1       = new TSConfi();
                        o1.intIdConfi    = 0;
                        o1.strCoConfi    = "ID_SERVICE"; //El encripdado
                        o1.strValorConfi = IDServidor;
                        o1.tipoControl   = "N";
                        o1.bitFlActivo   = true;
                        detalleConfig.Add(o1);

                        TSConfi o2       = new TSConfi();
                        o2.intIdConfi    = 0;
                        o2.strCoConfi    = "PK_SERVICE"; //"PCK_SERVICE";
                        o2.strValorConfi = Suma_; //---> Se llena el listado con los valores que dse grabaran 
                        o2.tipoControl   = "N";
                        o2.bitFlActivo   = true;
                        detalleConfig.Add(o2);

                        //añadido 06.07.2021 - INICIO
                        TSConfi o3 = new TSConfi();
                        o3.intIdConfi = 0;
                        o3.strCoConfi = "HAB_SERVICE";
                        if (EsQA)
                        {
                            o3.strValorConfi = "1";
                        }
                        else
                        {
                            o3.strValorConfi = "0";
                        }
                        o3.tipoControl = "N";
                        o3.bitFlActivo = true;
                        detalleConfig.Add(o3);
                        //añadido 06.07.2021 - FIN

                        rpta = ActualizarConfig(objSesion, detalleConfig, ref strMsjUsuario);
                    }
                    else
                    {
                        rpta = true;
                    }

                    if (rpta)
                    {
                        intRpta = 1;
                        if (Oper == 2) //Registrar
                        {
                            strMsjUsuario = Suma_;
                        }
                        else
                        {
                            strMsjUsuario = IDServidor;
                        }
                    }
                    else
                    {
                        intRpta = 0;
                        if (Oper == 2) //Registrar
                        {
                            strMsjUsuario = "No se pudo registrar Token";
                        }
                        //else
                        //{
                        //    strMsjUsuario = "No se pudo generar Token";
                        //}
                    }
                }
                else
                {
                    intRpta = 0;
                }

                return strMsjUsuario;
            }
            catch (SqlException ex)
            {
                UtilitarioBL.AlmacenarLogError(ex); // Añadido HGM log 02.11.2021
                throw new Exception("Ocurrió un error en BD (GenerarServerEncriptado)");
            }
            catch (Exception ex)
            {
                UtilitarioBL.AlmacenarLogError(ex); // Añadido HGM log 02.11.2021
                throw new Exception("Error General (GenerarServerEncriptado)");
            }
        }


        //1.17
        public bool ActualizarConfig(Session_Movi objSession, List<TSConfi> detalleConfig, ref string strMsjUsuario)
        {
            try
            {
                bool tudobem = false;
                int intResult = 0;
                string strMsjDB = "";

                DataTable tb = SerealizeDetalleConfig(detalleConfig);
                tudobem = zkLoginDao.UpdateDetalleConfig(objSession, tb, ref intResult, ref strMsjDB, ref strMsjUsuario);

                return tudobem;
            }
            catch (SqlException ex)
            {
                //Log.AlmacenarLogError(ex, "TSConfiBL.cs: SqlException");
                UtilitarioBL.AlmacenarLogError(ex); // Añadido HGM log 02.11.2021
                throw new Exception("Ocurrió un error en BD (ActualizarConfig)");
            }
            catch (Exception ex)
            {
                //Log.AlmacenarLogError(ex, "TSConfiBL.cs: Exception");
                UtilitarioBL.AlmacenarLogError(ex); // Añadido HGM log 02.11.2021
                throw new Exception("Error General (ActualizarConfig)");
            }
        }

        //
        private DataTable SerealizeDetalleConfig(List<TSConfi> listaDetalles)
        {
            DataTable table = new DataTable();
            table.Columns.Add("intIdConfi", typeof(int));
            table.Columns.Add("strCoConfi", typeof(string));
            table.Columns.Add("strValorConfi", typeof(string));
            // table.Columns.Add("TipoControl", typeof(int));
            table.Columns.Add("TipoControl", typeof(string));
            table.Columns.Add("bitFlActivo", typeof(bool));

            foreach (var item in listaDetalles)
            {
                DataRow rows = table.NewRow();
                rows["intIdConfi"] = item.intIdConfi;
                rows["strCoConfi"] = item.strCoConfi;
                rows["strValorConfi"] = item.strValorConfi;
                rows["TipoControl"] = item.tipoControl;
                rows["bitFlActivo"] = item.bitFlActivo;

                table.Rows.Add(rows);
            }

            return table;
        }

        //Para validar si el usuario con esa ipp esta en la lista de clientes
        public /*List<TG_USUARIO>*/int ValidarUsuario(string strIpHost, ref int Valida, ref string strMsjUsuario)
        {
            int result = 0;

            List<TG_USUARIO> lista = new List<TG_USUARIO>();
            try
            {
                int intResult = 0;
                string strMsjDB = "";


                string Msg_ = "";
                //añadido 05.07.2021
                //---------------------------------------------------------------------
                //Cotejar la existencia de dicha IP dentro de la cadena de Clientes permitidos
                bool rpta = false;
                ClientesQA(ref strIpHost, ref rpta, ref Msg_); //obj_
                //---------------------------------------------------------------------

                if (rpta)//añadido inicio 05.07.2021
                {//añadido fin 05.07.2021


                    //lista = objUsuario.ValidarUsuario(intIdSesion, 0, intIdSoft, strusuario, strcontraseña, strIpHost, strCoSoft, ref Valida, ref intResult, ref strMsjDB, ref strMsjUsuario);

                    result = 1;

                    if (intResult == 0)
                    {
                        if (!strMsjDB.Equals(""))
                        {
                            //Log.AlmacenarLogMensaje("[ValidarUsuario] => Respuesta del Procedimiento : " + strMsjDB);
                            if (strMsjUsuario.Equals(""))
                                strMsjUsuario = strMsjDB;
                        }
                    }
                }//añadido inicio 05.07.2021
                else
                {
                    strMsjUsuario = Msg_;
                    Valida = 1;//Añadido para devolver un valor al controlador
                }//añadido fin 05.07.2021


            }
            catch (SqlException ex)
            {
                //Log.AlmacenarLogError(ex, "UsuarioBL.cs");
                UtilitarioBL.AlmacenarLogError(ex); // Añadido HGM log 02.11.2021
                throw new Exception("Ocurrió un error en BD (ValidarUsuario)");
            }

            catch (Exception ex)
            {
                //Log.AlmacenarLogError(ex, "UsuarioBL.cs");
                UtilitarioBL.AlmacenarLogError(ex); // Añadido HGM log 02.11.2021
                throw new Exception("Error General (ValidarUsuario)");
            }

            return result; //
            //return lista;
        }

        //CLientes QA 
        public void ClientesQA(ref string IP, ref bool rpta, ref string Msg_)
        {
            int intResult = 0;
            string strMsjDB = "";
            try
            {
                Session_Movi objSession = new Session_Movi();

                objSession.intIdSesion = 1;
                objSession.intIdSoft = 6;// 1;
                objSession.intIdMenu = 1;

                string strMsjUsuario = "";
                string sCadena = "";
                int TopeCliWeb = 10; 
                string IDEncrypt = "";

                //--------------------------------------------------------------------------
                List<TSConfi> lista = new List<TSConfi>();
                lista = ListarConfig(objSession, "%_SERVICE", ref strMsjUsuario);

                for (int i = 0; i < lista.Count(); i++)
                {
                    if (lista[i].strCoConfi == "PCK_SERVICE")
                    {
                        //TopeCliWeb = Convert.ToInt32(objUser.DesencriptarPassword(lista[i].strValorConfi));
                        TopeCliWeb = Convert.ToInt32(lista[i].strValorConfi); //El tope maximo de PCs que puede usar este cliente --> El valor esta en la tabla de Configuraciones
                    }
                    if (lista[i].strCoConfi == "ID_SERVICE")
                    {
                        IDEncrypt = lista[i].strValorConfi;
                    }
                }

                //--------------------------------------------------------------------------zkLoginDao
                lista = ListarCliWeb(ref strMsjUsuario);

                if (lista.Count() > 0) //Cuando la IP ya hizo o hace uso del sistema entonces entra a este bloque
                {
                    if (lista.Count() < TopeCliWeb) ///<
                    {
                        for (int i = 0; i < lista.Count(); i++)
                        {
                            sCadena = lista[i].strValorConfi;
                            if (sCadena != "")
                            {
                                sCadena = DesencriptarPassword(sCadena);

                                if (sCadena == IP)
                                {
                                    rpta = true;
                                    break;
                                }
                            }
                        }

                        if (!rpta)
                        {
                            sCadena = EncriptarPassword(IP);
                            rpta = zkLoginDao.ICliWeb(sCadena, ref intResult, ref strMsjDB, ref strMsjUsuario);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < lista.Count(); i++)
                        {
                            sCadena = lista[i].strValorConfi;
                            if (sCadena != "")
                            {
                                sCadena = DesencriptarPassword(sCadena);

                                if (sCadena == IP)
                                {
                                    rpta = true; //Comentado para prueba 22.10.2021 HGM
                                    break; //Comentado para prueba 22.10.2021 HGM
                                }
                            }
                        }

                        if (!rpta)
                        {
                            string IDDesencrypt = DesencriptarPassword(IDEncrypt);
                            if (IDDesencrypt.Substring(0, 1) == "Q")
                            {
                                Msg_ = "Cliente NO Autorizado (Licencia de Prueba solo permite " + TopeCliWeb.ToString() + " clientes)";
                            }
                            else
                            {
                                Msg_ = "Cliente NO Autorizado (Licencia solo permite " + TopeCliWeb.ToString() + " clientes)";
                            }

                        }
                    }
                }
                else //Si no existe la IP de la PC se realiza una inserción 
                {
                    sCadena = EncriptarPassword(IP);
                    rpta = zkLoginDao.ICliWeb(sCadena, ref intResult, ref strMsjDB, ref strMsjUsuario);
                }
            }
            catch (SqlException ex)
            {
                //Log.AlmacenarLogError(ex, "TSConfiBL.cs: SqlException");
                UtilitarioBL.AlmacenarLogError(ex); // Añadido HGM log 02.11.2021
                throw new Exception("Ocurrió un error en BD (ClientesQA)");
            }
            catch (Exception ex)
            {
                //Log.AlmacenarLogError(ex, "TSConfiBL.cs: Exception");
                UtilitarioBL.AlmacenarLogError(ex); // Añadido HGM log 02.11.2021
                throw new Exception("Error General (ClientesQA)");
            }
        }

        //LISTAR CLI
        public List<TSConfi> ListarCliWeb(ref string strMsjUsuario)
        {
            List<TSConfi> lista = new List<TSConfi>();
            try
            {
                int intResult = 0;
                string strMsjDB = "";

                lista = zkLoginDao.ListarCliWeb(ref intResult, ref strMsjDB, ref strMsjUsuario);
                if (intResult == 0)
                {
                    if (!strMsjDB.Equals(""))
                    {
                        //Log.AlmacenarLogMensaje("[ListarCliWeb] => Respuesta del Procedimiento : " + strMsjDB);
                        if (strMsjUsuario.Equals(""))
                            strMsjUsuario = strMsjDB;
                    }
                }
            }
            catch (SqlException ex)
            {
                //Log.AlmacenarLogError(ex, "TSConfiBL.cs: SqlException");
                UtilitarioBL.AlmacenarLogError(ex); // Añadido HGM log 02.11.2021
                throw new Exception("Ocurrió un error en BD (ListarCliWeb)");
            }
            catch (Exception ex)
            {
                //Log.AlmacenarLogError(ex, "TSConfiBL.cs: Exception");
                UtilitarioBL.AlmacenarLogError(ex); // Añadido HGM log 02.11.2021
                throw new Exception("Error General (ListarCliWeb)");
            }
            return lista;
        }


        #endregion








      
        public bool IUTiempoEntreMrcaciones(ref ZKLoginOff x_login, string x_menus, ref string x_mensaje)
        {
            string funcion = "InsertarLogin";
            bool result = false;
            try
            {
                using (TransactionScope tscTrans = new TransactionScope())
                {
                    result = zkLoginDao.InsertarLogin(ref x_login, x_menus);
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








    }
}

