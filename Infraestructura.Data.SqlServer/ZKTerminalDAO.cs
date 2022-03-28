using System;
using System.Collections.Generic;
using System.Data;
using Dominio.Entidades;
using Dominio.Entidades.Personalizado;
using Dominio.Entidades.Tipo;

namespace Infraestructura.Data.SqlServer
{
    public class ZKTerminalDAO : ConexionDAO
    {
        public ListZKTerminal ListarLectores(string x_filtro, int x_estado, int x_IdTerminal)
        {
            string procedimiento = "TSP_ZKTerminal_Q00";
            DataTable dtResult;

            ListZKTerminal lista = new ListZKTerminal();//modificado ListZKLogin
            List<ParamSP> parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
             parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@strfiltro", strValParam = x_filtro });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@iActivo", strValParam = x_estado });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@iIdTerminal", strValParam = x_estado });

            bool result = ExecuteDataTable(procedimiento, ref parametros, out dtResult);
            if (result)
            {
                foreach (DataRow dr in dtResult.Rows)
                {
                    lista.Add(UtilitarioEN.MapearObjeto<ZKTerminal>(dr));
                }
            }
            return lista;
        }

        public bool InsertarLector(ref ZKTerminal x_terminal, ref string x_mensaje)
        {
            int Operacion = 0;
            if (x_terminal.iIdTerminal > 0)
            {
                Operacion = 1;//editar
            }

            string procedimiento = "TSP_ZKTERMINAL_I00";
            List<ParamSP> parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@iIdTerminal", strValParam = x_terminal.iIdTerminal });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@iNumero", strValParam = x_terminal.iNumero });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@sSerie", strValParam = x_terminal.sSerie });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@sDescripcion", strValParam = x_terminal.sDescripcion });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Salida, strNomParam = "@strMensaje", strValParam = "", intLongitud = 250 });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Salida, strNomParam = "@intResult", strValParam = 0 });
            bool result = ExecuteNonQuery(procedimiento, ref parametros);
            x_mensaje = parametros[4].strValParam.ToString();

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
                x_terminal.iIdTerminal = Convert.ToInt32(parametros[5].strValParam);

            return result;
        }

        public bool EliminarLector(int x_iIdTerminal)
        {
            string procedimiento = "TSP_ZKTERMINAL_D00";

            List<ParamSP> parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@iIdTerminal", strValParam = x_iIdTerminal });

            bool result = ExecuteNonQuery(procedimiento, ref parametros);

            return result;
        }

        public bool CambiarEstadoLector(int x_iIdTerminal, int x_estado)
        {
            string procedimiento = "TSP_ZKTERMINAL_U00";

            List<ParamSP> parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@iIdTerminal", strValParam = x_iIdTerminal });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@Estado", strValParam = x_estado });

            bool result = ExecuteNonQuery(procedimiento, ref parametros);

            return result;
        }
    }
}
