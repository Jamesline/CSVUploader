using System;
using System.Linq;
using AJ.Bulk;
using DataAccess;

namespace CSVUploader
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (!args.Any())
            {
                Console.WriteLine("No arguments");
            }
            try
            {
                var bulkData = new BulkData();
                System.Data.DataTable personTable = bulkData.DataTableFillFromCsv("");
                var result = bulkData.WriteNames(personTable);
                Console.WriteLine(result
                    ? "Update Suceeded!"
                        : "Update Failed!");
            }
            catch (Exception ex)
            {

                Console.WriteLine("Updated Errored!");
            }
            Console.ReadKey();
        }
    }
}