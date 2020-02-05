using SuperCodeBase.strings;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;

namespace SuperCodeBase.io
{
    /// <summary>
    /// FileMaster contains various methods regarding files and directories.
    /// </summary>
    static class FileMaster
    {
        #region misschien bad practice methode
        /// <summary>
        /// Deze methode probeert een bestand te schrijven.
        /// Als het bestand al bestaat en open staat, wordt een MessageBox met feedback,
        /// Zolang het bestand nog niet is geschreven blijft deze methode proberen te schrijven.
        /// </summary>
        /// <param name="contentToWrite">De StringBuilder met de content die je wilt schrijven.</param>
        /// <param name="filePath">Het volledige pad van het bestand.</param>
        public static void ForceWrite(StringBuilder contentToWrite, string filePath)
        {
            bool isDone = false;

            while (!isDone)
            {
                StreamWriter sw = null;

                try
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }

                    sw = new StreamWriter(filePath, false);
                    sw.Write(contentToWrite);
                    isDone = true;
                }

                catch (FileNotFoundException ex)
                {
                    MessageBox.Show(filePath + " kan niet worden gevonden. Schrijven geannuleerd.");
                    return;
                }

                catch (IOException ex)
                {
                    MessageBox.Show(filePath + " staat nog open. Sluit het bestand en klik op OK.");
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Er is een onbekende fout opgetreden. \n\n" + ex.StackTrace);
                    return;
                }

