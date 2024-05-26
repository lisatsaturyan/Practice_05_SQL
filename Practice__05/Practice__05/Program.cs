using System;
using System.Data.SqlClient;
using System.Security.Authentication;
using Practice__05;


class Program
{
    static void Main()
    {
        // Connection string with placeholder
        string connectionString = @"data source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=%DIR_APP%\AuthenticationDB.mdf;Database=AuthenticationDB;Integrated Security=True";

        // Replace %DIR_APP% with the actual directory path
        connectionString = ReplaceDirectoryPath(connectionString);

        // Create an instance of AuthenticationSqlServerFile
        AuthenticationSqlServerFile authService;
        try
        {
            authService = new AuthenticationSqlServerFile(connectionString);
        }
        catch (AuthenticationException ex)
        {
            Console.WriteLine("Error initializing authentication service: " + ex.Message);
            return;
        }

        // Get user input
        Console.Write("Enter User ID: ");
        var userId = Console.ReadLine();

        Console.Write("Enter Password: ");
        var password = ReadPassword();
        Console.WriteLine(); // To move to the next line after password input

        // Authenticate user
        var authCode = authService.IsAuthenticatedUser(userId, password);
        if (authCode == AuthenticationCode.CorrectAccess)
        {
            var user = authService.GetUser(userId);
            Console.WriteLine($"Welcome, {user.Name}!");
        }
        else
        {
            Console.WriteLine($"Authentication failed: {authCode}");
        }
    }

    static string ReplaceDirectoryPath(string connectionString)
    {
        string dirPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        return connectionString.Replace("%DIR_APP%", dirPath);
    }

    static string ReadPassword()
    {
        var password = string.Empty;
        while (true)
        {
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Enter) break;
            password += key.KeyChar;
        }
        return password;
    }
}
