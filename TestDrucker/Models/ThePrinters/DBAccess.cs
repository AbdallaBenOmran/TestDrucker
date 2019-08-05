using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TestDrucker.Models.ThePrinters;

namespace TestDrucker.Models.PrinterCanon
{
    public class DBAccess
    {
        string CS = "Data Source = 192.168.94.119; Initial Catalog = ServiceDb; User ID = bo_admin; Password =(BatigolC520%11%44)";

        // Get All Data from DeviceConfig
        public List<Printer> GetAllItems ()
        {
            List<Printer> elements = new List<Printer>();
            using (SqlConnection connection = new SqlConnection(CS))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("select *from [device].DeviceConfig", connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Printer printer = new Printer();
                        printer.Id = Convert.ToString(reader["Id"]);
                        printer.DeviceName = Convert.ToString(reader["DeviceName"]);
                        printer.DeviceIP = Convert.ToString(reader["DeviceIP"]);
                        printer.DeviceType = Convert.ToString(reader["DeviceSubtype"]);
                        printer.BranchNo = Convert.ToInt16(reader["BranchNo"]);
                        elements.Add(printer);
                    }
                    return elements;
                }
            }
        }

        // Get the specific printer based on the Filiale using SQL Param
        public List<Printer> GetAllBranchItems(string branchCode)
        {
            string[] branchIdentity = branchCode.Split('#');
            List<Printer> elements = new List<Printer>();
            using (SqlConnection connection = new SqlConnection(CS))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("select *from [device].DeviceConfig Where BranchNo = @BranchNo and BranchLocationCode = @BranchLocationCode", connection);
                command.Parameters.AddWithValue("@BranchNo", branchIdentity[0]);
                command.Parameters.AddWithValue("@BranchLocationCode", branchIdentity[1]);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Printer printer = new Printer();
                        printer.Id = Convert.ToString(reader["Id"]);
                        printer.DeviceName = Convert.ToString(reader["DeviceName"]);
                        printer.DeviceIP = Convert.ToString(reader["DeviceIP"]);
                        printer.DeviceType = Convert.ToString(reader["DeviceSubtype"]);
                        printer.BranchNo = Convert.ToInt16(reader["BranchNo"]);
                        elements.Add(printer);
                    }
                    return elements;
                }
            }
        }

        //Get the BranchNo with BranchLocationCode in same Line 
        public List<Branch> GetBranchAndLocation ()
        {
            List<Branch> elements = new List<Branch>();
            using (SqlConnection connection = new SqlConnection(CS))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT distinct [BranchNo] + '#' + [BranchLocationCode] FROM [ServiceDb].[device].[DeviceConfig] order by 1", connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var branch = new Branch();
                        branch.BranchLocationCode = reader.GetString(0);
                        elements.Add(branch);
                    }
                    return elements;
                }
            }
        }
    }
}
