using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using SisWebHuellas.App_Start;
using System.Text;
using System.Diagnostics;
using System.Web;
using Excel;
using System.IO;
using System.Collections;
using CBX_Web_SISCOP.Controllers;
//añadidos para quitar wcf
using Dominio.Entidades;
using Dominio.Repositorio;
using Dominio.Entidades.Personalizado;


namespace SisWebHuellas.Controllers
{
    public class LectorController : Controller
    {
        public ActionResult ListarLector() //añadir input 16.06.2021
        {
            try
            {
                #region VALIDAR SESION VIGENTE Y PERMISOS DE MENÚ
                if (Session["USUARIO"] == null)
                    return RedirectToAction("Login", "Login");

                ZKLoginOff usuario = (ZKLoginOff)Session["USUARIO"];
                string controlador = this.GetType().Name.ToLower().Replace("controller", "");
                string accion = (new StackTrace().GetFrame(0)).GetMethod().Name.ToLower();


                if (!usuario.OpcionesMenuOff.ToList().Exists(x => x.StrControlador.ToLower() == controlador && x.StrAccion.ToLower() == accion))
                {
                    return View("errorPermiso");
                }
                #endregion

                LoginController._menu = "04";
                //ListZKTerminal Lista = new ListZKTerminal();

                //using (ZKLectorBL objBL = new ZKLectorBL())
                //{
                //    Lista = objBL.ListarLectores("", 0, 0);//Lista = objBL.ListarLectores(x_filtro, x_estado, x_IdTerminal);
                //}
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
                //var x = ex.InnerException.GetType();
                Log.AlmacenarLogError(ex, "LectorController.cs | ListarLector");
                return View();
            }
        }

        [HttpPost]
        public JsonResult ListarLectorJson(string x_filtro, int x_estado, int x_IdTerminal)
        {
            ListZKTerminal listaT = new ListZKTerminal();
            try
            {
                using (ZKLectorBL objBL = new ZKLectorBL())
                {
                    listaT = objBL.ListarLectores(x_filtro, x_estado, x_IdTerminal);//Lista = objBL.ListarLectores(x_filtro, x_estado, x_IdTerminal);
                }
            }
            catch (Exception ex)
            {
                Log.AlmacenarLogError(ex, "LectorController.cs | ListarLectorJson");
            }
            return Json(listaT);
        }

        [HttpPost]
        public JsonResult RegistraLector(int IdTerminal, int x_numero, string x_serie, string x_descripcion)
        {
            Respuesta objRespuesta = new Respuesta();
            try
            {
                if (Session["USUARIO"] == null)
                {
                    objRespuesta = new Respuesta() { exito = false, type = "error", message = "Su sesión expiró, inicie sesión nuevamente.", entidad = 10 };
                    return Json(objRespuesta);
                }

                ZKTerminal x_terminal = new ZKTerminal();
                x_terminal.iIdTerminal = IdTerminal;
                x_terminal.iNumero = x_numero;
                x_terminal.sSerie = x_serie;
                x_terminal.sDescripcion = x_descripcion;

                string mensaje = "";
                bool resultado = false;
                // bool resultado = proxy.InsertarUsuario(x_dedosAnt, ref usuario, ref mensaje);//modificado 22.06.2021
                using (ZKLectorBL objBL = new ZKLectorBL())
                {
                    resultado = objBL.InsertarLector(ref x_terminal, ref mensaje);
                }
                if (resultado)
                {
                    mensaje = (IdTerminal != x_terminal.iIdTerminal ? "Se ha insertado correctamente." : "Se ha actualizado correctamente.");
                }

                objRespuesta = new Respuesta() { exito = resultado, message = mensaje, type = resultado ? "success" : "error", entidad = x_terminal };
            }
            catch (Exception ex)
            {
                Log.AlmacenarLogError(ex, "LectorController.cs | RegistraLector");
            }
            return Json(objRespuesta);
        }

        [HttpPost]
        public JsonResult EliminarLector(int x_idTerminal)
        {
            Respuesta objRespuesta = new Respuesta();
            try
            {
                if (Session["USUARIO"] == null)
                {
                    objRespuesta = new Respuesta() { exito = false, type = "error", message = "Su sesión expiró, inicie sesión nuevamente." };
                    return Json(objRespuesta);
                }

                ZKLoginOff usuario = (ZKLoginOff)Session["USUARIO"];

                string mensaje = "";
                bool resultado = false;

                using (ZKLectorBL objBL = new ZKLectorBL())
                {
                    resultado = objBL.EliminarLector(x_idTerminal, ref mensaje);
                }
                if (resultado)
                {
                    mensaje = "Se ha eliminado correctamente.";
                }

                objRespuesta = new Respuesta() { exito = resultado, message = mensaje, type = resultado ? "success" : "error" };
            }
            catch (Exception ex)
            {
                Log.AlmacenarLogError(ex, "LectorController.cs | EliminarRegistro");
            }
            return Json(objRespuesta);
        }

        [HttpPost]
        public JsonResult CambiarEstadoLector(int x_idTerminal, int x_estado)
        {
            Respuesta objRespuesta = new Respuesta();
            try
            {
                if (Session["USUARIO"] == null)
                {
                    objRespuesta = new Respuesta() { exito = false, type = "error", message = "Su sesión expiró, inicie sesión nuevamente." };
                    return Json(objRespuesta);
                }

                ZKLoginOff login = (ZKLoginOff)Session["USUARIO"];

                string mensaje = "";
                bool resultado = false;

                using (ZKLectorBL objBL = new ZKLectorBL())
                {
                    resultado = objBL.CambiarEstadoLector(x_idTerminal, x_estado, ref mensaje);
                }
                if (resultado)
                {
                    mensaje = x_estado == 1 ? "Se ha Activado correctamente." : "Se ha Inactivado correctamente.";
                }

                objRespuesta = new Respuesta() { exito = resultado, message = mensaje, type = resultado ? "success" : "error"};
                //objRespuesta = new Respuesta() { exito = resultado, message = mensaje, type = resultado ? "success" : "error", entidad = usuario };
            }
            catch (Exception ex)
            {
                Log.AlmacenarLogError(ex, "LectorController.cs | CambiarEstadoRegistro");
            }
            return Json(objRespuesta);
        }
    }
}