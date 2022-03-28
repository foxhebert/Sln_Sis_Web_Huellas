using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using Dominio.Entidades.Tipo;
using System.Linq;

namespace Infraestructura.Data.SqlServer
{
    public class ZKUsuariosDAO : ConexionDAO
    {

        public ZKUsuarios ListarUsuarioByCod(int x_iCodUsuario)
        {
            ZKHuellasDAO zkHuellasDao = new ZKHuellasDAO();
            string procedimiento = "TSP_ZKUSUA_Q06";

            DataTable dtResult;
            ZKUsuarios entidad = new ZKUsuarios();
            List<Dominio.Entidades.Tipo.ParamSP> parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
            parametros.Add(new Dominio.Entidades.Tipo.ParamSP() { enuDirParam = Dominio.Entidades.Tipo.enParamIO.Entrada, strNomParam = "@iCodUsuario", strValParam = x_iCodUsuario.ToString() });

            bool result = ExecuteDataTable(procedimiento, ref parametros, out dtResult);
            if (result)
            {
                foreach (DataRow dr in dtResult.Rows)
                {
                    entidad = UtilitarioEN.MapearObjeto<ZKUsuarios>(dr);
                    entidad.Huellas = zkHuellasDao.ListarHuellasAlg10Usuario(entidad.iIdUsuario);
                }
            }
            return entidad;
        }
        public ZKUsuarios ListarUsuarioByID(int x_iIdUsuario)
        {
            ZKHuellasDAO zkHuellasDao = new ZKHuellasDAO();
            string procedimiento = "TSP_ZKUSUA_Q07";

            DataTable dtResult;
            ZKUsuarios entidad = new ZKUsuarios();
            List<Dominio.Entidades.Tipo.ParamSP> parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
            parametros.Add(new Dominio.Entidades.Tipo.ParamSP() { enuDirParam = Dominio.Entidades.Tipo.enParamIO.Entrada, strNomParam = "@iIdUsuario", strValParam = x_iIdUsuario.ToString() });

            bool result = ExecuteDataTable(procedimiento, ref parametros, out dtResult);
            if (result)
            {
                foreach (DataRow dr in dtResult.Rows)
                {
                    entidad = UtilitarioEN.MapearObjeto<ZKUsuarios>(dr);
                    entidad.Huellas = zkHuellasDao.ListarHuellasAlg10Usuario(entidad.iIdUsuario);
                }
            }
            return entidad;
        }

        public List<ZKUsuarios> ListarUsuariosTodo(int x_idSede, int x_estadoActivo, int x_pagina, int x_tamanio, ref int x_total)
        {
            ZKHuellasDAO zkHuellasDao = new ZKHuellasDAO();
            string procedimiento = "TSP_ZKUSUA_Q08";

            DataTable dtResult;
            List<ZKUsuarios> lista = new List<ZKUsuarios>();
            List<Dominio.Entidades.Tipo.ParamSP> parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
            parametros.Add(new Dominio.Entidades.Tipo.ParamSP() { enuDirParam = Dominio.Entidades.Tipo.enParamIO.Entrada, strNomParam = "@intActivo", strValParam = x_estadoActivo });
            parametros.Add(new Dominio.Entidades.Tipo.ParamSP() { enuDirParam = Dominio.Entidades.Tipo.enParamIO.Entrada, strNomParam = "@PageIndex", strValParam = x_pagina });
            parametros.Add(new Dominio.Entidades.Tipo.ParamSP() { enuDirParam = Dominio.Entidades.Tipo.enParamIO.Entrada, strNomParam = "@PageSize", strValParam = x_tamanio });
            parametros.Add(new Dominio.Entidades.Tipo.ParamSP() { enuDirParam = Dominio.Entidades.Tipo.enParamIO.Salida, strNomParam = "@TotalRows", strValParam = 0 });

            parametros.Add(new Dominio.Entidades.Tipo.ParamSP() { enuDirParam = Dominio.Entidades.Tipo.enParamIO.Entrada, strNomParam = "@idSede", strValParam = x_idSede });


            bool result = ExecuteDataTable(procedimiento, ref parametros, out dtResult);
            if (result)
            {
                foreach (DataRow dr in dtResult.Rows)
                {
                    ZKUsuarios usuario = UtilitarioEN.MapearObjeto<ZKUsuarios>(dr);
                    lista.Add(usuario);
                }
            }
            x_total = Convert.ToInt32(parametros[3].strValParam);

            lista.ForEach(x => { x.Huellas = new List<ZKHuellas>(); x.strCoUsuarAnt = ""; });

            return lista;
        }

