using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Analysistem
{
    public struct CsvFile
    {
        public List<string> headers;
        public List<List<string>> columns;

        public CsvFile(string path)
        {
            string[] lines = File.ReadAllLines(path);

            string[][] cells = lines.Select((line) => line.Split(',')).ToArray();

            columns = new List<List<string>>(new List<string>[cells[0].Length]);
            headers = cells[0].ToList();

            for (int row = 1; row < cells.Length; row++)
            {
                List<string> column = new List<string>();
                for (int col = 0; col < cells[row].Length; col++)
                {
                    if (columns[col] == null)
                    {
                        columns[col] = new List<string>();
                    }
                    columns[col].Add(cells[row][col]);
                }
            }
        }

        public void Test(IEnumerable<string> scerela)
        {
            string[][] cells = scerela.Select((line) => line.Split(',')).ToArray();

            columns = new List<List<string>>(new List<string>[cells[0].Length]);
            headers = cells[0].ToList();

            for (int row = 1; row < cells.Length; row++)
            {
                List<string> column = new List<string>();
                for (int col = 0; col < cells[row].Length; col++)
                {
                    if (columns[col] == null)
                    {
                        columns[col] = new List<string>();
                    }
                    columns[col].Add(cells[row][col]);
                }
            }
        }

        public IEnumerable<string> Serialize()
        {
            List<string> cereal = new List<string>();
            cereal.Add(string.Join(",", headers));
            for (int col = 0; col < columns[0].Count; col++)
            {
                cereal.Add("");
                for (int row = 1; row < columns.Count; row++)
                {
                    cereal[row] += "," + columns[col][row];
                }
            }

            return cereal;
        }
        // 
    } // Coluin is gy
      //Cloin Alcie super mega fucking aw3eesome how we can do this shit you fucking ucnt ass mat\atrooujhp
      //so when can I go eat dinner
      //i like WATCHING myself type on cloins screen its just a little laggy


    static class CsvCombiner
    {
        /// <summary>
        /// Method <c>CombineCSVFiles</c> places the contents of two .csv files side-by-side in a new .csv file
        /// </summary>
        /// <param name="fileNameOne"></param>
        /// <param name="fileNameTwo"></param>
        /// <param name="fileNameDest"></param>
        public static CsvFile CombineCSVFiles(string fileNameOne, string fileNameTwo, string fileNameDest, string[] ignoreColumns)
        {
            CsvFile fileOne = new CsvFile(fileNameOne);
            CsvFile fileTwo = new CsvFile(fileNameTwo);
   
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

            fileOne.headers.InsertRange(fileOne.headers.Count, fileTwo.headers);
            fileOne.columns.InsertRange(fileOne.columns.Count, fileTwo.columns);

            return fileOne;
        }
    }
}
