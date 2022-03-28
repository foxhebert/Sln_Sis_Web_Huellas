using System;
using System.Configuration;
using System.IO;

namespace Dominio.Repositorio
{
    public static class UtilitarioBL
    {
        public static void AlmacenarLogError(Exception excepcion, string strNombreArchivo = "")
        {
            try
            {
                System.IO.StreamWriter objWriter = null;
                System.IO.FileStream objFile = null;
                System.IO.DirectoryInfo objDirectorio = null;

                //SE CREA EL DIRECTORIO LOG
                string rutaLog = "c:\\temp";
                //string rutaLog = "C:\temp\\RH\\WS";
                if (!ConfigurationManager.AppSettings["RutaLog"].Equals(null) || !ConfigurationManager.AppSettings["RutaLog"].Equals(""))
                    rutaLog = ConfigurationManager.AppSettings["RutaLog"];

                objDirectorio = new DirectoryInfo(rutaLog);
                if (!objDirectorio.Exists)
                {
                    objDirectorio.Create();
                }
                foreach (FileInfo objFileOld in objDirectorio.GetFiles("*.log"))
                {
                    if (objFileOld.LastAccessTime.AddDays(60) < DateTime.Now)
                    {
                        objFileOld.Delete();
                    }
                }

                objFile = new FileStream(objDirectorio.FullName + "\\LogServiceError" + DateTime.Now.ToString("yyyy-MM-dd") + ".log", FileMode.OpenOrCreate, FileAccess.Write);

                objWriter = new StreamWriter(objFile);
                objWriter.BaseStream.Seek(0, SeekOrigin.End);
                objWriter.WriteLine("[________________________________________________________________________]");
                objWriter.WriteLine("[Fecha         ][" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]");
                objWriter.WriteLine("[Source        ][" + excepcion.Source + "]");
                objWriter.WriteLine("[Mensaje       ][" + excepcion.Message + "]");
                if (excepcion.StackTrace != null)
                    objWriter.WriteLine("[StackTrace    ][" + excepcion.StackTrace + "]");
                if (excepcion.InnerException != null)
                    if (excepcion.InnerException.Message != null)
                        objWriter.WriteLine("[InnerException][" + excepcion.InnerException.Message + "]");

                objWriter.WriteLine();
                objWriter.Flush();
                objWriter.Close();
                objFile.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void AlmacenarLogMensaje(string strMensaje, string titulo)
        {
            try
            {
                System.IO.StreamWriter objWriter = null;
                System.IO.FileStream objFile = null;
                System.IO.DirectoryInfo objDirectorio = null;

                //SE CREA EL DIRECTORIO LOG
                string rutaLog = "c:\\temp";
                if (!ConfigurationManager.AppSettings["RutaLog"].Equals(null) || !ConfigurationManager.AppSettings["RutaLog"].Equals(""))
                    rutaLog = ConfigurationManager.AppSettings["RutaLog"]; //C:\Temp desde el webconfig

                objDirectorio = new DirectoryInfo(rutaLog);
                if (!objDirectorio.Exists)
                {
                    objDirectorio.Create();
                }
                foreach (FileInfo objFileOld in objDirectorio.GetFiles("*.log"))
                {
                    if (objFileOld.LastAccessTime.AddDays(60) < DateTime.Now)
                    {
                        objFileOld.Delete();
                    }
                }

                objFile = new FileStream(objDirectorio.FullName + "\\LogMsgService" + DateTime.Now.ToString("yyyy-MM-dd") + ".log", FileMode.OpenOrCreate, FileAccess.Write);

                objWriter = new StreamWriter(objFile);
                objWriter.BaseStream.Seek(0, SeekOrigin.End);
                objWriter.WriteLine("[________________________________________________________________________]");
                objWriter.WriteLine("[Fecha         ][" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]");
                objWriter.WriteLine("[Source        ][" + titulo + "]");
                objWriter.WriteLine("[Mensaje       ][" + strMensaje + "]");
                objWriter.WriteLine();
                objWriter.Flush();
                objWriter.Close();
                objFile.Close();

            }
            catch
            {
            }
        }
        public static string GetCC()
        {
            return new Infraestructura.Data.SqlServer.ConexionDAO().GetCC();
        }
    }
}