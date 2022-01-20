using MaterialSkin.Controls;
using PdfiumViewer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace fair_mark_desktop
{
    public partial class Form1 : MaterialForm
    {
        readonly MaterialSkin.MaterialSkinManager materialSkinManager;
        List<string> filePrint;
        string downloadUrl;
        string ext;
        bool fileDownloaded = false;
        public Form1(string[] args)
        {
            InitializeComponent();

            materialSkinManager = MaterialSkin.MaterialSkinManager.Instance;
            materialSkinManager.EnforceBackcolorOnAllComponents = true;
            materialSkinManager.ColorScheme = new MaterialSkin.ColorScheme(MaterialSkin.Primary.LightBlue800, MaterialSkin.Primary.LightBlue900, MaterialSkin.Primary.Blue900,
                MaterialSkin.Accent.Indigo700, MaterialSkin.TextShade.WHITE);
            filePrint = new List<string>();
            notifyIcon1.Visible = false;
            this.notifyIcon1.MouseDoubleClick += new MouseEventHandler(notifyIcon1_MouseDoubleClick);
            this.Resize += new System.EventHandler(this.Form1_Resize);

        }


        public bool Download(string paramUrl)
        {
            try
            {
                var url = paramUrl.Substring(8);
                WebClient client = new WebClient();
                client.DownloadProgressChanged += wc_DownloadProgressChanged;
                client.DownloadFileTaskAsync(new Uri($"{url}"),
                        Path.Combine(Path.GetTempPath(), $"test.{ext}")).ContinueWith(x => ExctractZip());
                materialLabel1.Text = url.Substring(url.LastIndexOf('/')+1);
                materialButton3.Enabled = true;
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
            materialProgressBar1.Value = e.ProgressPercentage;
        }

        //void wc_DownloadProgressCompleted(object sender, DownloadDataCompletedEventArgs e)
        //{
        //    ExctractZip();
        //}

        public void ExctractZip()
        {
            var path = Path.Combine(Path.GetTempPath(), $"{DateTime.Now:dd-MM-yyyy-HH-mm-ss}");
            if (ext == "zip")
            {
                Directory.CreateDirectory(path);
                ZipFile.ExtractToDirectory(Path.Combine(Path.GetTempPath(), $"test.{ext}"), path);
            }
            var dictinary = new DirectoryInfo(path);
            FileInfo[] files = dictinary.GetFiles("*.pdf");

            filePrint.AddRange(files.Select(x => x.FullName));

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            // проверяем наше окно, и если оно было свернуто, делаем событие        
            if (WindowState == FormWindowState.Minimized)
            {
                // прячем наше окно из панели
                this.ShowInTaskbar = false;
                // делаем нашу иконку в трее активной
                notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // делаем нашу иконку скрытой
            notifyIcon1.Visible = false;
            // возвращаем отображение окна в панели
            this.ShowInTaskbar = true;
            //разворачиваем окно
            WindowState = FormWindowState.Normal;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.WindowState = FormWindowState.Minimized;
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "openMenuItem":
                    notifyIcon1.Visible = false;
                    // возвращаем отображение окна в панели
                    this.ShowInTaskbar = true;
                    //разворачиваем окно
                    WindowState = FormWindowState.Normal;
                    break;
                case "closeMenuItem":
                    Application.Exit();
                    break;
            }
        }

        private void ShowDialogPrinter(List<string> files)
        {
            if (materialSwitch1.Checked == true)
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


        private void gggToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void Form1_Activated(object sender, EventArgs e)
        {

            var url = File.ReadAllText(Path.Combine(Path.GetTempPath(), "url.txt"));
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
            ShowDialogPrinter(filePrint);
        }
    }
}