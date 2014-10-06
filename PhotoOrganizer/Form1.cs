using com.drew.imaging.jpg;
using com.drew.imaging.tiff;
using com.drew.metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PhotoOrganizer
{
    public partial class frmMain : Form
    {
        private const string _logFileAddr = @"Logfile.txt";
        private string _sourceFolder, _destFolder, _action;
        private StringBuilder log;

        public frmMain()
        {
            InitializeComponent();
            CompareButton.Click += new EventHandler(CompareClick);
            Message.Text = string.Empty;
            txtSource.Text = @"C:\Users\Whit\Pictures\IN";
            txtDest.Text = @"C:\Users\Whit\Pictures\OUT";
            log = new StringBuilder();

        }

        private void HandleButtonClick(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtSource.Text) || string.IsNullOrEmpty(txtDest.Text))
            {
                Message.Text = "Please enter valid source and destination folders.";
                return;
            }

            _sourceFolder = txtSource.Text;
            _destFolder = txtDest.Text;
            txtSource.Enabled = false;
            txtDest.Enabled = false;
            string message = (_action == "compare") ? "Comparing folders" : "Organizing Photos";
            Message.Text = string.Format("{0}, please wait...", message);

            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += new DoWorkEventHandler(DoWork); //(txtSource.Text, txtDest.Text));
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Completed);

            try
            {
                worker.RunWorkerAsync();
            }
            catch(Exception ex)
            {
                Message.Text = "ERROR: " + ex.Message;
                txtSource.Enabled = true;
                txtDest.Enabled = true;
            }
        }

        private void CompareClick(object sender, EventArgs e)
        {
            _action = "compare";
            HandleButtonClick(sender, e);
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            _action = "organize";
            HandleButtonClick(sender, e);
        }

        private void DoWork(object sender, DoWorkEventArgs args)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;

            if(_action == "compare")
            {
                CompareFolders(_sourceFolder, _destFolder);
            }
            else
            {
                OrganizeFolder(_sourceFolder, _destFolder);
            }
            using (StreamWriter w = File.AppendText(_destFolder + "\\" + _logFileAddr))
            {
                w.WriteLine(log.ToString());
                w.Flush();
                w.Close();
            }
            
        }
        private void Completed(object sender, RunWorkerCompletedEventArgs args)
        {
            if (args.Error != null && !string.IsNullOrEmpty(args.Error.Message))
            {
                Message.Text = "ERROR: " + args.Error.Message;
            }
            else
            {
                Message.Text = "Completed Successfully!";
            }

            txtDest.Enabled = true;
            txtSource.Enabled = true;
        }

        private void OrganizeFolder(string sourceFolder, string destFolder)
        {
            List<string> files = Directory.EnumerateFiles(sourceFolder, "*.*", SearchOption.AllDirectories).ToList();
            List<string> subdirs = Directory.GetDirectories(sourceFolder).ToList();
            List<string> ignored = new List<string>();
            //Image img;
            //PropertyItem prop = null;

            FileInfo fi = null;
            Metadata data = null;
            bool isImage = false;
            DateTime date = DateTime.MinValue;
            string dateTaken = string.Empty, dateDigitized = string.Empty,
                dateModified = string.Empty, dateCreated = string.Empty;


            foreach (string fileName in files)
            {
                try
                {
                    fi = new FileInfo(fileName);

                    if (IsFileType(fileName, "JPEG"))
                    {
                        /*
                        img = Image.FromFile(fileName);
                        try
                        {
                            if (img.PropertyIdList.Contains(int.Parse("9003", System.Globalization.NumberStyles.AllowHexSpecifier)))
                            {
                                prop = img.GetPropertyItem(int.Parse("9003", System.Globalization.NumberStyles.AllowHexSpecifier));
                            }
                            else if (img.PropertyIdList.Contains(int.Parse("9004", System.Globalization.NumberStyles.AllowHexSpecifier)))
                            {
                                prop = img.GetPropertyItem(int.Parse("9004", System.Globalization.NumberStyles.AllowHexSpecifier));
                            }
                            else if (img.PropertyIdList.Contains(int.Parse("0132", System.Globalization.NumberStyles.AllowHexSpecifier)))
                            {
                                prop = img.GetPropertyItem(int.Parse("0132", System.Globalization.NumberStyles.AllowHexSpecifier));
                            }
                            else if (File.GetLastWriteTime(fileName) != DateTime.MinValue)
                            {
                                strDate = File.GetLastWriteTime(fileName).ToString("yyyyMMdd");
                            }

                            img.Dispose();

                            if (prop != null)
                            {
                                byte[] asciiBytes = prop.Value;
                                strDate = System.Text.Encoding.ASCII.GetString(asciiBytes);
                                strDate = strDate.Substring(0, strDate.IndexOf(" ")).Replace(":", "");
                            }

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        finally
                        {
                            if (img != null)
                            {
                                img.Dispose();
                            }
                        }
                        */
                        try
                        {

                            data = JpegMetadataReader.ReadMetadata(fi);
                            isImage = true;
                        }
                        catch (Exception ex)
                        {
                            log.AppendLine("Error reading " + fileName + ": " + ex.Message);
                        }

                    }
                    else if (IsFileType(fileName, "RAW"))
                    {
                        try
                        {
                            //handle Raws
                            data = TiffMetadataReader.ReadMetadata(fi);
                            isImage = true;
                        }
                        catch (Exception ex)
                        {
                            log.AppendLine("Error reading " + fileName + ": " + ex.Message);
                        }
                    }

                    if (data != null)
                    {
                        // Get date taken
                        foreach (AbstractDirectory lcDirectory in data)
                        {
                            string strDate;
                            // We look for potential error
                            if (lcDirectory.HasError)
                            {
                                Debug.WriteLine("Some errors were found, activate trace using /d:TRACE option with the compiler");
                            }

                            foreach (Tag t in lcDirectory.Where(x => x.GetTagName().ToUpper().Contains("DATE")))
                            {
                                strDate = t.GetDescription();
                                if (strDate.Contains(" "))
                                {
                                    strDate = strDate.Substring(0, strDate.IndexOf(" "));
                                    if (!string.IsNullOrEmpty(strDate))
                                        strDate = strDate.Replace(":", "\\");
                                }

                                if (t.GetTagName().ToUpper().Contains("ORIGINAL"))
                                {
                                    dateTaken = strDate;
                                }
                                if (t.GetTagName().ToUpper().Contains("DIGITIZED"))
                                {
                                    dateDigitized = strDate;
                                }
                            }
                        }
                        data = null;
                    }

                    if (isImage)
                    {
                        DateTime.TryParse(dateTaken, out date);
                        if (DateTime.MinValue == date)
                            DateTime.TryParseExact(dateTaken.Replace("\\", string.Empty), "yyyyMMdd", CultureInfo.InvariantCulture,
                           DateTimeStyles.None, out date);
                        if (DateTime.MinValue == date)
                            DateTime.TryParse(dateDigitized, out date);
                        if (DateTime.MinValue == date)
                            DateTime.TryParseExact(dateDigitized.Replace("\\", string.Empty), "yyyyMMdd", CultureInfo.InvariantCulture,
                           DateTimeStyles.None, out date);
                        if (DateTime.MinValue == date)
                            date = (fi.CreationTime < fi.LastWriteTime) ? fi.CreationTime : fi.LastWriteTime;

                        string s = CopyPicTo(fileName, date, destFolder);
                        if (!string.IsNullOrEmpty(s))
                            log.AppendLine(s);
                    }
                    else
                    {
                        ignored.Add(fileName);
                    }

                    isImage = false;

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            foreach (string dir in subdirs)
            {
                OrganizeFolder(dir, destFolder);
            }

            if (ignored.Count > 0)
            {
                log.AppendLine("Ignored files in :" + sourceFolder);
                foreach (string s in ignored)
                {
                    log.AppendLine("   - " + s);
                }
            }
        }
        private void CompareFolders(string sourceFolder, string targetFolder)
        {
            List<string> sourcefiles = Directory.EnumerateFiles(sourceFolder, "*.*", SearchOption.AllDirectories).ToList();
            List<string> targetfiles = Directory.EnumerateFiles(targetFolder, "*.*", SearchOption.AllDirectories).ToList();
            List<string> sourcesubdirs = Directory.GetDirectories(sourceFolder).ToList();
            List<string> targetsubdirs = Directory.GetDirectories(targetFolder).ToList();
            List<string> ignored = new List<string>();

            foreach (string fileName in sourcefiles)
            {
                if (!targetfiles.Contains(fileName.Replace(sourceFolder, targetFolder)))
                    ignored.Add(fileName);
            }

            //foreach (string dir in sourcesubdirs)
            //{
            //    if(!targetsubdirs.Contains(dir.Replace(sourceFolder, targetFolder)))
            //        log.AppendLine("Missing subdirectory: " + dir);

            //    CompareFolders(dir, targetsubdirs.Where(x => x.Contains(dir)).FirstOrDefault());
            //}

            if (ignored.Count > 0)
            {
                log.AppendLine("Missing files in :" + sourceFolder);
                foreach (string s in ignored)
                {
                    log.AppendLine("   - " + s);
                }
            }
        }

        private bool IsFileType(string fileName, string type)
        {
            if (type == "JPEG")
            {
                return fileName.ToUpper().EndsWith("JPG") ||
                    fileName.ToUpper().EndsWith("JPEG");
            }
            if (type == "RAW")
            {
                return fileName.ToUpper().EndsWith("CR2") ||
                    fileName.ToUpper().EndsWith("CRW") ||
                    fileName.ToUpper().EndsWith("RAW");
            }
            if (type == "VID")
            {
                return fileName.ToUpper().EndsWith("AVI") ||
                    fileName.ToUpper().EndsWith("MOV") ||
                    fileName.ToUpper().EndsWith("OGG") ||
                    fileName.ToUpper().EndsWith("MPG") ||
                    fileName.ToUpper().EndsWith("MPEG") ||
                    fileName.ToUpper().EndsWith("M4V");
            }
            return false;
        }

        private string CopyPicTo(string fileName, DateTime date, string destFolder)
        {
            string imgName = fileName.Substring(fileName.LastIndexOf(@"\") + 1);
            StringBuilder errors = new StringBuilder();
            string sourceFolder;
            if (DateTime.MinValue == date)
            {
                sourceFolder = "Unknown";
            }
            else
            {
                sourceFolder = date.Year.ToString() + @"\" + date.ToString("MM") + @"\" + date.ToString("yyyyMMdd");
            }
            sourceFolder = destFolder + @"\" + sourceFolder;
            bool failure = false;
            string imgPath = sourceFolder + @"\" + imgName;

            if (!Directory.Exists(sourceFolder))
            {
                try
                {
                    Directory.CreateDirectory(sourceFolder);
                }
                catch (Exception ex)
                {
                    failure = true;
                    errors.AppendLine("Could not create directory " + sourceFolder + " - " + ex.Message);
                }
            }

            if (!failure)
            {

                if (File.Exists(imgPath))
                {
                    if (SameFile(fileName, imgPath))
                    {
                        errors.AppendLine("File already exists:" + imgPath);
                    }
                    else
                    {
                        errors.AppendLine("Different file with same name exists:" + imgPath);
                        int ext = imgPath.LastIndexOf(".");

                        if(ext > 0)
                        {
                            imgPath = imgPath.Insert(ext, "_D");
                            if (copyCheck.Checked)
                                File.Copy(fileName, imgPath);
                            else
                                File.Move(fileName, imgPath);
                        }

                    }
                }
                else
                {
                    try
                    {
                        if (copyCheck.Checked)
                            File.Copy(fileName, imgPath);
                        else
                            File.Move(fileName, imgPath);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error copying file " + imgName + " - " + ex.Message);
                    }
                }
            }

            return errors.ToString();
        }

        private bool SameFile(string fileName, string imgPath)
        {
            FileInfo source, target;

            source = new FileInfo(fileName);
            target = new FileInfo(imgPath);

            return source.Length == target.Length;
        }

        private void btnBrowseSource_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                txtSource.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnBrowseDest_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog2.ShowDialog();

            if (result == DialogResult.OK)
            {
                txtDest.Text = folderBrowserDialog2.SelectedPath;
            }
        }
    }
}