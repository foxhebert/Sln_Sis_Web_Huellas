using CBX_Web_SISCOP.Controllers;
using SisWebHuellas.App_Start;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
//añadidos para quitar wcf
using Dominio.Entidades;
using Dominio.Entidades.Personalizado;
using Dominio.Repositorio;

namespace SisWebHuellas.Controllers
{
    public class UsuarioLoginController : Controller
    {
        public static List<ItemCombo> _listSedesOff_ { get; set; } //añadido 16.06.2021

        //añadido 16.06.2021
        public void llenarListas(List<ItemCombo> p_lst_)//modificado 22.06.2021
        {
            _listSedesOff_ = p_lst_;
        }

        public ActionResult ListarUsuarioLogin()
        {
            try
            {
                #region VALIDAR SESION VIGENTE Y PERMISOS DE MENÚ
                if (Session["USUARIO"] == null)
                    return RedirectToAction("Login", "Login");

                ZKLoginOff usuario = (ZKLoginOff)Session["USUARIO"];
                //srvHuellas.ZKLogin usuario = (srvHuellas.ZKLogin)Session["USUARIO"];
                string controlador = this.GetType().Name.ToLower().Replace("controller", "");
                string accion = (new StackTrace().GetFrame(0)).GetMethod().Name.ToLower();


                if (!usuario.OpcionesMenuOff.ToList().Exists(x => x.StrControlador.ToLower() == controlador && x.StrAccion.ToLower() == accion))
                    //if (!usuario.OpcionesMenu.ToList().Exists(x => x.StrControlador.ToLower() == controlador && x.StrAccion.ToLower() == accion))
                {
                    return View("errorPermiso");
                }
                #endregion

                LoginController._menu = "01"; //añadido 18.06.2021

                //proxy_s = new srvHuellas.SistemaCNClient();
                List<ItemCombo> listaT = new List<ItemCombo>();//modificado 22.06.2021
                using (ZKSedeBL objBL = new ZKSedeBL())
                {
                    listaT = objBL.ListarSedeCombo();
                }
                //List<srvHuellas.ItemCombo> listaT = proxy_s.ListarSedeCombo().ToList();//new List<srvHuellas.ItemCombo>();// proxy_s.ListarSedeCombo().ToList();
                return View(listaT);
            }
            catch (System.Exception ex)
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
                Log.AlmacenarLogError(ex, "UsuarioLoginController.cs | ListarUsuarioLogin");
                return View();
            }
        }

        [HttpPost]
        public JsonResult ListarSedeJson(bool p_estatus) //se añadio input
        {
            List<ItemCombo> listaT = new List<ItemCombo>();
            //List<srvHuellas.ItemCombo> listaT = new List<srvHuellas.ItemCombo>();
            try
            {
                if (p_estatus == false)
                {//OFFLINE
                    if(_listSedesOff_!= null)
                    {
                        listaT = _listSedesOff_;
                    }
                }
                else
                {
                    //proxy_s = new srvHuellas.SistemaCNClient();//modificado 22.06.2021
                    //listaT = proxy_s.ListarSedeCombo().ToList();//modificado 22.06.2021
                    using (ZKSedeBL objBL = new ZKSedeBL())
                    {
                        listaT= objBL.ListarSedeCombo();
                    }
                }
            }
            catch (Exception ex)
            {
                var x = ex.InnerException.GetType();
                Log.AlmacenarLogError(ex, "UsuarioLoginController.cs | ListarUsuarioLogin");
                //throw ex;
            }
            return Json(listaT);
        }
        [HttpPost]
        public JsonResult ListarUsersJson() //añadido 15.06
        {
            //srvHuellas.ListZKLoginOff listaT = new srvHuellas.ListZKLoginOff();//modificado 22.06.2021
            ListZKLoginOff listaT = new ListZKLoginOff();
            try
            {
                //proxy_s = new srvHuellas.SistemaCNClient();//modificado 22.06.2021
                //srvHuellas.ListZKLogin listaT = proxy_s.ListarUsers_();
                //listaT = proxy_s.ListarUsers_();//modificado 22.06.2021
                using (ZKLoginBL objBL = new ZKLoginBL())
                {
                    listaT= objBL.ListarUsers_(0);
                }
            }
            catch (Exception ex)
            {
                Log.AlmacenarLogError(ex, "UsuarioLoginController.cs | ListarUsuarioLogin");
            }
            return Json(listaT);
        }
        [HttpPost]
        public JsonResult ListarMenusUsersJson() //añadido 16.06
        {
            //srvHuellas.ListZKMenuOff listaT = new srvHuellas.ListZKMenuOff();//modificado 22.06.2021
            ListZKMenuOff listaT = new ListZKMenuOff();
            try
            {
                //proxy_s = new srvHuellas.SistemaCNClient();//modificado 22.06.2021
                ////srvHuellas.ListZKLogin listaT = proxy_s.ListarUsers_();
                //listaT = proxy_s.ListarMenuUsers_();//modificado 22.06.2021
                using (ZKLoginBL objBL = new ZKLoginBL())
                {
                    listaT= objBL.ListarMenuUsers_(0);
                }
            }
            catch (Exception ex)
            {
                Log.AlmacenarLogError(ex, "UsuarioLoginController.cs | ListarMenusUsersJson");
            }
            return Json(listaT);
        }

