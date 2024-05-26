using Practice__05;
using System;
using System.Data.SqlClient;
using System.Security.Authentication;

namespace Practice__05
{
    public class AuthenticationSqlServerFile : IAuthentication
    {
        private readonly string _connectionString;

        public AuthenticationSqlServerFile(string connectionString)
        {
            _connectionString = ReplaceDirectoryPath(connectionString);
            if (!TestConnection())
            {
                throw new AuthenticationException("Error accessing database.");
            }
        }

        private bool TestConnection()
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string ReplaceDirectoryPath(string connectionString)
        {
            string dirPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            return connectionString.Replace("%DIR_APP%", dirPath);
        }

        public AuthenticationCode IsAuthenticatedUser(string id, string password)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    var query = "SELECT * FROM Authentications WHERE Id = @Id";
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", id);

                    conn.Open();
                    var reader = cmd.ExecuteReader();

                    if (!reader.HasRows)
                    {
                        return AuthenticationCode.UserIdError;
                    }

                    reader.Read();
                    var storedPassword = reader["Password"].ToString();
                    var isValid = (bool)reader["IsValid"];

                    if (!isValid)
                    {
                        return AuthenticationCode.InvalidAccess;
                    }

                    if (storedPassword != password)
                    {
                        return AuthenticationCode.StepWordError;
                    }

                    return AuthenticationCode.CorrectAccess;
                }
            }
            catch (Exception)
            {
                return AuthenticationCode.DataError;
            }
        }

        public bool ModifyUser(string id, IUserView user)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    var query = "UPDATE Authentications SET Name = @Name, Password = @Password, Category = @Category, IsValid = @IsValid WHERE Id = @Id";
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Name", user.Name);
                    cmd.Parameters.AddWithValue("@Password", user.StepWord);
                    cmd.Parameters.AddWithValue("@Category", user.Category);
                    cmd.Parameters.AddWithValue("@IsValid", user.IsValid);

                    conn.Open();
                    var result = cmd.ExecuteNonQuery();
                    return result == 1;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool InsertUser(IUserView user)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    var query = "INSERT INTO Authentications (Id, Name, Password, Category, IsValid) VALUES (@Id, @Name, @Password, @Category, @IsValid)";
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", user.Id);
                    cmd.Parameters.AddWithValue("@Name", user.Name);
                    cmd.Parameters.AddWithValue("@Password", user.StepWord);
                    cmd.Parameters.AddWithValue("@Category", user.Category);
                    cmd.Parameters.AddWithValue("@IsValid", user.IsValid);

                    conn.Open();
                    var result = cmd.ExecuteNonQuery();
                    return result == 1;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteUser(string id)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    var query = "DELETE FROM Authentications WHERE Id = @Id";
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", id);

                    conn.Open();
                    var result = cmd.ExecuteNonQuery();
                    return result == 1;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IUserView GetUser(string id)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    var query = "SELECT * FROM Authentications WHERE Id = @Id";
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", id);

                    conn.Open();
                    var reader = cmd.ExecuteReader();

                    if (!reader.HasRows)
                    {
                        return null;
                    }

                    reader.Read();
                    var user = new UserView
                    {
                        Id = reader["Id"].ToString(),
                        Name = reader["Name"].ToString(),
                        StepWord = reader["Password"].ToString(),
                        Category = reader["Category"].ToString(),
                        IsValid = (bool)reader["IsValid"]
                    };

                    return user;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void SaveData()
        {
            // Not required for database-backed implementation
        }
    }
}
