using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Dapper;
using Oracle;
using Oracle.ManagedDataAccess;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Configuration;
namespace MESWebApi.DB
{
    public class OraDBHelper : IDisposable
    {
        private string connstr = "";
        public OraDBHelper()
        {
            connstr = ConfigurationManager.ConnectionStrings["tjmes"] != null ? ConfigurationManager.ConnectionStrings["tjmes"].ToString() : "";
        }

        public OraDBHelper(string conn)
        {
            connstr = ConfigurationManager.ConnectionStrings[conn] != null ? ConfigurationManager.ConnectionStrings[conn].ToString() : "";
        }

        public IDbConnection Conn
        {
            get
            {
                return new OracleConnection(connstr);
            }
        }        
        public void Dispose()
        {
        }
    }
}