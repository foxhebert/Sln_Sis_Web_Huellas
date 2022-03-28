using System.Configuration;

namespace Infraestructura.Data.SqlServer.Util
{
    public class Conexion
    {
        public string cnx = ConfigurationManager.ConnectionStrings["cn1"].ConnectionString;
    }
}
