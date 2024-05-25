using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace SQL_Test
{
    [TestClass]
    public class UnitTest1
    {
        public static string ExecutionDirectoryPathName()
        {
            var dirPath = Assembly.GetExecutingAssembly().Location;
            dirPath = Path.GetDirectoryName(dirPath);
            return dirPath;
        }

        [TestMethod]
        public void SqlServerFile_TestConnection()
        {
            string connectionString = @"data source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Laura\source\repos\2022-pss-yulhev2\Practice__05\Practice__05\AutentificacionDB.mdf;Integrated Security=True;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    conn.Close();
                }
                Assert.IsTrue(true);
            }
            catch (SqlException ex)
            {
                Assert.Fail($"SQL Exception: {ex.Message}");
            }
            catch (Exception ex)
            {
                Assert.Fail($"Exception: {ex.Message}");
            }
        }

        [TestMethod]
        public void SqlServerFile_InsertRecordTest()
        {
            string connectionString = @"data source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Laura\source\repos\2022-pss-yulhev2\Practice__05\Practice__05\AutentificacionDB.mdf;Integrated Security=True;";
            int result;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string name = "N_" + Guid.NewGuid().ToString();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "Insert INTO Authentications (Name, Password, Category, IsValid) " +
                                      "values (@newName, @newPalabraPaso, @newCategoria, @newEsValido)";
                    cmd.Parameters.Add("@newName", SqlDbType.NVarChar, 20).Value = name;
                    cmd.Parameters.Add("@newPalabraPaso", SqlDbType.NVarChar, 20).Value = "Stepword";
                    cmd.Parameters.Add("@newCategoria", SqlDbType.NVarChar, 20).Value = "Category";
                    cmd.Parameters.Add("@newEsValido", SqlDbType.Bit).Value = true;
                    result = cmd.ExecuteNonQuery();
                    if (result != 1) throw new Exception("Error inserting a new record.");
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("The exception was not expected: " + ex.Message);
            }
        }
    }

    public class AuthenticationException : Exception
    {
        public AuthenticationCode Code { get; }

        public AuthenticationException(string message, AuthenticationCode code) : base(message)
        {
            Code = code;
        }
    }

    public enum AuthenticationCode
    {
        DataError
    }
}