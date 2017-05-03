using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Xps.Packaging;
using Microsoft.Office.Interop.Word;
using Microsoft.Win32;
using Word = Microsoft.Office.Interop.Word;

namespace Degree_Work
{
    /// <summary>
    /// Логика взаимодействия для ReferenceWindow.xaml
    /// </summary>
    public partial class ReferenceWindow : System.Windows.Window
    {
        public ReferenceWindow(System.Windows.Window from)
        {
            InitializeComponent();
            docViewer.Zoom = 125;
            TipsList.Items.Add("Общие сведения");
            TipsList.Items.Add("Обтекание полуплоскости");
            TipsList.Items.Add("Обтекание полосы -π..π");
            TipsList.Items.Add("Обтекание единичной\nокружности");
            TipsList.Items.Add("Настройка визуальных\nпараметров");
            TipsList.Items.Add("Сохранение полученных\nрезультатов");
            TipsList.SelectionChanged += TipsList_SelectionChanged;
            if (from is HalfPlane)
            {
                TipsList.SelectedIndex = 1;
            }
            else if (from is ZoneWindow)
            {
                TipsList.SelectedIndex = 2;
            }
            else if (from is CircleWindow)
            {
                TipsList.SelectedIndex = 3;
            }
            else
            {
                TipsList.SelectedIndex = 0;
            }
        }

        private void TipsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string wordDocument = string.Empty;
            switch (TipsList.SelectedIndex)
            {
                case 0:
                    wordDocument = Directory.GetCurrentDirectory().ToString().Replace("bin\\Debug", string.Empty) + "Resources\\Documents\\GeneralInfo.docx";
                    break;
                case 1:
                    wordDocument = Directory.GetCurrentDirectory().ToString().Replace("bin\\Debug", string.Empty) + "Resources\\Documents\\HalfPlaneInfo.docx";
                    break;
                case 2:
                    wordDocument = Directory.GetCurrentDirectory().ToString().Replace("bin\\Debug", string.Empty) + "Resources\\Documents\\ZoneInfo.docx";
                    break;
                case 3:
                    wordDocument = Directory.GetCurrentDirectory().ToString().Replace("bin\\Debug", string.Empty) + "Resources\\Documents\\CircleInfo.docx";
                    break;
                case 4:
                    wordDocument = Directory.GetCurrentDirectory().ToString().Replace("bin\\Debug", string.Empty) + "Resources\\Documents\\SettingsInfo.docx";
                    break;
                case 5:
                    wordDocument = Directory.GetCurrentDirectory().ToString().Replace("bin\\Debug", string.Empty) + "Resources\\Documents\\SaveInfo.docx";
                    break;
            }
            if (string.IsNullOrEmpty(wordDocument) || !File.Exists(wordDocument))
            {
                MessageBox.Show("Файл документации отсутствует.");
            }
            else
            {
                string convertedXpsDoc = string.Concat(System.IO.Path.GetTempPath(), "\\", Guid.NewGuid().ToString(), ".xps");
                XpsDocument xpsDocument = ConvertWordToXps(wordDocument, convertedXpsDoc);
                if (xpsDocument == null)
                {
                    return;
                }
                docViewer.Document = xpsDocument.GetFixedDocumentSequence();
            }
        }

        private void ico_MouseEnter(object sender, MouseEventArgs e)
        {
            exitImage.Source = Settings.exitIcoSelectedSource;
        }

        private void ico_MouseLeave(object sender, MouseEventArgs e)
        {
            exitImage.Source = Settings.exitIcoSource;
        }

        private void ico_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        /// <summary> 
        ///  Convert the word document to xps document 
        /// </summary> 
        /// <param name="wordFilename">Word document Path</param> 
        /// <param name="xpsFilename">Xps document Path</param> 
        /// <returns></returns> 
        private XpsDocument ConvertWordToXps(string wordFilename, string xpsFilename)
        {
            // Create a WordApplication and host word document 
            Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            try
            {
                wordApp.Documents.Open(wordFilename);
                // To Invisible the word document 
                wordApp.Application.Visible = false;
                // Minimize the opened word document 
                wordApp.WindowState = WdWindowState.wdWindowStateMinimize;
                Document doc = wordApp.ActiveDocument;
                doc.SaveAs(xpsFilename, WdSaveFormat.wdFormatXPS);
                XpsDocument xpsDocument = new XpsDocument(xpsFilename, FileAccess.Read);
                return xpsDocument;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurs, The error message is  " + ex.ToString());
                return null;
            }
            finally
            {
                wordApp.Documents.Close();
                ((_Application)wordApp).Quit(WdSaveOptions.wdDoNotSaveChanges);
            }
        }
    }
}