        [HttpPost]
        public JsonResult ListarLoginJson()
        {
            //srvHuellas.ListZKLogin listaT = new srvHuellas.ListZKLogin();//modificado 22.06.2021
            ListZKLoginOff listaT = new ListZKLoginOff();
            try
            {
                //proxy_s = new srvHuellas.SistemaCNClient();//modificado 22.06.2021
                //listaT = proxy_s.ListarLogin();//modificado 22.06.2021
                using (ZKLoginBL objBL = new ZKLoginBL())
                {
                    listaT= objBL.ListarLogin(0);
                }
            }
            catch (Exception ex)
            {
                Log.AlmacenarLogError(ex, "UsuarioLoginController.cs | ListarLoginJson");
            }
            return Json(listaT);
        }
        [HttpPost]
        public JsonResult ListarMenuUsuarioJson(int x_idSesion)
        {
            //List<srvHuellas.ItemCombo> listaT = new List<srvHuellas.ItemCombo>();//modificado 22.06.2021
            List<ItemCombo> listaT = new List<ItemCombo>();
            try
            {
                //proxy_u = new srvHuellas.UsuarioCNClient();//modificado 22.06.2021
                //listaT = proxy_u.ListarMenuUsuarioCombo(x_idSesion).ToList();//modificado 22.06.2021
                using (ZKUsuariosBL objBL = new ZKUsuariosBL())
                {
                    listaT= objBL.ListarMenuUsuarioCombo(x_idSesion);
                }
            }
            catch (Exception ex)
            {
                Log.AlmacenarLogError(ex, "UsuarioLoginController.cs | ListarMenuUsuarioJson");
            }

            return Json(listaT);
        }

        //////[HttpPost]
        ////public ViewResult validarSesion()
        ////{
        ////    //if (Session["USUARIO"] == null)
        ////    //return RedirectToAction("Login", "Login");
        ////    return View("Login");
        ////}

        //////Prueba
        ////[HttpPost]
        ////public JsonResult ValidarSesionController()
        ////{
        ////    validarSesion();
        ////    Respuesta objRespuesta = new Respuesta();
        ////    objRespuesta = new Respuesta() { exito = false, type = "error", message = "Su sesión expiró, inicie sesión nuevamente." };
        ////    return Json(objRespuesta);

        ////}

