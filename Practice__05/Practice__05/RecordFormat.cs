using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice__05
{
    public enum FieldsRegistration { Id, Name, Password, Category, IsValid };

    // Define the class to describe the sequential order of the fields in the data record
    public class RecordFormat
    {
        private FieldsRegistration[] _recordFields;

        public FieldsRegistration[] RecordFields
        {
            get { return _recordFields; }
            set { _recordFields = value; }
        }

        public RecordFormat(FieldsRegistration[] recordFields)
        {
            _recordFields = recordFields;
        }
    }

}
