using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardfight_Vanguard_Plug_In
{
    internal class CFDBLoader
    {
        const string databaseFile = @".\plug-ins\cfvg\cfvg.db";
        static CFDBLoader? _instance;
        DataTable? _cards;
        DataTable? _arts;

        private CFDBLoader()
        {
            
        }

        public void InitializeDataset()
        {
            using (SqliteConnection sqliteConnection = new SqliteConnection())
            {

            }
        }

        public CFDBLoader Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CFDBLoader();
                }

                return _instance;
            }
        }
    }
}
