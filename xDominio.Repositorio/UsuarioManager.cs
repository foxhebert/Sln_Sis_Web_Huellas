using Dominio.Entidades;
using Dominio.Repositorio.util;
using Infraestructura.Data.SqlServer;
using System;

namespace Dominio.Repositorio
{
    public class UsuarioManager
    {
        private UsuarioDAL objDAL;

        public UsuarioEN loginUsuario(string usuario, string clave)
        {
            try
            {
                objDAL = new UsuarioDAL();
                return objDAL.loginUsuario(usuario, clave);
            }
            catch (Exception ex)
            {
                Log.writeLog(ex);
                return null;
            }
           
        }

        public int ActualizarUsuario(UsuarioEN objU)
        {
            try
            {
                objDAL = new UsuarioDAL();
                return objDAL.ActualizarUsuario(objU);
            }
            catch (Exception ex)
            {
                Log.writeLog(ex);
                return -1;
            }
            
        }
    }
}
