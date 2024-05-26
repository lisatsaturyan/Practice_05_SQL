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
        public void SqlServerFile_InsertUpdateRecordTest()
        {
            string connectionString = @"data source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Laura\source\repos\2022-pss-yulhev2\Practice__05\Practice__05\AutentificacionDB.mdf;Integrated Security=True;";
            int result;
            int maxId = 0;
            string name = "N_" + Guid.NewGuid().ToString();
            string modName = "N_" + Guid.NewGuid().ToString();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Insert a new record
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO Authentications (Name, Password, Category, IsValid) " +
                                      "VALUES (@newName, @newPalabraPaso, @newCategory, @newEsValido)";
                    cmd.Parameters.Add("@newName", SqlDbType.NVarChar, 20).Value = name;
                    cmd.Parameters.Add("@newPalabraPaso", SqlDbType.NVarChar, 20).Value = "Stepword";
                    cmd.Parameters.Add("@newCategory", SqlDbType.NVarChar, 20).Value = "Category";
                    cmd.Parameters.Add("@newEsValido", SqlDbType.Bit).Value = true;
                    result = cmd.ExecuteNonQuery();
                    if (result != 1) throw new Exception("Error inserting a record.");

                    // Retrieve the ID of the newly inserted record
                    SqlCommand cmdId = conn.CreateCommand();
                    cmdId.CommandText = @"SELECT @@IDENTITY";
                    var objId = cmdId.ExecuteScalar();
                    maxId = 1;
                    if (!Convert.IsDBNull(objId)) maxId = Int32.Parse(objId.ToString());

                    // Modify the inserted record
                    SqlCommand cmdMod = conn.CreateCommand();
                    cmdMod.CommandText = "UPDATE Authentications SET Name = @newName, Password = @newPalabraPaso, " +
                                         "Category = @newCategory, IsValid = @newEsValido WHERE Id = @UserId";
                    cmdMod.Parameters.Add("@UserId", SqlDbType.Int).Value = maxId;
                    cmdMod.Parameters.Add("@newName", SqlDbType.NVarChar, 20).Value = modName;
                    cmdMod.Parameters.Add("@newPalabraPaso", SqlDbType.NVarChar, 20).Value = "WordStepMod";
                    cmdMod.Parameters.Add("@newCategory", SqlDbType.NVarChar, 20).Value = "CategoryMod";
                    cmdMod.Parameters.Add("@newEsValido", SqlDbType.Bit).Value = false;
                    result = cmdMod.ExecuteNonQuery();
                    if (result != 1) throw new Exception("Error modifying a record.");
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