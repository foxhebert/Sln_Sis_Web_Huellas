using System;
using System.Collections.Generic;
using System.Data;
using Dominio.Entidades;
using Dominio.Entidades.Tipo;

namespace Infraestructura.Data.SqlServer
{
    public class ZKHuellasDAO : ConexionDAO
    {
        public List<ZKHuellas> ListarHuellasAlg10Usuario(int x_iIdUsuario)
        {
            string procedimiento = "TSP_ZKHuellas_Q01";

            DataTable dtResult;
            List<ZKHuellas> list = new List<ZKHuellas>();
            List<Dominio.Entidades.Tipo.ParamSP> parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
            parametros.Add(new Dominio.Entidades.Tipo.ParamSP() { enuDirParam = Dominio.Entidades.Tipo.enParamIO.Entrada, strNomParam = "@iIdUsuario", strValParam = x_iIdUsuario.ToString() });

            bool result = ExecuteDataTable(procedimiento, ref parametros, out dtResult);
            if (result)
            {
                foreach (DataRow dr in dtResult.Rows)
                {
                    ZKHuellas usuario = UtilitarioEN.MapearObjeto<ZKHuellas>(dr);
                    list.Add(usuario);
                }
            }
            return list;
        }
        public List<ZKHuellas> ListarHuellasAlg10Todo(int x_pagina, int x_tamanio, out int x_totalregistros)
        {
            string procedimiento = "TSP_ZKHuellas_Q02";
            x_totalregistros = 0;

            DataTable dtResult;

            List<ZKHuellas> list = new List<ZKHuellas>();
            List<ParamSP> parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@PageIndex", strValParam = x_pagina });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@PageSize", strValParam = x_tamanio });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Salida, strNomParam = "@TotalRows", strValParam = x_totalregistros });

            bool result = ExecuteDataTable(procedimiento, ref parametros, out dtResult);
            if (result)
            {
                foreach (DataRow dr in dtResult.Rows)
                {
                    ZKHuellas usuario = UtilitarioEN.MapearObjeto<ZKHuellas>(dr);
                    list.Add(usuario);
                }
            }

            x_totalregistros = Convert.ToInt32(parametros[2].strValParam);

            return list;
        }


        public bool InsertarHuella(int x_iIdUsuario, List<ZKHuellas> x_lstHuellas)
        {
            string procedimiento = "TSP_ZKHuellas_I01";
            List<ParamSP> parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
            bool result = false;

            ZKHuellas zkHuella = null;
            List<ZKHuellas> lstHuellasreg = ListarHuellasAlg10Usuario(x_iIdUsuario);

            foreach (var huella in x_lstHuellas)
            {
                zkHuella = lstHuellasreg.Find(x => x.iFingerNumber == huella.iFingerNumber);
                if (zkHuella != null)
                    EliminarHuella(zkHuella.iIdHuella, zkHuella.iIdUsuario, zkHuella.iFingerNumber);

                parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
                parametros.Add(new ParamSP() { enuDirParam = enParamIO.Salida, strNomParam = "@iIdHuella", strValParam = huella.iIdHuella });
                parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@iIdUsuario", strValParam = x_iIdUsuario });
                parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@iFingerNumber", strValParam = huella.iFingerNumber });
                parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@Huella", strValParam = huella.Huella });
                parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@nLngHuella", strValParam = huella.Huella.Length });
                parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@ifingernumberzk5000", strValParam = huella.ifingernumberzk5000 });
                parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@Huella10", strValParam = huella.Huella10 });
                parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@nLngHuella10", strValParam = huella.Huella10.Length });

                result = ExecuteNonQuery(procedimiento, ref parametros);
                if (!result)
                {
                    Error = "Error en inserción de huella.";
                    return false;
                }
            }
            return true;
        }
        public bool EliminarHuella(int x_iIdHuella, int x_iIdUsuario, int x_iFingerNumber)
        {
            string procedimiento = "TSP_ZKHUELLA_D01";
            List<ParamSP> parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
            bool result = false;


            parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@intIdHuella", strValParam = x_iIdHuella });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@intIdUsuario", strValParam = x_iIdUsuario });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@intIdIndice", strValParam = x_iFingerNumber });

            result = ExecuteNonQuery(procedimiento, ref parametros);
            if (!result)
            {
                Error = "Error en eliminación de huella.";
                return false;
            }
            return true;
        }

    }
}