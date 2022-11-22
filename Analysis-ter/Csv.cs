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

        // for testing:
        //public void Load(string path)
        //{
        //    Load(null, new string[] { "test1,test2,test3", "1,2,3", "11,22,33", "111,222,333" });
        //}

        public void Load(string path) // for testing: (..., IEnumerable<string> tempTest)
        {
            string[] lines = File.ReadAllLines(path);
            // for testing: replace `lines` with `tempTest`
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
            if (ignoreColumns == null) return;

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

            headers.InsertRange(headers.Count, fileTwo.headers);
            columns.InsertRange(columns.Count, fileTwo.columns);
        }

        public IEnumerable<string> Serialize()
        {
            List<string> cereal = new List<string>
            {
                string.Join(",", headers)
            };

            for (int row = 0; row < columns[0].Count; row++)
            {
                cereal.Add("");
                for (int col = 0; col < columns.Count; col++)
                {
                    if (col > 0) cereal[row + 1] += ',';

                    cereal[row + 1] += columns[col][row];
                }
            }

            return cereal;
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
}// Coluin is gy
    //Cloin Alcie super mega fucking aw3eesome how we can do this shit you fucking ucnt ass mat\atrooujhp
    //so when can I go eat dinner
    //i like TOUCHING myself. type on cloins screen its just a little laggy
