using CBX_Web_SISCOP.Controllers;
using SisWebHuellas.App_Start;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//añadidos para quitar wcf
using Dominio.Entidades;

namespace SisWebHuellas.Controllers
{
    public class SedeController : Controller
    {
        //private srvHuellas.SistemaCNClient proxy_s;
        // GET: Sede
        public ActionResult ListarSedes()
        {
            try
            {
                #region VALIDAR SESION VIGENTE Y PERMISOS DE MENÚ
                if (Session["USUARIO"] == null)
                    return RedirectToAction("Login", "Login");

                //srvHuellas.ZKLoginOff usuario = (srvHuellas.ZKLoginOff)Session["USUARIO"];//modificado 22.06.2021
                ZKLoginOff usuario = (ZKLoginOff)Session["USUARIO"];
                //srvHuellas.ZKLogin usuario = (srvHuellas.ZKLogin)Session["USUARIO"];
                string controlador = this.GetType().Name.ToLower().Replace("controller", "");
                string accion = (new StackTrace().GetFrame(0)).GetMethod().Name.ToLower();


                if (!usuario.OpcionesMenuOff.ToList().Exists(x => x.StrControlador.ToLower() == controlador && x.StrAccion.ToLower() == accion))//modificado 22.06.2021
                {
                    return View("errorPermiso");
                }
                #endregion

                //proxy_s = new srvHuellas.SistemaCNClient();
                //srvHuellas.ListZKSede listaT = new srvHuellas.ListZKSede();//modificado 22.06.2021
                ListZKSede listaT = new ListZKSede();
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
                Log.AlmacenarLogError(ex, "SedeController.cs | ListarSedes");
                return View();
            }
        }

        [HttpPost]
        public JsonResult RegistrarSede(int x_iIdSede, string x_strCoSede, string x_strDeSede, string x_strDireccion)
        {
            Respuesta objRespuesta = new Respuesta();
            if (Session["USUARIO"] == null)
             {
                 objRespuesta = new Respuesta() { exito = false, type = "error", message = "Su sesión expiró, inicie sesión nuevamente." };
                 return Json(objRespuesta);
             }

            /* 
             string mensaje = "";
             srvHuellas.ZKLogin zkLogin = new srvHuellas.ZKLogin();
             zkLogin.iIdSesion = x_iIdSesion;
             zkLogin.sNombreSesion = x_sNombreSesion;
             zkLogin.sPasswordSesion = x_sPasswordSesion;
             zkLogin.intIdSede = x_intIdSede;
             zkLogin.bitFlActivo = true;

             proxy_s = new srvHuellas.SistemaCNClient();
             bool resultado = proxy_s.InsertarLogin(ref zkLogin, x_menus, ref mensaje);
             if (resultado)
             {
                 zkLogin = proxy_s.ListarLoginID(zkLogin.iIdSesion);
                 mensaje = "Se ha insertado correctamente.";
             }

             objRespuesta = new Respuesta() { exito = resultado, message = mensaje, type = resultado ? "success" : "error", entidad = zkLogin };*/

            return Json(objRespuesta);
        }

        [HttpPost]
        public JsonResult EliminarSede(int x_iIdSede)
        {
            Respuesta objRespuesta = new Respuesta();
            /*if (Session["USUARIO"] == null)
            {
                objRespuesta = new Respuesta() { exito = false, type = "error", message = "Su sesión expiró, inicie sesión nuevamente." };
                return Json(objRespuesta);
            }
            proxy_s = new srvHuellas.SistemaCNClient();
            string mensaje = "";
            bool resultado = proxy_s.EliminarLogin(x_iIdSesion, ref mensaje);
            if (resultado)
            {
                mensaje = "Se ha eliminado correctamente.";
            }

            objRespuesta = new Respuesta() { exito = resultado, message = mensaje, type = resultado ? "success" : "error" };*/
            return Json(objRespuesta);
        }
        
    }
}