using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MESWebApi.DBAttr
{
    public class DbFieldAttribute : Attribute
    {
        private string _colname;
        public DbFieldAttribute(string fieldname)
        {
            this._colname = fieldname;
        }

        public string FieldName
        {
            get
            {
                return this._colname;
            }
        }
    }
}