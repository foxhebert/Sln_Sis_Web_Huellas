using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Dominio.Repositorio.util
{
    public class Log
    {
        public static void writeLog(Exception exception)
        {
            try
            {
                string str = null;
                str = string.Empty;

                StreamWriter objWriter = null;
                FileStream objFile = null;
                DirectoryInfo objDirectorio = null;
                string rutaLog = ConfigurationManager.AppSettings["rutaLog"];
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
                objFile = new FileStream(objDirectorio.FullName + "\\LogSisTicket_" + DateTime.Now.ToString("yyyy-MM-dd") + ".log", FileMode.OpenOrCreate, FileAccess.Write);

                objWriter = new StreamWriter(objFile);
                objWriter.BaseStream.Seek(0, SeekOrigin.End);
                objWriter.WriteLine("---------------------------------------------------------------------------------------------------------------");
                objWriter.WriteLine("[Fecha         ][" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]");
                objWriter.WriteLine("[Método        ][" + exception.TargetSite + "]");
                objWriter.WriteLine("[Origen        ][" + exception.Source + "]");
                objWriter.WriteLine("[Mensaje       ][" + exception.Message + "]");
                objWriter.WriteLine("[Segui. Pila   ][" + exception.StackTrace + "]");
                objWriter.WriteLine("\r\n");
                objWriter.Flush();
                objWriter.Close();
                objFile.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
