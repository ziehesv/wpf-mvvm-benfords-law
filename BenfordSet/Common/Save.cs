﻿using BenfordSet.Model;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using ceTe.DynamicPDF;
using ceTe.DynamicPDF.Text;
using ceTe.DynamicPDF.Forms;
using ceTe.DynamicPDF.PageElements;


namespace BenfordSet.Common
{
    internal class Save
    {
        private readonly string _initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        private const string _allowedFiles = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

        public event EventHandler FileHasBeenSaved;
        public event EventHandler FileHasNotBeenSaved;

        public bool IsText { get; set; }
        public string Destination { get; set; } = string.Empty;
        public string? OutputResult { get; set; }

        public Save(string outputresults, bool istext)
        {
            OutputResult = outputresults;
            IsText = istext;

        }


        public void OpenSaveDialog()
        {
            SaveFileDialog saveFileDialog = new();
            saveFileDialog.InitialDirectory = _initialDirectory;
            saveFileDialog.Filter = _allowedFiles;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                Destination = saveFileDialog.FileName;
        }

        public void SaveFile()
        {
            if (IsText && CheckDestination())
            {
                SaveAsText();
                this.FileHasBeenSaved?.Invoke(this, new EventArgs());
            }

            else if (!IsText && CheckDestination())
            {
                SaveAsPdf();
                this.FileHasBeenSaved?.Invoke(this, new EventArgs());
            }
            else
                this.FileHasNotBeenSaved?.Invoke(this, new EventArgs());

        }

        private bool CheckDestination() => !string.IsNullOrEmpty(Destination);

        private void SaveAsPdf()
        {
            Document doc = new Document();
            Page page = new Page(PageSize.A4, PageOrientation.Landscape);
            doc.Pages.Add(page);

            ceTe.DynamicPDF.PageElements.Label label = new
            ceTe.DynamicPDF.PageElements.Label(OutputResult, 0, 0, 504, 100, Font.Helvetica, 10, TextAlign.Left);

            page.Elements.Add(label);
            doc.Draw(Destination);
        }

        private void SaveAsText()
        {
            using StreamWriter fs = new(Destination);
            fs.Write(OutputResult);
        }
    }
}