        [HttpPost]
        public JsonResult RegistrarLogin(int x_iIdSesion, string x_sNombreSesion, string x_sPasswordSesion, int x_intIdSede, string x_menus)
        {
            Respuesta objRespuesta = new Respuesta();
            try
            {
                if (Session["USUARIO"] == null)
                {
                    objRespuesta = new Respuesta() { exito = false, type = "error", message = "Su sesión expiró, inicie sesión nuevamente." };
                    return Json(objRespuesta);
                }
                string mensaje = "";
                bool resultado = false;
                //srvHuellas.ZKLogin zkLogin = new srvHuellas.ZKLogin();//modificado 22.06.2021
                ZKLoginOff zkLogin = new ZKLoginOff();
                zkLogin.iIdSesion = x_iIdSesion;
                zkLogin.sNombreSesion = x_sNombreSesion;
                zkLogin.sPasswordSesion = x_sPasswordSesion;
                zkLogin.intIdSede = x_intIdSede;
                zkLogin.bitFlActivo = true;

                //proxy_s = new srvHuellas.SistemaCNClient();//modificado 22.06.2021
                //bool resultado = proxy_s.InsertarLogin(ref zkLogin, x_menus, ref mensaje);//modificado 22.06.2021
                using (ZKLoginBL objBL = new ZKLoginBL())
                {
                    resultado = objBL.InsertarLogin(ref zkLogin, x_menus, ref mensaje);
                }

                if (resultado)
                {
                    //zkLogin = proxy_s.ListarLoginID(zkLogin.iIdSesion);//modificado 22.06.2021
                    using (ZKLoginBL objBL = new ZKLoginBL())
                    {
                        zkLogin = objBL.ListarLoginID(zkLogin.iIdSesion);
                    }
                    mensaje = "Se ha insertado correctamente.";
                }

                objRespuesta = new Respuesta() { exito = resultado, message = mensaje, type = resultado ? "success" : "error", entidad = zkLogin };
            }
            catch (Exception ex)
            {
                Log.AlmacenarLogError(ex, "UsuarioLoginController.cs | RegistrarLogin");
            }
            return Json(objRespuesta);
        }


