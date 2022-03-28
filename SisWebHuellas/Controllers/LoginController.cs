using CBX_Web_SISCOP.Controllers;
using SisWebHuellas.App_Start;
using System;
using System.Linq;
using System.Web.Mvc;

//añadidos para quitar wcf
using Dominio.Entidades;
using Dominio.Repositorio;
using System.Collections.Generic;

namespace SisWebHuellas.Controllers
{
    public class LoginController : Controller
    {
        public static ListZKLoginOff _listUsersOff_ { get; set; }//añadido 16.06.2021
        public static ListZKMenuOff _listMenusOff_ { get; set; } //añadido 16.06.2021
        public static string _menu { get; set; } = "";
        public static bool _Online_ { get; set; } = true;

        //añadido 16.06.2021
        public void llenarListas(ListZKLoginOff p_lstUsers, ListZKMenuOff p_lstMenusUsers, bool p_status)
            //public void llenarListas(srvHuellas.ListZKLoginOff p_lstUsers, srvHuellas.ListZKMenuOff p_lstMenusUsers, bool p_status) //c/ wcf 22.06.2021
        {
            _listUsersOff_ = p_lstUsers;
            _listMenusOff_ = p_lstMenusUsers;
            _Online_ = p_status;
        }
        //añadido 16.06.2021
        public void Estatus(bool p_status)
        {
            _Online_ = p_status;
        }
        //añadido 18.06.2021
        public JsonResult Menu_()
        {
           return Json(_menu);
        }

        public ActionResult Login()
        {
            if (Session["USUARIO"] != null)
            {
                return RedirectToAction("ListarAsistencias", "Asistencia");
            }
            return View();
        }

        //////////////////////////////////////////////////////HGM_AÑADIDO_19.08.2021_15:51:08 
        public ZKLoginBL objZKLoginBL = new ZKLoginBL();
        public static string url { get; set; }
        public static string strIpHost { get; set; }
        ////////////////////////////////////////////////////
        //OBTENER EL IP CON CODIGIGO C# - A Nivel WebSite
        ////////public string GetUserIPAddress()
        ////////{
        ////////    //AÑADIDO 01.09.2021 HGM
        ////////    url = (System.Web.HttpContext.Current.Request.Url.AbsoluteUri).Replace("/LoginSiscop/LoginSiscop/", "");
        ////////    var context = System.Web.HttpContext.Current;
        ////////    string ip = String.Empty;

        ////////    if (context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
        ////////        ip = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
        ////////    else if (!String.IsNullOrWhiteSpace(context.Request.UserHostAddress))
        ////////        ip = context.Request.UserHostAddress;

        ////////    if (ip == "::1")
        ////////        ip = "127.0.0.1";

        ////////    strIpHost = ip;
        ////////    //string ipaddress = Request.UserHostAddress;
        ////////    return ip;
        ////////}

