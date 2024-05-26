using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;
using Practice__05;

namespace Practice__05
{
    public class UserView : IUserView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string StepWord { get; set; }
        public string Category { get; set; }
        public bool IsValid { get; set; }

        public UserView() { }
        public UserView(string id, string name, string wordStep, string category, bool isValid)
        {
            Id = id.ToString();
            Name = name;
            StepWord = wordStep;
            Category = category;
            IsValid = isValid;
        }
        public UserView(params string[] fields)
        {
            if (fields.Length > 0) Id = fields[0];
            if (fields.Length > 1) Name = fields[1];
            if (fields.Length > 2) StepWord = fields[2];
            if (fields.Length > 3) Category = fields[3];
            if (fields.Length > 4) IsValid = (fields[4].ToUpper() == "TRUE");
        }
    }
}
