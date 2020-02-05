using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCodeBase
{
    /// <summary>
    /// A DataTable that contains methods for searching through itself.
    /// </summary>
    class SearchableDataTable : DataTable
    {
        /// <summary>
        /// Finds all data rows containing a value in a column.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="value">The value to search for.</param>
        /// <returns>Returns all data rows containing the value.</returns>
        public DataRow[] FindByValue(String columnName, String value)
        {
            List<DataRow> matchingRows = new List<DataRow>();

            for (int i = 0; i < Rows.Count; i++)
            {
                if (Rows[i][columnName].Equals(value))
                {
                    matchingRows.Add(Rows[i]);
                }
            }

            return matchingRows.ToArray();
        }

        /// <summary>
        /// Finds all data rows not containing a value in a column.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="value">THe value to search for.</param>
        /// <returns>Returns all data rows not containing the value.</returns>
        public DataRow[] ExcludeByValue(String columnName, String value)
        {
            List<DataRow> matchingRows = new List<DataRow>();

            for (int i = 0; i < Rows.Count; i++)
            {
                if (!Rows[i][columnName].Equals(value))
                {
                    matchingRows.Add(Rows[i]);
                }
            }

            return matchingRows.ToArray();
        }
    }
}
