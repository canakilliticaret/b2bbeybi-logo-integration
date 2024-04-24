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
    public class DBClass
    {
        private static DBClass _defaultInstance = new DBClass();
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["IntegrationEntities"].ConnectionString;

        public static DBClass Default
        {
            get { return _defaultInstance; }
        }

        public DBClass()
        {

        }

        public DataTable Select(string query, string connectionstring = "")
        {
            SqlConnection connection = new SqlConnection(CheckConnectionString(connectionstring));
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
                //Logger.Error(ex, "DBClass", "Select");
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
                //Logger.Error(ex, "DBClass", "SelectWithParams");
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
                Console.WriteLine("Logo Sql DB Bağlantısı Başarılı");

                return retVal;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Logo Sql DB Bağlanamadı.");
                return 0;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
                command.Dispose();
            }
        }
        public object ExecuteWithParams(string query, params object[] parameters)
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
                //Logger.Error(ex, "DBClass", "SelectWithParams");
                return null;
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
                //Logger.Error(ex, "DBClass", "ExecuteScalar");
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
            string itemRef = query;
            string itemx = "";
            for (int i = 0; i < parameters.Length; i++)
            {
                command.Parameters.AddWithValue("@prm" + i, parameters[i]);
                itemx += parameters[i].ToString();
            }

            try
            {
                connection.Open();
                command.Connection = connection;
                var retVal = command.ExecuteScalar();

                Console.WriteLine(retVal);

                return retVal;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                //Logger.Error(ex, "DBClass", "CheckConnection");
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
