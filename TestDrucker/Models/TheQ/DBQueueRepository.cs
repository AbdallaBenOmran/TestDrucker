using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TestDrucker.Interfaces;
using TestDrucker.Models.ViewModels;


namespace TestDrucker.Models.TheQ
{
    public class DBQueueRepository : IQueueRepository
    {
        string CS;
        string CSTest;
        public DBQueueRepository(string ConStr, string ConStrTest)
        {
            CS = ConStr;
            CSTest = ConStrTest;
        }
        public List<TheQueue> GetQueue()
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
        public int AddQueue(string PrinterName, string Filename)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(CSTest))
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"INSERT INTO PrintQueue (PrinterName,Filename,LastStatus,AddedToQueue,LastStatusUpdate,LastStatusDetails) values ('{PrinterName}','{Filename}','AddedToQueue',GETDATE(),GETDATE(),null);SELECT SCOPE_IDENTITY() AS [PrintQueueId]", connection);

                rowsAffected = command.ExecuteNonQuery();
            }
            return rowsAffected;
        }
    }
}