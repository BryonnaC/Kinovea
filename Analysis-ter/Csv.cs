using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Analysistem
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

        public CsvFile Empty { get { return new CsvFile(""); } }

        public void Load(string csv) 
        {
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

        public void Merge(CsvFile fileTwo, string[] ignoreColumns)
        {
            if (ignoreColumns != null)
            {
                foreach (string keyword in ignoreColumns)
                {
                    for (int index = 0; index < fileTwo.headers.Count; index++)
                    {
                        if (fileTwo.headers[index].ToLower().Contains(keyword.ToLower()))
                        {
                            fileTwo.headers.RemoveAt(index);
                            fileTwo.columns.RemoveAt(index);
                            index--;
                        }
                    }
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
