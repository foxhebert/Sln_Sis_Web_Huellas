using System;
using System.Collections.Generic;
using System.Data;
using Dominio.Entidades;
using Dominio.Entidades.Tipo;
using System.Linq;

using System.Data.SqlClient;
using System.Configuration;

namespace Infraestructura.Data.SqlServer
{
    public class ZKLoginDAO : ConexionDAO
    {
        #region "Métodos NO Transaccionales"
        public ZKLoginOff ListarLoginByCod(string x_NombreSesion, string x_Password, ref string x_mensaje)
        //public ZKLogin ListarLoginByCod(string x_NombreSesion, string x_Password, ref string x_mensaje)
        {
            string procedimiento = "TSP_ZKLogin_Q01";

            DataTable dtResult;

            ZKLoginOff entidad = new ZKLoginOff();
            //ZKLogin entidad = new ZKLogin();
            List<ParamSP> parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@sNombreSesion", strValParam = x_NombreSesion });

            bool result = ExecuteDataTable(procedimiento, ref parametros, out dtResult);
            if (result)
            {
                foreach (DataRow dr in dtResult.Rows)
                {
                    entidad = UtilitarioEN.MapearObjeto<ZKLoginOff>(dr);
                    //entidad = UtilitarioEN.MapearObjeto<ZKLogin>(dr);
                }
            }

            if (entidad == null || entidad.iIdSesion == 0)
                x_mensaje = "El usuario no existe.";
            else if (!entidad.bitFlActivo)
                x_mensaje = "El usuario está inactivo.";
            else
            {
                TCX0001.ctcCryptografia crypto = new TCX0001.ctcCryptografia();
                string pwd = crypto.CifrarCadenaPwUsuar(x_Password);
                if (pwd != entidad.sPasswordSesion)
                    x_mensaje = "La contraseña ingresada es incorrecta";

                else
                {
                    ListZKMenuOff lstmenu = new ZKMenuDAO().ListarMenuLogin_(entidad.iIdSesion);
                    entidad.OpcionesMenuOff = new ListZKMenuOff();
                    //ListZKMenu lstmenu = new ZKMenuDAO().ListarMenuLogin(entidad.iIdSesion);
                    //entidad.OpcionesMenu = new ListZKMenu();
                    lstmenu.ToList().ForEach(x =>
                    {
                        if (x.IntAsing > 0)
                            entidad.OpcionesMenuOff.Add(x);
                        //entidad.OpcionesMenu.Add(x);
                    });
                }
            }
            if (!string.IsNullOrEmpty(x_mensaje))
                entidad = new ZKLoginOff();
            //entidad = new ZKLogin();
            return entidad;
        }
        public ListZKLoginOff ListarLogin(int x_idSesion)//modificado ListZKLogin
        {
            string procedimiento = "TSP_ZKLOGIN_Q02";

            DataTable dtResult;

            ListZKLoginOff lista = new ListZKLoginOff();//modificado ListZKLogin
            List<ParamSP> parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@iIdSesion", strValParam = x_idSesion });

            bool result = ExecuteDataTable(procedimiento, ref parametros, out dtResult);
            if (result)
            {
                foreach (DataRow dr in dtResult.Rows)
                {
                    lista.Add(UtilitarioEN.MapearObjeto<ZKLoginOff>(dr));//modificado ZKLogin
                }
            }
            return lista;
        }

        #endregion

