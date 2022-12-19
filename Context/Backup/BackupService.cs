using System;
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

        public bool FixSalesinvoiceTotal(string databaseName)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = @"


                    ---Transfere Salesinvoice to Safe
                    SELECT *
                    into #Temp_Salesinvoice
                    FROM SalesManagement.SalesinvoicesHeaders
                    WHERE Id  Not In
                      (SELECT HeaderId  
                      FROM SalesManagement.Safes where AccountTypeId=2 and HeaderId !=0
                      )
                      Order by Id
                    
                    
                      Insert into SalesManagement.Safes
                      (
                            Created,
                    		Modified,
                    		Date,
                    		AccountId,
                    		AccountTypeId,
                    		Outcoming,
                    		Incoming,
                    		Notes,
                    		IsHidden,
                    		IsTransfered,
                    		HeaderId,
                    		OrderId)
                     
                     SELECT GETDATE(),GETDATE(), SalesinvoicesDate, SellerId, 2,Total,0,('رقم الكشف :' +CONVERT(varchar(500),Id)),1,1,Id,0
                    FROM #Temp_Salesinvoice
                    
                    drop table #Temp_Salesinvoice

                                    ---Fix SalesinvoiceTotal
                                    Select 
                                    SalesHeaders.Id,
                                    (SalesHeaders.Total) as Total,
                                    SalesHeaders.Created,
                                    Safes.Outcoming,
                                    SalesHeaders.SellerId,
                                    Seller.Name,
                                    Sum( ceiling((salesDetials.Price * salesDetials.Weight)+salesDetials.Byaa+salesDetials.Mashal)) as DetailsTotal
                                    into #Temp_Sales
                                    
                                    from  SalesManagement.SalesinvoicesHeaders SalesHeaders
                                    Inner Join SalesManagement.SalesinvoicesDetials salesDetials on salesDetials.SalesinvoicesHeaderId=SalesHeaders.Id
                                    Inner Join SalesManagement.Safes Safes on Safes.HeaderId=SalesHeaders.Id and Safes.AccountTypeId=2
                                    Inner Join SalesManagement.Sellers Seller on Seller.Id=SalesHeaders.SellerId 
                                    
                                    Group by SalesHeaders.Id,
                                    SalesHeaders.Total,
                                    SalesHeaders.Created,
                                    Safes.Outcoming,
                                    SalesHeaders.SellerId,
                                    Seller.Name
                                    
                                    
                                    Select * from #Temp_Sales
                                    where Total !=DetailsTotal
                                    
                                    
                                    UPDATE SalesManagement.SalesinvoicesHeaders
                                    SET Total = t.DetailsTotal
                                    FROM #Temp_Sales t
                                    JOIN SalesManagement.SalesinvoicesHeaders salesHeader
                                    ON t.Id = salesHeader.Id
                                    
                                    drop table #Temp_Sales


                                    ---Fix Safes Totals
                                    Select 
                                    SalesHeaders.Id,
                                    (SalesHeaders.Total) as Total,
                                    SalesHeaders.Created,
                                    Safes.Outcoming,
                                    SalesHeaders.SellerId,
                                    Seller.Name,
                                    Sum( ceiling((salesDetials.Price * salesDetials.Weight)+salesDetials.Byaa+salesDetials.Mashal)) as DetailsTotal
                                    into #Temp_Safes
                                    
                                    from  SalesManagement.SalesinvoicesHeaders SalesHeaders
                                    Inner Join SalesManagement.SalesinvoicesDetials salesDetials on salesDetials.SalesinvoicesHeaderId=SalesHeaders.Id
                                    Inner Join SalesManagement.Safes Safes on Safes.HeaderId=SalesHeaders.Id and Safes.AccountTypeId=2
                                    Inner Join SalesManagement.Sellers Seller on Seller.Id=SalesHeaders.SellerId 
                                    
                                    Group by SalesHeaders.Id,
                                    SalesHeaders.Total,
                                    SalesHeaders.Created,
                                    Safes.Outcoming,
                                    SalesHeaders.SellerId,
                                    Seller.Name
                                    
                                    
                                    Select * from #Temp_Safes
                                    where Total !=Outcoming
                                    
                                    
                                    UPDATE SalesManagement.Safes
                                    SET Outcoming = t.Total
                                    FROM #Temp_Safes t
                                    JOIN SalesManagement.Safes Safes
                                    ON t.Id = Safes.HeaderId and Safes.AccountTypeId=2
                                    
                                    drop table #Temp_Safes
                                    
                                    
";
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

        public bool UpdateFarmersBalance()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = @"

                                 Select AccountId,
                                 Sum(Incoming)-Sum(Outcoming) Balance
                                 into #FarmerBalances
                                 from SalesManagement.Safes
                                 where AccountTypeId=1  and IsTransfered=1
                                 Group by AccountId
                                 Select * from #FarmerBalances
                                 
                                 UPDATE SalesManagement.Farmers
                                 SET Balance = b.Balance
                                 FROM #FarmerBalances b
                                 JOIN SalesManagement.Farmers farmer
                                 ON b.AccountId = farmer.Id
                                 drop table #FarmerBalances                               
                                 Select * from SalesManagement.Farmers

                                 ";
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

        public bool UpdateSellersBalance()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = @"

                                
                                    Select AccountId,
                                    Sum(Outcoming)-Sum(Incoming) Balance
                                    into #SellerBalances
                                    from SalesManagement.Safes
                                    where AccountTypeId=2
                                    Group by AccountId
                                    
                                    UPDATE SalesManagement.Sellers
                                    SET Balance = b.Balance
                                    FROM #SellerBalances b
                                    JOIN SalesManagement.Sellers seller
                                    ON b.AccountId = seller.Id
                                    drop table #SellerBalances                               
                                    Select * from SalesManagement.Sellers

                                 ";
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

    }
}
