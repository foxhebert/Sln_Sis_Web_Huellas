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
    public class RegistroHuellasController : Controller
    {
        //private srvHuellas.SistemaCNClient proxy_s;//modificado 22.06.2021
        //private srvHuellas.UsuarioCNClient proxy;//modificado 22.06.2021

        public ActionResult ListarPersonal() //añadir input 16.06.2021
        {
            try
            {
                #region VALIDAR SESION VIGENTE Y PERMISOS DE MENÚ
                if (Session["USUARIO"] == null)
                    return RedirectToAction("Login", "Login");

                ZKLoginOff usuario = (ZKLoginOff)Session["USUARIO"];
                //srvHuellas.ZKLoginOff usuario = (srvHuellas.ZKLoginOff)Session["USUARIO"];//modificado 22.06.2021
                //srvHuellas.ZKLogin usuario = (srvHuellas.ZKLogin)Session["USUARIO"];
                string controlador = this.GetType().Name.ToLower().Replace("controller", "");//modificado 22.06.2021
                string accion = (new StackTrace().GetFrame(0)).GetMethod().Name.ToLower();


                if (!usuario.OpcionesMenuOff.ToList().Exists(x => x.StrControlador.ToLower() == controlador && x.StrAccion.ToLower() == accion))
                //if (!usuario.OpcionesMenu.ToList().Exists(x => x.StrControlador.ToLower() == controlador && x.StrAccion.ToLower() == accion))
                {
                    return View("errorPermiso");
                }
                #endregion

                LoginController._menu="02"; //añadido 18.06.2021
                List<ItemCombo> listaT = new List<ItemCombo>();
                // proxy_s = new srvHuellas.SistemaCNClient();
                using (ZKSedeBL objBL = new ZKSedeBL())
                {
                    listaT =objBL.ListarSedeCombo();
                }
               // List<srvHuellas.ItemCombo> listaT = proxy_s.ListarSedeCombo().ToList();//modificado 22.06.2021
                return View(listaT);
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
                Log.AlmacenarLogError(ex, "RegistroHuellasController.cs | ListarPersonal");
                return View();
            }
        }

        [HttpPost]
        public JsonResult ListarPersonalJson(int x_idSede, int x_estado)
        {
            //List<srvHuellas.ZKUsuarios> listaT = new List<srvHuellas.ZKUsuarios>();//modificado 22.06.2021
            List<ZKUsuarios> listaT = new List<ZKUsuarios>();
            try
            {
                //if (Session["USUARIO"] == null)
                //    return RedirectToAction("Login", "Login");
                int x_total = 0;
               // proxy = new srvHuellas.UsuarioCNClient();//modificado 22.06.2021
                using (ZKUsuariosBL objBL = new ZKUsuariosBL())
                {
                    listaT = objBL.ListarUsuariosTodo(x_idSede, x_estado, 1, 10000, ref x_total).ToList();
                }

                
                //listaT = proxy.ListarUsuariosTodo(x_idSede, x_estado, 1, 10000, ref x_total).ToList();//modificado 22.06.2021
            }
            catch (Exception ex)
            {
                Log.AlmacenarLogError(ex, "RegistroHuellasController.cs | ListarPersonalJson");
            }

            return Json(listaT);
            //return View(listaT);

        }

        [HttpPost]
        public JsonResult ObtenerHuellasUsuario(int x_idUsuario)
        {
            string huellas = "";
            try
            {
                //proxy = new srvHuellas.UsuarioCNClient();//modificado 22.06.2021
                //huellas = proxy.ListarHuellasAlg10UsuarioCadena(x_idUsuario);//modificado 22.06.2021
                using (ZKHuellasBL objBL = new ZKHuellasBL())
                {
                    huellas = objBL.ListarHuellasAlg10UsuarioCadena(x_idUsuario);
                }
            }
            catch (Exception ex)
            {
                Log.AlmacenarLogError(ex, "RegistroHuellasController.cs | ObtenerHuellasUsuario");
            }
            return Json(huellas);
        }

        //POST: impresion pedido
        [HttpPost]
        public JsonResult RegistraUsuario(int x_idUsuario, int x_codigo, string x_nombres, string x_tarjeta, string x_codPersonal, int x_sede, int x_estado, string x_dedos, string x_huellas, string x_dedosAnt)
        {
            Respuesta objRespuesta = new Respuesta();

            try
            {
                if (Session["USUARIO"] == null)
                {
                    objRespuesta = new Respuesta() { exito = false, type = "error", message = "Su sesión expiró, inicie sesión nuevamente.", entidad = 10 };
                    return Json(objRespuesta);
                }

                ZKUsuarios usuario = new ZKUsuarios();
                //srvHuellas.ZKUsuarios usuario = new srvHuellas.ZKUsuarios();//modificado 22.06.2021
                long ivalor = 0;
                long.TryParse(x_tarjeta.Trim(), out ivalor);

                usuario.iIdUsuario = x_idUsuario;
                usuario.iCodUsuario = x_codigo;
                usuario.sNombre = x_nombres;
                usuario.CardNumber = ivalor;
                usuario.Cod_Personal = x_codPersonal;

                usuario.Estado = 1;
                usuario.iCambioNumero = 0;
                usuario.iIdArea = 0;
                usuario.iIdGrupo = 0;
                usuario.iModoverificacion = 0;
                usuario.iPrivilegio = 0;
                usuario.sPassword = "";
                usuario.Estado = x_estado;
                usuario.iIdSede = x_sede;

                if (!string.IsNullOrEmpty(x_dedos))
                {
                    string[] dedos = x_dedos.Split(',');
                    if (dedos.Length > 0)
                    {
                        usuario.Huellas = new ZKHuellas[dedos.Length].ToList(); 
                        //usuario.Huellas = new srvHuellas.ZKHuellas[dedos.Length];//modificado 22.06.2021
                        string[] huellas = x_huellas.Split(',');
                        for (int i = 0; i < dedos.Length; i++)
                        {
                            //srvHuellas.ZKHuellas huella = new srvHuellas.ZKHuellas();//modificado 22.06.2021
                            ZKHuellas huella = new ZKHuellas();
                            huella.Huella = "";
                            huella.Huella10 = huellas[i];
                            huella.ifingernumberzk5000 = 0;
                            huella.iFingerNumber = Convert.ToInt32(dedos[i]);
                            huella.iIdUsuario = x_idUsuario;
                            huella.iIdHuella = 0;
                            huella.nLngHuella = 0;
                            huella.nLngHuella10 = huella.Huella10.Length;

                            usuario.Huellas[i] = huella;
                            Log.AlmacenarLogMensaje(huellas[i], "Dedo :"+ dedos[i]);//añadido para revisar dato 22.06.2021
                        }
                    }
                }
                Log.AlmacenarLogMensaje(x_dedosAnt, "Dedo Anteriores Tras Editar");//añadido para revisar dato 23.06.2021
                                                                         // proxy = new srvHuellas.UsuarioCNClient();//modificado 22.06.2021
                string mensaje = "";
                bool resultado = false;
               // bool resultado = proxy.InsertarUsuario(x_dedosAnt, ref usuario, ref mensaje);//modificado 22.06.2021
                using (ZKUsuariosBL objBL = new ZKUsuariosBL())
                {
                    resultado = objBL.InsertarUsuario(x_dedosAnt, ref usuario, ref mensaje);
                }
                if (resultado)
                {
                    mensaje = (x_idUsuario != usuario.iIdUsuario ? "Se ha insertado correctamente." : "Se ha actualizado correctamente.");
                }

                objRespuesta = new Respuesta() { exito = resultado, message = mensaje, type = resultado ? "success" : "error", entidad = usuario };
            }
            catch (Exception ex)
            {
                Log.AlmacenarLogError(ex, "RegistroHuellasController.cs | RegistraUsuario");
            }
            return Json(objRespuesta);
        }

        [HttpPost]
        public JsonResult EliminarRegistro(int x_idUsuario)
        {
            Respuesta objRespuesta = new Respuesta();
            try
            {
                if (Session["USUARIO"] == null)
                {
                    objRespuesta = new Respuesta() { exito = false, type = "error", message = "Su sesión expiró, inicie sesión nuevamente." };
                    return Json(objRespuesta);
                }

                //srvHuellas.ZKLogin usuario = (srvHuellas.ZKLogin)Session["USUARIO"];
                //srvHuellas.ZKLoginOff usuario = (srvHuellas.ZKLoginOff)Session["USUARIO"];//modificado 22.06.2021
                ZKLoginOff usuario = (ZKLoginOff)Session["USUARIO"];
                //proxy = new srvHuellas.UsuarioCNClient();//modificado 22.06.2021
                string mensaje = "";
                bool resultado = false;
                //bool resultado = proxy.EliminarUsuario(x_idUsuario, usuario.iIdSesion, ref mensaje);//modificado 22.06.2021
                using (ZKUsuariosBL objBL = new ZKUsuariosBL())
                {
                    resultado = objBL.EliminarUsuario(x_idUsuario, usuario.iIdSesion, ref mensaje);
                }
                if (resultado)
                {
                    mensaje = "Se ha eliminado correctamente.";
                }

                objRespuesta = new Respuesta() { exito = resultado, message = mensaje, type = resultado ? "success" : "error" };
            }
            catch (Exception ex)
            {
                Log.AlmacenarLogError(ex, "RegistroHuellasController.cs | EliminarRegistro");
            }
            return Json(objRespuesta);
        }

        [HttpPost]
        public JsonResult ObtenerVersion()
        {
            string version = "";
            try
            {
                //proxy_s = new srvHuellas.SistemaCNClient(); //modificado 22.06.2021
                //version = proxy_s.Version(); //modificado 22.06.2021
                version = "";
            }
            catch (Exception ex)
            {
                Log.AlmacenarLogError(ex, "RegistroHuellasController.cs | ObtenerVersion");
            }
            return Json("Servicio: " + version);
        }

        [HttpPost]
        public JsonResult CambiarEstadoRegistro(int x_idUsuario, int x_estado)
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
                //srvHuellas.ZKLoginOff login = (srvHuellas.ZKLoginOff)Session["USUARIO"];//modificado 22.06.2021
                //srvHuellas.ZKLogin login = (srvHuellas.ZKLogin)Session["USUARIO"];


                //proxy = new srvHuellas.UsuarioCNClient();//modificado 22.06.2021
                ZKUsuarios usuario = new ZKUsuarios();
                //srvHuellas.ZKUsuarios usuario = new srvHuellas.ZKUsuarios();//modificado 22.06.2021
                string mensaje = "";
                bool resultado = false;
                //bool resultado = proxy.CambiarEstadoUsuario(x_idUsuario, x_estado, login.iIdSesion, ref usuario, ref mensaje);//modificado 22.06.2021
                using (ZKUsuariosBL objBL = new ZKUsuariosBL())
                {
                    resultado = objBL.CambiarEstadoUsuario(x_idUsuario, x_estado, login.iIdSesion, ref usuario, ref mensaje);
                }
                if (resultado)
                {
                    mensaje = x_estado == 1 ? "Se ha Activado correctamente." : "Se ha Inactivado correctamente.";
                }

                objRespuesta = new Respuesta() { exito = resultado, message = mensaje, type = resultado ? "success" : "error", entidad = usuario };
            }
            catch (Exception ex)
            {
                Log.AlmacenarLogError(ex, "RegistroHuellasController.cs | CambiarEstadoRegistro");
            }
            return Json(objRespuesta);
        }

        public JsonResult Upload()
        {
            Respuesta objRespuesta = new Respuesta();
            //try
            //{
            //    string dir = "";
            //    //string s = "";
            //    srvHuellas.ListItemPersonaImport lst = new srvHuellas.ListItemPersonaImport();//modificado 22.06.2021
            //    srvHuellas.ListItemErrorCarga lstError = new srvHuellas.ListItemErrorCarga();//modificado 22.06.2021
            //    proxy = new srvHuellas.UsuarioCNClient();//modificado 22.06.2021
            //    int insertados = 0;
            //    int erroneos = 0;
            //    for (int i = 0; i < Request.Files.Count; i++)
            //    {
            //        HttpPostedFileBase file = Request.Files[i]; //Uploaded file 

            //        //Use the following properties to get file's name, size and MIMEType 
            //        int fileSize = file.ContentLength;
            //        string fileName = file.FileName;
            //        string mimeType = file.ContentType;
            //        System.IO.Stream fileContent = file.InputStream; //To save file, use SaveAs method 

            //        dir = Server.MapPath("~/") + "\\files\\" + fileName;
            //        file.SaveAs(dir); //File will be saved in application root 


            //        System.IO.FileStream stream = System.IO.File.Open(dir, FileMode.Open, FileAccess.Read);
            //        IExcelDataReader excelReader;

            //        Console.WriteLine(dir);

            //        if (fileName.EndsWith(".xls"))
            //            excelReader = (IExcelDataReader)Excel.ExcelReaderFactory.CreateBinaryReader(stream);
            //        else if (fileName.EndsWith(".xlsx"))
            //            excelReader = (IExcelDataReader)Excel.ExcelReaderFactory.CreateOpenXmlReader(stream);
            //        else
            //        {
            //            objRespuesta.exito = false;
            //            objRespuesta.message = "No es un archivo excel";
            //            objRespuesta.type = "error";
            //            return Json("No es un archivo excel");
            //        }

            //        excelReader.IsFirstRowAsColumnNames = true;

            //        System.Data.DataSet ds = new System.Data.DataSet();
            //        ds = excelReader.AsDataSet(true);
            //        System.Data.DataTable dt = ds.Tables[0];
            //        stream.Close();

            //        //Validar Cabeceras:
            //        string cabeceras = "[código][cod. personal][personal][nro. tarjeta][sede]";

            //        for (int j = 0; j < dt.Columns.Count; j++)
            //        {
            //            dt.Columns[j].ColumnName = dt.Columns[j].ColumnName.ToLower().Trim();
            //            cabeceras = cabeceras.Replace("[" + dt.Columns[j].ColumnName + "]", "");
            //        }
            //        if (string.IsNullOrEmpty(cabeceras))
            //        {
            //            objRespuesta.exito = false;
            //            objRespuesta.message = "El archvio no tiene el formato de Carga.<br><b>Descargar el formato y llenar los datos a cargar</b>";
            //            return Json(objRespuesta);
            //        }

            //        //recuperar todos los campos

            //        int numero = 0;
            //        long tarjeta = 0;
            //        int lin = 1;//la primera es de las cabeceras
            //        string nombre = "";
            //        foreach (System.Data.DataRow dr in dt.Rows)
            //        {
            //            lin++;
            //            var valor = dr["código"];
            //            if (valor == null || valor.ToString() == "" || !int.TryParse(valor.ToString(), out numero))
            //            {
            //                lstError.Add(new srvHuellas.ItemErrorCarga() { intLinea = lin, strError = "Se ha ingresado un valor no numérico en la columna Código" });//modificado 22.06.2021
            //                continue;
            //            }
            //            valor = dr["personal"];
            //            if (valor == null || valor.ToString().Trim() == "")
            //            {
            //                lstError.Add(new srvHuellas.ItemErrorCarga() { intLinea = lin, strError = "Debe ingresar el nombre del personal" });//modificado 22.06.2021
            //                continue;
            //            }
            //            nombre = valor.ToString().Trim();

            //            valor = dr["nro. tarjeta"];
            //            if (valor != null)
            //                long.TryParse(valor.ToString(), out tarjeta);
            //            else
            //                tarjeta = 0;

            //            lst.Add(new srvHuellas.ItemPersonaImport()
            //            {
            //                intCodigo = numero,
            //                intLinea = lin,
            //                intNumTarjeta = tarjeta,
            //                strCodigoPersonal = dr["cod. personal"] == null ? "" : dr["cod. personal"].ToString().Trim(),
            //                strNombres = nombre,
            //                strSede = dr["sede"] == null ? "" : dr["sede"].ToString().Trim()
            //            });
            //        }
            //    }

            //    //Insertar las marcas en intgervalos de 20 items
            //    int limMin = 0, tamanio = 20, total = lst.Count;
            //    string mensaje = "";
            //    while (limMin < total)
            //    {
            //        srvHuellas.ListItemPersonaImport sub = new srvHuellas.ListItemPersonaImport();//modificado 22.06.2021
            //        sub.AddRange(lst.ToList().GetRange(limMin, tamanio));
            //        limMin += tamanio;
            //        int ins = 0, err = 0;
            //        srvHuellas.ListItemErrorCarga lstErro = new srvHuellas.ListItemErrorCarga();//modificado 22.06.2021
            //        lstErro = proxy.InsertMasivo(sub, ref ins, ref err, ref mensaje);//modificado 22.06.2021
            //        insertados += ins;
            //        erroneos += err;
            //        lstError.AddRange(lstErro);
            //    }

            //    objRespuesta.exito = true;
            //    objRespuesta.message = "La carga se ejecutó con los siguientes resultados:<br><ul><li>Insertados correctamente: " + insertados.ToString() + "</li><li>Registros con errores: " + erroneos.ToString() + "</li></ul>";
            //    objRespuesta.entidad = lstError;
            //}
            //catch (Exception ex)
            //{
            //    Log.AlmacenarLogError(ex, "RegistroHuellasController.cs | Upload");
            //}
            return Json(objRespuesta);
        }

        //Fuente: https://www.iteramos.com/pregunta/76364/ajax-de-jquery-cargar-el-archivo-en-aspnet-mvc


        ////prueba manual 09/06/2021 añadido por ES -- INICIO
        //[HttpPost]
        //public JsonResult EjecutarBat()
        //{
        //    bool todoOk = false;
        //    try
        //    {
        //        var strModoBat = "";
        //        //string RutaImprimirBat = "C:\\Temp";//ConfigurationManager.AppSettings["rutaLog"];
        //        //string filePathBat = "Consola_";
        //        string filePathBat = "C:\\Users\\Tecflex\\Downloads" + @"\Consola_.bat"; //25.09.2020
                
        //        if (strModoBat == "P")
        //        {
        //            //--Con Proceso
        //            Process p = new Process();
        //            p.StartInfo.RedirectStandardOutput = true;
        //            p.StartInfo.FileName = filePathBat;
        //            p.StartInfo.UseShellExecute = false;
        //            p.StartInfo.Verb = "runas";
        //            p.StartInfo.CreateNoWindow = true;
        //            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        //            p.Start();

        //            string stdoutx = "";
        //            stdoutx = p.StandardOutput.ReadToEnd();
        //            Console.WriteLine("ErrorProceso", stdoutx);
        //            p.WaitForExit();

        //            //mostrarError:
        //            //StreamWriter serror;
        //            //string filePathError;
        //            //filePathError = RutaImprimirBat + @"\MsjExecBAT.txt"; //25.09.2020
        //            //serror = File.CreateText(filePathError);
        //            //serror.WriteLine(stdoutx);
        //            //serror.Close();
        //            //serror.Dispose();
        //            //serror = null;
        //            //---fin

        //            if (stdoutx.Trim() == "1 archivo(s) copiado(s).")
        //            { todoOk = true; }
        //            else
        //            { todoOk = false; }
        //        }
        //        else
        //        {
        //            //--original Funciona en Mi Ambiente
        //            ProcessStartInfo info = new ProcessStartInfo(filePathBat);
        //            info.UseShellExecute = false;
        //            info.CreateNoWindow = true;
        //            info.WindowStyle = ProcessWindowStyle.Hidden;
        //            info.Verb = "runas";
        //            try
        //            {

        //                Process.Start(info);
        //                todoOk = true;
        //            }
        //            catch (Exception err)
        //            {
        //                Console.WriteLine(err.Message);
        //                todoOk = false;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.AlmacenarLogError(ex, "RegistroHuellasController.cs | EjecutarBat");
        //    }
        //    return Json("Bat: ejecutado : " + todoOk.ToString());
        //}
        ////prueba manual 09/06/2021 añadido por ES -- FIN
    }
}