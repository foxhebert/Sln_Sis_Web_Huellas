using System;
using System.Collections.Generic;
using System.Data;
using Dominio.Entidades;
using Dominio.Entidades.Personalizado;
using Dominio.Entidades.Tipo;

namespace Infraestructura.Data.SqlServer
{
    public class ZKSedesDAO : ConexionDAO
    {
        public ListZKSede ListarSedes(string x_filtro, int x_estado, int x_intIdSede)
        {
            string procedimiento = "TSP_ZK_SEDES_Q00";//TSP_ZKTerminal_Q00
            DataTable dtResult;

            ListZKSede lista = new ListZKSede();//modificado ListZKLogin
            List<ParamSP> parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
             parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@strfiltro", strValParam = x_filtro });
             parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@intEstado", strValParam = x_estado });
             parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@intIdSede", strValParam = x_intIdSede });

            bool result = ExecuteDataTable(procedimiento, ref parametros, out dtResult);
            if (result)
            {
                foreach (DataRow dr in dtResult.Rows)
                {
                    lista.Add(UtilitarioEN.MapearObjeto<ZKSede>(dr));
                }
            }
            return lista;
        }

        public bool InsertarSedes(ref ZKSede objSedes, ref string x_mensaje)
        {
            int Operacion = 0;
            //if (x_terminal.iIdTerminal > 0)
                if (objSedes.intIdSede > 0)
                {

                Operacion = 1;//editar
            }

            string procedimiento = "TSP_ZK_SEDES_UI01";
            List<ParamSP> parametros = new List<Dominio.Entidades.Tipo.ParamSP>();

            //parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@strfiltro", strValParam = x_filtro });
            //parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@intEstado", strValParam = x_estado });
            //parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@intIdSede", strValParam = x_intIdSede });

            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@intIdSede", strValParam = objSedes.intIdSede });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@strCoLocal", strValParam = objSedes.strCoLocal });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@strDeLocal", strValParam = objSedes.strDeLocal });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@strDireccion", strValParam = objSedes.strDireccion });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@bitActivo", strValParam = objSedes.bitActivo });
            //--------------------------------------------------
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Salida, strNomParam = "@strMensaje", strValParam = "", intLongitud = 250 });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Salida, strNomParam = "@intResult", strValParam = 0 });
            bool result = ExecuteNonQuery(procedimiento, ref parametros);
            x_mensaje = parametros[5].strValParam.ToString();

            //if (!result)
            if (x_mensaje != "")
                {
                if (Operacion == 0)
                {
                    Error = "Error en insertar el lector: " + x_mensaje;
                }
                else
                {
                    Error = "Error en actualizar el lector: " + x_mensaje;
                }
                return false;
            }
            if(Operacion == 0)
                objSedes.intIdSede = Convert.ToInt32(parametros[6].strValParam);

            return result;
        }

        public bool EliminarSedes(int x_intIdSede)
        {
            string procedimiento = "TSP_ZK_SEDES_D01";

            List<ParamSP> parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@intIdSede", strValParam = x_intIdSede });

            bool result = ExecuteNonQuery(procedimiento, ref parametros);

            return result;
        }

        public bool CambiarEstadoSedes(int x_intIdSede, int x_estado)
        {
            string procedimiento = "TSP_ZK_SEDES_ESTADO_U00";

            List<ParamSP> parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@intIdSede", strValParam = x_intIdSede });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@Estado", strValParam = x_estado });

            bool result = ExecuteNonQuery(procedimiento, ref parametros);

            return result;
        }


        public ListZKSede ListarTiempoEntreMarca(string x_filtro)
        {
            string procedimiento = "TSP_ZK_TIEMPOMARCA_Q00"; //TCX_ListarSedes_Q00
            DataTable dtResult;

            ListZKSede lista = new ListZKSede();//modificado ListZKLogin
            List<ParamSP> parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@strfiltro", strValParam = x_filtro });

            bool result = ExecuteDataTable(procedimiento, ref parametros, out dtResult);
            if (result)
            {
                foreach (DataRow dr in dtResult.Rows)
                {
                    lista.Add(UtilitarioEN.MapearObjeto<ZKSede>(dr));
                }
            }
            return lista;
        }



    }
}
