using fair_mark_desktop.CustomModels;
using fair_mark_desktop.Extensions;
using fair_mark_desktop.Service;
using MaterialSkin;
using MaterialSkin.Controls;
using Microsoft.Win32;
using PdfiumViewer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fair_mark_desktop
{
    public partial class Form1 : MaterialForm
    {
        private MaterialSkinManager materialSkinManager;
        private string downloadUrl;
        private string ext;
        private bool fileDownloaded = false;
        private bool isHiden;
        private readonly static string path = $"{Path.GetTempPath()}FCode";
        private readonly static string urlFilePath = $"{path}\\url.txt";
        private readonly static string hiddenFilePath = $"{path}\\IsHiden.txt";
        private readonly StorageFilePaths storateFiles = new StorageFilePaths();
        public Form1(string[] args)
        {
            InitializeComponent();
            InitMatetialColor();
            Text = $"FairCode Print {ApplicationSettings.GetNormalizeProductVersion()}";

            WorkSchedulerService.IntervalInHours(1, async () => await CheckVersion());
            WorkSchedulerService.IntervalInHours(1/60.0, () => (Path.Combine(path, $"downloads")).Clean());
            hiddenFilePath.FirstCreateFile("False");
            isHiden = File.ReadAllText(hiddenFilePath) == "True";
            Watcher();

            notifyIcon1.Visible = true;
            autoPrintSwitch.Checked = isHiden;
            notifyIcon1.MouseDoubleClick += new MouseEventHandler(notifyIcon1_MouseDoubleClick);
            Resize += new EventHandler(Form1_Resize);

            Path.Combine(Path.GetTempPath(), $"FCode\\url.txt").WriteToFile(args.FirstOrDefault());
            AddFilesPrint(storateFiles.GetPaths());
        }

        private void InitMatetialColor()
        {
            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.EnforceBackcolorOnAllComponents = true;

            materialSkinManager.ColorScheme = new ColorScheme(Color.FromArgb(10, 68, 99),
                Color.FromArgb(10, 68, 99), Color.FromArgb(10, 68, 99),
                Color.FromArgb(10, 68, 99), TextShade.WHITE);
        }


        public bool Download(string paramUrl)
        {
            try
            {
                var url = paramUrl.Substring(8);
                WebClient client = new WebClient();
                client.DownloadProgressChanged += wc_DownloadProgressChanged;

                client.DownloadFileTaskAsync(new Uri($"{url}"),
                   Path.Combine(path, $"test.{ext}")).ContinueWith(x => ExctractZip(Path.Combine(path, $"test.{ext}")));
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }


        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            materialProgressBar1.Invoke((MethodInvoker)(() => materialProgressBar1.Value = e.ProgressPercentage));
        }


        public void ExctractZip(string pathtofile)
        {
            var pathtoextract = Path.Combine(path, $"downloads");
            if (!Directory.Exists(pathtoextract))
            {
                Directory.CreateDirectory(pathtoextract);
            }

            var pathExtract = Path.Combine(pathtoextract, $"{DateTime.Now:dd-MM-yyyy-HH-mm-ss}");
            if (ext == "zip")
            {
                Directory.CreateDirectory(pathExtract);
                ZipFile.ExtractToDirectory(pathtofile, pathExtract);
            }
            var dictinary = new DirectoryInfo(pathExtract);
            FileInfo[] files = dictinary.GetFiles("*.pdf");

            AddFilesPrint(files.Select(x => x.FullName).ToList());
           
            if (isHiden)
                SendToPrinter();
        }

        private void AddFilesPrint(List<string> filesToAdd)
        {
            storateFiles.AddRange(filesToAdd);

            materialCheckedListBox1.Invoke((MethodInvoker)(() =>
            {
                filesToAdd.ForEach(x =>
                {
                    var item = new CustomMaterailCheckBox()
                    {
                        Text = x.GetFileNameFromPath() + " (" + x.GetCountPagesPdf() + " стр.)",
                        Value = x
                    };
                    item.CheckedChanged += (sender, e) =>
                    {
                        blockFromItem = true;
                        UpdateButtonsState();

                        if (!isSwitchBlock)
                            selectAllSwitch.Checked = materialCheckedListBox1.Items.All(z => z.Checked);
                        blockFromItem = false;
                    };

                    item.CreateContextMenu(new Dictionary<string, EventHandler>()
                    {
                        { "Открыть",  OpenFileHandler},
                        { "Показать в папке", ShowFolderHandler}
                    });
                    materialCheckedListBox1.Items.Add(item);
                    item.Checked = true;
                });
            }));
        }

        private void UpdateButtonsState()
        {
            materialButton3.Enabled = materialCheckedListBox1.Items.Any(z => z.Checked);
            materialButton2.Enabled = materialCheckedListBox1.Items.Any(z => z.Checked);
        }

        private void OpenFileHandler(object sender, EventArgs e)
        {
            var itemPath = ((CustomMaterailCheckBox)((MaterialToolStripMenuItem)sender).GetItemBoxFromContext()).Value;
            Process.Start(itemPath);
        }
        private void ShowFolderHandler(object sender, EventArgs e)
        {
            var itemPath = ((CustomMaterailCheckBox)((MaterialToolStripMenuItem)sender).GetItemBoxFromContext()).Value;
            Process.Start("explorer.exe", Path.GetDirectoryName(itemPath));
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                ToTray();
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OpenForm();
        }

        public void ToTray()
        {
            ShowInTaskbar = false;
            notifyIcon1.Visible = true;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            WindowState = FormWindowState.Minimized;
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "openMenuItem":
                    OpenForm();
                    break;
                case "closeMenuItem":
                    Application.Exit();
                    break;
            }
        }

        private void OpenForm()
        {
            notifyIcon1.Visible = false;
            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
            Focus();
            Activate();
        }

        private void ShowDialogPrinter()
        {
            if (!defaultPrinterSwitch.Checked)
            {
                if (new PrintDialog().ShowDialog() == DialogResult.OK)
                    SendToPrinter();
            }
            else
                SendToPrinter();
        }

        private void SendToPrinter()
        {
            var list = materialCheckedListBox1.Items.Where(x => x.Checked)
                .Select(x => ((CustomMaterailCheckBox)x).Value).ToList();
            RemoveFromList(x => x.Checked);
            foreach (var file in list)
            {
                if (File.Exists(file))
                {
                    var document = PdfDocument.Load(file);
                    var printDocument = document.CreatePrintDocument();
                    printDocument.Print();
                }
            }
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
            ToTray();
        }

        private void OnUrlFileChanged(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath.Replace("\\\\", "\\") != urlFilePath) return;

            if (!autoPrintSwitch.Checked)
            {
                Invoke((MethodInvoker)OpenForm);
            }

            Thread.Sleep(1000);
            var url = File.ReadAllText(urlFilePath);
            if (string.IsNullOrEmpty(url)) return;
            if (downloadUrl != url)
            {
                downloadUrl = url;
                fileDownloaded = false;
                materialProgressBar1.Execute(() => materialProgressBar1.Value = 0);
            }

            if (!string.IsNullOrEmpty(downloadUrl) && !fileDownloaded)
            {
                fileDownloaded = true;
                ext = downloadUrl.Substring(downloadUrl.Length - 3, 3);
                Download(downloadUrl);
            }
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            ShowDialogPrinter();
        }

        private void materialSwitch2_CheckedChanged(object sender, EventArgs e)
        {
            isHiden = ((MaterialSwitch)sender).CheckState == CheckState.Checked && (isHiden = true);
            File.WriteAllText(hiddenFilePath, isHiden.ToString());
            if (isHiden)
                defaultPrinterSwitch.Checked = true;
        }

        private bool isSwitchBlock = false;
        private bool blockFromItem = false;
        private void materialSwitch3_CheckStateChanged(object sender, EventArgs e)
        {
            isSwitchBlock = true;
            if (!blockFromItem)
                materialCheckedListBox1.Items.ForEach(x => x.Checked = selectAllSwitch.Checked);
            isSwitchBlock = false;
        }

        static FileSystemWatcher watcher = null;
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void Watcher()
        {
            watcher = new FileSystemWatcher()
            {
                Path = $"{Path.GetTempPath()}\\FCode",
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.LastAccess
                    | NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Size,
                Filter = "*.*",
                EnableRaisingEvents = true
            };

            watcher.Changed += OnUrlFileChanged;
        }

        public async Task CheckVersion()
        {
            var version = ApplicationSettings.GetNormalizeProductVersion();
            var fmarkService = new FMarkApiService();
            var result = await fmarkService.CheckNewVersion();
            if (result.IsSuccess && !Equals(version, result.Value))
            {
                materialLabel1.Invoke((MethodInvoker)(() =>
                {
                    materialLabel1.Text = $"Доступна новая версия {result.Value}";
                    materialLabel1.Visible = true;
                }));
            }
            else
                materialLabel1.Invoke((MethodInvoker)(() =>
                {
                    materialLabel1.Visible = false;
                }));
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            RemoveFromList(x => x.Checked);
        }

        public void RemoveFromList(Func<MaterialCheckbox, bool> pred)
        {
            while (materialCheckedListBox1.Items.Any(pred))
            {
                var item = (CustomMaterailCheckBox)materialCheckedListBox1.Items.FirstOrDefault(pred);
                materialCheckedListBox1.Execute(() => materialCheckedListBox1.Items.Remove(item));
                storateFiles.Remove(item.Value);
            }

            UpdateButtonsState();
            if (!materialCheckedListBox1.Items.Any())
                selectAllSwitch.Checked = false;
        }

        private void materialButton4_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", String.Empty).ToString();
                openFileDialog.Filter = "pdf files (*.pdf)|*.pdf| zip files (*.zip)|*.zip";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;

                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }

                    ext = filePath.Substring(filePath.Length - 3, 3);
                    if (ext == "zip")
                    {
                        ExctractZip(filePath);
                    }
                    else if (ext == "pdf")
                    {
                        var print = new List<string> { filePath };
                        AddFilesPrint(print);
                    }
                }
            }
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            materialButton3.Focus();
        }

        private void defaultPrinterSwitch_CheckedChanged(object sender, EventArgs e)
        {
            if (autoPrintSwitch.Checked)
                defaultPrinterSwitch.Checked = true;
        }
    }
}