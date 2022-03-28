using Dominio.Entidades;
using Dominio.Entidades.Tipo;
using System;
using System.Collections.Generic;
using System.Data;

namespace Infraestructura.Data.SqlServer
{
    public class ZKSedeDAO : ConexionDAO
    {
        public ListZKSede ListarSedes(int x_intEstadoActivo)
        {
            string procedimiento = "TSP_ZKSede_Q01";

            DataTable dtResult;
            ListZKSede list = new ListZKSede();
            List<ParamSP> parametros = new List<ParamSP>();

            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@intActivo", strValParam = x_intEstadoActivo });

            bool result = ExecuteDataTable(procedimiento, ref parametros, out dtResult);

            if (result)
            {
                foreach (DataRow dr in dtResult.Rows)
                {
                    ZKSede usuario = UtilitarioEN.MapearObjeto<ZKSede>(dr);
                    list.Add(usuario);
                }
            }
            return list;
        }

        public bool MantenimientoSede(ref ZKSede x_sede)
        {
            string procedimiento = "TSP_ZKSede_I01";

            //ListZKLogin lista = new ListZKLogin(); //modificado 22.06.2021
            List<ParamSP> parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@intIdSede", strValParam = x_sede.intIdSede });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@strCoLocal", strValParam = x_sede.strCoLocal });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@strDeLocal", strValParam = x_sede.strDeLocal });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@strDireccion", strValParam = x_sede.strDireccion });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@bitActivo", strValParam = x_sede.bitActivo });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Salida, strNomParam = "@insertado", strValParam = 0 });


            bool result = ExecuteNonQuery(procedimiento, ref parametros);
            int idInsertado = 0;
            if (!result || !int.TryParse(parametros[5].strValParam.ToString(), out idInsertado) || idInsertado == 0)
            {
                Error = "Error en inserción de usuario";
                return false;
            }

            x_sede.intIdSede = idInsertado;

            return result;
        }
    }
}