        [HttpPost]
        public JsonResult EliminarLogin(int x_iIdSesion)
        {
            Respuesta objRespuesta = new Respuesta();
            try
            {
                bool resultado = false;
                if (Session["USUARIO"] == null)
                {
                    objRespuesta = new Respuesta() { exito = false, type = "error", message = "Su sesión expiró, inicie sesión nuevamente." };
                    return Json(objRespuesta);
                }
                string mensaje = "";
                //proxy_s = new srvHuellas.SistemaCNClient();//modificado 22.06.2021
                //bool resultado = proxy_s.EliminarLogin(x_iIdSesion, ref mensaje);//modificado 22.06.2021
                using (ZKLoginBL objBL = new ZKLoginBL())
                {
                    resultado = objBL.EliminarLogin(x_iIdSesion, ref mensaje);
                }

                if (resultado)
                {
                    mensaje = "Se ha eliminado correctamente.";
                }

                objRespuesta = new Respuesta() { exito = resultado, message = mensaje, type = resultado ? "success" : "error" };
            }
            catch (Exception ex)
            {
                Log.AlmacenarLogError(ex, "UsuarioLoginController.cs | EliminarLogin");
            }

            return Json(objRespuesta);
        }
        [HttpPost]
        public JsonResult CambiarSede(int x_iIdSesion, int x_intIdSede, string x_menus)
        {
            Respuesta objRespuesta = new Respuesta();
            try
            {
                bool resultado = false;
                if (Session["USUARIO"] == null)
                {
                    objRespuesta = new Respuesta() { exito = false, type = "error", message = "Su sesión expiró, inicie sesión nuevamente." };
                    return Json(objRespuesta);
                }
                string mensaje = "";
                //proxy_s = new srvHuellas.SistemaCNClient();//modificado 22.06.2021
                //resultado = proxy_s.CambiarSede(x_iIdSesion, x_intIdSede, x_menus, ref mensaje);//modificado 22.06.2021

                using (ZKLoginBL objBL = new ZKLoginBL())
                {
                    resultado = objBL.CambiarSede(x_iIdSesion, x_intIdSede, x_menus, ref mensaje);
                }

                //srvHuellas.ZKLogin zklogin = new srvHuellas.ZKLogin();//modificado 22.06.2021
                ZKLoginOff zklogin = new ZKLoginOff();
                if (resultado)
                {
                    //zklogin = proxy_s.ListarLoginID(x_iIdSesion);//modificado 22.06.2021
                    using (ZKLoginBL objBL = new ZKLoginBL())
                    {
                        zklogin = objBL.ListarLoginID(x_iIdSesion);
                    }
                    mensaje = "Se ha actualizado correctamente.";
                }

                objRespuesta = new Respuesta() { exito = resultado, message = mensaje, type = resultado ? "success" : "error", entidad = zklogin };
            }
            catch (Exception ex)
            {
                Log.AlmacenarLogError(ex, "UsuarioLoginController.cs | CambiarSede");
            }

            return Json(objRespuesta);
        }
        [HttpPost]
        public JsonResult CambiarPassword(int x_iIdSesion, string x_sPasswordSesion)
        {
            Respuesta objRespuesta = new Respuesta();
            try
            {
                bool resultado = false;
                if (Session["USUARIO"] == null)
                {
                    objRespuesta = new Respuesta() { exito = false, type = "error", message = "Su sesión expiró, inicie sesión nuevamente." };
                    return Json(objRespuesta);
                }

                string mensaje = "";
                //proxy_s = new srvHuellas.SistemaCNClient();//modificado 22.06.2021
                //bool resultado = proxy_s.CambiarContraseña(x_iIdSesion, x_sPasswordSesion, ref mensaje);//modificado 22.06.2021
                using (ZKLoginBL objBL = new ZKLoginBL())
                {
                    resultado = objBL.CambiarContraseña(x_iIdSesion, x_sPasswordSesion, ref mensaje);
                }

                if (resultado)
                {
                    mensaje = "Se ha cambiado la clave correctamente.";
                }

                objRespuesta = new Respuesta() { exito = resultado, message = mensaje, type = resultado ? "success" : "error" };
            }
            catch (Exception ex)
            {
                Log.AlmacenarLogError(ex, "UsuarioLoginController.cs | CambiarPassword");
            }

            return Json(objRespuesta);
        }






        [HttpPost]
        public JsonResult IUTiempoEntreMrcaciones(int x_iIdSesion, string x_sNombreSesion, string x_sPasswordSesion, int x_intIdSede, string x_menus)
        {
            Respuesta objRespuesta = new Respuesta();
            try
            {
                if (Session["USUARIO"] == null)
                {
                    objRespuesta = new Respuesta() { exito = false, type = "error", message = "Su sesión expiró, inicie sesión nuevamente." };
                    return Json(objRespuesta);
                }
                string mensaje = "";
                bool resultado = false;
                ZKLoginOff zkLogin = new ZKLoginOff();
                zkLogin.iIdSesion = x_iIdSesion;
                zkLogin.sNombreSesion = x_sNombreSesion;
                zkLogin.sPasswordSesion = x_sPasswordSesion;
                zkLogin.intIdSede = x_intIdSede;
                zkLogin.bitFlActivo = true;

                using (ZKLoginBL objBL = new ZKLoginBL())
                {
                    resultado = objBL.IUTiempoEntreMrcaciones(ref zkLogin, x_menus, ref mensaje);
                }

                if (resultado)
                {
                    using (ZKLoginBL objBL = new ZKLoginBL())
                    {
                        zkLogin = objBL.ListarLoginID(zkLogin.iIdSesion);
                    }
                    mensaje = "Se ha insertado correctamente.";
                }

                objRespuesta = new Respuesta() { exito = resultado, message = mensaje, type = resultado ? "success" : "error", entidad = zkLogin };
            }
            catch (Exception ex)
            {
                Log.AlmacenarLogError(ex, "UsuarioLoginController.cs | RegistrarLogin");
            }
            return Json(objRespuesta);
        }






    }
}