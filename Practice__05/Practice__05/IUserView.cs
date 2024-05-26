using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Practice__05
{
    public interface IUserView
    {
        string Id { get; set; } //Unique primary key for each user
        string Name { get; set; } //User name
        string StepWord { get; set; } //Password to check user authentication.
        string Category { get; set; } //User Category
        bool IsValid { get; set; } //Indicates if a user is valid
    }
    /// <summary>
    ///Enumerated type with Flags to represent possible coded authentication errors that may occur together
    /// </summary>
    [FlagsAttribute]
    public enum AuthenticationCode
    {
        ///The user ID and password match what is stored in the data source
        CorrectAccess = 0,
        ///Error in accessing the data source (e.g. error in opening the Users file, connection to the database...)
        DataError = 1,
        ///The user ID exists, but it is also an invalid user (not allowed to access the system). It is cumulative with StepwordError.
        InvalidAccess = 2,
        ///The primary key (UserId) is not found in the data source
        UserIdError = 4,
        ///The password of the user found does not match the one stored in the data source
        StepWordError = 8
    }
}
