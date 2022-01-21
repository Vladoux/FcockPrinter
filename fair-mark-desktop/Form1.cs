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
using static MaterialSkin.Controls.MaterialCheckedListBox;

namespace fair_mark_desktop
{
    public partial class Form1 : MaterialForm
    {
        readonly MaterialSkin.MaterialSkinManager materialSkinManager;
        List<string> filePrint;
        string downloadUrl;
        string ext;
        string path;
        bool fileDownloaded = false;
        bool isHiden;
        public Form1(string[] args)
        {
            InitializeComponent();

            path = $"{Path.GetTempPath()}/Fcode";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (!File.Exists($"{path}/IsHiden.txt"))
            {
                File.WriteAllText($"{path}/IsHiden.txt", "False");
            }
            isHiden = File.ReadAllText($"{path}/IsHiden.txt") == "True" ? true : false;

            materialSkinManager = MaterialSkin.MaterialSkinManager.Instance;
            materialSkinManager.EnforceBackcolorOnAllComponents = true;
            materialSkinManager.ColorScheme = new MaterialSkin.ColorScheme(MaterialSkin.Primary.LightBlue800, MaterialSkin.Primary.LightBlue900, MaterialSkin.Primary.Blue900,
                MaterialSkin.Accent.Indigo700, MaterialSkin.TextShade.WHITE);
            filePrint = new List<string>();
            notifyIcon1.Visible = false;

            materialSwitch2.Checked = isHiden;
            this.notifyIcon1.MouseDoubleClick += new MouseEventHandler(notifyIcon1_MouseDoubleClick);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            //if (isHiden)
            //{
            //    OnClosing(new CancelEventArgs());
            //}

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
            materialProgressBar1.Value = e.ProgressPercentage;
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
                    var item = new MaterialCheckbox()
                    {
                        Text = x,
                        Checked = false,
                    };
                    item.CheckedChanged += (sender, e) =>
                    {
                        materialButton3.Enabled = materialCheckedListBox1.Items.Any(z => z.Checked) ? true : false;
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
            var files = new List<string>();
            materialCheckedListBox1.Items.ForEach(x =>
            {
                if (x.Checked)
                    files.Add(x.Text);
            });

            ShowDialogPrinter(files);
        }

        private void materialSwitch2_CheckedChanged(object sender, EventArgs e)
        {
            isHiden = ((MaterialSwitch)sender).CheckState == CheckState.Checked ? isHiden = true : false;
            File.WriteAllText($"{path}/IsHiden.txt", $"{isHiden}");
        }

        private void materialSwitch3_CheckStateChanged(object sender, EventArgs e)
        {
            if (materialSwitch3.Checked)
            {
                materialCheckedListBox1.Items.ForEach(x => x.Checked = true);
            }
        }
    }
}