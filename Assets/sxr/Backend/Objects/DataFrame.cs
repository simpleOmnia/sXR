using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace sxr_internal
{

    /// <summary>
    /// Extremely basic dataframe, for more advanced options use .Net Dataframe
    /// (https://learn.microsoft.com/en-us/dotnet/api/microsoft.data.analysis.dataframe?view=ml-dotnet-preview)
    /// </summary>
    public class DataFrame {
        private static List<string[]> ListOfRows = new List<string[]>();

        public DataFrame LoadFromCSV(string path) {
            ListOfRows.Clear();
            List<string> lineList = new List<string>(File.ReadAllLines(path));
            foreach (var row in lineList)
                ListOfRows.Add(row.Split(','));
            return this; }

        public List<string[]> ToStringArrayList(bool removeHeader = false) {
            if (removeHeader)
                ListOfRows.RemoveAt(0);
            return ListOfRows; }

        public void PrintDataFrame() {
            string output = "";
            for (int row = 0; row < ListOfRows.Count; row++)
            for (int col = 0; col < ListOfRows[row].Length; col++)
                if (col < ListOfRows[row].Length - 1)
                    output += (ListOfRows[row][col] + ",");
                else
                    output += (ListOfRows[row][col] + "\n");
            Debug.Log(output); }

        public bool ColumnExists(string colName)
        { return ListOfRows[0].Contains(colName); }
        public string[] GetColumnByName(string colName) {
            int count = 0;
            foreach (var col in ListOfRows[0]) {
                if (col == colName)
                    return GetColByIndex(count);
                count++; }

            Debug.LogWarning("Could not find column " + colName);
            return new string[] { }; }

        public string[] GetRowByName(string rowName) {
            int count = 0;
            foreach (var row in ListOfRows) {
                if (row[0] == rowName)
                    return row;
                count++; }

            Debug.LogWarning("Did not find row with name \"" + rowName + "\"");
            return new string[] { }; }

        public string[] GetColByIndex(int colIndex) {
            List<string> returnString = new List<string>();
            int rowCount = 0;
            foreach (var row in ListOfRows) {
                if (row.Length > colIndex - 1)
                    returnString.Add(row[colIndex]);
                else
                    Debug.LogWarning("Did not find col with index " + colIndex + " for row: " + rowCount);
                rowCount++; }

            return returnString.ToArray(); }
    }
}