        [HttpPost]
        public ActionResult Login(string x_NombreSesion, string x_Password)
        {


            try
            {
                string x_mensaje = "";
                ZKLoginOff entidad = new ZKLoginOff();

                /////////////////////////////////////////////////////////////////////////////INI REGISTRAR LICENCIA

                /************1.- Validamos Server **************/
                int Oper = 0;
                x_mensaje = objZKLoginBL.ValidaServer("", ref Oper);

                if (x_mensaje == "")
                {

                    /************2.- Validamos Usuario (N° de clientes)**************/
                    string strMsgUsuario = "";
                    int Valida = 0;
                    //List<TG_USUARIO> detConcepto = new List<TG_USUARIO>();
                    int detConcepto = objZKLoginBL.ValidarUsuario(GetUserIPAddress(), ref Valida, ref strMsgUsuario);//.ToList();


                    if (strMsgUsuario == "")
                    {


                        if (_Online_ == false)
                        {
                            if (_listUsersOff_ != null)
                            {
                                _listUsersOff_.ToList().ForEach(x =>
                                {
                                    if (x.sNombreSesion == x_NombreSesion)
                                        entidad = x;
                                });
                                if (entidad == null || entidad.iIdSesion == 0)
                                    x_mensaje = "El usuario no existe.";
                                else if (!entidad.bitFlActivo)
                                    x_mensaje = "El usuario está inactivo.";
                                else
                                {
                                    TCX0001.ctcCryptografia crypto = new TCX0001.ctcCryptografia();
                                    string pwd = crypto.CifrarCadenaPwUsuar(x_Password);
                                    if (pwd != entidad.sPasswordSesion)
                                        x_mensaje = "La contraseña ingresada es incorrecta";
                                    else
                                    {
                                        //entidad.OpcionesMenuOff = new srvHuellas.ListZKMenuOff(); //c/ WCF 22.06.2021
                                        entidad.OpcionesMenuOff = new ListZKMenuOff();
                                        if (_listMenusOff_ != null)
                                        {
                                            _listMenusOff_.ToList().ForEach(x =>
                                            {
                                                if (x.iIdSesion == entidad.iIdSesion && x.IntAsing > 0)
                                                    entidad.OpcionesMenuOff.Add(x);
                                            });
                                        }
                                    }
                                }


                                if (!string.IsNullOrEmpty(x_mensaje) || x_mensaje != "")
                                    TempData["ERROR_LOGIN"] = x_mensaje;
                                else
                                {
                                    Session["USUARIO"] = entidad;

                                    if (entidad.OpcionesMenuOff.ToList().Exists(x => x.StrControlador.ToLower() == "asistencia" && x.StrAccion.ToLower() == "listarasistencias"))
                                    {
                                        _menu = "03"; //añadido 18.06.2021
                                        return RedirectToAction("ListarAsistencias", "Asistencia");
                                    }
                                    else if (entidad.OpcionesMenuOff.Count > 0)
                                    {
                                        _menu = entidad.OpcionesMenuOff[0].StrCoMenus;//añadido 18.06.2021
                                        return RedirectToAction(entidad.OpcionesMenuOff[0].StrAccion, entidad.OpcionesMenuOff[0].StrControlador);
                                    }
                                    else
                                    {
                                        _menu = "";
                                        return View("errorPermiso");
                                    }

                                }
                            }
                            else
                            {
                                x_mensaje = "No existen usuarios en memoria local";
                                if (!string.IsNullOrEmpty(x_mensaje) || x_mensaje != "")
                                    TempData["ERROR_LOGIN"] = x_mensaje;
                            }
                        }
                        else
                        {
                            using (ZKLoginBL objBL = new ZKLoginBL())
                            {
                                entidad = objBL.ListarLoginByCod(x_NombreSesion, x_Password, ref x_mensaje);
                            }

                            if (!string.IsNullOrEmpty(x_mensaje))
                                TempData["ERROR_LOGIN"] = x_mensaje;
                            else
                            {
                                Session["USUARIO"] = entidad;

                                if (entidad.OpcionesMenuOff.ToList().Exists(x => x.StrControlador.ToLower() == "asistencia" && x.StrAccion.ToLower() == "listarasistencias"))
                                {
                                    _menu = "03"; //añadido 18.06.2021
                                    return RedirectToAction("ListarAsistencias", "Asistencia");
                                }
                                else if (entidad.OpcionesMenuOff.Count > 0)
                                {
                                    _menu = entidad.OpcionesMenuOff[0].StrCoMenus;//añadido 18.06.2021
                                    return RedirectToAction(entidad.OpcionesMenuOff[0].StrAccion, entidad.OpcionesMenuOff[0].StrControlador);
                                }
                                else
                                {
                                    _menu = "";
                                    return View("errorPermiso");
                                }
                            }
                        }

                    }  
                    //////////////////////////////////////////////////////////////////////////////FIN REGISTRAR LICENCIA
                    else//Del Validar Usuario --> Cuantos Clientes esta permitido
                    {
                        x_mensaje = strMsgUsuario;
                        if (!string.IsNullOrEmpty(x_mensaje) || x_mensaje != "")
                            TempData["ERROR_LOGIN"] = x_mensaje;

                    }

                }
                else//Del Validar Server
                {
                    if (Oper == 4)//Cuando no se ha enviado ningun correo a Sorporte para Generar 
                    {
                        //..una Licencia, es decir que ya existe una licencia en la tabla de configuraciones por lo tanto continua la ejecucion
                        if (!string.IsNullOrEmpty(x_mensaje) || x_mensaje != "")
                            TempData["ERROR_LOGIN"] = x_mensaje;
                            TempData["ERROR_LICENCIA"] = x_mensaje;

                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(x_mensaje) || x_mensaje != "")
                            TempData["ERROR_LICENCIA"] = x_mensaje;
                        TempData["ERROR_LOGIN"] = x_mensaje;

                    }
                }

                return View();


            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.GetType() == typeof(System.Net.WebException))
                    {
                        if (ex.InnerException.Message.Contains("(404)"))
                            TempData["ERROR_LOGIN"] = "El servicio web no se encuentra disponible.";
                        else
                            TempData["ERROR_LOGIN"] = ex.InnerException.Message;
                        return View();
                    }
                }
                var x = ex.InnerException.GetType();
                Log.AlmacenarLogError(ex, "LoginController.cs | Login");
                return View();
            }

        }


        [HttpPost]
        public ActionResult Login_3(string x_NombreSesion, string x_Password)
        {





            /************1.- Validamos Usuario **************/


            try
            {
                string x_mensaje = "";
                //srvHuellas.ZKLoginOff entidad = new srvHuellas.ZKLoginOff();//c/ webservice 22.06.2021
                ZKLoginOff entidad = new ZKLoginOff();

                /////////////////////////////////////////////////////////////////////////////INI REGISTRAR LICENCIA

                //int Valida = 0;
                int Oper = 0;
                x_mensaje = objZKLoginBL.ValidaServer("", ref Oper);


                if (x_mensaje == "")
                {


                    if (_Online_ == false)
                    {
                        if (_listUsersOff_ != null)
                        {
                            _listUsersOff_.ToList().ForEach(x =>
                            {
                                if (x.sNombreSesion == x_NombreSesion)
                                    entidad = x;
                            });
                            if (entidad == null || entidad.iIdSesion == 0)
                                x_mensaje = "El usuario no existe.";
                            else if (!entidad.bitFlActivo)
                                x_mensaje = "El usuario está inactivo.";
                            else
                            {
                                TCX0001.ctcCryptografia crypto = new TCX0001.ctcCryptografia();
                                string pwd = crypto.CifrarCadenaPwUsuar(x_Password);
                                if (pwd != entidad.sPasswordSesion)
                                    x_mensaje = "La contraseña ingresada es incorrecta";
                                else
                                {
                                    //entidad.OpcionesMenuOff = new srvHuellas.ListZKMenuOff(); //c/ WCF 22.06.2021
                                    entidad.OpcionesMenuOff = new ListZKMenuOff();
                                    if (_listMenusOff_ != null)
                                    {
                                        _listMenusOff_.ToList().ForEach(x =>
                                        {
                                            if (x.iIdSesion == entidad.iIdSesion && x.IntAsing > 0)
                                                entidad.OpcionesMenuOff.Add(x);
                                        });
                                    }
                                }
                            }


                            if (!string.IsNullOrEmpty(x_mensaje) || x_mensaje != "")
                                TempData["ERROR_LOGIN"] = x_mensaje;
                            else
                            {
                                Session["USUARIO"] = entidad;

                                if (entidad.OpcionesMenuOff.ToList().Exists(x => x.StrControlador.ToLower() == "asistencia" && x.StrAccion.ToLower() == "listarasistencias"))
                                {
                                    _menu = "03"; //añadido 18.06.2021
                                    return RedirectToAction("ListarAsistencias", "Asistencia");
                                }
                                else if (entidad.OpcionesMenuOff.Count > 0)
                                {
                                    _menu = entidad.OpcionesMenuOff[0].StrCoMenus;//añadido 18.06.2021
                                    return RedirectToAction(entidad.OpcionesMenuOff[0].StrAccion, entidad.OpcionesMenuOff[0].StrControlador);
                                }
                                else
                                {
                                    _menu = "";
                                    return View("errorPermiso");
                                }

                            }
                        }
                        else
                        {
                            x_mensaje = "No existen usuarios en memoria local";
                            if (!string.IsNullOrEmpty(x_mensaje) || x_mensaje != "")
                                TempData["ERROR_LOGIN"] = x_mensaje;
                        }
                    }
                    else
                    {
                        // proxy_s = new srvHuellas.SistemaCNClient();//c/ webservice 22.06.2021

                        //entidad = proxy_s.ListarLoginByCod(x_NombreSesion, x_Password, ref x_mensaje);//c/ webservice 22.06.2021
                        using (ZKLoginBL objBL = new ZKLoginBL())
                        {
                            entidad = objBL.ListarLoginByCod(x_NombreSesion, x_Password, ref x_mensaje);
                        }

                        if (!string.IsNullOrEmpty(x_mensaje))
                            TempData["ERROR_LOGIN"] = x_mensaje;
                        else
                        {
                            Session["USUARIO"] = entidad;

                            if (entidad.OpcionesMenuOff.ToList().Exists(x => x.StrControlador.ToLower() == "asistencia" && x.StrAccion.ToLower() == "listarasistencias"))
                            {
                                _menu = "03"; //añadido 18.06.2021
                                return RedirectToAction("ListarAsistencias", "Asistencia");
                            }
                            else if (entidad.OpcionesMenuOff.Count > 0)
                            {
                                _menu = entidad.OpcionesMenuOff[0].StrCoMenus;//añadido 18.06.2021
                                return RedirectToAction(entidad.OpcionesMenuOff[0].StrAccion, entidad.OpcionesMenuOff[0].StrControlador);
                            }
                            else
                            {
                                _menu = "";
                                return View("errorPermiso");
                            }
                        }
                    }





                }            //////////////////////////////////////////////////////////////////////////////FIN REGISTRAR LICENCIA
                else
                {
                    if (Oper == 4)
                    { //Cuando no se ha enviado ningun correo a Sorporte para Generar 
                      //..una Licencia, es decir que ya existe un alicencia en la tabla de configuraciones por lo tanto continua la ejecucion
                      //x_mensaje = "No existen usuarios en memoria local";
                        if (!string.IsNullOrEmpty(x_mensaje) || x_mensaje != "")
                            TempData["ERROR_LOGIN"] = x_mensaje;
                        TempData["ERROR_LICENCIA"] = x_mensaje;

                    }

                    else
                    {

                        //x_mensaje = "No existen usuarios en memoria local";
                        if (!string.IsNullOrEmpty(x_mensaje) || x_mensaje != "")

                            TempData["ERROR_LICENCIA"] = x_mensaje;

                    }
                }



                return View();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.GetType() == typeof(System.Net.WebException))
                    {
                        if (ex.InnerException.Message.Contains("(404)"))
                            TempData["ERROR_LOGIN"] = "El servicio web no se encuentra disponible.";
                        else
                            TempData["ERROR_LOGIN"] = ex.InnerException.Message;
                        return View();
                    }
                }
                var x = ex.InnerException.GetType();
                Log.AlmacenarLogError(ex, "LoginController.cs | Login");
                return View();
            }



        }



        [HttpPost]
        public ActionResult Login_o_2(string x_NombreSesion, string x_Password)
        {


            int Valida = 0;
            int tiempoEspera = 0;
            string msj = "";
            msj = ""; //objZKLoginBL.ValidaServer("", 0);
            //msj = objTSConfiBL.ValidaServer("", 0);
            if (msj == "")
            {

                msj = "Usuario Permitido";
            }
            else {
                //Session["LoginAttempts"] = null;
                //Session["LoginTimeAttempts"] = null;
                //return Json(new { codValida = Valida, error = "login", data = tiempoEspera, strMsgAlert = msj, strUserNameInput = "", strMensajeError = "" });

                _menu = "";
                return View(msj);
            }




            return View();




            /************
            try
            {
                string x_mensaje = "";
                //srvHuellas.ZKLoginOff entidad = new srvHuellas.ZKLoginOff();//c/ webservice 22.06.2021
                ZKLoginOff entidad = new ZKLoginOff();

                if (_Online_ == false)
                {
                    if (_listUsersOff_ != null)
                    {
                        _listUsersOff_.ToList().ForEach(x =>
                        {
                            if (x.sNombreSesion == x_NombreSesion)
                                entidad = x;
                        });
                        if (entidad == null || entidad.iIdSesion == 0)
                            x_mensaje = "El usuario no existe.";
                        else if (!entidad.bitFlActivo)
                            x_mensaje = "El usuario está inactivo.";
                        else
                        {
                            TCX0001.ctcCryptografia crypto = new TCX0001.ctcCryptografia();
                            string pwd = crypto.CifrarCadenaPwUsuar(x_Password);
                            if (pwd != entidad.sPasswordSesion)
                                x_mensaje = "La contraseña ingresada es incorrecta";
                            else
                            {
                                //entidad.OpcionesMenuOff = new srvHuellas.ListZKMenuOff(); //c/ WCF 22.06.2021
                                entidad.OpcionesMenuOff = new ListZKMenuOff();
                                if (_listMenusOff_ != null)
                                {
                                    _listMenusOff_.ToList().ForEach(x =>
                                    {
                                        if (x.iIdSesion == entidad.iIdSesion && x.IntAsing > 0)
                                            entidad.OpcionesMenuOff.Add(x);
                                    });
                                }
                            }
                        }


                        if (!string.IsNullOrEmpty(x_mensaje) || x_mensaje != "")
                            TempData["ERROR_LOGIN"] = x_mensaje;
                        else
                        {
                            Session["USUARIO"] = entidad;

                            if (entidad.OpcionesMenuOff.ToList().Exists(x => x.StrControlador.ToLower() == "asistencia" && x.StrAccion.ToLower() == "listarasistencias"))
                            {
                                _menu = "03"; //añadido 18.06.2021
                                return RedirectToAction("ListarAsistencias", "Asistencia");
                            }
                            else if (entidad.OpcionesMenuOff.Count > 0)
                            {
                                _menu = entidad.OpcionesMenuOff[0].StrCoMenus;//añadido 18.06.2021
                                return RedirectToAction(entidad.OpcionesMenuOff[0].StrAccion, entidad.OpcionesMenuOff[0].StrControlador);
                            }
                            else
                            {
                                _menu = "";
                                return View("errorPermiso");
                            }

                        }
                    }
                    else
                    {
                        x_mensaje = "No existen usuarios en memoria local";
                        if (!string.IsNullOrEmpty(x_mensaje) || x_mensaje != "")
                            TempData["ERROR_LOGIN"] = x_mensaje;
                    }
                }
                else
                {
                    // proxy_s = new srvHuellas.SistemaCNClient();//c/ webservice 22.06.2021

                    //entidad = proxy_s.ListarLoginByCod(x_NombreSesion, x_Password, ref x_mensaje);//c/ webservice 22.06.2021
                    using (ZKLoginBL objBL = new ZKLoginBL())
                    {
                        entidad = objBL.ListarLoginByCod(x_NombreSesion, x_Password, ref x_mensaje);
                    }

                    if (!string.IsNullOrEmpty(x_mensaje))
                        TempData["ERROR_LOGIN"] = x_mensaje;
                    else
                    {
                        Session["USUARIO"] = entidad;

                        if (entidad.OpcionesMenuOff.ToList().Exists(x => x.StrControlador.ToLower() == "asistencia" && x.StrAccion.ToLower() == "listarasistencias"))
                        {
                            _menu = "03"; //añadido 18.06.2021
                            return RedirectToAction("ListarAsistencias", "Asistencia");
                        }
                        else if (entidad.OpcionesMenuOff.Count > 0)
                        {
                            _menu = entidad.OpcionesMenuOff[0].StrCoMenus;//añadido 18.06.2021
                            return RedirectToAction(entidad.OpcionesMenuOff[0].StrAccion, entidad.OpcionesMenuOff[0].StrControlador);
                        }
                        else
                        {
                            _menu = "";
                            return View("errorPermiso");
                        }
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.GetType() == typeof(System.Net.WebException))
                    {
                        if (ex.InnerException.Message.Contains("(404)"))
                            TempData["ERROR_LOGIN"] = "El servicio web no se encuentra disponible.";
                        else
                            TempData["ERROR_LOGIN"] = ex.InnerException.Message;
                        return View();
                    }
                }
                var x = ex.InnerException.GetType();
                Log.AlmacenarLogError(ex, "LoginController.cs | Login");
                return View();
            }
            **************/


        }

        [HttpPost]
        public ActionResult Login_o(string x_NombreSesion, string x_Password)
        {
            try
            {
                string x_mensaje = "";
                //srvHuellas.ZKLoginOff entidad = new srvHuellas.ZKLoginOff();//c/ webservice 22.06.2021
                ZKLoginOff entidad = new ZKLoginOff();

                if (_Online_ == false)
                {
                    if(_listUsersOff_ != null)
                    {
                        _listUsersOff_.ToList().ForEach(x =>
                        {
                            if (x.sNombreSesion == x_NombreSesion)
                                entidad = x;
                        });
                        if (entidad == null || entidad.iIdSesion == 0)
                            x_mensaje = "El usuario no existe.";
                        else if (!entidad.bitFlActivo)
                            x_mensaje = "El usuario está inactivo.";
                        else
                        {
                            TCX0001.ctcCryptografia crypto = new TCX0001.ctcCryptografia();
                            string pwd = crypto.CifrarCadenaPwUsuar(x_Password);
                            if (pwd != entidad.sPasswordSesion)
                                x_mensaje = "La contraseña ingresada es incorrecta";
                            else
                            {
                                //entidad.OpcionesMenuOff = new srvHuellas.ListZKMenuOff(); //c/ WCF 22.06.2021
                                entidad.OpcionesMenuOff = new ListZKMenuOff();
                                if (_listMenusOff_!= null)
                                {
                                    _listMenusOff_.ToList().ForEach(x =>
                                    {
                                        if (x.iIdSesion == entidad.iIdSesion && x.IntAsing > 0)
                                            entidad.OpcionesMenuOff.Add(x);
                                    });
                                }
                            }
                        }


                        if (!string.IsNullOrEmpty(x_mensaje) || x_mensaje != "")
                            TempData["ERROR_LOGIN"] = x_mensaje;
                        else
                        {
                            Session["USUARIO"] = entidad;

                            if (entidad.OpcionesMenuOff.ToList().Exists(x => x.StrControlador.ToLower() == "asistencia" && x.StrAccion.ToLower() == "listarasistencias"))
                            {
                                _menu = "03"; //añadido 18.06.2021
                                return RedirectToAction("ListarAsistencias", "Asistencia");
                            }else if (entidad.OpcionesMenuOff.Count > 0)
                            {
                                _menu = entidad.OpcionesMenuOff[0].StrCoMenus;//añadido 18.06.2021
                                return RedirectToAction(entidad.OpcionesMenuOff[0].StrAccion, entidad.OpcionesMenuOff[0].StrControlador);
                            }
                            else
                            {
                                _menu = "";
                                return View("errorPermiso");
                            }
                                
                        }
                    }
                    else
                    {
                        x_mensaje = "No existen usuarios en memoria local";
                        if (!string.IsNullOrEmpty(x_mensaje) || x_mensaje != "")
                            TempData["ERROR_LOGIN"] = x_mensaje;
                    }
                }
                else
                {
                    // proxy_s = new srvHuellas.SistemaCNClient();//c/ webservice 22.06.2021

                    //entidad = proxy_s.ListarLoginByCod(x_NombreSesion, x_Password, ref x_mensaje);//c/ webservice 22.06.2021
                    using (ZKLoginBL objBL = new ZKLoginBL())
                    {
                        entidad=objBL.ListarLoginByCod(x_NombreSesion, x_Password, ref x_mensaje);
                    }

                    if (!string.IsNullOrEmpty(x_mensaje))
                        TempData["ERROR_LOGIN"] = x_mensaje;
                    else
                    {
                        Session["USUARIO"] = entidad;

                        if (entidad.OpcionesMenuOff.ToList().Exists(x => x.StrControlador.ToLower() == "asistencia" && x.StrAccion.ToLower() == "listarasistencias"))
                        {
                            _menu = "03"; //añadido 18.06.2021
                            return RedirectToAction("ListarAsistencias", "Asistencia");
                        }
                        else if (entidad.OpcionesMenuOff.Count > 0)
                        {
                            _menu = entidad.OpcionesMenuOff[0].StrCoMenus;//añadido 18.06.2021
                            return RedirectToAction(entidad.OpcionesMenuOff[0].StrAccion, entidad.OpcionesMenuOff[0].StrControlador);
                        }
                        else
                        {
                            _menu = "";
                            return View("errorPermiso");
                        }
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.GetType() == typeof(System.Net.WebException))
                    {
                        if (ex.InnerException.Message.Contains("(404)"))
                            TempData["ERROR_LOGIN"] = "El servicio web no se encuentra disponible.";
                        else
                            TempData["ERROR_LOGIN"] = ex.InnerException.Message;
                        return View();
                    }
                }
                var x = ex.InnerException.GetType();
                Log.AlmacenarLogError(ex, "LoginController.cs | Login");
                return View();
            }
        }

        public ActionResult cerrarSession()
        {
            try
            {
                Session.Clear(); //comentado para pruebas 15.06.2021
                return RedirectToAction("Login", "Login");
            }
            catch (Exception ex)
            {
                Log.AlmacenarLogError(ex, "LoginController.cs | cerrarSession");
                throw;
            }
        }

        [HttpPost]
        public JsonResult ObtenerUrlWslc()
        {
            Respuesta objRespuesta = new Respuesta();
            try
            {
                // proxy_s = new srvHuellas.SistemaCNClient();//c/ webservice 22.06.2021

                objRespuesta.exito = true;
                //objRespuesta.message = proxy_s.GetWslc();//c/ webservice 22.06.2021
                objRespuesta.message = "";
            }
            catch(Exception ex)
            {
                Log.AlmacenarLogError(ex, "LoginController.cs | ObtenerUrlWslc");
            }
            return Json(objRespuesta);
        }

        [HttpPost]
        public JsonResult ObtenerUrlWsse()
        {
            //srvHuellas.SistemaCNClient proxy = new srvHuellas.SistemaCNClient(); //c/ webservice 22.06.2021
            //string url = proxy.GetWs();//c/ webservice 22.06.2021
            string url = "";//c/ webservice 22.06.2021

            url = url.Replace("/", "~");

            return Json(url);
        }

        [HttpPost]
        public JsonResult LogsJs(Exception Excep,string NameFunction)
        {
            bool rpta = true;
            Log.AlmacenarLogError(Excep, NameFunction);
            return Json(rpta);
        }

        //añadido de la Internet como contingencia, falta probar en Servidor
        public static string GetUserIPAddress()
        {
            var context = System.Web.HttpContext.Current;
            string ip = String.Empty;

            if (context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                ip = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            else if (!String.IsNullOrWhiteSpace(context.Request.UserHostAddress))
                ip = context.Request.UserHostAddress;

            if (ip == "::1")
                ip = "127.0.0.1";
            return ip;
        }

        //                  

        [HttpPost]
        public JsonResult DescargaDriver()
        {
            Respuesta objRespuesta = new Respuesta();
            string strIpHost = GetUserIPAddress();
            try
            {
                bool resultado = false;
                if (Session["USUARIO"] == null)
                {
                    objRespuesta = new Respuesta() { exito = false, type = "error", message = "Su sesión expiró, inicie sesión nuevamente." };
                    return Json(objRespuesta);
                }
                string mensaje = "";
                int rpta = 0;

                using (ZKLoginBL objBL = new ZKLoginBL())
                {
                    ZKLoginOff usuario = (ZKLoginOff)Session["USUARIO"];
                    objBL.DescargaDriver(usuario.iIdSesion, strIpHost, ref mensaje,ref rpta);
                }

                if (rpta==1)//primer registro  
                {
                    mensaje = "";
                    resultado = true;
                }
                else
                {
                    resultado = false;  //rpta=0 ya fue descargado
                }

                objRespuesta = new Respuesta() { exito = resultado, message = mensaje, type = resultado ? "success" : "error" };
            }
            catch (Exception ex)
            {
                Log.AlmacenarLogError(ex, "LoginController.cs | DescargaDriver");
            }

            return Json(objRespuesta);
        }


        [HttpPost]
        public JsonResult ConsultarDescargaDriver()
        {
            Respuesta objRespuesta = new Respuesta();
            string strIpHost = GetUserIPAddress();
            try
            {
                bool resultado = false;
                if (Session["USUARIO"] == null)
                {
                    objRespuesta = new Respuesta() { exito = false, type = "error", message = "Su sesión expiró, inicie sesión nuevamente." };
                    return Json(objRespuesta);
                }
                string mensaje = "";
                int rpta = 0;

                using (ZKLoginBL objBL = new ZKLoginBL())
                {
                    ZKLoginOff usuario = (ZKLoginOff)Session["USUARIO"];
                    objBL.ConsultarDescargaDriver(usuario.iIdSesion, strIpHost, ref mensaje, ref rpta);
                }

                if (rpta == 1)//primer registro  
                {
                    mensaje = "";
                    resultado = true;
                }
                else
                {
                    resultado = false;  //rpta=0 ya fue descargado
                }

                objRespuesta = new Respuesta() { exito = resultado, message = mensaje, type = resultado ? "success" : "error" };
            }
            catch (Exception ex)
            {
                Log.AlmacenarLogError(ex, "LoginController.cs | DescargaDriver");
            }

            return Json(objRespuesta);
        }


        /**************************************************************************
         Añadido desde lo desarrollado en siscop  por Elizabeth 05.07.2021 10AM          
        ***************************************************************************/
        //HGM_OK
        public ActionResult RegistrarServerWCF(string llave, int Oper)//public string RegistrarServerWCF(string llave, int Oper)
        {
            CustomResponse result = new CustomResponse();

            Session_Movi objSesion = new Session_Movi();
            objSesion.intIdSesion = 1;
            objSesion.intIdSoft = 1;
            objSesion.intIdMenu = 1;
            int intRpta = 0;
            //string x_mensaje = "";

            try
            {

                //using (Seguridad_tsp = new SeguridadSrvClient())
                //{
                string msj = objZKLoginBL.GenerarServerEncriptado(objSesion, ref intRpta, llave, Oper); //Oper= 0: Encriptar //1: Encriptar y Registrar // 2: Registrar
                if (intRpta == 1)
                {
                    result.type = "success";

                    //x_mensaje = "success";

                    //if (!string.IsNullOrEmpty(x_mensaje) || x_mensaje != "")
                    //    TempData["ERROR_LOGIN"] = x_mensaje;
                }
                else
                {
                    //x_mensaje = "success2";
                    result.type = "errorInt";
                }

                //x_mensaje = "success3";
                result.message = msj;
                //}
            }
            catch (Exception ex)
            {
                Log.AlmacenarLogError(ex, "LoginController.cs");
                //result.type = "errorInt";
                //result.message = "Ocurrió un inconveniente al RegistrarServerWCF";
            }

            //if (!string.IsNullOrEmpty(x_mensaje) || x_mensaje != "")
            //    TempData["ERROR_LICENCIA"] = x_mensaje;
            //    //return View();

            return Json(result);



        }


    }
}