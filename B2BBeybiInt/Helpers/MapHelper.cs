using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using INT.BeybiB2B.DataAccess;

namespace INT.BeybiB2B.Helpers
{
    public static class MapHelper
    {
        public static List<T> BindListLocal<T>(this string query, params object[] parameters) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();
                DataTable table = DBClassLocal.Default.SelectWithParams(query, parameters);

                foreach (var row in table.AsEnumerable())
                {
                    T obj = new T();


                    foreach (DataColumn column in table.Columns)
                    {
                        try
                        {
                            string cname = column.ColumnName;
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(cname);
                            if (propertyInfo == null)
                            {
                                continue;
                            }
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[cname], propertyInfo.PropertyType), null);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }

                    //foreach (var prop in obj.GetType().GetProperties())
                    //{
                    //    try
                    //    {
                    //        PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                    //        propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                    //    }
                    //    catch
                    //    {
                    //        continue;
                    //    }
                    //}

                    list.Add(obj);
                }

                return list;
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex, "MapHelper", "BindList");
                return null;
            }
        }

        public static T BindObjectLocal<T>(this string query, params object[] parameters) where T : class, new()
        {
            try
            {
                DataTable table = DBClassLocal.Default.SelectWithParams(query, parameters);
                if (table == null || table.Rows.Count == 0)
                {
                    return null;
                }
                DataRow row = table.Rows[0];
                T obj = new T();


                foreach (DataColumn column in table.Columns)
                {
                    try
                    {
                        string cname = column.ColumnName;
                        PropertyInfo propertyInfo = obj.GetType().GetProperty(cname);
                        if (propertyInfo == null)
                        {
                            continue;
                        }
                        propertyInfo.SetValue(obj, Convert.ChangeType(row[cname], propertyInfo.PropertyType), null);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }

                //foreach (var prop in obj.GetType().GetProperties())
                //{
                //    try
                //    {
                //        PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                //        propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                //    }
                //    catch
                //    {
                //        continue;
                //    }
                //}

                return obj;
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex, "MapHelper", "BindObject");
                return null;
            }
        }

        public static List<T> BindListAccount<T>(this string query, params object[] parameters) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();
                DataTable table = DBClass.Default.SelectWithParams(query, parameters);

                foreach (var row in table.AsEnumerable())
                {
                    T obj = new T();

                    foreach (DataColumn column in table.Columns)
                    {
                        try
                        {
                            string cname = column.ColumnName;
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(cname);
                            if (propertyInfo == null)
                            {
                                continue;
                            }
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[cname], propertyInfo.PropertyType), null);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }

                    //foreach (var prop in obj.GetType().GetProperties())
                    //{
                    //    try
                    //    {
                    //        PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                    //        propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                    //    }
                    //    catch
                    //    {
                    //        continue;
                    //    }
                    //}

                    list.Add(obj);
                }

                return list;
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex, "MapHelper", "BindList");
                return null;
            }
        }

        public static T BindObjectAccount<T>(this string query, params object[] parameters) where T : class, new()
        {
            try
            {
                DataTable table = DBClass.Default.SelectWithParams(query, parameters);
                if (table == null || table.Rows.Count == 0)
                {
                    return null;
                }
                DataRow row = table.Rows[0];
                T obj = new T();

                foreach (DataColumn column in table.Columns)
                {
                    try
                    {
                        string cname = column.ColumnName;
                        PropertyInfo propertyInfo = obj.GetType().GetProperty(cname);
                        if (propertyInfo == null)
                        {
                            continue;
                        }
                        propertyInfo.SetValue(obj, Convert.ChangeType(row[cname], propertyInfo.PropertyType), null);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }

                //foreach (var prop in obj.GetType().GetProperties())
                //{
                //    try
                //    {
                //        PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                //        propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                //    }
                //    catch
                //    {
                //        continue;
                //    }
                //}

                return obj;
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex, "MapHelper", "BindObject");
                return null;
            }
        }

        public static List<T> BindListDataTable<T>(this DataTable table, params object[] parameters) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (var row in table.AsEnumerable())
                {
                    T obj = new T();

                    foreach (DataColumn column in table.Columns)
                    {
                        try
                        {
                            string cname = column.ColumnName;
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(cname);
                            if (propertyInfo == null)
                            {
                                continue;
                            }
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[cname], propertyInfo.PropertyType), null);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }

                    //foreach (var prop in obj.GetType().GetProperties())
                    //{
                    //    try
                    //    {
                    //        PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                    //        propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                    //    }
                    //    catch
                    //    {
                    //        continue;
                    //    }
                    //}

                    list.Add(obj);
                }

                return list;
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex, "MapHelper", "BindList");
                return null;
            }
        }

        public static T BindObjectDataTable<T>(this DataTable table, params object[] parameters) where T : class, new()
        {
            try
            {
                if (table == null || table.Rows.Count == 0)
                {
                    return null;
                }
                DataRow row = table.Rows[0];
                T obj = new T();

                foreach (DataColumn column in table.Columns)
                {
                    try
                    {
                        string cname = column.ColumnName;
                        PropertyInfo propertyInfo = obj.GetType().GetProperty(cname);
                        if (propertyInfo == null)
                        {
                            continue;
                        }
                        propertyInfo.SetValue(obj, Convert.ChangeType(row[cname], propertyInfo.PropertyType), null);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }

                //foreach (var prop in obj.GetType().GetProperties())
                //{
                //    try
                //    {
                //        PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                //        propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                //    }
                //    catch
                //    {
                //        continue;
                //    }
                //}

                return obj;
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex, "MapHelper", "BindObject");
                return null;
            }
        }

    }

}
