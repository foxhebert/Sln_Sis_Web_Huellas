using Dominio.Entidades;
using Dominio.Repositorio;
using SisWebTickets.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace SisWebTickets.Controllers
{
    public class TicketController : Controller
    {
        private EstadoManager EstadoManager = new EstadoManager();

        private MotivoManager MotivoManager = new MotivoManager();

        private PrioridadManager PrioridadManager = new PrioridadManager();

        private TicketManager TicketManager = new TicketManager();

        private EncargadoManager EncargadoManager = new EncargadoManager();

        private AreaManager areaManager = new AreaManager();

        public ActionResult ConsultaTicket()
        {
            if (Session["USUARIO"] != null)
            {
                new UsuarioEN();
                UsuarioEN objUsuario = (UsuarioEN)Session["USUARIO"];
                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFin = DateTime.Now;
                if (Session["FECHAS_MEMORY"] != null)
                {
                    string fechasMemory = Session["FECHAS_MEMORY"].ToString();
                    string[] fechas = fechasMemory.Split(',');
                    fechaInicio = DateTime.Parse(fechas[0]);
                    fechaFin = DateTime.Parse(fechas[1]);
                }
                ViewBag.ESTADO = new SelectList(EstadoManager.ListarEstado(), "IdEstado", "DesEstado");
                if (objUsuario.rol.IdRol == 1 | objUsuario.rol.IdRol == 3)
                    ViewBag.AREA = new SelectList(areaManager.ListarAreas(), "IdArea", "descArea");

                List<TicketEN> listaT = TicketManager.ListarTicketsGenerados(0, fechaInicio, fechaFin, objUsuario.IdUsu, 0, 0);
                return View(listaT);
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public PartialViewResult FiltrarTicketLista(string fechaInicio, string fechaFin, int nroTck = 0, int intEstado = 0, int intArea = 0)
        {
            Session["FECHAS_MEMORY"] = fechaInicio + "," + fechaFin;

            UsuarioEN objUsuario = (UsuarioEN)Session["USUARIO"];
            if (objUsuario != null)
            {
                List<TicketEN> listaT = TicketManager.ListarTicketsGenerados(nroTck, DateTime.Parse(fechaInicio), DateTime.Parse(fechaFin), objUsuario.IdUsu, intEstado, intArea);
                return PartialView("_partialFiltrarTicketLista", listaT);
            }
            else
            {
                return PartialView("_partialEmpty");
            }

        }

        public ActionResult Nuevo_Ticket()
        {
            if (Session["USUARIO"] != null)
            {
                DetalleTicketEN objDet = new DetalleTicketEN();
                objDet.Id = 0;
                TicketEN objTck = new TicketEN();
                objTck.Nro = TicketManager.ConsultarCorrelativo();
                objTck.detalle = objDet;
                objTck.detalle.FechaRegistro = DateTime.Now;
                objTck.detalle.horaRegistro = DateTime.Now.ToString("HH:mm:ss");
                ViewBag.MOTIVO = new SelectList(MotivoManager.ListarMotivo(), "IdMotivo", "DescMotivo");
                ViewBag.PRIORIDAD = new SelectList(PrioridadManager.ListarPrioridad(), "IdPrio", "DesPrio");
                ViewBag.ASIGNAR = new SelectList(EncargadoManager.ListarEncargado(), "IdEnc", "Nombre");
                ViewBag.ESTADO = new SelectList(EstadoManager.ListarEstado(), "IdEstado", "DesEstado", 1);
                return View(objTck);
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult Nuevo_Ticket(TicketEN objT)
        {
            int insert = -1;
            try
            {
                UsuarioEN objUsuario = (UsuarioEN)Session["USUARIO"];
                string imagenes = "";
                objT.detalle.FechaRegistro = DateTime.Parse(objT.detalle.FechaRegistro.ToString("dd/MM/yyyy").Trim() + " " + objT.detalle.horaRegistro.Trim());
                int indice = 0;
                if (objT.detalle.adjunto != null && Request.Files.Count > 0)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase file = Request.Files[i];
                        string extension = Path.GetExtension(file.FileName);
                        Path.GetFileName(file.FileName);
                        string folder = Server.MapPath("~/images/" + objUsuario.empresa.RazonSocial);
                        if (!Directory.Exists(folder))
                        {
                            Directory.CreateDirectory(folder);
                        }
                        string rutaImagen = objUsuario.empresa.RazonSocial + "/Ticket_" + objT.Nro + "_" + objT.detalle.Id + "_" + indice + extension;
                        string filePath = Server.MapPath("~/images/" + rutaImagen);
                        file.SaveAs(filePath);
                        imagenes = imagenes + rutaImagen + ";";
                        indice++;
                    }
                    objT.detalle.adjunto = imagenes.Substring(0, imagenes.Length - 1);
                }
                int nroRegis = 0;
                insert = TicketManager.InsertarNuevoTicket(objT, objUsuario.IdUsu, ref nroRegis);
                if (nroRegis > 0)
                {
                    TicketEN datosCorreo = TicketManager.datosEmail(nroRegis);
                    SendEmail send = new SendEmail();
                    send.enviarCorreo(datosCorreo);
                }
            }
            catch (Exception ex)
            {
                ViewBag.MENSAJE = ex.Message;
            }
            if (insert >= 1)
            {
                string respuesta = "Ticket grabado correctamente";
                TempData["RESPUESTA"] = respuesta;
                return RedirectToAction("ConsultaTicket");
            }
            return RedirectToAction("Nuevo_Ticket");
        }

        public ActionResult Editar_Ticket(int id = 0, bool add = false)
        {

            if (Session["USUARIO"] != null)
            {
                UsuarioEN objUsuario = (UsuarioEN)Session["USUARIO"];
                List<TicketEN> listaT = TicketManager.BuscarTicket(id, add, objUsuario.IdUsu);
                ViewBag.MOTIVO = MotivoManager.ListarMotivo();
                ViewBag.PRIORIDAD = PrioridadManager.ListarPrioridad();
                ViewBag.ASIGNAR = EncargadoManager.ListarEncargado();
                ViewBag.ESTADO = EstadoManager.ListarEstado();
                ViewBag.AVANCE = listaAvance();
                return View(listaT);
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult Editar_Ticket(TicketEN objT)
        {
            int insert = -1;
            string fecha = "";
            try
            {
                UsuarioEN objUsuario = (UsuarioEN)Session["USUARIO"];
                string imagenes = "";
                string pdfs = "";
                int indice = 0;
                //if ((objT.detalle.adjunto != null || objT.detalle.GuiaServicio != null) && Request.Files.Count > 0)
                if(Request.Files.Count > 0)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase file = Request.Files[i];
                        string extension = Path.GetExtension(file.FileName);
                        Path.GetFileName(file.FileName);
                        if (extension.ToLower().Equals(".pdf"))
                        {
                            string folderPDF = Server.MapPath("~/PDF/" + objUsuario.empresa.RazonSocial);
                            if (!Directory.Exists(folderPDF))
                            {
                                Directory.CreateDirectory(folderPDF);
                            }
                            string rutaPDF = objUsuario.empresa.RazonSocial + "/GuiaServicio_" + objT.Nro + "_" + objT.detalle.Id + "_" + indice + extension;
                            string filePath2 = Server.MapPath("~/PDF/" + rutaPDF);
                            file.SaveAs(filePath2);
                            pdfs = pdfs + rutaPDF + ";";
                        }
                        else if (extension.Equals(".jpg") | extension.Equals(".gif") | extension.Equals(".jpeg") | extension.Equals(".png"))
                        {
                            string folder = Server.MapPath("~/images/" + objUsuario.empresa.RazonSocial);
                            if (!Directory.Exists(folder))
                            {
                                Directory.CreateDirectory(folder);
                            }
                            string rutaImagen = objUsuario.empresa.RazonSocial + "/Ticket_" + objT.Nro + "_" + objT.detalle.Id + "_" + indice + extension;
                            string filePath = Server.MapPath("~/images/" + rutaImagen);
                            file.SaveAs(filePath);
                            imagenes = imagenes + rutaImagen + ";";
                        }
                        indice++;
                    }
                    if (!imagenes.Equals(""))
                    {
                        objT.detalle.adjunto = imagenes.Substring(0, imagenes.Length - 1);
                    }
                    if (!pdfs.Equals(""))
                    {
                        objT.detalle.GuiaServicio = pdfs.Substring(0, pdfs.Length - 1);
                    }
                }
                insert = TicketManager.InsertarDetalleTicket(objT, ref fecha);
            }
            catch (Exception ex)
            {
                ViewBag.MENSAJE = ex.Message;
            }
            if (insert >= 1)
            {
                string hora = DateTime.Parse(fecha).ToString("HH:mm:ss");
                string date = DateTime.Parse(fecha).ToString("dd/MM/yyyy");
                string respuesta = "Soporte grabado correctamente para el ticket Nro " + objT.Nro + " a las " + hora + " del día " + date;
                TempData["RESPUESTA"] = respuesta;
                return RedirectToAction("ConsultaTicket");
            }
            return RedirectToAction("Editar_Ticket/" + objT.Nro);
        }

        [HttpPost]
        public JsonResult CambiarEstadoTicket(int id, string observa)
        {
            Response respuesta = new Response();
            try
            {
                int delete = TicketManager.CambiarEstadoTicket(id, observa);
                if (delete >= 1)
                {
                    respuesta.success = "#" + id;
                }
            }
            catch (Exception ex)
            {
                respuesta.error = "No se pudo eliminar " + ex.Message;
            }
            return Json(respuesta);
        }

        private List<int> listaAvance()
        {
            List<int> listAvance = new List<int>();
            for (int i = 0; i <= 100; i += 10)
            {
                listAvance.Add(i);
            }
            return listAvance;
        }

        private List<Valor> CartaConfList()
        {
            List<Valor> cartaConf = new List<Valor>();
            cartaConf.Add(new Valor(false, "NO"));
            cartaConf.Add(new Valor(true, "SI"));
            return cartaConf;
        }
    }
}