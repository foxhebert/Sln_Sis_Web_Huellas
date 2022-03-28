using Dominio.Entidades;
using Dominio.Repositorio;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SisWebTickets.Controllers
{
    public class ReporteController : Controller
    {
        public ActionResult ReporteDiario()
        {
            if (Session["USUARIO"] != null)
            {                
                ViewBag.AREA = new SelectList(new AreaManager().ListarAreas(), "IdArea", "descArea");
                return View();
            }
            else
                return RedirectToAction("Login", "Login");
        }

        public ActionResult ReporteHistorial()
        {
            if (Session["USUARIO"] != null)
            {
                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFin = DateTime.Now;
                ViewBag.ESTADO = new SelectList(new EstadoManager().ListarEstado(), "IdEstado", "DesEstado");
                ViewBag.AREA = new SelectList(new AreaManager().ListarAreas(), "IdArea", "descArea");
                ViewBag.RESPONSABLE = new SelectList(new EncargadoManager().ListarEncargado(), "IdEnc", "Nombre");

                var listaHistorial = new ReporteManager().ReporteHistorialTickets(fechaInicio,fechaFin,0,0,0,false);

                return View(listaHistorial);
            }
            else
                return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public PartialViewResult filtrarHistorialTicket(string startDate,string endDate,int idState=0,int idArea=0,int idResp=0,bool includeFecReg=false)
        {
            DateTime fechaInicio = DateTime.Parse(startDate);
            DateTime fechaFin = DateTime.Parse(endDate);
            var listaHistorial = new ReporteManager().ReporteHistorialTickets(fechaInicio, fechaFin, idState, idArea,idResp, includeFecReg);

            return PartialView("_partialHistorialTicket", listaHistorial);
        }

        [HttpPost]
        public JsonResult ReporteProgresoBarras(string dateStart = "", string dateEnd = "",int idResp=0)
        {
            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;
            if (!dateStart.Equals("") && !dateEnd.Equals(""))
            {
                fechaInicio = DateTime.Parse(dateStart);
                fechaFin = DateTime.Parse(dateEnd);
            }
            List<TicketEN> resporteBarras = new ReporteManager().ReporteProgresoBarras(fechaInicio, fechaFin,idResp);
            return Json(resporteBarras);
        }

        [HttpPost]
        public JsonResult ReporteAvanceBarras(string dateStart = "", string dateEnd = "", int idResp = 0,int idarea=0)
        {
            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;
            if (!dateStart.Equals("") && !dateEnd.Equals(""))
            {
                fechaInicio = DateTime.Parse(dateStart);
                fechaFin = DateTime.Parse(dateEnd);
            }
            List<TicketEN> resporteBarras = new ReporteManager().ReporteAvanceBarras(fechaInicio, fechaFin, idResp,idarea);
            return Json(resporteBarras);
        }

        [HttpPost]
        public JsonResult ReporteEstadoDonuts(string dateStart = "", string dateEnd = "", int idResp = 0,int idarea=0)
        {
            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;
            if (!dateStart.Equals("") && !dateEnd.Equals(""))
            {
                fechaInicio = DateTime.Parse(dateStart);
                fechaFin = DateTime.Parse(dateEnd);
            }
            List<EstadoEN> reporteDonuts = new ReporteManager().ReporteEstadoDonuts(fechaInicio, fechaFin,idResp,idarea);
            return Json(reporteDonuts);
        }

        [HttpPost]
        public JsonResult getEncargadoArea(int idarea)
        {           
            return Json(new EncargadoManager().ListarEncargadoporArea(idarea));
        }
    }
}