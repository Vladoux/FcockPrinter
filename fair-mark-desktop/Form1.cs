using fair_mark_desktop.Extensions;
using fair_mark_desktop.Service;
using MaterialSkin;
using MaterialSkin.Controls;
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
using System.Reflection;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fair_mark_desktop
{
    public partial class Form1 : MaterialForm
    {
        private MaterialSkinManager materialSkinManager;
        private readonly List<string> filePrint;
        private string downloadUrl;
        private string ext;
        private bool fileDownloaded = false;
        private bool isHiden;
        private readonly static string path = $"{Path.GetTempPath()}FCode";
        private readonly static string urlFilePath = $"{path}\\url.txt";
        private readonly static string hiddenFilePath = $"{path}\\IsHiden.txt";
        private List<string> fullFilePaths = new List<string>();
        public Form1()
        {
            InitializeComponent();
            InitMatetialColor();
            Text = $"FairCode Print {ApplicationSettings.GetNormalizeProductVersion()}";

            WorkSchedulerService.IntervalInHours(1, async () => await CheckVersion());
            Watcher();

            hiddenFilePath.FirstCreateFile("False");
            isHiden = File.ReadAllText(hiddenFilePath) == "True";

            filePrint = new List<string>();
            notifyIcon1.Visible = false;
            autoPrintSwitch.Checked = isHiden;
            notifyIcon1.MouseDoubleClick += new MouseEventHandler(notifyIcon1_MouseDoubleClick);
            Resize += new EventHandler(Form1_Resize);
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
                        Path.Combine(path, $"test.{ext}")).ContinueWith(x => ExctractZip());

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


        public void ExctractZip()
        {
            var pathExtract = Path.Combine(path, $"{DateTime.Now:dd-MM-yyyy-HH-mm-ss}");
            if (ext == "zip")
            {
                Directory.CreateDirectory(pathExtract);
                ZipFile.ExtractToDirectory(Path.Combine(path, $"test.{ext}"), pathExtract);
            }
            var dictinary = new DirectoryInfo(pathExtract);
            FileInfo[] files = dictinary.GetFiles("*.pdf");

            filePrint.AddRange(files.Select(x => x.FullName));

            materialCheckedListBox1.Invoke((MethodInvoker)(() =>
            {
                filePrint.ForEach(x =>
                {
                    fullFilePaths.Add(x);
                    var item = new MaterialCheckbox()
                    {
                        Text = x.GetFileNameFromPath() + " (" + x.GetCountPagesPdf() + " стр.)",
                        Checked = false,
                    };
                    item.CheckedChanged += (sender, e) =>
                    {
                        materialButton3.Enabled = materialCheckedListBox1.Items.Any(z => z.Checked);
                        selectAllSwitch.Checked = materialCheckedListBox1.Items.All(z => z.Checked);
                    };
                    materialCheckedListBox1.Items.Add(item);
                });
            }));

            if (isHiden)
            {
                SendToPrinter(filePrint);
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                ShowInTaskbar = false;
                notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OpenForm();
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
        }

        private void ShowDialogPrinter(List<string> files)
        {
            if (defaultPrinterSwitch.Checked == true)
            {
                SendToPrinter(files);
            }
            else
            {
                var pd = new PrintDialog();
                if (pd.ShowDialog() == DialogResult.OK)
                {
                    SendToPrinter(files);
                }
            }
        }

        private void SendToPrinter(List<string> files)
        {
            foreach (var file in files)
            {
                var document = PdfDocument.Load(file);
                var printDocument = document.CreatePrintDocument();
                printDocument.Print();
            }
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
            if (downloadUrl != url)
            {
                downloadUrl = url;
                fileDownloaded = false;
                materialProgressBar1.Value = 0;
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
            var toPrintFiles = new List<string>();
            materialCheckedListBox1.Items.ForEach(x =>
            {
                if (x.Checked)
                    toPrintFiles.Add(fullFilePaths
                        .FirstOrDefault(full => full.Contains(x.Text.Substring(0, x.Text.IndexOf("(") - 1))));
            });

            ShowDialogPrinter(toPrintFiles);
        }

        private void materialSwitch2_CheckedChanged(object sender, EventArgs e)
        {
            isHiden = ((MaterialSwitch)sender).CheckState == CheckState.Checked && (isHiden = true);
            File.WriteAllText(hiddenFilePath, isHiden.ToString());
        }

        private void materialSwitch3_CheckStateChanged(object sender, EventArgs e)
        {
            materialCheckedListBox1.Items.ForEach(x => x.Checked = selectAllSwitch.Checked);
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
                    materialLabel1.Text = $"Доступна новая версия - {result.Value}";
                    materialLabel1.Visible = true;
                }));
            }
            else
                materialLabel1.Invoke((MethodInvoker)(() =>
                {
                    materialLabel1.Visible = false;
                }));
        }
    }
}