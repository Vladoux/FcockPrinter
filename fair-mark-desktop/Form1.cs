using fair_mark_desktop.CustomModels;
using fair_mark_desktop.CustomModels.Enums;
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
using System.Drawing.Printing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Security;
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
        private readonly static string userIdFilePath = $"{path}\\userId.txt";
        private readonly static string hiddenFilePath = $"{path}\\IsHiden.txt";
        private readonly StorageFilePaths storateFiles = new StorageFilePaths();
        private bool isOldVersion = false;

        private static string connectedUserId = string.Empty;
        private string AppVersion => ApplicationSettings.GetNormalizeProductVersion();

        public Form1(string[] args)
        {
            InitializeComponent();
            InitMatetialColor();
            Text = $"FairCode Print {AppVersion}";

            // ставим метод на повтор - проверка версии
            WorkSchedulerService.IntervalInHours(1, async () => await CheckVersion());
            // ставим метод на повтор - очистка 
            WorkSchedulerService.IntervalInHours(1 / 60.0, () => (Path.Combine(path, $"downloads")).Clean());
            hiddenFilePath.FirstCreateFile("False");
            isHiden = File.ReadAllText(hiddenFilePath) == "True";
            Watcher();

            notifyIcon1.Visible = true;
            autoPrintSwitch.Checked = isHiden;
            notifyIcon1.MouseDoubleClick += new MouseEventHandler(notifyIcon1_MouseDoubleClick);
            Resize += new EventHandler(Form1_Resize);

            Path.Combine(Path.GetTempPath(), $"FCode\\url.txt").WriteToFile(args.FirstOrDefault());
            connectedUserId = storateFiles.LastConnectionUserId;
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

        /// <summary>
        /// Метод для скачивания файла по ссылке
        /// </summary>
        /// <param name="paramUrl"></param>
        /// <returns>Успешно или нет</returns>
        public async Task<bool> Download(string paramUrl)
        {
            try
            {
                // обрезка в строке 'fcode://'
                var url = paramUrl.Substring(8);
                WebClient client = new WebClient();
                client.DownloadProgressChanged += wc_DownloadProgressChanged;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((o, cert, chain, policy) => true);

                // скачивание файла
                _ = client.DownloadFileTaskAsync(new Uri($"{url}"),
                   Path.Combine(path, $"test.{ext}")).ContinueWith(x => ExctractZip(Path.Combine(path, $"test.{ext}")));
                return true;
            }
            catch (Exception e)
            {
                await NotifyUser(e.Message, NotificationType.FileReceivedError, true);
                return false;
            }
        }


        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            materialProgressBar1.Invoke((MethodInvoker)(() => materialProgressBar1.Value = e.ProgressPercentage));
        }


        /// <summary>
        /// Метод для разархивации файла
        /// </summary>
        /// <param name="pathtofile">Путь к скачанному файлу</param>
        public async Task ExctractZip(string pathtofile)
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
            var files = dictinary.GetFilesDeepInfo("*.pdf");

            if (files.Any())
                await NotifyUser("Файл успешно получен приложением", NotificationType.FileReceivedSuccess);

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
            var canButtonBeEnable = !isOldVersion && materialCheckedListBox1.Items.Any(z => z.Checked);
            materialButton3.Enabled = canButtonBeEnable;
            materialButton2.Enabled = canButtonBeEnable;
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
                var pp = new PrintDialog();
                if (pp.ShowDialog() == DialogResult.OK)
                    SendToPrinter(pp);
            }
            else
                SendToPrinter();
        }

        /// <summary>
        /// Отправка файлов на печать
        /// </summary>
        /// <param name="pd">Диалоговое окно печати (windows)</param>
        private async void SendToPrinter(PrintDialog pd = null)
        {
            var selectedList = SelectedFiles.ToList();
            // зачеркиваем выбранные файлы по предикату
            StrikeText(x => x.Checked);
            // инициализвация количества напечатанных файлов
            var countPrintedFiles = 0;
            // проход по путям файлов и отправка на принтер
            foreach (var file in selectedList)
            {
                // проверка на существование файла
                if (!File.Exists(file))
                    continue;

                try
                {
                    // загрузка документа
                    var document = PdfDocument.Load(file);
                    // создание объекта для печати
                    var printDocument = document.CreatePrintDocument();
                    // если был послан объект диалога
                    if (pd != null)
                        // то применяем новые настройки для печати
                        printDocument.PrinterSettings = pd.PrinterSettings;
                    // инициализируем событие на конец печати последней страницы файла
                    printDocument.EndPrint += (s, e) =>
                    {
                        if (!e.Cancel)
                            countPrintedFiles++;
                    };
                    // запускаем печать
                    printDocument.Print();
                }
                // проверка на ошибки принтера
                catch (InvalidPrinterException exc)
                {
                    await NotifyUser(exc.Message, NotificationType.PrinterError, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            if (countPrintedFiles > 0)
                await NotifyUser($"Файлы ({countPrintedFiles}) успешно напечатаны", NotificationType.PrinterCompleted);
        }

        /// <summary>
        /// Список путей выбранных из списка файлов
        /// </summary>
        public List<string> SelectedFiles => materialCheckedListBox1.Items.Where(x => x.Checked)
                .Select(x => ((CustomMaterailCheckBox)x).Value).ToList();

        /// <summary>
        /// Метод для оповещения пользователя
        /// </summary>
        /// <param name="sendMessage">Сообщение для отправки на бэк</param>
        /// <param name="notificationType">Тип оповещения</param>
        /// <param name="withDialogError">Флаг - показывать ошибку в приложении</param>
        /// <returns></returns>
        public async Task NotifyUser(string sendMessage, NotificationType notificationType, bool withDialogError = false)
        {
            await FMarkApiService.NotifyUserFMark(connectedUserId, sendMessage, notificationType);
            if (withDialogError)
                MessageBox.Show(sendMessage);
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
            ToTray();
        }

        /// <summary>
        /// Метод события на изменение файлов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnUrlFileChanged(object sender, FileSystemEventArgs e)
        {
            // проверка версии приложения
            await CheckVersion();

            if (e.FullPath.Replace("\\\\", "\\") != urlFilePath) return;

            if (!autoPrintSwitch.Checked)
            {
                Invoke((MethodInvoker)OpenForm);
            }

            Thread.Sleep(1000);

            var urlWithUser = File.ReadAllText(urlFilePath);
            var urlParams = urlWithUser.Split(new string[] { "?userId=" }, StringSplitOptions.None);
            var url = urlParams[0];

            if (string.IsNullOrEmpty(url)) return;

            connectedUserId = urlParams.Length > 1 ? urlParams[1] : null;
            userIdFilePath.WriteToFile(connectedUserId);

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
                await Download(downloadUrl);
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
        /// <summary>
        /// Метод инициализации системного вочера
        /// </summary>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void Watcher()
        {
            // вочер просматривает папку FCode (в Temp) на любые изменения файлов
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
            var result = await FMarkApiService.CheckNewVersion();

            // если получили ответ и есть разлиция в версих
            if (result.IsSuccess && !Equals(AppVersion, result.Value))
            {
                // показывает текст о доступности новой версии
                materialLabel1.Invoke((MethodInvoker)(() =>
                {
                    materialLabel1.Text = $"Доступна новая версия {result.Value}";
                    materialLabel1.Visible = true;
                }));

                // помечаем невозможность автопечати
                autoPrintSwitch.Invoke((MethodInvoker)(() => { autoPrintSwitch.Checked = false; }));

                // инициализируем список кнопок
                var buttons = new List<MaterialButton>
                {
                    materialButton1,
                    materialButton2,
                    materialButton3,
                    materialButton4
                };

                // все кнопки ставим в состояние - неактивно
                buttons.ForEach(button => button.Invoke((MethodInvoker)(() => button.Enabled = false)));
                isOldVersion = true;
            }
            else
            {
                isOldVersion = false;
                materialLabel1.Invoke((MethodInvoker)(() =>
                {
                    materialLabel1.Visible = false;
                }));
            }
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

        public void StrikeText(Func<MaterialCheckbox, bool> pred)
        {
            var items = materialCheckedListBox1.Items.Where(pred);
            foreach (var item in items)
            {
                item.Text = item.Text.ToStrikeText();
                item.Checked = false;
            }
        }

        private async void materialButton4_Click(object sender, EventArgs e)
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
                        await ExctractZip(filePath);
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