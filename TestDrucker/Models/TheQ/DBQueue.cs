using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TestDrucker.Models.ViewModels;

namespace TestDrucker.Models.TheQ
{
    public class DBQueue
    {
        string CS = "Data Source = 192.168.94.119; Initial Catalog = Intrax; User ID = bo_admin; Password =(BatigolC520%11%44)";
        public List<TheQueue> GetTheQ()
        {
            List<TheQueue> elements = new List<TheQueue>();
            using (SqlConnection connection = new SqlConnection(CS))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT TOP 1 * FROM PrintQueue ORDER BY ID DESC", connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TheQueue queue = new TheQueue();
                        queue.Id = Convert.ToInt32(reader["Id"]);
                        queue.PrinterName = Convert.ToString(reader["PrinterName"]);
                        queue.Filename = Convert.ToString(reader["Filename"]);
                        queue.LastStatus = Convert.ToString(reader["LastStatus"]);
                        queue.LastStatusDetails = Convert.ToString(reader["LastStatusDetails"]);
                        queue.AddedToQueue = Convert.ToDateTime(reader["AddedToQueue"]);
                        elements.Add(queue);
                    }
                    return elements;
                }
            }
        }
        public int AddId(string PrinterName, string Filename)
        {
            int rowsAffected = 0;
            string CS = "Data Source = 192.168.94.5; Initial Catalog = Intrax_Test; User ID = bo_admin; Password =(BatigolC520%11%44)";
            using (SqlConnection connection = new SqlConnection(CS))
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"INSERT INTO PrintQueue (PrinterName,Filename,LastStatus,AddedToQueue,LastStatusUpdate,LastStatusDetails) values ('{PrinterName}','{Filename}','AddedToQueue',GETDATE(),GETDATE(),null);SELECT SCOPE_IDENTITY() AS [PrintQueueId]", connection);
              
                rowsAffected = command.ExecuteNonQuery();
            }
            return rowsAffected;
        }
    }
}