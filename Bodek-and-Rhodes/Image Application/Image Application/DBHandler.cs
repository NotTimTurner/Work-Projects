using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Image_Application
{
    public class DBHandler
    {
        public static string SrvName = @"192.168.1.84"; //Your SQL Server Name
        public static string DbName = @"WMS";//Your Database Name
        public static string UsrName = "bodek";//Your SQL Server User Name
        public static string Pasword = "xxxx";//Your SQL Server Password

        /// <summary>
        /// Public static method to access connection string throw out the project 
        /// </summary>
        /// <returns>return database connection string</returns>
        public static string GetConnectionString()
        {
            return "Data Source=" + SrvName + "; initial catalog=" + DbName + "; user id="
            + UsrName + "; password=" + Pasword + ";";//Build Connection String and Return
        }
    }
}