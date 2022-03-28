﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBX_Web_SISCOP.Controllers
{
    public class Log
    {
        public static void AlmacenarLogError(Exception excepcion, string strNombreArchivo = "")
        {
            StreamWriter objWriter = null;
            FileStream objFile = null;
            DirectoryInfo objDirectorio = null;
            try
            {
                string rutaLog = "c:\\temp";
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
                objFile = new FileStream(objDirectorio.FullName + "\\LogWebError" + DateTime.Now.ToString("yyyy-MM-dd") + ".log", FileMode.OpenOrCreate, FileAccess.Write);

                objWriter = new StreamWriter(objFile);
                if (excepcion != null)
                {
                    objWriter.BaseStream.Seek(0, SeekOrigin.End);
                    objWriter.WriteLine("[________________________________________________________________________]");
                    objWriter.WriteLine("[Fecha         ][" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]");
                    objWriter.WriteLine("[File          ][" + strNombreArchivo + "]");
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
                else
                {

                    objWriter.BaseStream.Seek(0, SeekOrigin.End);
                    objWriter.WriteLine("[________________________________________________________________________]");
                    objWriter.WriteLine("[Fecha         ][" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]");
                    objWriter.WriteLine("[File          ][" + strNombreArchivo + "]");
                    objWriter.WriteLine();
                    objWriter.Flush();
                    objWriter.Close();
                    objFile.Close();
                }
            }
            catch (Exception ex)
            {
                if (objWriter != null)
                {
                    objWriter.Close();
                    objFile.Close();
                }
                throw ex;
            }
        }

        public static void AlmacenarLogMensaje(string strMensaje, string titulo)
        {
            try
            {
                StreamWriter objWriter = null;
                FileStream objFile = null;
                DirectoryInfo objDirectorio = null;

                string rutaLog = "c:\\temp";
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

                objFile = new FileStream(objDirectorio.FullName + "\\Log" + DateTime.Now.ToString("yyyy-MM-dd") + ".log", FileMode.OpenOrCreate, FileAccess.Write);

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
            catch (Exception ex)
            {
                AlmacenarLogError(ex);
            }
        }
    }
}