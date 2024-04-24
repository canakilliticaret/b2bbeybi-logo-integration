using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INT.BeybiB2B.DataAccess
{
    public class DBClassLocal
    {
        private static DBClassLocal _defaultInstance = new DBClassLocal();
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["LocalEntities"].ConnectionString;

        public static DBClassLocal Default
        {
            get { return _defaultInstance; }
        }

        public DBClassLocal()
        {

        }

        public DataTable Select(string query, string connectionstring = "")
        {
            var connection = new SqlConnection(CheckConnectionString(connectionstring));
            var adapter = new SqlDataAdapter(query, connection);
            adapter.SelectCommand.CommandTimeout = 180;
            try
            {
                connection.Open();
                DataSet myData = new DataSet();
                adapter.Fill(myData);
                adapter.Dispose();
                connection.Close();
                connection.Dispose();
                return myData.Tables[0];
            }
            catch (Exception ex)
            {
                //Logger.Error(ex, "DBClassLocal", "Select");
                return null;
            }
            finally
            {
                adapter.Dispose();
                connection.Close();
                connection.Dispose();
            }
        }

        public DataTable SelectWithParams(string query, params object[] parameters)
        {
            //SqlConnection.ClearAllPools();
            var connection = new SqlConnection(CheckConnectionString(""));
            var adapter = new SqlDataAdapter(query, connection);
            adapter.SelectCommand.CommandTimeout = 180;

            for (int i = 0; i < parameters.Length; i++)
            {
                adapter.SelectCommand.Parameters.AddWithValue("@prm" + i, parameters[i]);
            }

            try
            {
                connection.Open();
                DataSet myData = new DataSet();
                adapter.Fill(myData);
                adapter.Dispose();
                connection.Close();
                connection.Dispose();
                return myData.Tables[0];
            }
            catch (Exception ex)
            {
                //Logger.Error(ex, "DBClassLocal", "SelectWithParams");
                return null;
            }
            finally
            {
                adapter.Dispose();
                connection.Close();
                connection.Dispose();
            }
        }

        public DataRow SelectRowWithParams(string query, params object[] parameters)
        {
            var connection = new SqlConnection(CheckConnectionString(""));
            var adapter = new SqlDataAdapter(query, connection);

            for (int i = 0; i < parameters.Length; i++)
            {
                adapter.SelectCommand.Parameters.AddWithValue("@prm" + i, parameters[i]);
            }
            try
            {
                connection.Open();
                DataSet myData = new DataSet();
                adapter.Fill(myData);
                adapter.Dispose();
                connection.Close();
                connection.Dispose();
                return myData.Tables[0].Rows[0];
            }
            catch (Exception ex)
            {
                //Logger.Error(ex, "DBClassLocal", "SelectWithParams");
                return null;
            }
            finally
            {
                adapter.Dispose();
                connection.Close();
                connection.Dispose();
            }
        }

        public DataSet SelectDS(string query, string connectionstring = "")
        {
            var connection = new SqlConnection(CheckConnectionString(connectionstring));
            var adapter = new SqlDataAdapter(query, connection);

            try
            {
                connection.Open();
                DataSet myData = new DataSet();
                adapter.Fill(myData);
                adapter.Dispose();
                connection.Close();
                connection.Dispose();
                return myData;
            }
            catch (Exception ex)
            {
                //Logger.Error(ex, "DBClassLocal", "SelectDS");
                return null;
            }
            finally
            {
                adapter.Dispose();
                connection.Close();
                connection.Dispose();
            }
        }

        public int Execute(string query, string connectionstring = "")
        {
            var connection = new SqlConnection(CheckConnectionString(connectionstring));
            var command = new SqlCommand(query);

            try
            {
                connection.Open();
                command.Connection = connection;
                var retVal = command.ExecuteNonQuery();
                return retVal;
            }
            catch (Exception ex)
            {
                //Logger.Error(ex, "DBClassLocal", "Execute " + query);
                return 0;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
                command.Dispose();
            }
        }

        public int ExecuteWithParams(string query, params object[] parameters)
        {
            var connection = new SqlConnection(CheckConnectionString(""));
            var command = new SqlCommand(query);
            for (int i = 0; i < parameters.Length; i++)
            {
                command.Parameters.AddWithValue("@prm" + i, parameters[i]);
            }

            try
            {
                connection.Open();
                command.Connection = connection;
                var retVal = command.ExecuteNonQuery();
                return retVal;
            }
            catch (Exception ex)
            {
                //Logger<DBClass>.Log.Error(ex);
                return 0;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
                command.Dispose();
            }
        }

        public object ExecuteScalar(string query, string connectionstring = "")
        {
            var connection = new SqlConnection(CheckConnectionString(connectionstring));
            var command = new SqlCommand(query);

            try
            {
                connection.Open();
                command.Connection = connection;
                var retVal = command.ExecuteScalar();
                return retVal;
            }
            catch (Exception ex)
            {
                //Logger.Error(ex, "DBClassLocal", "ExecuteScalar");
                return null;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
                command.Dispose();
            }
        }

        public object ExecuteScalarWithParams(string query, params object[] parameters)
        {
            var connection = new SqlConnection(CheckConnectionString(""));
            var command = new SqlCommand(query);
            for (int i = 0; i < parameters.Length; i++)
            {
                command.Parameters.AddWithValue("@prm" + i, parameters[i]);
            }

            try
            {
                connection.Open();
                command.Connection = connection;
                var retVal = command.ExecuteScalar();
                return retVal;
            }
            catch (Exception ex)
            {
                //Logger.Error(ex, "DBClassLocal", "ExecuteScalarWithParams");
                return null;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
                command.Dispose();
            }
        }

        public bool CheckConnection(string connectionstring = "")
        {
            var result = false;
            var connection = new SqlConnection(CheckConnectionString(connectionstring));
            try
            {
                connection.Open();
                result = true;
            }
            catch (Exception ex)
            {
                //Logger.Error(ex, "DBClassLocal", "CheckConnection");
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
            return result;
        }

        private string CheckConnectionString(string connectionstring)
        {
            if (string.IsNullOrEmpty(connectionstring))
            {
                return _connectionString;
            }
            return connectionstring;
        }
    }

}
