using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;

namespace SQL_Test
{
    [TestClass]
    public class UnitTest1
    {
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