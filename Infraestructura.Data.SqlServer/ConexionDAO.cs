using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace Infraestructura.Data.SqlServer
{
    public class ConexionDAO
    {
        private string cadCnx = ConfigurationManager.ConnectionStrings["cnSQL"].ConnectionString;
        public string Error { get; set; }

        #region Unitario

        public bool ExecuteNonQuery(string x_nombreSP, ref List<Dominio.Entidades.Tipo.ParamSP> x_lstParametros)
        {
            using (SqlConnection cn = new SqlConnection(cadCnx))
            {

                cn.Open();

                SqlCommand cmd = new SqlCommand(x_nombreSP, cn);
                cmd.CommandType = CommandType.StoredProcedure;
                // si se han enviado valores para los parámetros
                if (x_lstParametros.Equals(null))
                    x_lstParametros = new List<Dominio.Entidades.Tipo.ParamSP>();

                if (x_lstParametros.Count > 0)
                {
                    foreach (var par in x_lstParametros)
                    {
                        System.Data.SqlClient.SqlParameter pList = new System.Data.SqlClient.SqlParameter();
                        pList.ParameterName = par.strNomParam;
                        pList.Value = par.strValParam;

                        if (par.blnEsEstructura)
                        {

                            pList.SqlDbType = SqlDbType.Structured;
                            pList.TypeName = par.strEstructuraNombre;
                            pList.Direction = ParameterDirection.Input;
                        }
                        else
                        {
                            pList.Direction = par.enuDirParam == Dominio.Entidades.Tipo.enParamIO.Entrada ? ParameterDirection.Input : ParameterDirection.Output;
                            if (par.intLongitud > 0)
                                pList.Size = par.intLongitud;
                        }
                        cmd.Parameters.Add(pList);
                    }

                }
                cmd.ExecuteNonQuery();

                for (int i = 0; i < x_lstParametros.Count; i++)
                {
                    if (x_lstParametros[i].enuDirParam == Dominio.Entidades.Tipo.enParamIO.Entrada)
                        continue;

                    if (!cmd.Parameters[i].Value.Equals(DBNull.Value))
                        x_lstParametros[i].strValParam = Convert.ToString(cmd.Parameters[i].Value);
                }
            }
            return true;
        }

        public bool ExecuteDataTable(string x_nombreSP, ref List<Dominio.Entidades.Tipo.ParamSP> x_lstParametros, out DataTable x_dt)
        {
            x_dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(cadCnx))
            {

                cn.Open();

                SqlCommand cmd = new SqlCommand(x_nombreSP, cn);
                cmd.CommandType = CommandType.StoredProcedure;
                // si se han enviado valores para los parámetros
                if (x_lstParametros.Equals(null))
                    x_lstParametros = new List<Dominio.Entidades.Tipo.ParamSP>();

                if (x_lstParametros.Count > 0)
                {
                    foreach (var par in x_lstParametros)
                    {
                        System.Data.SqlClient.SqlParameter pList = new System.Data.SqlClient.SqlParameter();
                        pList.ParameterName = par.strNomParam;
                        pList.Value = par.strValParam;

                        if (par.blnEsEstructura)
                        {

                            pList.SqlDbType = SqlDbType.Structured;
                            pList.TypeName = par.strEstructuraNombre;
                            pList.Direction = ParameterDirection.Input;
                        }
                        else
                        {
                            pList.Direction = par.enuDirParam == Dominio.Entidades.Tipo.enParamIO.Entrada ? ParameterDirection.Input : ParameterDirection.Output;
                            if (par.intLongitud > 0)
                                pList.Size = par.intLongitud;
                        }
                        cmd.Parameters.Add(pList);
                    }
                }
                DbDataReader dr = cmd.ExecuteReader();
                x_dt.Load(dr);

                for (int i = 0; i < x_lstParametros.Count; i++)
                {
                    if (x_lstParametros[i].enuDirParam == Dominio.Entidades.Tipo.enParamIO.Entrada)
                        continue;

                    if (cmd.Parameters[i].Value != DBNull.Value)
                        x_lstParametros[i].strValParam = Convert.ToString(cmd.Parameters[i].Value);
                }
            }
            return true;
        }

        public string GetCC()
        {
            return new TCX0001.ctcCryptografia().CifrarCadenaPwUsuar(cadCnx);
        }

        #endregion


    }
}