        public bool InsertarLogin(ref ZKLoginOff x_login, string x_menus)//modificado ZkLogin
        {
            string procedimiento = "TSP_ZKLOGIN_I01";

            TCX0001.ctcCryptografia crypto = new TCX0001.ctcCryptografia();
            if (x_login.sPasswordSesion != "")
                x_login.sPasswordSesion = crypto.CifrarCadenaPwUsuar(x_login.sPasswordSesion);


            //ListZKLogin lista = new ListZKLogin();
            List<ParamSP> parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@iIdSesion", strValParam = x_login.iIdSesion });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@sNombreSesion", strValParam = x_login.sNombreSesion });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@sPasswordSesion", strValParam = x_login.sPasswordSesion });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@intIdSede", strValParam = x_login.intIdSede });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@bitFlActivo", strValParam = x_login.bitFlActivo });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Salida, strNomParam = "@insertado", strValParam = 0 });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@strMenus", strValParam = x_menus });

            bool result = ExecuteNonQuery(procedimiento, ref parametros);
            int idSesion = 0;
            if (!result || !int.TryParse(parametros[5].strValParam.ToString(), out idSesion) || idSesion == 0)
            {
                Error = "Error en inserción de usuario";
                return false;
            }

            x_login.iIdSesion = idSesion;

            return result;
        }


        //añadido 15.06.2021
        public ListZKLoginOff ListarUsers_(int x_idSesion)
        {
            string procedimiento = "TSP_ZKLOGIN_Q00";

            DataTable dtResult;

            ListZKLoginOff lista = new ListZKLoginOff();
            List<ParamSP> parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@iIdSesion", strValParam = x_idSesion });

            bool result = ExecuteDataTable(procedimiento, ref parametros, out dtResult);
            if (result)
            {
                foreach (DataRow dr in dtResult.Rows)
                {
                    lista.Add(UtilitarioEN.MapearObjeto<ZKLoginOff>(dr));
                }
            }
            return lista;
        }

        //añadido 28.06.2021
        public bool DescargaDriver(int x_idSesion,string strIpHost, ref string x_mensaje, ref int x_rpta)
        {
            string procedimiento = "TSP_DOWNDRIVER_IU00";

            List<ParamSP> parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@strIp", strValParam = strIpHost });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@iIdSesion", strValParam = x_idSesion });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Salida, strNomParam = "@strMensaje", strValParam = "", intLongitud = 250 });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Salida, strNomParam = "@intrpta", strValParam = 0 });


            bool result = ExecuteNonQuery(procedimiento, ref parametros);
            x_mensaje = parametros[2].strValParam.ToString();
            int.TryParse(parametros[3].strValParam.ToString(), out x_rpta);

            if (!result)
            {
                Error = "Error en registro de descarga en Driver";
                return false;
            }

            return result;
        }


        //añadido 02.11.2021 HGM
        public bool ConsultarDescargaDriver(int x_idSesion, string strIpHost, ref string x_mensaje, ref int x_rpta)
        {
            string procedimiento = "TSP_DOWNDRIVER_Q00";

            List<ParamSP> parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@strIp", strValParam = strIpHost });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@iIdSesion", strValParam = x_idSesion });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Salida, strNomParam = "@strMensaje", strValParam = "", intLongitud = 250 });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Salida, strNomParam = "@intrpta", strValParam = 0 });


            bool result = ExecuteNonQuery(procedimiento, ref parametros);
            x_mensaje = parametros[2].strValParam.ToString();
            int.TryParse(parametros[3].strValParam.ToString(), out x_rpta);

            if (!result)
            {
                Error = "Error en registro de descarga en Driver";
                return false;
            }

            return result;
        }




        #region registrar licencia HGM Añadido 20.10.2021

        //Cadena de  Conexion
        public string cadCnx = ConfigurationManager.ConnectionStrings["cnSQL"].ConnectionString;

        //1.16 listar Configuraciones para mostrarse en la ventana de Configuración.
        public List<TSConfi> ListarConfig(Session_Movi objSession, string strCoConfi, ref int intResult, ref string strMsjDB, ref string strMsjUsuario)
        {

            List<TSConfi> listaConf = new List<TSConfi>();
            using (SqlConnection cn = new SqlConnection(cadCnx))
            {

                SqlCommand cmd = new SqlCommand("TSP_TSCONFI_Q01", cn);
                //cmd.Transaction = trans;
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.CommandTimeout = timeSQL;//añadido 15.04.2021 ES
                cn.Open();

                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("@intIdSesion", objSession.intIdSesion);
                param.Add("@intIdMenu", objSession.intIdMenu);
                param.Add("@intIdSoft", objSession.intIdSoft);
                param.Add("@strCoConfi", strCoConfi);
                //salida
                param.Add("@intResult", 1);
                param.Add("@strMsjDB", "");
                param.Add("@strMsjUsuario", "");

                AsignarParametros(cmd, param);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TSConfi obj = new TSConfi();
                        obj.intIdConfi    = reader.GetInt32(0);
                        obj.strCoConfi    = reader.GetString(1);
                        obj.strValorConfi = reader.GetString(2);
                        obj.tipoControl   = reader.GetString(3);
                        obj.bitFlActivo   = true;// reader.GetBoolean(4);
                        listaConf.Add(obj);
                    }
                }
                intResult = Convert.ToInt32(cmd.Parameters["@intResult"].Value.ToString());
                strMsjDB = cmd.Parameters["@strMsjDB"].Value.ToString();
                strMsjUsuario = cmd.Parameters["@strMsjUsuario"].Value.ToString();

            }
            return listaConf;
        }

        //Asignar  Parametos
        protected void AsignarParametros(SqlCommand cmd, Dictionary<string, object> parametros)
        {
            // descubrir los parametros del SqlCommand enviado
            SqlCommandBuilder.DeriveParameters(cmd);

            foreach (KeyValuePair<string, object> item in parametros)
            {
                if (cmd.Parameters[item.Key].SqlDbType == SqlDbType.Structured)
                {
                    string typeName = cmd.Parameters[item.Key].TypeName;
                    int positionDot = typeName.LastIndexOf(".");
                    positionDot = positionDot > 0 ? positionDot + 1 : 0;
                    cmd.Parameters[item.Key].TypeName = typeName.Substring(positionDot);
                }

                cmd.Parameters[item.Key].Value = item.Value;
            }

        }

        //5.48
        public List<EnCorreo> obtenerDatosCorreo(Session_Movi objSession, ref int intResult, ref string strMsjDB, ref string strMsjUsuario)
        {
            List<EnCorreo> listobj = new List<EnCorreo>();
            using (SqlConnection cn = new SqlConnection(cadCnx))
            {
                SqlCommand cmd = new SqlCommand("TSP_TSCONFIGCORREOS_Q01", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("@intIdSesion", objSession.intIdSesion);
                param.Add("@intIdMenu", objSession.intIdMenu);
                param.Add("@intIdSoft", objSession.intIdSoft);
                param.Add("@intResult", 0);
                param.Add("@strMsjDB", "");
                param.Add("@strMsjUsuario", "");
                AsignarParametros(cmd, param);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        EnCorreo obj = new EnCorreo();
                        obj.strhost            = reader.GetString(0);
                        obj.strpuerto          = reader.GetString(1);
                        obj.bitAutentificacion = reader.GetBoolean(2);
                        obj.strccorreo         = reader.GetString(3);
                        obj.strcpass           = reader.GetString(4);
                        obj.strremitente       = reader.GetString(6);

                        listobj.Add(obj);
                    }
                }

                intResult = Convert.ToInt32(cmd.Parameters["@intResult"].Value.ToString());
                strMsjDB = cmd.Parameters["@strMsjDB"].Value.ToString();
                strMsjUsuario = cmd.Parameters["@strMsjUsuario"].Value.ToString();
            }
            return listobj;
        }

        //3.22.5  
        public TEXTOCORREO GetTextoCorreoObj(string strFiltro, CorreoEmp obj_, int intFiltro, bool boolFiltro, string adicional, ref int intResult, ref string strMsjDB, ref string strMsjUsuario)
        {
            TEXTOCORREO obj = new TEXTOCORREO();
            using (SqlConnection cn = new SqlConnection(cadCnx))
            {
                SqlCommand cmd = new SqlCommand("TSP_TSCORREOMENSAJE_Q01", cn);//TSP_TSCORREOMENSAJE_Q01
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.CommandTimeout = timeSQL;//21.04.2021
                cn.Open();

                Dictionary<string, object> param = new Dictionary<string, object>();

                param.Add("@strFiltro", strFiltro);
                param.Add("@intIdPersonal", obj_.intIdPersonal);

                param.Add("@strCampo1", obj_.strOC);
                param.Add("@strCampo2", obj_.strRUC);
                param.Add("@strCampo3", obj_.strCadena);
                param.Add("@strCampo4", obj_.strCadena2);

                //-------------------------------------------
                param.Add("@intFiltro", intFiltro);
                param.Add("@boolFiltro", boolFiltro);
                param.Add("@adicional", adicional);

                //salida
                param.Add("@intResult", 0);
                param.Add("@strMsjDB", "");
                param.Add("@strMsjUsuario", "");

                AsignarParametros(cmd, param);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    obj.saludo    = reader.GetString(0);
                    obj.despedida = reader.GetString(1);
                    obj.texto1    = reader.GetString(2);
                    obj.texto2    = reader.GetString(3);
                    obj.texto3    = reader.GetString(4);
                    obj.texto4    = reader.GetString(5);
                    obj.texto5    = reader.GetString(6);
                    obj.pie1      = reader.GetString(7);
                    obj.pie2      = reader.GetString(8);
                    obj.pie3      = reader.GetString(9);
                }
                reader.Close();

                intResult = Convert.ToInt32(cmd.Parameters["@intResult"].Value.ToString());
                strMsjDB = cmd.Parameters["@strMsjDB"].Value.ToString();
                strMsjUsuario = cmd.Parameters["@strMsjUsuario"].Value.ToString();

            }

            return obj;
        }

        //1.17 Actualizar configuraciones de la ventana de Configuración.
        public bool UpdateDetalleConfig(Session_Movi objSession, DataTable tt_config, ref int intResult, ref string strMsjDB, ref string strMsjUsuario)
        {
            bool tudobem = false;
            using (SqlConnection cn = new SqlConnection(cadCnx))
            {
                SqlCommand cmd = new SqlCommand("TSP_TSCONFI_U01", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("@intIdSesion", objSession.intIdSesion);
                param.Add("@intIdMenu", objSession.intIdMenu);
                param.Add("@intIdSoft", objSession.intIdSoft);
                param.Add("@TT_TSCONFI", tt_config);
                //Parámetros de Salida
                param.Add("@intResult", 0);
                param.Add("@strMsjDB", "");
                param.Add("@strMsjUsuario", "");

                AsignarParametros(cmd, param);
                int result = cmd.ExecuteNonQuery();

                intResult = Convert.ToInt32(cmd.Parameters["@intResult"].Value.ToString());
                strMsjDB = cmd.Parameters["@strMsjDB"].Value.ToString();
                strMsjUsuario = cmd.Parameters["@strMsjUsuario"].Value.ToString();

                tudobem = true;
            }
            return tudobem;
        }



        /// PRIMERO SE CONSULTA SI LA IP EXISTE EN LA TABLA TSCLIE
        public List<TSConfi> ListarCliWeb(ref int intResult, ref string strMsjDB, ref string strMsjUsuario)
        {
            List<TSConfi> lista = new List<TSConfi>();
            using (SqlConnection cn = new SqlConnection(cadCnx))
            {
                SqlCommand cmd = new SqlCommand("TSP_TSCLI_Q00", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                 cn.Open();
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("@intResult", 1);
                param.Add("@strMsjDB", "");
                param.Add("@strMsjUsuario", "");
                AsignarParametros(cmd, param);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TSConfi obj = new TSConfi();
                        obj.strValorConfi = reader.GetString(0);
                        lista.Add(obj);
                    }
                }
                intResult = Convert.ToInt32(cmd.Parameters["@intResult"].Value.ToString());
                strMsjDB = cmd.Parameters["@strMsjDB"].Value.ToString();
                strMsjUsuario = cmd.Parameters["@strMsjUsuario"].Value.ToString();

            }
            return lista;
        }
        //EN CASO NO EXISTA SE INSERTA AUTOMATICAMENTE LA IP DE LA PC EN LA TABLA "TSCLIE"
        public bool ICliWeb(string IPCli, ref int intResult, ref string strMsjDB, ref string strMsjUsuario)
        {
            bool tudobem = false;
            using (SqlConnection cn = new SqlConnection(cadCnx))
            {
                cn.Open();
                using (SqlTransaction trans = cn.BeginTransaction())
                {
                    try
                    {
                        /******************************                            
                            COMMIT ROLLBACK HMG c
                        ******************************/
                        SqlCommand cmd = new SqlCommand("TSP_TSCLI_I00", cn);
                        cmd.Transaction = trans;
                        cmd.CommandType = CommandType.StoredProcedure;

                        Dictionary<string, object> param = new Dictionary<string, object>();
                        param.Add("@IPCli", IPCli);
                        //Parámetros de Salida
                        param.Add("@intResult", 0);
                        param.Add("@strMsjDB", "");
                        param.Add("@strMsjUsuario", "");

                        AsignarParametros(cmd, param);
                        int result = cmd.ExecuteNonQuery();

                        intResult = Convert.ToInt32(cmd.Parameters["@intResult"].Value.ToString());
                        strMsjDB = cmd.Parameters["@strMsjDB"].Value.ToString();
                        strMsjUsuario = cmd.Parameters["@strMsjUsuario"].Value.ToString();

                        tudobem = true;

                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }

                    trans.Commit();
                }
                cn.Close();

            }
            return tudobem;
        }




        #endregion


        #region Tiempo entre Marcaciones HGM 11.11.2021

        public bool IUTiempoEntreMrcaciones(ref ZKLoginOff x_login, string x_menus)//modificado ZkLogin
        {
            string procedimiento = "TSP_ZKTIEMPOENTREMARCACIONES_I01"; //TSP_ZKLOGIN_I01

            TCX0001.ctcCryptografia crypto = new TCX0001.ctcCryptografia();
            if (x_login.sPasswordSesion != "")
                x_login.sPasswordSesion = crypto.CifrarCadenaPwUsuar(x_login.sPasswordSesion);


            //ListZKLogin lista = new ListZKLogin();
            List<ParamSP> parametros = new List<Dominio.Entidades.Tipo.ParamSP>();
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@iIdSesion", strValParam = x_login.iIdSesion });
            parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@sNombreSesion", strValParam = x_login.sNombreSesion });
            //parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@sPasswordSesion", strValParam = x_login.sPasswordSesion });
            //parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@intIdSede", strValParam = x_login.intIdSede });
            //parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@bitFlActivo", strValParam = x_login.bitFlActivo });
            //parametros.Add(new ParamSP() { enuDirParam = enParamIO.Salida, strNomParam = "@insertado", strValParam = 0 });
            //parametros.Add(new ParamSP() { enuDirParam = enParamIO.Entrada, strNomParam = "@strMenus", strValParam = x_menus });

            bool result = ExecuteNonQuery(procedimiento, ref parametros);
            int idSesion = 0;
            if (!result || !int.TryParse(parametros[5].strValParam.ToString(), out idSesion) || idSesion == 0)
            {
                Error = "Error en inserción";
                return false;
            }

            x_login.iIdSesion = idSesion;

            return result;
        }
        #endregion





    }
}