using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MESWebApi.DBAttr
{
    public class DbFieldAttribute : Attribute
    {
        private string _colname;
        private string _collabel;
        public DbFieldAttribute(string fieldname, string label)
        {
            this._colname = fieldname;
            this._collabel = label;
        }

        public string FieldName
        {
            get
            {
                return this._colname;
            }
        }

        public string Label
        {
            get
            {
                return this._collabel;
            }
        }
    }
}