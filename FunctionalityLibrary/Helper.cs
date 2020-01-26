using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;

namespace FunctionalityLibrary
{
    public class Helper
    {
        public static SQLiteConnection ConnectToDb()
        {
            string connectionstring = ConfigurationManager.ConnectionStrings["SqliteConnectionString"].ConnectionString;
            return new SQLiteConnection(connectionstring);
            
        }

    }
}
