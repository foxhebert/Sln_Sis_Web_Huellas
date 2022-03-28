using Dominio.Entidades;
using Dominio.Entidades.Tipo;
using System.Collections.Generic;
using System.Data;

namespace Infraestructura.Data.SqlServer
{
    public class ZKMenuDAO : ConexionDAO
    {
        public ListZKMenuOff ListarMenuLogin(int x_intIdLogin) //modificado ListZKMenu
        {
            string procedimiento = "TSP_ZKMenu_Q01";

            DataTable dtResult;
            ListZKMenuOff list = new ListZKMenuOff();//modificado ListZKMenu
            List<ParamSP> parametros = new List<ParamSP>();

            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@IDLOGIN", strValParam = x_intIdLogin });

            bool result = ExecuteDataTable(procedimiento, ref parametros, out dtResult);

            if (result)
            {
                foreach (DataRow dr in dtResult.Rows)
                {
                    ZKMenuOff entidad = UtilitarioEN.MapearObjeto<ZKMenuOff>(dr);//modificado ZKMenu
                    list.Add(entidad);
                }
            }
            return list;
        }
        //añadido 16.06.2021
        public ListZKMenuOff ListarMenuLogin_(int x_intIdLogin)
        {
            string procedimiento = "TSP_ZKMenu_Q00";

            DataTable dtResult;
            ListZKMenuOff list = new ListZKMenuOff();
            List<ParamSP> parametros = new List<ParamSP>();

            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@IDLOGIN", strValParam = x_intIdLogin });

            bool result = ExecuteDataTable(procedimiento, ref parametros, out dtResult);

            if (result)
            {
                foreach (DataRow dr in dtResult.Rows)
                {
                    ZKMenuOff entidad = UtilitarioEN.MapearObjeto<ZKMenuOff>(dr);
                    list.Add(entidad);
                }
            }
            return list;
        }
    }
}