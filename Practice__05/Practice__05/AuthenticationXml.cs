using Practice__05;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Practice__05
{
    public class AuthenticationXml : IAuthentication
    {
        private string _xmlFile;
        private List<UserView> _users;

        public AuthenticationXml(string xmlFile)
        {
            _xmlFile = xmlFile;
            LoadUsers();
        }

        private void LoadUsers()
        {
            _users = new List<UserView>();

            if (File.Exists(_xmlFile))
            {
                var doc = XDocument.Load(_xmlFile);
                foreach (var element in doc.Descendants("User"))
                {
                    var user = new UserView
                    {
                        Id = element.Element("Id").Value,
                        Name = element.Element("Name").Value,
                        StepWord = element.Element("StepWord").Value,
                        Category = element.Element("Category").Value,
                        IsValid = bool.Parse(element.Element("IsValid").Value)
                    };
                    _users.Add(user); 
                }
            }
        }

        public AuthenticationCode IsAuthenticatedUser(string id, string password) 
        {
            var user = _users.FirstOrDefault(u => u.Id == id);

            if (user == null)
                return AuthenticationCode.UserIdError;

            if (!user.IsValid)
                return AuthenticationCode.InvalidAccess;

            if (user.StepWord != password)
                return AuthenticationCode.StepWordError;

            return AuthenticationCode.CorrectAccess;
        }

        public bool ModifyUser(string id, IUserView user)
        {
            var existingUser = _users.FirstOrDefault(u => u.Id == id);
            if (existingUser == null) return false;

            existingUser.Name = user.Name;
            existingUser.StepWord = user.StepWord;
            existingUser.Category = user.Category;
            existingUser.IsValid = user.IsValid;
            SaveData();
            return true;
        }

        public bool InsertUser(IUserView user)
        {
            if (_users.Any(u => u.Id == user.Id)) return false;

            var newUser = new UserView(user.Id, user.Name, user.StepWord, user.Category, user.IsValid);
            _users.Add(newUser);
            SaveData();
            return true;
        }

        public bool DeleteUser(string id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null) return false;

            _users.Remove(user);
            SaveData();
            return true;
        }

        public IUserView GetUser(string id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public void SaveData()
        {
            var doc = new XDocument(
                new XElement("Users",
                    _users.Select(u =>
                        new XElement("User",
                            new XElement("Id", u.Id),
                            new XElement("Name", u.Name),
                            new XElement("StepWord", u.StepWord),
                            new XElement("Category", u.Category),
                            new XElement("IsValid", u.IsValid)
                        )
                    )
                )
            );
            doc.Save(_xmlFile);
        }
    }

}
