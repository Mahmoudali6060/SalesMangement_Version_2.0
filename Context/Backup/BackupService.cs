﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace Database.Backup
{
    public class BackupService
    {
        private readonly string _connectionString;
        private readonly string _backupFolderFullPath;
        private readonly string[] _systemDatabaseNames = { "master", "tempdb", "model", "msdb" };

        public BackupService(string connectionString, string backupFolderFullPath)
        {
            _connectionString = connectionString;
            _backupFolderFullPath = backupFolderFullPath;
        }

        public void BackupAllUserDatabases()
        {
            foreach (string databaseName in GetAllUserDatabases())
            {
                BackupDatabase(databaseName);
            }
        }

        public bool BackupDatabase(string databaseName)
        {
            try
            {
                string filePath = BuildBackupPathWithFilename(databaseName);

                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = String.Format("BACKUP DATABASE [{0}] TO DISK='{1}'", databaseName, filePath);
                    //>>>>>>> It works with SQL Express 2017
                    //BACKUP DATABASE SalesManagement TO  DISK = N'F:\My Projects\Sales Management\Database\SalesManagement-2020-07-18.bak' WITH INIT, NOUNLOAD, NAME = N'Sales Management',  STATS = 10,  FORMAT
                    using (var command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool RestoreDatabase(string databaseName)
        {
            try
            {
                string filePath = BuildBackupPathWithFilename(databaseName);

                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = "use master RESTORE DATABASE " + databaseName + " FROM DISK = '" + filePath + "' WITH REPLACE";
                    using (var command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private IEnumerable<string> GetAllUserDatabases()
        {
            var databases = new List<String>();

            DataTable databasesTable;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                databasesTable = connection.GetSchema("Databases");

                connection.Close();
            }

            foreach (DataRow row in databasesTable.Rows)
            {
                string databaseName = row["database_name"].ToString();

                if (_systemDatabaseNames.Contains(databaseName))
                    continue;

                databases.Add(databaseName);
            }

            return databases;
        }

        private string BuildBackupPathWithFilename(string databaseName)
        {
            string filename = string.Format("{0}-{1}.bak", databaseName, DateTime.Now.ToString("yyyy-MM-dd"));
            return Path.Combine(_backupFolderFullPath, filename);
        }
    }
}