        public bool InsertarUsuario(string x_dedosAntes, ref ZKUsuarios x_usuario)
        {
            string procedimiento = "TSP_ZKUSUARIOS_I03";

            List<string> lstDedos = new List<string>();
            if (x_dedosAntes == "")
                x_dedosAntes = "1111111111";
            for (int i = 0; i < 10; i++)
            {
                if (x_dedosAntes[i] == '1')
                    lstDedos.Add((i + 1).ToString());
            }


            List<ParamSP> parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@iIdUsuario", strValParam = x_usuario.iIdUsuario });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@intCoUsuar", strValParam = x_usuario.iCodUsuario });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@strNoUsuar", strValParam = x_usuario.sNombre });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@intNuTarje", strValParam = x_usuario.CardNumber });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@strCoPerso", strValParam = x_usuario.Cod_Personal });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@intIdSede", strValParam = x_usuario.iIdSede });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@Estado", strValParam = x_usuario.Estado });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Salida, strNomParam = "@insertado", strValParam = 0 });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@dedosAnt", strValParam = string.Join(",", lstDedos.ToArray()) });

            bool result = ExecuteNonQuery(procedimiento, ref parametros);

            int idUsuario = 0;
            if (!result || !int.TryParse(parametros[7].strValParam.ToString(), out idUsuario) || idUsuario == 0)
            {
                Error = "Error en inserción de usuario";
                return false;
            }

            x_usuario.iIdUsuario = Convert.ToInt32(parametros[7].strValParam);

            if (x_usuario.Huellas != null && x_usuario.Huellas.Count > 0)
            {
                ZKHuellasDAO zkHuellasDao = new ZKHuellasDAO();
                result = zkHuellasDao.InsertarHuella(x_usuario.iIdUsuario, x_usuario.Huellas);
            }

            x_usuario = ListarUsuarioByID(x_usuario.iIdUsuario);
            return result;

        }

        public bool EliminarUsuario(int x_idUsuario, int x_idSesion)
        {
            string procedimiento = "TSP_ZKUSUARIOS_D01";

            List<ParamSP> parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@iIdUsuario", strValParam = x_idUsuario });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@iIdSession", strValParam = x_idSesion });

            bool result = ExecuteNonQuery(procedimiento, ref parametros);

            return result;
        }
        public bool CambiarEstadoUsuario(int x_idUsuario, int x_estado, int x_idSesion)
        {
            string procedimiento = "TSP_ZKUSUARIOS_U01";

            List<ParamSP> parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@iIdUsuario", strValParam = x_idUsuario });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@Estado", strValParam = x_estado });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@iIdSession", strValParam = x_idSesion });

            bool result = ExecuteNonQuery(procedimiento, ref parametros);

            return result;
        }

        #region SINCRONIZACIÓN

        //public ListItemPersonaExport ListarPersonalExport(string x_strCodUnico, int x_idSede, int x_pagina, int x_numReg, ref int x_intTotalReg, ref string x_mensaje)
        //{
        //    ListItemPersonaExport lista = new ListItemPersonaExport();

        //    string procedimiento = "TSP_ZKUSUA_Q10";

        //    List<ParamSP> parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
        //    parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@strCodSedeWinSer", strValParam = x_strCodUnico });
        //    parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@intIdSede", strValParam = x_idSede });
        //    parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@pageIndex", strValParam = x_pagina });
        //    parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@pageSize", strValParam = x_numReg });
        //    parametros.Add(new ParamSP() { enuDirParam = enParamIO.Salida, strNomParam = "@TotalRows", strValParam = 0 });

        //    DataTable dtResult = new DataTable();
        //    bool result = ExecuteDataTable(procedimiento, ref parametros, out dtResult);

        //    if (!result || !int.TryParse(parametros[4].strValParam.ToString(), out x_intTotalReg))
        //    {
        //        x_mensaje = "Error en listado de usuarios export";
        //        return lista;
        //    }

        //    foreach (DataRow dr in dtResult.Rows)
        //    {
        //        ItemPersonaExport entidad = UtilitarioEN.MapearObjeto<ItemPersonaExport>(dr);
        //        lista.Add(entidad);
        //    }
        //    return lista;
        //}

        //public string ListarPersonalInactivoExport(string x_strCodUnico, int x_idSede, ref string x_mensaje)
        //{
        //    ListItemHuellaExport lista = new ListItemHuellaExport();

        //    string procedimiento = "TSP_ZKUSUA_Q11";

        //    List<ParamSP> parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
        //    parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@strCodSedeWinSer", strValParam = x_strCodUnico });
        //    parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@intIdSede", strValParam = x_idSede });
        //    parametros.Add(new ParamSP() { enuDirParam = enParamIO.Salida, strNomParam = "@idElim", strValParam = "" });

        //    bool result = ExecuteNonQuery(procedimiento, ref parametros);

        //    if (!result)
        //    {
        //        x_mensaje = "Error en listado de usuarios inactivos export";
        //        return "";
        //    }

        //    return parametros[2].strValParam.ToString();
        //}

        //public ListItemHuellaExport ListarHuellasExport(string x_strCodUnico, int x_idSede, int x_pagina, int x_numReg, ref int x_intTotalReg, ref string x_mensaje)
        //{
        //    ListItemHuellaExport lista = new ListItemHuellaExport();

        //    string procedimiento = "TSP_ZKUSUA_Q12";

        //    List<ParamSP> parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
        //    parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@strCodSedeWinSer", strValParam = x_strCodUnico });
        //    parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@intIdSede", strValParam = x_idSede });
        //    parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@pageIndex", strValParam = x_pagina });
        //    parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@pageSize", strValParam = x_numReg });
        //    parametros.Add(new ParamSP() { enuDirParam = enParamIO.Salida, strNomParam = "@TotalRows", strValParam = 0 });

        //    DataTable dtResult = new DataTable();
        //    bool result = ExecuteDataTable(procedimiento, ref parametros, out dtResult);

        //    if (!result || !int.TryParse(parametros[4].strValParam.ToString(), out x_intTotalReg))
        //    {
        //        x_mensaje = "Error en listado de huellas export";
        //        return lista;
        //    }

        //    foreach (DataRow dr in dtResult.Rows)
        //    {
        //        ItemHuellaExport entidad = UtilitarioEN.MapearObjeto<ItemHuellaExport>(dr);
        //        lista.Add(entidad);
        //    }
        //    return lista;
        //}

        //public bool InsertarMarcasImport(ListItemMarcaImport x_lst, ref string x_mensaje)
        //{
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("idPersonal", typeof(int));
        //    dt.Columns.Add("idNumDedo", typeof(int));
        //    dt.Columns.Add("dttFechaMarca", typeof(DateTime));
        //    dt.Columns.Add("intIdSede", typeof(int));
        //    dt.Columns.Add("strSerie", typeof(string));

        //    x_lst.ToList().ForEach(x =>
        //    {
        //        DataRow dr = dt.NewRow();
        //        dr["idPersonal"] = x.idPersonal;
        //        dr["idNumDedo"] = x.intNumDedo;
        //        dr["dttFechaMarca"] = x.dttFechaMarca;
        //        dr["intIdSede"] = x.intIdSede;
        //        dr["strSerie"] = x.strSerie;
        //        dt.Rows.Add(dr);
        //    });

        //    string procedimiento = "TSP_ZKMarcaciones_I02";

        //    List<ParamSP> parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
        //    parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@lst", strValParam = dt, blnEsEstructura = true, strEstructuraNombre = "TT_ZKMarcaImport" });
        //    parametros.Add(new ParamSP() { enuDirParam = enParamIO.Salida, strNomParam = "@intResult", strValParam = 0 });
        //    parametros.Add(new ParamSP() { enuDirParam = enParamIO.Salida, strNomParam = "@strMensaje", strValParam = "" });

        //    bool result = ExecuteNonQuery(procedimiento, ref parametros);

        //    if (!result)
        //    {
        //        x_mensaje = parametros[2].strValParam.ToString();
        //    }

        //    return result;
        //}

        //public void ActualizaFechaSincro(string x_codUnico)
        //{
        //    string procedimiento = "TSP_ZKSedeSincro_U01";

        //    List<ParamSP> parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
        //    parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@strCodSedeWinSer", strValParam = x_codUnico });

        //    ExecuteNonQuery(procedimiento, ref parametros);
        //}

        //public int ObtieneConfigValores(ref int x_tiempoSincro)
        //{
        //    int tiempoEntreMarcas = 0;
        //    string procedimiento = "TSP_ZKConfiguracion_Q01";

        //    List<ParamSP> parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
        //    parametros.Add(new ParamSP() { enuDirParam = enParamIO.Salida, strNomParam = "@tiempoEntreMarcas", strValParam = 0 });
        //    parametros.Add(new ParamSP() { enuDirParam = enParamIO.Salida, strNomParam = "@tiempoSincroniza", strValParam = 0 });

        //    ExecuteNonQuery(procedimiento, ref parametros);

        //    tiempoEntreMarcas = int.Parse(parametros[0].strValParam.ToString());
        //    x_tiempoSincro = int.Parse(parametros[1].strValParam.ToString());

        //    return tiempoEntreMarcas;
        //}

        //public ListItemErrorCarga InsertMasivo(ListItemPersonaImport x_lst, ref int x_intInsertados, ref int x_intErrores)
        //{
        //    ListItemErrorCarga lstErrores = new ListItemErrorCarga();
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("intCodigo", typeof(int));
        //    dt.Columns.Add("strCodigoPersonal", typeof(string));
        //    dt.Columns.Add("strNombres", typeof(string));
        //    dt.Columns.Add("intNumTarjeta", typeof(long));
        //    dt.Columns.Add("strSede", typeof(string));
        //    dt.Columns.Add("intLinea", typeof(int));

        //    x_lst.ToList().ForEach(x =>
        //    {
        //        DataRow dr = dt.NewRow();
        //        dr["intCodigo"] = x.intCodigo;
        //        dr["strCodigoPersonal"] = x.strCodigoPersonal;
        //        dr["strNombres"] = x.strNombres;
        //        dr["intNumTarjeta"] = x.intNumTarjeta;
        //        dr["strSede"] = x.strSede;
        //        dr["intLinea"] = x.intLinea;
        //        dt.Rows.Add(dr);
        //    });

        //    string procedimiento = "TSP_ZKUSUARIOS_I04";

        //    List<ParamSP> parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
        //    parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@lst", strValParam = dt, blnEsEstructura = true, strEstructuraNombre = "TT_ZKUsuariosImport" });
        //    parametros.Add(new ParamSP() { enuDirParam = enParamIO.Salida, strNomParam = "@INSERTADOS", strValParam = 0 });
        //    parametros.Add(new ParamSP() { enuDirParam = enParamIO.Salida, strNomParam = "@ERRONEOS", strValParam = 0 });

        //    DataTable dtError = new DataTable();
        //    bool result = ExecuteDataTable(procedimiento, ref parametros, out dtError);


        //    if (!result)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            ItemErrorCarga error = new ItemErrorCarga();
        //            error.intLinea = int.Parse(dr["intLinea"].ToString());
        //            error.strRegistro = dr["strRegistro"].ToString();
        //            error.strError = dr["strError"].ToString();
        //            lstErrores.Add(error);
        //        }
        //        x_intInsertados = int.Parse(parametros[1].strValParam.ToString());
        //        x_intErrores = int.Parse(parametros[2].strValParam.ToString());
        //    }

        //    return lstErrores;
        //}
        #endregion
    }
}