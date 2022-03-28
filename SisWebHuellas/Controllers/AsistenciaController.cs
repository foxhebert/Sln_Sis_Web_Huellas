using System;
using System.Globalization;
using System.Web.Mvc;
using SisWebHuellas.App_Start;
using System.Linq;
using System.Diagnostics;
using CBX_Web_SISCOP.Controllers;
//añadidos para quitar wcf
using Dominio.Entidades;
using Dominio.Entidades.Personalizado;
using Dominio.Repositorio;

namespace SisWebHuellas.Controllers
{
    public class AsistenciaController : Controller
    {
        //private srvHuellas.UsuarioCNClient proxy;//modificado 22.06.2021

        // GET: Pedido
        public ActionResult ListarAsistencias()
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


                if (!usuario.OpcionesMenuOff.ToList().Exists(x => x.StrControlador.ToLower() == controlador && x.StrAccion.ToLower() == accion))
                {
                    return View("errorPermiso");
                }
                #endregion
                LoginController._menu = "03"; //añadido 18.06.2021
                //proxy = new srvHuellas.UsuarioCNClient();//modificado 22.06.2021
                //var pedidos = proxy.ListarMarcaciones(DateTime.Today.AddDays(-1), DateTime.Today, "", "", 1, usuario.intIdSede);//modificado 22.06.2021
                ListItemAsistencia pedidos = new ListItemAsistencia();

                using (ZKMarcacionesBL objBL = new ZKMarcacionesBL())
                {
                    pedidos= objBL.ListarMarcaciones(DateTime.Today.AddDays(-1), DateTime.Today, "", "", 1, usuario.intIdSede);
                }

