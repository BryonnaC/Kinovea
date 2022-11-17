using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CombineCSV
{
    public partial class CombineCSV : Form
    {
        public CombineCSV()
        {
            InitializeComponent();
        }

        private void combineCSV(string fileNameOne, string fileNameTwo, string fileNameDest)
        {
            var xPositions = File.ReadAllLines(fileNameOne);
            var yPositions = File.ReadAllLines(fileNameTwo);
            var result = xPositions.Zip(yPositions, (f, s) => string.Join(",", f, s));
            File.WriteAllLines(fileNameDest, result);
        }

        private bool populateTextBoxFromClipboard(TextBox textBox)
        {
            if (!Clipboard.ContainsText(TextDataFormat.Text)) return false;
            string clipboardText = Clipboard.GetText().Trim('"');

            if (!FileUtils.IsFilePathValid(clipboardText)) return false;
            if (!clipboardText.Contains(".csv")) {
                lblUserMessage.Text = "File must end in \".csv\"";
                return false;
            }


            textBox.Text = clipboardText;
            return true;
        }

        private void btnPasteXPos_Click(object sender, EventArgs e)
        {
            if(!populateTextBoxFromClipboard(txtXPos))
            {
                lblUserMessage.Text = "Clipboard must contain valid text!";
            } else {
                lblUserMessage.Text = "";
            }
        }

        private void btnPasteYPos_Click(object sender, EventArgs e)
        {
            if (!populateTextBoxFromClipboard(txtYPos))
            {
                lblUserMessage.Text = "Clipboard must contain valid text!";
            } else
            {
                lblUserMessage.Text = "";
            }
        }

        private void btnCombine_Click(object sender, EventArgs e)
        {
            // TODO: unify path/file verification code and fight the spaghetti monster

            string xPosText = txtXPos.Text.Trim('"');
            string yPosText = txtYPos.Text.Trim('"');
            if (xPosText.Length == 0) {
                lblUserMessage.Text = "X positions file path cannot be empty";
                return;
            }
            if (yPosText.Length == 0) {
                lblUserMessage.Text = "Y positions file path cannot be empty";
                return;
            }
            if (xPosText == yPosText) {
                lblUserMessage.Text = "Files cannot be the same!";
                return;
            }
            string xPosFilename = Path.GetFileName(xPosText);
            string yPosFilename = Path.GetFileName(yPosText);
            string destFilename = xPosText.Replace(".csv", "") + yPosFilename;

            combineCSV(xPosText, yPosText, destFilename);
            lblUserMessage.Text = "Success!";
        }
    }

    public static class FileUtils
    {
        public static bool IsFilePathValid(string pathName)
        {
            if (pathName.Trim() == string.Empty) return false;

            string pathname;
            string filename;
            try
            {
                pathname = Path.GetPathRoot(pathName);
                filename = Path.GetFileName(pathName);
            }
            catch (ArgumentException)
            {
                return false;
            }

            if (filename.Trim() == string.Empty) return false;
            if (!pathname.Contains("\\")) return false;
            if (pathname.IndexOfAny(Path.GetInvalidPathChars()) >= 0) return false;
            if (filename.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0) return false;

            return true;
        }
    }

}
