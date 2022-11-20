using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CombineCSV
{
    public struct CSV
    {
        public List<string> Lines;
    }

    public struct CSVLine
    {
        public List<string> Fields;
    }

    /// <summary>
    /// Class <c>CombineCSV</c> contains fuctionality related to combining and synchronizing .csv files from motion analysis and force plate data
    /// </summary>
    public partial class CombineCSV : Form
    {
        public CombineCSV()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Method <c>CombineCSVFiles</c> places the contents of two .csv files side-by-side in a new .csv file
        /// </summary>
        /// <param name="fileNameOne"></param>
        /// <param name="fileNameTwo"></param>
        /// <param name="fileNameDest"></param>
        private void CombineCSVFiles(string fileNameOne, string fileNameTwo, string fileNameDest)
        {
            string[] xPositions = File.ReadAllLines(fileNameOne);
            string[] yPositions = File.ReadAllLines(fileNameTwo);
            IEnumerable<string> result = xPositions.Zip(yPositions, (f, s) => string.Join(",", f, s));
            File.WriteAllLines(fileNameDest, result);
        }

        private bool PopulateTextBoxFromClipboard(TextBox textBox)
        {
            if (!Clipboard.ContainsText(TextDataFormat.Text)) return false;
            string clipboardText = Clipboard.GetText().Trim('"');

            FileUtils.PathInfo clipboardPath = new FileUtils.PathInfo(clipboardText);

            if (!FileUtils.IsFilePathValid(clipboardPath)) return false;
            if (!clipboardText.Contains(".csv"))
            {
                lblUserMessage.Text = "File must end in \".csv\"";
                return false;
            }


            textBox.Text = clipboardText;
            return true;
        }

        private void btnPasteXPos_Click(object sender, EventArgs e)
        {
            if (!PopulateTextBoxFromClipboard(txtXPos))
            {
                lblUserMessage.Text = "Clipboard must contain valid text!";
            }
            else
            {
                lblUserMessage.Text = "";
            }
        }

        private void btnPasteYPos_Click(object sender, EventArgs e)
        {
            if (!PopulateTextBoxFromClipboard(txtYPos))
            {
                lblUserMessage.Text = "Clipboard must contain valid text!";
            }
            else
            {
                lblUserMessage.Text = "";
            }
        }

        private void btnCombine_Click(object sender, EventArgs e)
        {
            // TODO: unify path/file verification code and fight the spaghetti monster

            string xPosText = txtXPos.Text.Trim('"');
            string yPosText = txtYPos.Text.Trim('"');
            if (xPosText.Length == 0)
            {
                lblUserMessage.Text = "X positions file path cannot be empty";
                return;
            }
            if (yPosText.Length == 0)
            {
                lblUserMessage.Text = "Y positions file path cannot be empty";
                return;
            }
            if (xPosText == yPosText)
            {
                lblUserMessage.Text = "Files cannot be the same!";
                return;
            }
            string xPosFilename = Path.GetFileName(xPosText);
            string yPosFilename = Path.GetFileName(yPosText);
            string destFilename = xPosText.Replace(".csv", "") + yPosFilename;

            CombineCSVFiles(xPosText, yPosText, destFilename);
            lblUserMessage.Text = "Success!";
        }
    }

    public static class FileUtils
    {
        public struct PathInfo
        {
            public string PathString;
            public string PathName;
            public string FileName;

            public PathInfo(string path)
            {
                PathString= path;
                PathName = "";
                FileName = "";

                if (path.Trim() == string.Empty) return;
                
                string pathName = "";
                string fileName = "";
                try
                {
                    pathName = Path.GetPathRoot(path);
                    fileName = Path.GetFileName(path);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.StackTrace);
                }

                PathName = pathName;
                FileName = fileName;
            }
        }

        public static bool IsFilePathValid(PathInfo path)
        {
            if (path.FileName.Trim() == string.Empty) return false;
            if (!path.PathName.Contains("\\")) return false;
            if (path.PathName.IndexOfAny(Path.GetInvalidPathChars()) >= 0) return false;
            if (path.FileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0) return false;

            return true;
        }

        public static bool IsFileCSV(PathInfo path)
        {
            if (!path.FileName.EndsWith(".csv")) return false;

            return true;
        }
    }

}
