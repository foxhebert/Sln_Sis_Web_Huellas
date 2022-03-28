using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Microsoft.VisualBasic.CompilerServices;

namespace Dominio.Entidades.Tipo
{
    public static class UtilitarioEN
    {
        public static T MapearObjeto<T>(IDataReader p_dbrLectu)
        {
            T instance = Activator.CreateInstance<T>();
            string name = string.Empty;
            MemberInfo[] members = instance.GetType().GetMembers();
            int num1 = 0;
            int num2 = checked(p_dbrLectu.FieldCount - 1);
            int i = num1;
            while (i <= num2)
            {
                try
                {
                    name = p_dbrLectu.GetName(i).Trim();
                    name = ((IEnumerable<MemberInfo>)members).Where<MemberInfo>((Func<MemberInfo, bool>)(vt => string.Compare(vt.Name.Trim(), name.Trim(), true) == 0)).Select<MemberInfo, MemberInfo>((Func<MemberInfo, MemberInfo>)(vt => vt)).FirstOrDefault<MemberInfo>().Name.Trim();
                    Type propertyType1 = instance.GetType().GetProperty(name).PropertyType;
                    if (!p_dbrLectu.IsDBNull(p_dbrLectu.GetOrdinal(name)))
                    {
                        Type propertyType2 = instance.GetType().GetProperty(name).PropertyType;
                        string name1 = propertyType2.Name;
                        if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(string).Name, false) == 0)
                            instance.GetType().GetProperty(name).SetValue((object)instance, (object)p_dbrLectu.GetString(p_dbrLectu.GetOrdinal(name)), (object[])null);
                        else if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(int).Name, false) == 0)
                            instance.GetType().GetProperty(name).SetValue((object)instance, (object)p_dbrLectu.GetInt32(p_dbrLectu.GetOrdinal(name)), (object[])null);
                        else if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(DateTime).Name, false) == 0)
                            instance.GetType().GetProperty(name).SetValue((object)instance, (object)p_dbrLectu.GetDateTime(p_dbrLectu.GetOrdinal(name)), (object[])null);
                        else if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(bool).Name, false) == 0)
                            instance.GetType().GetProperty(name).SetValue((object)instance, (object)p_dbrLectu.GetBoolean(p_dbrLectu.GetOrdinal(name)), (object[])null);
                        else if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(int).Name, false) == 0)
                            instance.GetType().GetProperty(name).SetValue((object)instance, (object)p_dbrLectu.GetInt32(p_dbrLectu.GetOrdinal(name)), (object[])null);
                        else if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(long).Name, false) == 0)
                            instance.GetType().GetProperty(name).SetValue((object)instance, (object)p_dbrLectu.GetInt64(p_dbrLectu.GetOrdinal(name)), (object[])null);
                        else if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(short).Name, false) == 0)
                            instance.GetType().GetProperty(name).SetValue((object)instance, (object)p_dbrLectu.GetInt16(p_dbrLectu.GetOrdinal(name)), (object[])null);
                        else if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(byte).Name, false) == 0)
                            instance.GetType().GetProperty(name).SetValue((object)instance, (object)p_dbrLectu.GetByte(p_dbrLectu.GetOrdinal(name)), (object[])null);
                        else if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(char).Name, false) == 0)
                            instance.GetType().GetProperty(name).SetValue((object)instance, (object)p_dbrLectu.GetChar(p_dbrLectu.GetOrdinal(name)), (object[])null);
                        else if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(Decimal).Name, false) == 0)
                            instance.GetType().GetProperty(name).SetValue((object)instance, (object)p_dbrLectu.GetDecimal(p_dbrLectu.GetOrdinal(name)), (object[])null);
                        else if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(double).Name, false) == 0)
                            instance.GetType().GetProperty(name).SetValue((object)instance, (object)p_dbrLectu.GetDouble(p_dbrLectu.GetOrdinal(name)), (object[])null);
                        else if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(byte[]).Name, false) == 0)
                        {
                            long bytes = p_dbrLectu.GetBytes(p_dbrLectu.GetOrdinal(name), 0L, (byte[])null, 0, 0);
                            byte[] buffer = new byte[checked(Convert.ToInt32(bytes) + 1)];
                            p_dbrLectu.GetBytes(p_dbrLectu.GetOrdinal(name), 0L, buffer, 0, checked((int)bytes));
                            instance.GetType().GetProperty(name).SetValue((object)instance, (object)buffer, (object[])null);
                        }
                        else
                        {
                            if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(DateTime?).Name, false) != 0 && Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(int?).Name, false) != 0)
                            {
                                if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(bool?).Name, false) != 0)
                                    goto label_38;
                            }
                            string fullName = propertyType2.FullName;
                            if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(fullName, typeof(DateTime?).FullName, false) == 0)
                                instance.GetType().GetProperty(name).SetValue((object)instance, (object)p_dbrLectu.GetDateTime(p_dbrLectu.GetOrdinal(name)), (object[])null);
                            else if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(fullName, typeof(int?).FullName, false) == 0)
                                instance.GetType().GetProperty(name).SetValue((object)instance, (object)p_dbrLectu.GetInt32(p_dbrLectu.GetOrdinal(name)), (object[])null);
                            else if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(fullName, typeof(bool?).FullName, false) == 0)
                                instance.GetType().GetProperty(name).SetValue((object)instance, (object)p_dbrLectu.GetBoolean(p_dbrLectu.GetOrdinal(name)), (object[])null);
                        }
                    }
                    else
                    {
                        string name1 = propertyType1.Name;
                        if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(DateTime?).Name, false) != 0 && Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(int?).Name, false) != 0)
                        {
                            if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(bool?).Name, false) != 0)
                                goto label_38;
                        }
                        instance.GetType().GetProperty(name).SetValue((object)instance, (object)null, (object[])null);
                    }
                }
                catch (Exception ex)
                {
                    ProjectData.SetProjectError(ex);
                    throw new Exception("El campo " + name.Trim() + " no se encuentra correctamente definido");
                }
            label_38:
                checked { ++i; }
            }
            return instance;
        }

        public static T MapearObjeto<T>(DataRow p_drLectu)
        {
            T instance = Activator.CreateInstance<T>();
            string name = string.Empty;
            MemberInfo[] members = instance.GetType().GetMembers();
            int num1 = 0;
            int num2 = checked(p_drLectu.Table.Columns.Count - 1);
            int i = num1;
            while (i <= num2)
            {
                try
                {
                    name = p_drLectu.Table.Columns[i].ColumnName.Trim();
                    name = ((IEnumerable<MemberInfo>)members).Where<MemberInfo>((Func<MemberInfo, bool>)(vt => string.Compare(vt.Name.Trim(), name.Trim(), true) == 0)).Select<MemberInfo, MemberInfo>((Func<MemberInfo, MemberInfo>)(vt => vt)).FirstOrDefault<MemberInfo>().Name.Trim();
                    Type propertyType1 = instance.GetType().GetProperty(name).PropertyType;
                    if (p_drLectu[name] != DBNull.Value)
                    {
                        var valor = p_drLectu[name];
                        Type propertyType2 = instance.GetType().GetProperty(name).PropertyType;
                        string name1 = propertyType2.Name;
                        if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(string).Name, false) == 0)
                            instance.GetType().GetProperty(name).SetValue((object)instance, valor, (object[])null);
                        else if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(int).Name, false) == 0)
                            instance.GetType().GetProperty(name).SetValue((object)instance, valor, (object[])null);
                        else if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(DateTime).Name, false) == 0)
                            instance.GetType().GetProperty(name).SetValue((object)instance, (object)p_drLectu[name], (object[])null);
                        else if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(bool).Name, false) == 0)
                            instance.GetType().GetProperty(name).SetValue((object)instance, (object)p_drLectu[name], (object[])null);
                        else if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(int).Name, false) == 0)
                            instance.GetType().GetProperty(name).SetValue((object)instance, (object)p_drLectu[name], (object[])null);
                        else if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(long).Name, false) == 0)
                            instance.GetType().GetProperty(name).SetValue((object)instance, (object)p_drLectu[name], (object[])null);
                        else if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(short).Name, false) == 0)
                            instance.GetType().GetProperty(name).SetValue((object)instance, (object)p_drLectu[name], (object[])null);
                        else if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(byte).Name, false) == 0)
                            instance.GetType().GetProperty(name).SetValue((object)instance, (object)p_drLectu[name], (object[])null);
                        else if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(char).Name, false) == 0)
                            instance.GetType().GetProperty(name).SetValue((object)instance, (object)p_drLectu[name], (object[])null);
                        else if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(Decimal).Name, false) == 0)
                            instance.GetType().GetProperty(name).SetValue((object)instance, (object)p_drLectu[name], (object[])null);
                        else if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(double).Name, false) == 0)
                            instance.GetType().GetProperty(name).SetValue((object)instance, (object)p_drLectu[name], (object[])null);
                        else if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(byte[]).Name, false) == 0)
                        {
                            //long bytes = p_drLectu.GetBytes(p_drLectu.GetOrdinal(name), 0L, (byte[])null, 0, 0);
                            //byte[] buffer = new byte[checked(Convert.ToInt32(bytes) + 1)];
                            //p_drLectu.GetBytes(p_drLectu.GetOrdinal(name), 0L, buffer, 0, checked((int)bytes));
                            //instance.GetType().GetProperty(name).SetValue((object)instance, (object)buffer, (object[])null);
                        }
                        else
                        {
                            if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(DateTime?).Name, false) != 0 && Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(int?).Name, false) != 0)
                            {
                                if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(bool?).Name, false) != 0)
                                    goto label_38;
                            }
                            string fullName = propertyType2.FullName;
                            //if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(fullName, typeof(DateTime?).FullName, false) == 0)
                            //    instance.GetType().GetProperty(name).SetValue((object)instance, (object)p_drLectu.GetDateTime(p_drLectu.GetOrdinal(name)), (object[])null);
                            //else if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(fullName, typeof(int?).FullName, false) == 0)
                            //    instance.GetType().GetProperty(name).SetValue((object)instance, (object)p_drLectu.GetInt32(p_drLectu.GetOrdinal(name)), (object[])null);
                            //else if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(fullName, typeof(bool?).FullName, false) == 0)
                            //    instance.GetType().GetProperty(name).SetValue((object)instance, (object)p_drLectu.GetBoolean(p_drLectu.GetOrdinal(name)), (object[])null);
                        }
                    }
                    else
                    {
                        string name1 = propertyType1.Name;
                        if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(DateTime?).Name, false) != 0 && Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(int?).Name, false) != 0)
                        {
                            if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(name1, typeof(bool?).Name, false) != 0)
                                goto label_38;
                        }
                        instance.GetType().GetProperty(name).SetValue((object)instance, (object)null, (object[])null);
                    }
                }
                catch (Exception ex)
                {
                    ProjectData.SetProjectError(ex);
                    throw new Exception("El campo " + name.Trim() + " no se encuentra correctamente definido");
                }
            label_38:
                checked { ++i; }
            }
            return instance;
        }
    }
}