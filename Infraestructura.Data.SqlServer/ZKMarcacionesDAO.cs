using System;
using System.Collections.Generic;
using System.Data;
using Dominio.Entidades;
using Dominio.Entidades.Personalizado;
using Dominio.Entidades.Tipo;

namespace Infraestructura.Data.SqlServer
{
    public class ZKMarcacionesDAO : ConexionDAO
    {
        public ListItemAsistencia ListarMarcaciones(DateTime x_feIni, DateTime x_feFin, string x_criterio, string x_filtro, int x_estado, int x_intIdSede)
        {
            string procedimiento = "TSP_ZKMarcaciones_Q02";

            DataTable dtResult;
            ListItemAsistencia list = new ListItemAsistencia();
            List<ParamSP> parametros = new List<ParamSP>();

            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@feIni", strValParam = x_feIni.ToString("yyyyMMdd") });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@feFin", strValParam = x_feFin.ToString("yyyyMMdd") });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@criterio", strValParam = x_criterio });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@filtro", strValParam = x_filtro });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@soloActivos", strValParam = x_estado });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@intIdSede", strValParam = x_intIdSede });

            bool result = ExecuteDataTable(procedimiento, ref parametros, out dtResult);

            if (result)
            {
                foreach (DataRow dr in dtResult.Rows)
                {
                    ItemAsistencia usuario = UtilitarioEN.MapearObjeto<ItemAsistencia>(dr);
                    list.Add(usuario);
                }
            }
            return list;
        }

        public bool RegistrarMarca(int x_iIdUsuario, string x_sSerie, int x_iFingerNumber, int x_iIdSede, ref string x_mensaje)
        {
            string procedimiento = "TSP_ZKMarcaciones_I01";

            DataTable dtResult;
            ListItemAsistencia list = new ListItemAsistencia();
            List<ParamSP> parametros = new List<ParamSP>();

            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@iIdUsuario", strValParam = x_iIdUsuario });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@sSerie", strValParam = x_sSerie });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@iIdLocal", strValParam = x_iIdSede });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Salida, strNomParam = "@intResult", strValParam = 0 });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Salida, strNomParam = "@strMensaje", strValParam = "", intLongitud = 250 });

            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@iIdFingerPrint", strValParam = x_iFingerNumber });

            bool result = ExecuteDataTable(procedimiento, ref parametros, out dtResult);

            x_mensaje = parametros[4].strValParam.ToString();

            return Convert.ToInt32(parametros[3].strValParam) == 1;
        }
        //añadida 16.06.2021
        public bool RegistrarMarca_(int x_iIdUsuario, string x_sSerie, int x_iFingerNumber, int x_iIdSede, DateTime feHor, ref string x_mensaje)
        {
            string procedimiento = "TSP_ZKMarcaciones_I00";

            DataTable dtResult;
            ListItemAsistencia list = new ListItemAsistencia();
            List<ParamSP> parametros = new List<ParamSP>();

            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@iIdUsuario", strValParam = x_iIdUsuario });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@sSerie", strValParam = x_sSerie });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@iIdLocal", strValParam = x_iIdSede });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@dttFecHora", strValParam = feHor.ToString("yyyyMMdd HH:mm:ss") });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Salida, strNomParam = "@intResult", strValParam = 0 });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Salida, strNomParam = "@strMensaje", strValParam = "", intLongitud = 250 });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@iIdFingerPrint", strValParam = x_iFingerNumber });

            bool result = ExecuteDataTable(procedimiento, ref parametros, out dtResult);

            x_mensaje = parametros[4].strValParam.ToString();

            return Convert.ToInt32(parametros[4].strValParam) == 1;
        }
    }
}