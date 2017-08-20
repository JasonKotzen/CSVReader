using System;
using System.IO;
using System.Windows.Forms;
using CSVReader.Framework;

namespace CSVReader
{
    public partial class Form1 : Form
    {
        private readonly IClientInfoFileGenerator _ClientInfoFileGenerator;

        public Form1() : this(new ClientInfoFileGenerator())
        {
            InitializeComponent();
        }

        public Form1(IClientInfoFileGenerator clientInfoFileGenerator)
        {
            _ClientInfoFileGenerator = clientInfoFileGenerator;
        }

        private void SelectFileButton_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = @"Open CSV File",
                Filter = @"CSV files|*.csv",
                InitialDirectory = @"C:\"
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                _ClientInfoFileGenerator.ProduceClientAnalysisFiles(openFileDialog.FileName);
                MessageBox.Show($@"Output files were produced and written to {Path.GetDirectoryName(openFileDialog.FileName)}");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
