using Practice__05;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice__05
{
    public class AuthenticationTextFile : IAuthentication
    {
        private string _textFile;
        private RecordFormat _recordFormat;
        private string _endField;
        private List<UserView> _users;

        public AuthenticationTextFile(string textFile, RecordFormat recordFormat, string endField)
        {
            _textFile = textFile;
            _recordFormat = recordFormat;
            _endField = endField;
            LoadUsers();
        }

        private void LoadUsers()
        {
            _users = new List<UserView>();

            foreach (var line in File.ReadLines(_textFile))
            {
                var fields = line.Split(new[] { _endField }, StringSplitOptions.None);
                var user = new UserView(fields);
                _users.Add(user);
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
            var lines = _users.Select(u => $"{u.Id}{_endField}{u.Name}{_endField}{u.StepWord}{_endField}{u.Category}{_endField}{u.IsValid}");
            File.WriteAllLines(_textFile, lines);
        }
    }

}
