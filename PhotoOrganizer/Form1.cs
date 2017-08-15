using MetadataExtractor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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


            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += new DoWorkEventHandler(DoWork); //(txtSource.Text, txtDest.Text));
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Completed);


            string message = string.Empty;

            switch (_action)
            {
                case "compare":
                    message = "Comparing folders";
                    break;
                case "organize":
                    message = "Organizing Photos";
                    break;
                case "videos":
                    message = "Processing Videos";
                    break;
                default:
                    break;
            }

            Message.Text = string.Format("{0}, please wait...", message);

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

        private void CompareVidsButton_Click(object sender, EventArgs e)
        {
            _action = "videos";
            HandleButtonClick(sender, e);
        }

        private void SortByType_Click(object sender, EventArgs e)
        {
            _action = "type";
            HandleButtonClick(sender, e);

        }

        private void DoWork(object sender, DoWorkEventArgs args)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;

            string logPath = _destFolder + "\\" + _logFileAddr;

            if (!System.IO.Directory.Exists(_destFolder))
                System.IO.Directory.CreateDirectory(_destFolder);

            if (!File.Exists(logPath))
                File.Create(logPath);

            switch (_action)
            {
                case "compare":
                    CompareFolders(_sourceFolder, _destFolder);
                    break;
                case "organize":
                    OrganizeFolder(_sourceFolder, _destFolder);
                    break;
                case "videos":
                    CompareVideos(_sourceFolder, _destFolder);
                    break;
                case "type":
                    OrganizeType(_sourceFolder, _destFolder);
                    break;
                default:
                    break;
            }

            Debug.WriteLine(log.ToString());

            //using (StreamWriter w = File.AppendText(logPath))
            //{
            //    w.WriteLine(log.ToString());
            //    w.Flush();
            //    w.Close();
            //}
            
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

        private void OrganizeType(string sourceFolder, string destFolder)
        {
            List<string> files = null;
            try
            {
                files = System.IO.Directory.GetFiles(sourceFolder, "*.*", SearchOption.TopDirectoryOnly).ToList();
            }
            catch(Exception ex)
            {
                log.AppendLine(string.Format("ERROR: Can't get all files for directory {0}: {1}", sourceFolder, ex.Message));
                return;
            }
            List<string> subdirs = System.IO.Directory.GetDirectories(sourceFolder).ToList();
            string extensionFolder = string.Empty;
            foreach (string fileName in files)
            {
                if(!string.IsNullOrEmpty(Path.GetExtension(fileName)))
                {
                    if (!System.IO.Directory.Exists(destFolder))
                        System.IO.Directory.CreateDirectory(destFolder);

                    if (destFolder.EndsWith("\\"))
                        destFolder = destFolder.Substring(0, destFolder.Length - 1);

                    extensionFolder = destFolder + "\\" + Path.GetExtension(fileName).ToUpper().Replace(".", "") + "\\";
                    if (!System.IO.Directory.Exists(extensionFolder))
                        System.IO.Directory.CreateDirectory(extensionFolder);

                    try
                    {
                        if (copyCheck.Checked)
                            File.Copy(fileName, extensionFolder + Path.GetFileName(fileName));
                        else
                            File.Move(fileName, extensionFolder + Path.GetFileName(fileName));
                    }
                    catch(Exception ex)
                    {
                        log.AppendLine(string.Format("ERROR: copying file {0}: {1}", Path.GetFileName(fileName), ex.Message));
                    }
                }
            }

            foreach (string dir in subdirs)
            {
                OrganizeType(dir, destFolder);
            }
        }

        private void OrganizeFolder(string sourceFolder, string destFolder)
        {
            List<string> files = System.IO.Directory.EnumerateFiles(sourceFolder, "*.*", SearchOption.AllDirectories).ToList();
            List<string> subdirs = System.IO.Directory.GetDirectories(sourceFolder).ToList();
            List<string> ignored = new List<string>();
            //Image img;
            //PropertyItem prop = null;

            FileInfo fi = null;
            //Metadata data = null;
            bool isVideo = false;
            DateTime date = DateTime.MinValue;
            DateTime dateOriginal = DateTime.MinValue, dateDigitized = DateTime.MinValue,
                dateFile = DateTime.MinValue,
                dateModified = DateTime.MinValue, dateCreated = DateTime.MinValue;
            
            string copyPath;    
            foreach (string fileName in files)
            {
                if(string.IsNullOrEmpty(Path.GetExtension(fileName)))
                {
                    ignored.Add(fileName);
                    continue;
                }

                isVideo = false;
                copyPath = string.Empty;
                try
                {

                    string strDate;
                    if ("MOV|MP4|M4V|MPG|MPEG|AVI|3GP".Contains(Path.GetExtension(fileName).Replace(".", "").ToUpper()))
                    {
                        isVideo = true;
                    }
                    else
                    {
                        Dictionary<string, string> properties = GetMetadata(fileName);

                        if(properties == null || !properties.Any())
                        {
                            ignored.Add(fileName);
                            continue;
                        }
                        foreach (var t in properties.Where(x => x.Key.ToUpper().Contains("DATE")))
                        {
                            strDate = t.Value;
                            // video //DateTime.TryParseExact(strDate, "ddd MMM dd HH:mm:ss zzz yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
                            //jpg // DateTime.TryParseExact(t.Value, "yyyy:MM:dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

                            if (t.Key.ToUpper().Contains("FILE"))
                            {
                                DateTime.TryParseExact(strDate, "ddd MMM dd HH:mm:ss zzz yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateFile);
                            }
                            if (t.Key.ToUpper().Contains("ORIGINAL"))
                            {
                                DateTime.TryParseExact(strDate, "yyyy:MM:dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOriginal);
                            }
                            if (t.Key.ToUpper().Contains("DIGITIZED"))
                            {
                                DateTime.TryParseExact(strDate, "yyyy:MM:dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateDigitized);
                            }
                        }

                        fi = new FileInfo(fileName);

                        if (dateOriginal != DateTime.MinValue)
                            date = dateOriginal;
                        else if (dateDigitized != DateTime.MinValue)
                            date = dateDigitized;
                        else if (dateFile != DateTime.MinValue)
                            date = dateFile;
                        else
                            date = (fi.CreationTime < fi.LastWriteTime) ? fi.CreationTime : fi.LastWriteTime;
                    }

                    string s = CopyPicTo(fileName, date, destFolder, isVideo);
                    if (!string.IsNullOrEmpty(s))
                        log.AppendLine(s);

                }
                catch (Exception ex)
                {
                    log.AppendLine("ERROR: " + ex);
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

        private Dictionary<string, string> GetMetadata(string fileName)
        {
            IReadOnlyList<MetadataExtractor.Directory> directories = null;
            try
            {
                directories = ImageMetadataReader.ReadMetadata(fileName);
            }
            catch
            {
                return null;
            }
            Dictionary<string, string> properties = new Dictionary<string, string>();
            foreach (var directory in directories)
            {
                foreach (var tag in directory.Tags)
                {
                    if (!properties.ContainsKey(tag.Name))
                        properties.Add(tag.Name, tag.Description);
                    else
                        properties[tag.Name] = tag.Description;
                }

                if (directory.HasError)
                {
                    foreach (var error in directory.Errors)
                        Debug.WriteLine($"ERROR: {error}");
                }
            }
            return properties;
        }
        class VidFile : IEquatable<VidFile>
        {
            public long Duration
            {
                get;
                set;
            }

            public DateTime? Created
            {
                get;
                set;
            }

            public DateTime? FileDate
            {
                get;
                set;
            }

            public long FileSize
            {
                get;
                set;
            }
            
            public string Path
            {
                get;
                set;
            }
            
            public bool IsValid
            {
                get
                {
                    return (Duration > 0 || FileSize > 0) && (Created.HasValue || FileDate.HasValue);
                }
            }

            public bool Equals(VidFile other)
            {
                //vids are equal if their durations or filesize match and either the file date or created date are within 12 hours of each other 
                // (to account for screwiness around timezones)
                return (this.Duration == other.Duration || this.FileSize == other.FileSize) && 
                    (
                        ((this.FileDate.HasValue && other.FileDate.HasValue) && ((this.FileDate.Value - other.FileDate.Value).TotalHours < 12)) ||
                        ((this.Created.HasValue && other.Created.HasValue) && ((this.Created.Value - other.Created.Value).TotalHours < 12))
                    );
            }
        }

        private void RenameVideosByCreatedDate(string sourceFolder, string targetFolder)
        {

            List<string> files = System.IO.Directory.EnumerateFiles(sourceFolder, "*.*", SearchOption.TopDirectoryOnly).ToList();
            List<VidFile> sourceVids = new List<VidFile>(),
                targetVids = new List<VidFile>();

            var count = 0;
            string renameFolder = string.Format(@"{0}\Renamed", sourceFolder);

            if (!System.IO.Directory.Exists(renameFolder))
                System.IO.Directory.CreateDirectory(renameFolder);

            foreach (var file in files.Where(x => "MOV |MP4|M4V|MPG|MPEG|AVI|3GP".Contains(Path.GetExtension(x).Replace(".", "").ToUpper())))
            {
                bool copied = false;
                var properties = GetMetadata(file);

                if (properties != null)
                {
                    DateTime created = DateTime.MinValue;
                    var vid = new VidFile() { Path = file };

                    foreach (var p in properties)
                    {
                        if (p.Key.ToUpper().Trim() == "CREATED")
                        {
                            if (DateTime.TryParseExact(p.Value, "ddd MMM dd HH:mm:ss yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out created))
                            {
                                if(created > new DateTime(2005, 5, 30))
                                    File.Copy(file, string.Format(@"{0}\{1}_{2}{3}", renameFolder, created.ToString("yyyyMMdd"), Path.GetFileNameWithoutExtension(file), Path.GetExtension(file)));
                                else
                                	File.Copy(file, string.Format(@"{0}\{1}{2}", renameFolder, Path.GetFileNameWithoutExtension(file), Path.GetExtension(file)));
                                copied = true;

                            }
                        }
                    }
                }
                
                if (!copied)
                    File.Copy(file, string.Format(@"{0}\{1}{2}", renameFolder, Path.GetFileNameWithoutExtension(file), Path.GetExtension(file)));
            }
        }

        private void CompareVideos(string sourceFolder, string targetFolder)
        {
            
            List<string> files = System.IO.Directory.EnumerateFiles(sourceFolder, "*.*", SearchOption.TopDirectoryOnly).ToList();
            List<VidFile> sourceVids = new List<VidFile>(), 
                targetVids = new List<VidFile>();

            foreach (var file in files.Where(x => "MOV|MP4|M4V|MPG|MPEG|AVI|3GP".Contains(Path.GetExtension(x).Replace(".", "").ToUpper())))
            {
                var properties = GetMetadata(file);

                if (properties == null)
                    continue;

                DateTime fileDate = DateTime.MinValue, created = DateTime.MinValue ;
                long duration = 0, filesize = 0;
                var vid = new VidFile() { Path = file };

                foreach (var p in properties)
                {
                    if (p.Key.ToUpper().Contains("FILE") && p.Key.ToUpper().Contains("DATE"))
                    {
                        if(DateTime.TryParseExact(p.Value, "ddd MMM dd HH:mm:ss zzz yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fileDate))
                            vid.FileDate = fileDate;
                    }
                    if (p.Key.ToUpper().Trim() == "DURATION")
                    {
                        if(long.TryParse(p.Value, out duration))
                            vid.Duration = duration;
                    }
                    if (p.Key.ToUpper().Trim() == "CREATED")
                    {
                        if(DateTime.TryParseExact(p.Value, "ddd MMM dd HH:mm:ss yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out created))
                            vid.Created = created;
                    }
                    if (p.Key.ToUpper().Trim() == "FILE SIZE")
                    {
                        if(!string.IsNullOrEmpty(p.Value))
                        {
                            if (long.TryParse(Regex.Match(p.Value, @"\d+").Value, out filesize))
                                vid.FileSize = filesize;
                        }
                    }
                }

                if(vid.IsValid)
                {
                    sourceVids.Add(vid);
                }
            }

            files = System.IO.Directory.EnumerateFiles(targetFolder, "*.*", SearchOption.TopDirectoryOnly).ToList();
            foreach (var file in files.Where(x => "MOV|MP4|M4V|MPG|MPEG|AVI|3GP".Contains(Path.GetExtension(x).Replace(".", "").ToUpper())))
            {
                var properties = GetMetadata(file);

                if (properties == null)
                    continue;

                DateTime fileDate = DateTime.MinValue, created = DateTime.MinValue;
                long duration = 0, filesize = 0;
                var vid = new VidFile() { Path = file };

                foreach (var p in properties)
                {
                    if (p.Key.ToUpper().Contains("FILE") && p.Key.ToUpper().Contains("DATE"))
                    {
                        if (DateTime.TryParseExact(p.Value, "ddd MMM dd HH:mm:ss zzz yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fileDate))
                            vid.FileDate = fileDate;
                    }
                    if (p.Key.ToUpper().Trim() == "DURATION")
                    {
                        if (long.TryParse(p.Value, out duration))
                            vid.Duration = duration;
                    }
                    if (p.Key.ToUpper().Trim() == "CREATED")
                    {
                        if (DateTime.TryParseExact(p.Value, "ddd MMM dd HH:mm:ss yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out created))
                            vid.Created = created;
                    }
                    if (p.Key.ToUpper().Trim() == "FILE SIZE")
                    {
                        if (!string.IsNullOrEmpty(p.Value))
                        {
                            if (long.TryParse(Regex.Match(p.Value, @"\d+").Value, out filesize))
                                vid.FileSize = filesize;
                        }
                    }
                }

                if (vid.IsValid)
                {
                    targetVids.Add(vid);
                }
            }
            string outputPath = sourceFolder + "\\Duplicates\\";

            if (!System.IO.Directory.Exists(outputPath))
                System.IO.Directory.CreateDirectory(outputPath);
            
            using (StreamWriter w = File.AppendText(outputPath + "vidComparison.csv"))
            {
                w.WriteLine("/* Vid comparison on {0} of '{1}' and '{2}' */", DateTime.Now.ToShortTimeString(), sourceFolder, targetFolder);

                foreach(var s in sourceVids)
                {
                    var t = targetVids.FirstOrDefault(x => x.Equals(s));
                    if (t != null)
                    {
                        w.WriteLine(Path.GetFileName(s.Path) + "," + Path.GetFileName(t.Path));
                        File.Move(s.Path, outputPath + Path.GetFileName(s.Path));
                    }
                }

                w.Flush();
                w.Close();
            }
            
        }

        private void CompareFolders(string sourceFolder, string targetFolder)
        {
            List<string> sourcefiles = System.IO.Directory.EnumerateFiles(sourceFolder, "*.*", SearchOption.AllDirectories).ToList();
            List<string> targetfiles = System.IO.Directory.EnumerateFiles(targetFolder, "*.*", SearchOption.AllDirectories).ToList();
            List<string> sourcesubdirs = System.IO.Directory.GetDirectories(sourceFolder).ToList();
            List<string> targetsubdirs = System.IO.Directory.GetDirectories(targetFolder).ToList();
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
                    fileName.ToUpper().EndsWith("M4V") ||
                    fileName.ToUpper().EndsWith("MP4");
            }
            if (type == "PNG")
            {
                return fileName.ToUpper().EndsWith("PNG");
            }
            return false;
        }

        private string CopyPicTo(string fileName, DateTime date, string destFolder, bool isVideo)
        {
            string imgName = fileName.Substring(fileName.LastIndexOf(@"\") + 1);
            StringBuilder errors = new StringBuilder();
            string sourceFolder;
            if(isVideo)
            {
                sourceFolder = "Video";
            }
            else if (DateTime.MinValue == date)
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

            if (!System.IO.Directory.Exists(sourceFolder))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(sourceFolder);
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
                    int ext = imgPath.LastIndexOf(".");
                    string dupePath = imgPath.Insert(ext, "_D");

                    if (File.Exists(dupePath))
                    {
                        errors.AppendLine("Dupes of your dupes...check on that:" + imgPath);
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
                            log.AppendLine("ERROR: " + ex);
                        }
                    }
                }
            }

            return errors.ToString();
        }

        private bool SameFile(string fileName, string imgPath)
        {
            FileInfo source, target;
            bool sameFile = false;
            try
            {
                source = new FileInfo(fileName);
                target = new FileInfo(imgPath);

                sameFile = source.Length == target.Length;
            }
            catch (Exception ex)
            {
                log.AppendLine(string.Format("Error in SameFile check for {0} and {1}: {2}", fileName, imgPath, ex.Message));
            }
            return sameFile;
        }

        private async void renameVidsBtn_Click(object sender, EventArgs e)
        {
            bool success = false;
            try
            {
                Message.Text = "Renaming videos, please wait...";

                await Task.Run(() => RenameVideosByCreatedDate(txtSource.Text, txtDest.Text))
                    .ContinueWith((prev) =>
                    {
                        if (prev.IsFaulted)
                            throw prev.Exception.InnerException;

                        success = true;
                    });
            }
            catch (AggregateException aex)
            {
                foreach (var ex in aex.InnerExceptions)
                    MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                Completed(success);
            }
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


        private void Completed(bool success)
        {
            if (!success)
            {
                Message.Text = "Error occurred.";
            }
            else
            {
                Message.Text = "Completed Successfully!";
            }

            txtDest.Enabled = true;
            txtSource.Enabled = true;
        }
    }
}