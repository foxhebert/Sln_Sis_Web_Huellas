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
using Dominio.Entidades;
using Dominio.Repositorio;
//añadidos para quitar wcf
namespace SisWebHuellas.Controllers
{
    public class SedesController : Controller
    {
       
        // GET: Sedes
        public ActionResult ListarSedes()
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

            LoginController._menu = "05";
            return View();
        }


        [HttpPost]
        //public JsonResult ListarLectorJson(string x_filtro, int x_estado, int x_IdTerminal)
        public JsonResult ListarSedesJson(string x_filtro, int x_estado, int x_intIdSede)
        {
            

            ListZKSede listaT = new ListZKSede();
            try
            {
                using (ZKSedesBL objBL = new ZKSedesBL())
                {
                    listaT = objBL.ListarSedes(x_filtro, x_estado, x_intIdSede);//Lista = objBL.ListarLectores(x_filtro, x_estado, x_IdTerminal);
                }
            }
            catch (Exception ex)
            {
                Log.AlmacenarLogError(ex, "LectorController.cs | ListarLectorJson");
            }
            return Json(listaT);
        }

        [HttpPost]
        public JsonResult RegistraSedes(int _intIdSede, string _strCoLocal, string _strDeLocal, string _strDireccion, int _bitActivo, string _strEstadoActivo)
             //public JsonResult RegistraLector(int IdTerminal, int x_numero, string x_serie, string x_descripcion)
        {
            Respuesta objRespuesta = new Respuesta();
            try
            {
                if (Session["USUARIO"] == null)
                {
                    objRespuesta = new Respuesta() { exito = false, type = "error", message = "Su sesión expiró, inicie sesión nuevamente.", entidad = 10 };
                    return Json(objRespuesta);
                }

                ZKSede objSedes = new ZKSede();
                objSedes.intIdSede       = _intIdSede    ; 
                objSedes.strCoLocal      = _strCoLocal   ;
                objSedes.strDeLocal      = _strDeLocal   ;
                objSedes.strDireccion    = _strDireccion ;
                objSedes.bitActivo       = Convert.ToBoolean(_bitActivo); //Integer to bool C#  https://www.delftstack.com/howto/csharp/convert-int-to-bool-in-csharp/
                objSedes.strEstadoActivo = _strEstadoActivo;


                 string mensaje = "";
                bool resultado = false;
                // bool resultado = proxy.InsertarUsuario(x_dedosAnt, ref usuario, ref mensaje);//modificado 22.06.2021
                using (ZKSedesBL objBL = new ZKSedesBL())
                {
                    resultado = objBL.InsertarSedes(ref objSedes, ref mensaje);
                    //resultado = objBL.InsertarLector(ref x_terminal, ref mensaje);
                }
                if (resultado)
                {
                    mensaje = (_intIdSede != objSedes.intIdSede ? "Se ha insertado correctamente." : "Se ha actualizado correctamente.");
                }

                objRespuesta = new Respuesta() { exito = resultado, message = mensaje, type = resultado ? "success" : "error", entidad = objSedes };
            }
            catch (Exception ex)
            {
                Log.AlmacenarLogError(ex, "LectorController.cs | RegistraLector");
            }
            return Json(objRespuesta);
        }

        [HttpPost]
        public JsonResult EliminarSedes(int x_intIdSede)
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

                using (ZKSedesBL objBL = new ZKSedesBL())
                {
                    resultado = objBL.EliminarSedes(x_intIdSede, ref mensaje);
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
        public JsonResult CambiarEstadoSedes(int x_intIdSede, int x_estado)
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

                using (ZKSedesBL objBL = new ZKSedesBL())
                {
                    resultado = objBL.CambiarEstadoSedes(x_intIdSede, x_estado, ref mensaje);
                }
                if (resultado)
                {
                    mensaje = x_estado == 1 ? "Se ha Activado correctamente." : "Se ha Inactivado correctamente.";
                }

                objRespuesta = new Respuesta() { exito = resultado, message = mensaje, type = resultado ? "success" : "error" };
                //objRespuesta = new Respuesta() { exito = resultado, message = mensaje, type = resultado ? "success" : "error", entidad = usuario };
            }
            catch (Exception ex)
            {
                Log.AlmacenarLogError(ex, "LectorController.cs | CambiarEstadoRegistro");
            }
            return Json(objRespuesta);
        }

        [HttpPost]
        //public JsonResult ListarLectorJson(string x_filtro, int x_estado, int x_IdTerminal)
        public JsonResult ListarTiempoEntreMarca(string x_filtro)
        {
            ListZKSede listaT = new ListZKSede();
            try
            {
                using (ZKSedesBL objBL = new ZKSedesBL())
                {
                    listaT = objBL.ListarTiempoEntreMarca(x_filtro);//Lista = objBL.ListarLectores(x_filtro, x_estado, x_IdTerminal);
                }
            }
            catch (Exception ex)
            {
                Log.AlmacenarLogError(ex, "LectorController.cs | ListarLectorJson");
            }
            return Json(listaT);
        }




    }
}