                return View(pedidos);
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
                Log.AlmacenarLogError(ex, "AsistenciaController.cs | ListarAsistencias");
                return View();
            }
        }

        //POST: Filtrar Pedido
        [HttpPost]
        public PartialViewResult FiltrarAsistencia(string x_fechaIni, string x_fechaFin, string x_criterio, string x_filtro, int x_estado)
        {
            //srvHuellas.ListItemAsistencia lista = new srvHuellas.ListItemAsistencia();//modificado 22.06.2021
            ListItemAsistencia lista = new ListItemAsistencia();
            try
            {
                if (Session["USUARIO"] == null)
                {
                    return PartialView("_partialSessionExpired");
                }
                DateTime feIni = DateTime.ParseExact(x_fechaIni, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime feFin = DateTime.ParseExact(x_fechaFin, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                using (ZKMarcacionesBL objBL = new ZKMarcacionesBL())
                {
                    lista = objBL.ListarMarcaciones(feIni, feFin, x_criterio, x_filtro, x_estado, 0);
                }
                //proxy = new srvHuellas.UsuarioCNClient();//modificado 22.06.2021
                //lista = proxy.ListarMarcaciones(feIni, feFin, x_criterio, x_filtro, x_estado, 0);//modificado 22.06.2021
            }
            catch (Exception ex)
            {
                Log.AlmacenarLogError(ex, "AsistenciaController.cs | FiltrarAsistencia");
            }
            return PartialView("FiltrarAsistencia", lista);
        }

        [HttpPost]
        public JsonResult FiltrarAsistenciaJSON(string x_fechaIni, string x_fechaFin, string x_criterio, string x_filtro, int x_estado, int x_idSede)
        {
            //srvHuellas.ListItemAsistencia lista = new srvHuellas.ListItemAsistencia();//modificado 22.06.2021
            ListItemAsistencia lista = new ListItemAsistencia();
            try
            {
                DateTime feIni = DateTime.ParseExact(x_fechaIni, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime feFin = DateTime.ParseExact(x_fechaFin, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                using (ZKMarcacionesBL objBL = new ZKMarcacionesBL())
                {
                    lista = objBL.ListarMarcaciones(feIni, feFin, x_criterio, x_filtro, x_estado, x_idSede);
                }
                //proxy = new srvHuellas.UsuarioCNClient();//modificado 22.06.2021
                //lista = proxy.ListarMarcaciones(feIni, feFin, x_criterio, x_filtro, x_estado, x_idSede);//modificado 22.06.2021
            }
            catch (Exception ex)
            {
                Log.AlmacenarLogError(ex, "AsistenciaController.cs | FiltrarAsistenciaJSON");
            }
            return Json(lista);

        }

        [HttpPost]
        public JsonResult RegistraMarca(string x_huella, string x_sSerie)
        {
            Respuesta objRespuesta = new Respuesta();

            try
            {
                bool exito = false;
                if (Session["USUARIO"] == null)
                {
                    objRespuesta = new Respuesta() { exito = false, type = "error", message = "Su sesión expiró, inicie sesión nuevamente." };
                    return Json(objRespuesta);
                }

                int idLocal = 0;
                if (Session["LOCAL"] != null)
                    idLocal = Convert.ToInt32(Session["LOCAL"]);


                string mensaje = "";

                //proxy = new srvHuellas.UsuarioCNClient();//modificado 22.06.2021
                // proxy.RegistrarMarca(x_huella, x_sSerie, idLocal, ref mensaje);//modificado 22.06.2021
                using (ZKMarcacionesBL objBL = new ZKMarcacionesBL())
                {
                    int numDedo = 0;
                    exito = objBL.RegistrarMarca(x_huella, x_sSerie, idLocal, ref numDedo, ref mensaje);
                }


                string tipo = mensaje.Substring(0, 1);
                mensaje = mensaje.Substring(1);

                if (tipo == "0")
                    tipo = "warning";
                else if (tipo == "1")
                    tipo = "success";
                else if (tipo == "2")
                    tipo = "error";

                objRespuesta = new Respuesta() { exito = exito, message = mensaje, type = tipo };
            }
            catch (Exception ex)
            {
                Log.AlmacenarLogError(ex, "AsistenciaController.cs | RegistraMarca");
            }
            return Json(objRespuesta);
        }

        [HttpPost]
        public JsonResult RegistraMarca_(string x_huella, string x_sSerie, int x_idLocal, DateTime x_FechaHora, bool x_Pre)
        {
            Log.AlmacenarLogMensaje(x_huella, "Huella 10: ");//añadido para revisar dato 22.06.2021
            Respuesta objRespuesta = new Respuesta();
            try
            {
                if (Session["USUARIO"] == null)
                {
                    objRespuesta = new Respuesta() { exito = false, type = "error", message = "Su sesión expiró, inicie sesión nuevamente." };
                    return Json(objRespuesta);
                }

                string mensaje = "";
                bool exito = false;
                int numDedo = 0;
                //proxy = new srvHuellas.UsuarioCNClient();//modificado 22.06.2021

                if (x_Pre)//del Local Storage: x_Pre=true
                {
                    using (ZKMarcacionesBL objBL = new ZKMarcacionesBL())
                    {
                        exito = objBL.RegistrarMarca_(x_huella, x_sSerie, x_idLocal, ref numDedo, x_FechaHora, ref mensaje);
                    }
                    //exito = proxy.RegistrarMarca_(x_huella, x_sSerie, x_idLocal, x_FechaHora, ref mensaje); //nuevo método//modificado 22.06.2021
                }
                else
                {
                    if (x_idLocal == 0)
                    {
                        //srvHuellas.ZKLoginOff usuario = (srvHuellas.ZKLoginOff)Session["USUARIO"];//modificado 22.06.2021
                        ZKLoginOff usuario = (ZKLoginOff)Session["USUARIO"];
                        x_idLocal = usuario.intIdSede;
                    }
                    using (ZKMarcacionesBL objBL = new ZKMarcacionesBL())
                    {
                        exito = objBL.RegistrarMarca(x_huella, x_sSerie, x_idLocal, ref numDedo, ref mensaje);
                    }
                    //exito = proxy.RegistrarMarca(x_huella, x_sSerie, x_idLocal, ref mensaje);//modificado 22.06.2021
                }
                Log.AlmacenarLogMensaje(x_idLocal.ToString(), "IdSedeMarca: ");//añadido para revisar dato 22.06.2021
                string tipo = mensaje.Substring(0, 1);
                mensaje = mensaje.Substring(1);

                if (tipo == "0")
                    tipo = "warning";
                else if (tipo == "1")
                    tipo = "success";
                else if (tipo == "2")
                    tipo = "error";

                objRespuesta = new Respuesta() { exito = exito, message = mensaje, type = tipo };
            }
            catch (Exception ex)
            {
                Log.AlmacenarLogError(ex, "AsistenciaController.cs | RegistraMarca_");
            }
            return Json(objRespuesta);
        }

    }
}