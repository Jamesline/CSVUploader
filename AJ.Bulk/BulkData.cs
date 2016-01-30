using System;
using System.Data;
using System.Data.SqlClient;
using DataAccess;
using DataTable = System.Data.DataTable;

namespace AJ.Bulk
{
    public class BulkData
    {
        private string _connectionString = @"Server=tcp:vp5rg59cow.database.windows.net,1433;Database=SoliTeam;User ID=LiverAdmin@vp5rg59cow;Password=Langley14;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public bool WriteNames(DataTable dt)
        {

            DataTable tablem = ReadTable(_connectionString);
            tablem.Columns.Remove("ID");
            //tablem.Columns.RemoveAt(columnIndex);
            // Need to check first...
            var match = DtMatch(dt, tablem);
            bool status = false;
            if (match != null)
            {
                status = InsertBulkData(match);
            }

           // var status = InsertBulkData(match);
            return status;
        }

        private DataTable DtMatch(DataTable dt1, DataTable dt2)
        {
            dt1.Merge(dt2);
            //System.Data.DataTable changesTable = dt1.GetChanges();

            return dt1;
        }

        public bool InsertBulkData(DataTable peopleTable)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var oSqlCommand = new SqlCommand())
                {
                    oSqlCommand.Connection = connection;
                    oSqlCommand.CommandTimeout = 0;
                    oSqlCommand.CommandText = "InsertName";
                    oSqlCommand.CommandType = CommandType.StoredProcedure;
                    oSqlCommand.Parameters.AddWithValue("@NameTable", peopleTable);
                    connection.Open();
                    int x = Convert.ToInt32(oSqlCommand.ExecuteScalar());
                    oSqlCommand.Dispose();
                    connection.Close();
                    connection.Dispose();
                    return x > 0;
                }
            }

        }

        private DataTable ReadTable(string sqlConn)
        {
            using (SqlDataAdapter dataAdapter
                = new SqlDataAdapter("SELECT * FROM Names", sqlConn))
            {
                var table = new DataTable();
                // create the DataSet 
                
                // fill the DataSet using our DataAdapter 
                dataAdapter.Fill(table);
                return table;
            }
        }

        public DataTable DataTableFillFromCsv(string fileLocation)
        {
            DataAccess.DataTable dt = DataAccess.DataTable.New.ReadCsv(@"D:\TestData\TeamNames.csv");
            DataTable personTable = new DataTable();
            //personTable.Columns.Add("ID", typeof(int));
            personTable.Columns.Add("Name", typeof(string));
            personTable.Columns.Add("Other", typeof(string));
            int counter = 0;
            foreach (Row row in dt.Rows)
            {
                Console.WriteLine(row["Name"]);
                var prow = personTable.NewRow();
                //prow["ID"] = counter;
                prow["Name"] = row["Name"];
                prow["Other"] = "";
                counter = counter + 1;
                personTable.Rows.Add(prow);
            }
            return personTable;
        }
    }
}