                finally
                {
                    if (sw != null)
                    {
                        sw.Close();
                    }
                }
            }

        }
        #endregion

        /// <summary>
        /// Opens an OpenFileDialog and returns the selected path.
        /// </summary>
        /// <returns>Returns the selected path or String.Empty if the user canceled.</returns>
        public static string OpenFileDialog()
        {
            return OpenFileDialog("");
        }

        /// <summary>
        /// Opens an OpenFileDialog and returns the selected path.
        /// </summary>
        /// <param name="title">The title that shows in the dialog.</param>
        /// <returns>Returns the selected path or String.Empty if the user canceled.</returns>
        public static string OpenFileDialog(string initialDirectory, string title = "OpenFileDialog")
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = title;
            ofd.InitialDirectory = initialDirectory;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                return ofd.FileName;
            }

            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Tries to read a file and convert it to a DataTable. 
        /// When the file cannot be opened a dialog with feedback appears.
        /// </summary>
        /// <param name="filePath">The path to the file to be read.</param>
        /// <returns>Returns a DataTable with content from the read file.</returns>
        public static System.Data.DataTable ForceRead(string filePath)
        {
            if (Path.GetExtension(filePath) == ".xls")
            {
                return DataTableFromXls(filePath);
            }

            if (Path.GetExtension(filePath) == ".xlsx")
            {
                return DataTableFromXlsx(filePath);
            }



            return null;
        }

        /// <summary>
        /// Reads a file and converts the data to a DataTable.
        /// Supports [.csv | .txt | .xls | .xlsx | .xml].
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <returns>The DataTable with the data from the file.</returns>
        public static System.Data.DataTable GetDataTable(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return null; //stop method here.
                }

                System.Data.DataTable dtTemp = new System.Data.DataTable();

                switch (Path.GetExtension(filePath))
                {
                    case ".csv":
                        dtTemp = DataTableFromCsv(filePath);
                        return dtTemp;
                    case ".txt":
                        dtTemp = DataTableFromTxt(filePath);
                        return dtTemp;
                    case ".xls":
                        dtTemp = DataTableFromXls(filePath);
                        return dtTemp;
                    case ".xlsx":
                        dtTemp = DataTableFromXlsx(filePath);
                        return dtTemp;
                    case ".xml":
                        dtTemp = DataTableFromXml(filePath);
                        return dtTemp;
                }

            }
            catch (IOException ex)
            {
                //System.Windows.Forms.MessageBox.Show("File is al in gebruik!\nSluit de programma's af die de file nu gebruiken om te importeren.");
            }
            return null;

        }

        /// <summary>
        /// Reads an .xls file and turns it into a DataTable.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <returns>Returns a DataTable with content from the file.</returns>
        private static System.Data.DataTable DataTableFromXls(string filePath)
        {
            var connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", filePath);

            var adapter = new System.Data.OleDb.OleDbDataAdapter("SELECT * FROM [Blad1$]", connectionString);
            var ds = new DataSet();

            adapter.Fill(ds, "anyNameHere");

            System.Data.DataTable data = ds.Tables["anyNameHere"];

            return data;
        }

        /// <summary>
        /// Reads an .xlsx file and turns it into a DataTable.
        /// Will not work if environment has Excel 2003 or lower.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <returns>Returns a DataTable with content from the file.</returns>
        private static System.Data.DataTable DataTableFromXlsx(string filePath)
        {
            var connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}; Extended Properties=Excel 12.0;", filePath);

            var adapter = new System.Data.OleDb.OleDbDataAdapter("SELECT * FROM [workSheetNameHere$]", connectionString);
            var ds = new DataSet();
            System.Data.DataTable data = null;

            try
            {
                adapter.Fill(ds, "anyNameHere");
                data = ds.Tables["anyNameHere"];
            }

            catch (Exception ex)
            {
                MessageBox.Show(".xlsx benodigt Excel 2007 of recenter op de huidige omgeving.");
            }


            return data;
        }

        /// <summary>
        /// Reads an .xml file and turns it into a DataTable.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <returns>Returns a DataTable with content from the file.</returns>
        private static System.Data.DataTable DataTableFromXml(string filePath)
        {
            DataSet data = new DataSet();
            data.ReadXml(filePath);
            return data.Tables[0];
        }

        /// <summary>
        /// Reads a .txt file and turns it into a DataTable.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <returns>Returns a DataTable with content from the file.</returns>
        private static System.Data.DataTable DataTableFromTxt(string filePath)
        {
            System.Data.DataTable dtTemp = new System.Data.DataTable();
            StreamReader sr = new StreamReader(filePath);

            string line = sr.ReadLine();
            char separator = StringMaster.FindSeparator(line);

            string[] columnNames = line.Split(separator);
            foreach (string columnName in columnNames)
            {
                if (dtTemp.Columns.Contains(columnName))
                {
                    dtTemp.Columns.Add(columnName + " - DUBBEL");
                }

                else
                {
                    dtTemp.Columns.Add(columnName);
                }

            }
            // ------- hier werden de kolommen aan de datatable toegevoegd

            while (!sr.EndOfStream)
            {
                dtTemp.Rows.Add(sr.ReadLine().Split(separator));
            }
            sr.Close();
            return dtTemp;
        }

        /// <summary>
        /// Reads a .csv file and turns it into a DataTable.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <returns>Returns a DataTable with content from the file.</returns>
        private static System.Data.DataTable DataTableFromCsv(string filePath)
        {
            System.Data.DataTable dtTemp = new System.Data.DataTable();
            StreamReader sr = new StreamReader(filePath);

            string line = sr.ReadLine();
            char separator = StringMaster.FindSeparator(line);

            string[] columnNames = line.Split(separator);
            foreach (string columnName in columnNames)
            {
                if (dtTemp.Columns.Contains(columnName))
                {
                    dtTemp.Columns.Add(columnName + " - DUBBEL");
                }

                else
                {
                    dtTemp.Columns.Add(columnName);
                }

            }
            // ------- hier werden de kolommen aan de datatable toegevoegd

            while (!sr.EndOfStream)
            {
                dtTemp.Rows.Add(sr.ReadLine().Split(separator));
            }
            sr.Close();
            return dtTemp;
        }

        /// <summary>
        /// Checks if a file is in use by a process.
        /// </summary>
        /// <param name="filePath">The path to file to be checked.</param>
        /// <returns>Returns true if the file is in use.</returns>
        public static bool IsFileInUse(string filePath)
        {
            FileStream stream = null;

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }

            FileInfo file = new FileInfo(filePath);

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }

            catch (IOException)
            {
                return true;
            }

            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

            return false;
        }

        /// <summary>
        /// Opens a folder browser dialog and returns the selected folder afterwards.
        /// </summary>
        /// <returns>Returns the selected folder.</returns>
        public static string OpenFolderDialog()
        {
            return OpenFolderDialog("");
        }

        /// <summary>
        /// Opens a folder browser dialog and returns the selected folder afterwards.
        /// </summary>
        /// <param name="title">The title to be displayed in the dialog.</param>
        /// <returns>Returns the selected folder.</returns>
        public static string OpenFolderDialog(string title, bool showNewFolderButton = false)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = showNewFolderButton;
            fbd.Description = title;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                return fbd.SelectedPath;
            }

            else
            {
                return "";
            }
        }

        /// <summary>
        /// Finds the most recent file in a directory.
        /// </summary>
        /// <param name="inDirectory">The directory to search in.</param>
        /// <param name="pattern">Optional. Search with a pattern.</param>
        /// <returns>Returns the path to the most recent file.</returns>
        public static string FindMostRecentFile(string inDirectory, string pattern = "*.*")
        {
            if (inDirectory.Trim().Length == 0)
                return string.Empty; //Error handler can go here

            if ((pattern.Trim().Length == 0) || (pattern.Substring(pattern.Length - 1) == "."))
                return string.Empty; //Error handler can go here

            if (Directory.GetFiles(inDirectory, pattern).Length == 0)
                return string.Empty; //Error handler can go here

            //string pattern = "*.txt"

            var dirInfo = new DirectoryInfo(inDirectory);
            var file = (from f in dirInfo.GetFiles(pattern) orderby f.LastWriteTime descending select f).First();

            return file.ToString();
        }

        /// <summary>
        /// Turns a DataTable into a .csv file.
        /// </summary>
        /// <param name="content">The DataTable with content to be placed in the csv file.</param>
        /// <param name="fileName">The name of the new file.</param>
        /// <param name="filePath">The path to the new folder, excluding filename and last '/'.</param>
        /// <param name="separator">The separator to be used in the csv file.</param>
        public static void DataTableToCsv(System.Data.DataTable content, char separator = ';', string filePath = "", string fileName = "test")
        {
            StringBuilder sb = new StringBuilder();

            foreach (DataColumn column in content.Columns)
            {
                sb.Append(column.ColumnName + separator);
            }

            sb.Remove(sb.Length - 1, 1);
            sb.Append("\n");

            for (int rowIndex = 0; content.Rows.Count > rowIndex; rowIndex++)
            {
                for (int columnIndex = 0; content.Columns.Count > columnIndex; columnIndex++)
                {
                    sb.Append(content.Rows[rowIndex][columnIndex].ToString() + separator);
                }

                sb.Remove(sb.Length - 1, 1);
                sb.Append("\n");
            }

            sb.Remove(sb.Length - 1, 1);

            using (StreamWriter writer = new StreamWriter(filePath + "//" + fileName + ".csv"))
            {
                writer.Write(sb);
            }
        }

        /// <summary>
        /// Turns a DataTable into a .txt file.
        /// </summary>
        /// <param name="content">The DataTable with content to be placed in the txt file.</param>
        /// <param name="separator">The separator to be used in the csv file.</param>
        /// <param name="filePath">The path to the new folder, excluding filename and last '/'.</param>
        /// <param name="fileName">The name of the new file.</param>
        public static void DataTableToTxt(System.Data.DataTable content, char separator = ';', string filePath = "", string fileName = "test")
        {
            StringBuilder sb = new StringBuilder();

            foreach (DataColumn column in content.Columns)
            {
                sb.Append(column.ColumnName + separator);
            }

            sb.Remove(sb.Length - 1, 1);
            sb.Append("\n");

            for (int rowIndex = 0; content.Rows.Count > rowIndex; rowIndex++)
            {
                for (int columnIndex = 0; content.Columns.Count > columnIndex; columnIndex++)
                {
                    sb.Append(content.Rows[rowIndex][columnIndex].ToString() + separator);
                }

                sb.Remove(sb.Length - 1, 1);
                sb.Append("\n");
            }

            sb.Remove(sb.Length - 1, 1);

            using (StreamWriter writer = new StreamWriter(filePath + "//" + fileName + ".txt"))
            {
                writer.Write(sb);
            }
        }

        /// <summary>
        /// Turns a DataTable into a .xml file.
        /// </summary>
        /// <param name="content">The DataTable with content to be placed in the xml file.</param>
        /// <param name="filePath">The path to the new folder, excluding filename and last '/'.</param>
        /// <param name="fileName">The name of the new file.</param>
        public static void DataTableToXml(System.Data.DataTable content, string filePath = "", string fileName = "test", string rootNode = "List", string nodeName = "Node")
        {
            StringBuilder sb = new StringBuilder();

            string _rootNode = rootNode;
            string _nodeName = nodeName;

            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.AppendLine("<" + _rootNode + ">");

            for (int rowIndex = 0; content.Rows.Count > rowIndex; rowIndex++)
            {
                sb.AppendLine("\t<" + _nodeName + ">");

                for (int columnIndex = 0; content.Columns.Count > columnIndex; columnIndex++)
                {
                    string subNodeName = content.Columns[columnIndex].ColumnName;
                    subNodeName = subNodeName.Replace(" ", "_");
                    string value = content.Rows[rowIndex][columnIndex].ToString();
                    //value = StringMaster.RemoveNonAscii(value);
                    string line = "\t\t<" + subNodeName + ">";
                    line += value;
                    line += "</" + subNodeName + ">";
                    sb.AppendLine(line);
                }

                sb.AppendLine("\t</" + _nodeName + ">");
                //sb.AppendLine("\t<" + content.Columns[columnIndex].ColumnName + ">");

                //for (int rowIndex = 0; content.Rows.Count > rowIndex; rowIndex++)
                //{
                //    sb.AppendLine("\t\t<"+content.Rows[rowIndex].ItemArray[columnIndex].ToString() + ">");
                //}

                //sb.AppendLine("\t</" + content.Columns[columnIndex].ColumnName + ">");
            }

            sb.AppendLine("</" + _rootNode + ">");

            using (StreamWriter writer = new StreamWriter(filePath + "//" + fileName + ".xml"))
            {
                writer.Write(sb);
            }
        }

        /// <summary>
        /// Turns a DataTable into a .xls file.
        /// </summary>
        /// <param name="content">The DataTable with content to be placed in the xls file.</param>
        /// <param name="filePath">The path to the new folder, excluding filename and last '/'.</param>
        /// <param name="fileName">The name of the new file.</param>
        public static void DataTableToXls(System.Data.DataTable content, string filePath = "", string fileName = "test")
        {
            Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            if (xlApp == null)
            {
                MessageBox.Show("Excel staat niet op huidige omgeving.");
                return;
            }

            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlApp.DisplayAlerts = false;
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            for (int columnIndex = 0; content.Columns.Count > columnIndex; columnIndex++)
            {
                xlWorkSheet.Cells[1, columnIndex + 1] = content.Columns[columnIndex].ColumnName;
            }

            for (int rowIndex = 0; content.Rows.Count > rowIndex; rowIndex++)
            {
                for (int columnIndex = 0; content.Columns.Count > columnIndex; columnIndex++)
                {
                    xlWorkSheet.Cells[rowIndex + 2, columnIndex + 1] = content.Rows[rowIndex][columnIndex].ToString();
                }
            }

            //xlWorkSheet.Cells[1, 1] = "Sheet 1 content";

            xlWorkBook.SaveAs(filePath + "\\" + fileName + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            ReleaseObject(xlWorkSheet);
            ReleaseObject(xlWorkBook);
            ReleaseObject(xlApp);
        }

        /// <summary>
        /// Turns a DataTable into a .xlsx file.
        /// </summary>
        /// <param name="content">The DataTable with content to be placed in the xlsx file.</param>
        /// <param name="filePath">The path to the new folder, excluding filename and last '/'.</param>
        /// <param name="fileName">The name of the new file.</param>
        public static void DataTableToXlsx(System.Data.DataTable content, string filePath = "", string fileName = "test")
        {
            Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            if (xlApp == null)
            {
                MessageBox.Show("Excel staat niet op huidige omgeving.");
                return;
            }

            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlApp.DisplayAlerts = false;
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            for (int columnIndex = 0; content.Columns.Count > columnIndex; columnIndex++)
            {
                xlWorkSheet.Cells[1, columnIndex + 1] = content.Columns[columnIndex].ColumnName;
            }

            for (int rowIndex = 0; content.Rows.Count > rowIndex; rowIndex++)
            {
                for (int columnIndex = 0; content.Columns.Count > columnIndex; columnIndex++)
                {
                    xlWorkSheet.Cells[rowIndex + 2, columnIndex + 1] = content.Rows[rowIndex][columnIndex].ToString();
                }
            }

            //xlWorkSheet.Cells[1, 1] = "Sheet 1 content";

            xlWorkBook.SaveAs(filePath + "\\" + fileName + ".xlsx", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            ReleaseObject(xlWorkSheet);
            ReleaseObject(xlWorkBook);
            ReleaseObject(xlApp);
        }

        /// <summary>
        /// Releases an object that might have not been picked up by garbage collection.
        /// </summary>
        /// <param name="obj">The object to release.</param>
        private static void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception occurred while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        /// <summary>
        /// Retrieves all files in a directory.
        /// </summary>
        /// <param name="directory">The directory to retrieve files from.</param>
        /// <param name="searchSubFolders">If you also want to search through the subfolders.</param>
        /// <returns>Returns all files in a directory.</returns>
        public static string[] GetFilesFrom(string directory, bool searchSubFolders = false)
        {
            System.IO.SearchOption so = searchSubFolders ? System.IO.SearchOption.AllDirectories : System.IO.SearchOption.TopDirectoryOnly;
            string[] files = Directory.GetFiles(directory, "*.*", so);
            return files;
        }

        /// <summary>
        /// Checks if a path is a directory.
        /// </summary>
        /// <param name="path">The path to be checked.</param>
        /// <returns>Returns true if the path is not a directory.</returns>
        public static bool IsPathDirectory(string path)
        {
            FileAttributes attr = File.GetAttributes(path);

            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        /// <summary>
        /// Saves an object with all the properties including data.
        /// The target file will be a .xml file.
        /// Set the getters and setters of the properties in your class like: "public string name { set; get; }".
        /// This way the properties will be able to get saved.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="objectToSave">The object to be saved.</param>
        /// <param name="path">The path the object will be saved to.</param>
        public static void SaveObject<T>(T objectToSave, string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            path = Path.GetFileNameWithoutExtension(path);
            path += ".xml";
            using (StreamWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, objectToSave);
            }
        }

        /// <summary>
        /// Loads an object from file and returns the object.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="objectToRead">The object to be loaded to.</param>
        /// <param name="path">The path the object will be saved to.</param>
        /// <returns>Returns an object with their properties filled.</returns>
        public static T LoadObject<T>(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            path = Path.GetFileNameWithoutExtension(path);
            path += ".xml";
            T t;
            using (StreamReader reader = new StreamReader(path))
            {
                t = (T)serializer.Deserialize(reader);
            }
            return t;
        }

        /// <summary>
        /// Reads a Word document and places the words into a StringBuilder.
        /// </summary>
        /// <param name="filePath">The path to the document.</param>
        /// <returns>Returns a StringBuilder with the content of the document.</returns>
        public static StringBuilder ReadWordDocument(string filePath)
        {
            if (Path.GetExtension(filePath) == ".doc" ||
                Path.GetExtension(filePath) == ".docx")
            {
                Microsoft.Office.Interop.Word.Application a = new Microsoft.Office.Interop.Word.Application();
                Word.Document doc = a.Documents.Open(filePath);

                int wordCount = doc.Words.Count;

                StringBuilder content = new StringBuilder();

                for (int i = 1; i <= wordCount; i++)
                {
                    string text = doc.Words[i].Text;
                }

                a.Quit();

                return content;
            }

            else
            {
                Console.WriteLine(filePath + " does not have a valid Word extension.");
                return null;
            }
        }

        /// <summary>
        /// Creates a Word document with plain text.
        /// </summary>
        /// <param name="filePath">The path to the file to be created.</param>
        /// <param name="contentToWrite">The content to be added to the document.</param>
        public static void WriteWordDocument(string filePath, StringBuilder contentToWrite)
        {
            if (Path.GetExtension(filePath) == ".doc" ||
                Path.GetExtension(filePath) == ".docx")
            {
                Word.Application a = new Word.Application();

                a.Visible = false;

                object missing = System.Reflection.Missing.Value;

                Word.Document doc = a.Documents.Add(ref missing, ref missing, ref missing, ref missing);

                WordManager wm = new WordManager();

                wm.AddParagraph(doc, "Lorem ipsum");
                doc.Content.Text += contentToWrite.ToString();
                wm.AddHeader(doc, "headertext");
                wm.AddFooter(doc, "footertext");
                string imagePath = @"C:\Users\gebruiker\Desktop\PrijsManager.png";
                //wm.AddFooterImage(doc, imagePath);

                var start = doc.Content.Start;
                var end = doc.Content.End;

                doc.Range(start, end).Font.Name = "Calibri";
                doc.Range(start, end).Font.Size = 11;

                doc.SaveAs(filePath);
                doc.Close(ref missing, ref missing, ref missing);
                doc = null;
                a.Quit(ref missing, ref missing, ref missing);
                a = null;
                Console.WriteLine(filePath + " is successfully created.");
            }
        }

        private class WordManager
        {
            private object missing = System.Reflection.Missing.Value;

            public Word.Document AddHeader(Word.Document document, string headerText)
            {
                foreach (Word.Section section in document.Sections)
                {
                    Word.Range headerRange = section.Headers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                    headerRange.Fields.Add(headerRange, Word.WdFieldType.wdFieldPage);
                    headerRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    headerRange.Font.ColorIndex = Word.WdColorIndex.wdBlue;
                    headerRange.Font.Size = 10;
                    headerRange.Text = headerText;
                }

                return document;
            }

            public Word.Document AddFooter(Word.Document document, string footerText)
            {
                foreach (Word.Section section in document.Sections)
                {
                    Word.Range footerRange = section.Footers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                    footerRange.Font.ColorIndex = Word.WdColorIndex.wdDarkRed;
                    footerRange.Font.Size = 10;
                    footerRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    footerRange.Text = footerText;
                }

                return document;
            }

            public Word.Document AddFooterImage(Word.Document document, string imagePath)
            {
                object tr = true;
                object fa = false;

                foreach (Word.Section section in document.Sections)
                {
                    Word.Range footerRange = section.Footers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                    Word.InlineShape picture = document.InlineShapes.AddPicture(imagePath, tr, fa, footerRange);
                }

                return document;
            }

            public Word.Document AddParagraph(Word.Document document, string paragraphText)
            {
                Word.Paragraph p1 = document.Content.Paragraphs.Add(missing);

                p1.Range.set_Style(Word.WdBuiltinStyle.wdStyleHeading1);
                p1.Range.Text = paragraphText;
                p1.Range.Font.Bold = 0;
                p1.Range.InsertParagraphAfter();

                return document;
            }
        }
    }
}
