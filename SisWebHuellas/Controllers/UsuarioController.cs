using Dominio.Entidades;
using Dominio.Repositorio;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace SisWebTickets.Controllers
{
    public class UsuarioController : Controller
    {
        public ActionResult Perfil()
        {
            new UsuarioEN();
            if (Session["USUARIO"] != null)
            {
                UsuarioEN objUsuario = (UsuarioEN) Session["USUARIO"];
                ViewBag.ROL = new SelectList(new RolManager().ListarRol(), "IdRol", "DesRol", objUsuario.rol.IdRol);
                ViewBag.EMPRESA = new SelectList(new EmpresaManager().ListarEmpresa(), "IdEmp", "RazonSocial", objUsuario.empresa.IdEmp);
                return View(objUsuario);
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public JsonResult Perfil(UsuarioEN objU)
        {
            Response response = new Response();
            try
            {
                if (new UsuarioManager().ActualizarUsuario(objU) >= 1)
                {
                    response.success = "Datos Actualizados Correctamente";
                }
                else
                {
                    response.error = "No se puedo actualizar los datos, Intente más tarde";
                }
            }
            catch (Exception)
            {
                response.error = "No se puedo actualizar los datos, Intente más tarde";
            }
            return Json(response);
        }

        [HttpPost]
        public ActionResult EditarPerfil(UsuarioEN objU)
        {
            if (Session["USUARIO"] != null)
            {
                try
                {
                    byte[] imageData = null;
                    if (Request.Files.Count > 0)
                    {
                        HttpPostedFileBase poImgFile = Request.Files[0];
                        if (poImgFile.ContentLength > 0 && poImgFile.ContentType.Contains("image/") && poImgFile.FileName != "")
                        {
                            using (BinaryReader binary = new BinaryReader(poImgFile.InputStream))
                            {
                                imageData = binary.ReadBytes(poImgFile.ContentLength);
                            }
                            if (imageData.Length != 0)
                            {
                                objU.imgUsu = imageData;
                            }
                        }
                    }
                    new UsuarioManager().ActualizarUsuario(objU);
                    return RedirectToAction("cerrarSession", "Login");
                }
                catch
                {
                    return View();
                }
            }
            return RedirectToAction("Login", "Login");
        }

        public FileResult FotoPerfil()
        {           
            UsuarioEN objUsuario = (UsuarioEN) Session["USUARIO"];
            if (objUsuario.imgUsu != null)
            {
                return File(objUsuario.imgUsu, "image/png");
            }
            string noImage = Server.MapPath("~/images/user.png");
            return File(noImage, "image/png");
        }
    }
}