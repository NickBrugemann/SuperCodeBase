using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
namespace SuperCodeBase.io
{
    static class FileMaster_v2
    {
        /// <summary>
        /// Creates a data table from a file.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <returns></returns>
        public static DataTable GetDataTableFrom(string path)
        {
            string extension = Path.GetExtension(path);

            switch (extension)
            {
                case ".csv":
                    return DataTableFromCsv(path);

                case ".txt":
                    return DataTableFromCsv(path);

                case ".xls":
                    throw new NotSupportedException(".xls not supported yet");

                case ".xlsx":
                    throw new NotSupportedException(".xlsx not supported yet");

                case ".odt":
                    throw new NotSupportedException(".odt not supported yet");

                default:
                    throw new NotSupportedException(extension + " not supported now or in the future");
            }
        }

        /// <summary>
        /// Creates a data table from a csv file.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <returns></returns>
        private static DataTable DataTableFromCsv(string path)
        {
            string pathOnly = Path.GetDirectoryName(path);
            string fileName = Path.GetFileName(path);

            string query = @"SELECT * FROM [" + fileName + "]";

            using (OleDbConnection connection = new OleDbConnection(
                      @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathOnly +
                      ";Extended Properties=\"Text;HDR=" + true + "\""))
            using (OleDbCommand command = new OleDbCommand(query, connection))
            using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
            {
                DataTable dataTable = new DataTable();
                dataTable.Locale = CultureInfo.CurrentCulture;
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        /// <summary>
        /// Creates a csv file from a data table.
        /// </summary>
        /// <param name="dataTable">The data table to be written to a file.</param>
        /// <param name="separator">The character which the values will be separated with.</param>
        /// <param name="path">The full path the file will be written to.</param>
        public static void DataTableToCsv(DataTable dataTable, string separator, string path)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = dataTable.Columns
                                           .Cast<DataColumn>()
                                           .Select(column => column.ColumnName);
            sb.AppendLine(String.Join(separator, columnNames));

            foreach (DataRow row in dataTable.Rows)
            {
                IEnumerable<string> cells = row.ItemArray.Select(cell => cell.ToString().CsvEscape());
                sb.AppendLine(string.Join(separator, cells));
            }

            string fullPath = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + ".csv";
            File.WriteAllText(fullPath, sb.ToString());
        }

        /// <summary>
        /// Escapes a string if it would be invalid for a csv.
        /// </summary>
        /// <param name="value">The value to escape</param>
        /// <returns></returns>
        private static string CsvEscape(this string value)
        {
            if (value.Contains(","))
            {
                return "\"" + value.Replace("\"", "\"\"") + "\"";
            }
            return value;
        }

        /// <summary>
        /// Returns all files that match with given search pattern.
        /// Asterisk [*] functions as wildcard.
        /// </summary>
        /// <param name="directory">The directory to search in.</param>
        /// <param name="fileNameSearchPattern">The search pattern for file name.</param>
        /// <param name="extensionSearchPattern">The search pattern for extension.</param>
        /// <param name="searchSubdirectories">True if subdirectories need to be searched.</param>
        /// <returns></returns>
        public static string[] FindFiles(string directory, 
                                         string fileNameSearchPattern = "*", 
                                         string extensionSearchPattern = "*",
                                         bool searchSubdirectories = false)
        {
            SearchOption searchOption = searchSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            return Directory.GetFiles(directory, fileNameSearchPattern + "." + extensionSearchPattern, searchOption);
        }
    }
}
