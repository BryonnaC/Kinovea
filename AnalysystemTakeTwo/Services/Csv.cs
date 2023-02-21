using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System;

namespace AnalysystemTakeTwo
{
    public struct CsvFile
    {
        public List<string> headers;
        public List<List<string>> columns;

        public CsvFile(string path)
        {
            headers = new List<string>();
            columns = new List<List<string>>();

            Load(path);
        }

        public CsvFile(string pathOne, string pathTwo, string[] ignoreColumns = null)
        {
            headers = new List<string>();
            columns = new List<List<string>>();

            Load(pathOne);
            Merge(pathTwo, ignoreColumns);
        }

        public void AppendColumn(CsvFile from, int index)
        {
            headers.Add(from.headers[index]);
            columns.Add(new List<string>(from.columns[index]));
        }

        public static CsvFile Empty { get { return new CsvFile(""); } }

        public void Load(string csv)
        {
            if (csv == "") return;
            string[] lines = csv.EndsWith(".csv") ? File.ReadAllLines(csv) : csv.Split('\n');
            string[][] cells = lines.Select((line) => line.Split(',')).ToArray();

            columns = new List<List<string>>();
            headers = cells[0].ToList();

            for (int row = 1; row < cells.Length; row++)
            {
                for (int col = 0; col < cells[row].Length; col++)
                {
                    if (columns.Count == col) columns.Add(new List<string>());

                    columns[col].Add(cells[row][col]);
                }
            }
        }

        public void Merge(string pathTwo, string[] ignoreColumns = null)
        {
            CsvFile fileTwo = new CsvFile(pathTwo);
            Merge(fileTwo, ignoreColumns);
        }

        public void Merge(CsvFile fileTwo, string[] ignoreColumns = null)
        {
            if (ignoreColumns != null)
            {
                foreach (string keyword in ignoreColumns)
                {
                    int index = GetColumnIndex(keyword);
                    fileTwo.headers.RemoveAt(index);
                    fileTwo.columns.RemoveAt(index);
                }
            }

            headers.InsertRange(headers.Count, fileTwo.headers);
            columns.InsertRange(columns.Count, fileTwo.columns);
        }

        public string Serialize()
        {
            List<string> serialized = new List<string>
            {
                string.Join(",", headers)
            };

            for (int row = 0; row < columns[0].Count; row++)
            {
                serialized.Add("");
                for (int col = 0; col < columns.Count; col++)
                {
                    if (col > 0) serialized[row + 1] += ',';
                    
                    serialized[row + 1] += columns[col][row];
                }
            }

            return string.Join("\n", serialized);
        }

        public void AppendColumn(CsvFile fileTwo, string header)
        {
            int index = fileTwo.GetColumnIndex(header);
            headers.Add(fileTwo.headers[index]);
            columns.Add(new List<string>(fileTwo.columns[index]));
        }

        public string[] GetColumn(string header)
        {
            int index = GetColumnIndex(header);
            if (index != -1)
            {
                List<string> headerColumn = new List<string>() { headers[index] };
                headerColumn = headerColumn.Concat(columns[index]).ToList();
                return headerColumn.ToArray();
            }
            return null;
        }

        public int GetColumnIndex(string header)
        {
            for (int index = 0; index < headers.Count; index++)
            {
                if (headers[index].ToLower().Contains(header.ToLower()))
                {
                    return index;
                }
            }
            return -1;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            int index = 0;
            foreach (List<string> column in columns)
            {
                sb.Append(headers[index++] + ": ");
                foreach (string s in column)
                {
                    sb.Append(s + " ");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
