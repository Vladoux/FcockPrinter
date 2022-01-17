using fair_mark_desktop.Service;
using MaterialSkin.Controls;
using Microsoft.Win32;
using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static fair_mark_desktop.Service.DesktopService;

namespace fair_mark_desktop
{
    public partial class Form1 : MaterialForm
    {
        readonly MaterialSkin.MaterialSkinManager materialSkinManager;
        readonly DesktopService desktopService;
        List<string> printStrings;
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
            desktopService = new DesktopService();
            printStrings = new List<string>();
            var printers = desktopService.GetPrinterList();
            notifyIcon1.Visible = false;
            this.notifyIcon1.MouseDoubleClick += new MouseEventHandler(notifyIcon1_MouseDoubleClick);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            BindingSource comboBoxBindingSource = new BindingSource();

            comboBoxBindingSource.DataSource = printers;

            materialComboBox1.DataSource = comboBoxBindingSource;
            materialComboBox1.DisplayMember = "Name";
        }


        public bool Download(string paramUrl)
        {
            try
            {
                var url = paramUrl.Substring(8);
                WebClient client = new WebClient();
                client.DownloadProgressChanged += wc_DownloadProgressChanged;
                client.DownloadFileTaskAsync(new Uri($"{url}"),
                        Path.Combine(desktopService.GetPath(), $"test.{ext}")).ContinueWith(x => ExctractZip());
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

        void wc_DownloadProgressCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            ExctractZip();
        }

        public void ExctractZip()
        {
            var path = Path.Combine(desktopService.GetPath(), $"{DateTime.Now:dd-MM-yyyy-HH-mm-ss}");
            if (ext == "zip")
            {
                Directory.CreateDirectory(path);
                ZipFile.ExtractToDirectory(Path.Combine(desktopService.GetPath(), $"test.{ext}"), path);
            }
            var dictinary = new DirectoryInfo(path);
            FileInfo[] files = dictinary.GetFiles("*.pdf");

            printStrings.AddRange(files.Select(x=>x.FullName));

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

        private void SendToPrinter(List<string> files)
        {
            foreach (var file in files)
            {
                PdfDocument doc = new PdfDocument();
                doc.LoadFromFile(file);

                // Specify a printer
                doc.PrintSettings.PrinterName = ((MyPrinter)(materialComboBox1.SelectedItem)).Name;

                // Print PDF documents
                doc.Print();

            }
        }

        private void materialComboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            materialLabel2.Text = ((MyPrinter)((MaterialComboBox)sender).SelectedItem).Status;
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
           
            var url = File.ReadAllText(Path.Combine(desktopService.GetPath(), "url.txt"));
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
            SendToPrinter(printStrings);
        }
    }
}