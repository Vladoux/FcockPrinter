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
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Printing;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fair_mark_desktop
{
    public sealed partial class Form1 : MaterialForm
    {
        private MaterialSkinManager _materialSkinManager;
        private string _downloadUrl;

        private string _ext;

        private bool _isHiden;
        private static readonly string TempPath = $"{Path.GetTempPath()}FCode";
        private static readonly string UrlFilePath = $"{TempPath}\\url.txt";
        private static readonly string UserIdFilePath = $"{TempPath}\\userId.txt";
        private static readonly string HiddenFilePath = $"{TempPath}\\IsHiden.txt";
        private readonly StorageFilePaths _storateFiles = new StorageFilePaths();
        private bool _isOldVersion;

        private static string _connectedUserId = string.Empty;
        private static string AppVersion => ApplicationSettings.GetNormalizeProductVersion();

        public Form1(IEnumerable<string> args)
        {
            InitializeComponent();
            InitMatetialColor();
            Text = $@"FairCode Print {AppVersion}";

            HiddenFilePath.FirstCreateFile("False");
            _isHiden = File.ReadAllText(HiddenFilePath) == "True";
            Watcher();

            notifyIcon1.Visible = true;
            autoPrintSwitch.Checked = _isHiden;
            notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
            Resize += Form1_Resize;

            Path.Combine(UrlFilePath).WriteToFile(args.FirstOrDefault());
            _connectedUserId = _storateFiles.LastConnectionUserId;
            AddFilesPrint(_storateFiles.GetPaths());

            // ставим метод на повтор - проверка версии
            WorkSchedulerService.IntervalInHours(1, async () => await CheckVersion());
            // ставим метод на повтор - очистка 
            WorkSchedulerService.IntervalInHours(1 / 60.0, () => Path.Combine(TempPath, "downloads").Clean());
        }

        private void InitMatetialColor()
        {
            _materialSkinManager = MaterialSkinManager.Instance;
            _materialSkinManager.EnforceBackcolorOnAllComponents = true;

            _materialSkinManager.ColorScheme = new ColorScheme(Color.FromArgb(10, 68, 99),
                Color.FromArgb(10, 68, 99), Color.FromArgb(10, 68, 99),
                Color.FromArgb(10, 68, 99), TextShade.WHITE);
        }

        /// <summary>
        /// Метод для скачивания файла по ссылке
        /// </summary>
        /// <param name="paramUrl"></param>
        /// <returns>Успешно или нет</returns>
        private async Task Download(string paramUrl)
        {
            try
            {
                // обрезка в строке 'fcode://'
                var url = paramUrl.Substring(8);
                // скачивание файла
                _ = Download(url, Path.Combine(TempPath, $"test.{_ext}"))
                    .ContinueWith(x => ExctractZip(Path.Combine(TempPath, $"test.{_ext}")));
            }
            catch (Exception e)
            {
                await NotifyUser(e.Message, NotificationType.FileReceivedError);
            }
        }

        /// <summary>
        /// Метод скачивания файла по ссылке
        /// </summary>
        /// <param name="downloadUrl">Ссылка для скачивания</param>
        /// <param name="fileName">Название полученного файла</param>
        /// <returns></returns>
        private Task Download(string downloadUrl, string fileName)
        {
            var client = new WebClient();
            client.DownloadProgressChanged += wc_DownloadProgressChanged;
            ServicePointManager.ServerCertificateValidationCallback = (o, cert, chain, policy) => true;
            // скачивание файла
            return client.DownloadFileTaskAsync(new Uri(downloadUrl), fileName);
        }


        /// <summary>
        /// Метод для отслеживания изменений в ProgressBar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            materialProgressBar1.Invoke((MethodInvoker)(() => materialProgressBar1.Value = e.ProgressPercentage));
        }


        /// <summary>
        /// Метод для разархивации файла
        /// </summary>
        /// <param name="pathtofile">Путь к скачанному файлу</param>
        private async Task ExctractZip(string pathtofile)
        {
            var pathtoextract = Path.Combine(TempPath, "downloads");
            if (!Directory.Exists(pathtoextract))
            {
                Directory.CreateDirectory(pathtoextract);
            }

            var pathExtract = Path.Combine(pathtoextract, $"{DateTime.Now:dd-MM-yyyy-HH-mm-ss}");
            if (_ext == "zip")
            {
                Directory.CreateDirectory(pathExtract);
                ZipFile.ExtractToDirectory(pathtofile, pathExtract);
            }

            var dictinary = new DirectoryInfo(pathExtract);
            var files = dictinary.GetFilesDeepInfo("*.pdf").ToList();

            if (files.Any())
                await NotifyUser("Файл успешно получен приложением", NotificationType.FileReceivedSuccess);

            AddFilesPrint(files.Select(x => x.FullName).ToList());

            if (_isHiden)
                SendToPrinter();
        }

        private void AddFilesPrint(List<string> filesToAdd)
        {
            _storateFiles.AddRange(filesToAdd);

            materialCheckedListBox1.Invoke((MethodInvoker)(() =>
            {
                filesToAdd.ForEach(x =>
                {
                    var item = new CustomMaterailCheckBox
                    {
                        Text = $@"{x.GetFileNameFromPath()} ({x.GetCountPagesPdf()} стр.)",
                        Value = x
                    };
                    item.CheckedChanged += (sender, e) =>
                    {
                        _blockFromItem = true;
                        UpdateButtonsState();

                        if (!_isSwitchBlock)
                            selectAllSwitch.Checked = materialCheckedListBox1.Items.All(z => z.Checked);
                        _blockFromItem = false;
                    };

                    item.CreateContextMenu(new Dictionary<string, EventHandler>()
                    {
                        { "Открыть", OpenFileHandler },
                        { "Показать в папке", ShowFolderHandler }
                    });
                    materialCheckedListBox1.Items.Add(item);
                    item.Checked = true;
                });
            }));
        }

        private void UpdateButtonsState()
        {
            var canButtonBeEnable = !_isOldVersion && materialCheckedListBox1.Items.Any(z => z.Checked);
            materialButton3.Enabled = canButtonBeEnable;
            materialButton2.Enabled = canButtonBeEnable;
        }

        private static void OpenFileHandler(object sender, EventArgs e)
        {
            var itemPath = ((CustomMaterailCheckBox)((MaterialToolStripMenuItem)sender).GetItemBoxFromContext()).Value;
            Process.Start(itemPath);
        }

        private static void ShowFolderHandler(object sender, EventArgs e)
        {
            var itemPath = ((CustomMaterailCheckBox)((MaterialToolStripMenuItem)sender).GetItemBoxFromContext()).Value;
            Process.Start("explorer.exe", Path.GetDirectoryName(itemPath));
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                ToTray();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OpenForm();
        }

        private void ToTray()
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
            // проход по путям файлов и отправка на принтер
            foreach (var file in selectedList.Where(File.Exists))
            {
                try
                {
                    // загрузка документа
                    var document = PdfDocument.Load(file);
                    // создание объекта для печати
                    var printDocument = document.CreatePrintDocument();
                    printDocument.DocumentName = $"FCodePrinter {Guid.NewGuid()}";

                    // если был послан объект диалога
                    if (pd != null)
                        // то применяем новые настройки для печати
                        printDocument.PrinterSettings = pd.PrinterSettings;
                    // инициализируем событие на конец печати последней страницы файла
                    printDocument.EndPrint += (s, e) =>
                    {
                        var currentPrinterName = printDocument.PrinterSettings.PrinterName.ToLower();

                        Task.Run(async () =>
                            await CheckPrinterDocumentStatus(printDocument.DocumentName, currentPrinterName));
                    };
                    // запускаем печать
                    printDocument.Print();
                }
                // проверка на ошибки принтера
                catch (InvalidPrinterException exc)
                {
                    await NotifyUser(exc.Message, NotificationType.PrinterError);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private static async Task CheckPrinterDocumentStatus(string documentName, string printerName)
        {
            var printServer = new PrintServer();
            // получение всех принтеров
            var myPrintQueues = printServer.GetPrintQueues(new[]
                { EnumeratedPrintQueueTypes.Local, EnumeratedPrintQueueTypes.Connections });
            // поиск принтера по имени
            var printQueue = myPrintQueues.FirstOrDefault(x => x.Name.ToLower() == printerName);
            // проверка на наличие принтера
            if (printQueue != null)
            {
                // получение списка печатаемых документов
                printQueue.Refresh();
                var jobs = printQueue.GetPrintJobInfoCollection();
                // поиск печатаемого документа по имени
                var findedJob = jobs.FirstOrDefault(x =>
                    string.Equals(x.Name, documentName, StringComparison.CurrentCultureIgnoreCase));
                
                // проверка на статус "В работе"
                while (findedJob.InWork())
                    findedJob?.Refresh();

                var (message, toNotify, notificationType) = findedJob.GetStatusWithNotify();
                if (toNotify)
                    await NotifyUser(message, notificationType);

                // проверка принтера на "Офлайн"
                // так как статус документа не показывает статус "Офлайн"
                if (findedJob?.HostingPrintQueue.IsOffline ?? false)
                    await NotifyUser("Принтер недоступен", NotificationType.PrinterError);
            }
        }

        /// <summary>
        /// Список путей выбранных из списка файлов
        /// </summary>
        private IEnumerable<string> SelectedFiles => materialCheckedListBox1.Items.Where(x => x.Checked)
            .Select(x => ((CustomMaterailCheckBox)x).Value).ToList();

        /// <summary>
        /// Метод для оповещения пользователя
        /// </summary>
        /// <param name="sendMessage">Сообщение для отправки на бэк</param>
        /// <param name="notificationType">Тип оповещения</param>
        /// <returns></returns>
        private static async Task NotifyUser(string sendMessage, NotificationType notificationType)
        {
            await FMarkApiService.NotifyUserFMark(_connectedUserId, sendMessage, notificationType);
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
            await CheckVersion();

            if (e.FullPath.Replace("\\\\", "\\") != UrlFilePath) return;

            materialProgressBar1.Execute(() => materialProgressBar1.Value = 0);
            if (!autoPrintSwitch.Checked)
            {
                Invoke((MethodInvoker)OpenForm);
            }

            Thread.Sleep(1000);

            var urlWithUser = File.ReadAllText(UrlFilePath);
            var urlParams = urlWithUser.Split(new[] { "?userId=" }, StringSplitOptions.None);
            var url = urlParams[0];

            if (string.IsNullOrEmpty(url)) return;

            _connectedUserId = urlParams.Length > 1 ? urlParams[1] : null;

            if (_downloadUrl != url)
                _downloadUrl = url;

            if (!string.IsNullOrEmpty(_downloadUrl))
            {
                _ext = _downloadUrl.Substring(_downloadUrl.Length - 3, 3);
                UserIdFilePath.WriteToFile(_connectedUserId);
                await Download(_downloadUrl);
                UrlFilePath.WriteToFile("");
            }
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            ShowDialogPrinter();
        }

        private void materialSwitch2_CheckedChanged(object sender, EventArgs e)
        {
            _isHiden = ((MaterialSwitch)sender).CheckState == CheckState.Checked && (_isHiden = true);
            File.WriteAllText(HiddenFilePath, _isHiden.ToString());
            if (_isHiden)
                defaultPrinterSwitch.Checked = true;
        }

        private bool _isSwitchBlock;
        private bool _blockFromItem;

        private void materialSwitch3_CheckStateChanged(object sender, EventArgs e)
        {
            _isSwitchBlock = true;
            if (!_blockFromItem)
                materialCheckedListBox1.Items.ForEach(x => x.Checked = selectAllSwitch.Checked);
            _isSwitchBlock = false;
        }

        private static FileSystemWatcher _watcher;

        /// <summary>
        /// Метод инициализации системного вочера
        /// </summary>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private void Watcher()
        {
            var folder = $"{Path.GetTempPath()}\\FCode";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            // вочер просматривает папку FCode (в Temp) на любые изменения файлов
            _watcher = new FileSystemWatcher
            {
                Path = $"{Path.GetTempPath()}\\FCode",
                NotifyFilter = NotifyFilters.LastWrite,
                Filter = "*.*",
                EnableRaisingEvents = true
            };

            _watcher.Changed += OnUrlFileChanged;
        }

        private async Task CheckVersion()
        {
            var result = await FMarkApiService.CheckNewVersion();

            // если получили ответ и есть разлиция в версих
            if (result.IsSuccess && !Equals(AppVersion, result.Value))
            {
                // показывает текст о доступности новой версии
                materialLabel1.Invoke((MethodInvoker)(() =>
                {
                    materialLabel1.Text = $@"Доступна новая версия {result.Value}";
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
                _isOldVersion = true;

                versionPanel.Invoke((MethodInvoker)(() => { versionPanel.Visible = true; }));

                var panel = versionPanel;

                panel1.Invoke((MethodInvoker)(() =>
                {
                    panel1.Controls.Remove(materialCheckedListBox1);
                    panel1.Controls.Add(panel);
                }));
            }
            else
            {
                _isOldVersion = false;
                materialLabel1.Invoke((MethodInvoker)(() => { materialLabel1.Visible = false; }));

                versionPanel.Invoke((MethodInvoker)(() => { versionPanel.Visible = false; }));
            }
        }

        private void DownloadNewVersion()
        {
            materialProgressBar1.Value = 0;
            using (var saveFileDialog = new SaveFileDialog
                   {
                       Filter = @"Application file(*.exe)|*.exe|All files(*.*)|*.*",
                       FileName = "FCodePrinterInstaller.exe"
                   })
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var filename = saveFileDialog.FileName;
                    var folder = Path.GetDirectoryName(filename);

                    var link = FMarkApiService.DownloadAppUrl;
                    _ = Download(link, filename).ContinueWith(x => Task.Run(() =>
                    {
                        Process.Start("explorer.exe", folder);
                    }));
                }
            }
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            RemoveFromList(x => x.Checked);
        }

        private void RemoveFromList(Func<MaterialCheckbox, bool> pred)
        {
            while (materialCheckedListBox1.Items.Any(pred))
            {
                var item = (CustomMaterailCheckBox)materialCheckedListBox1.Items.FirstOrDefault(pred);
                materialCheckedListBox1.Execute(() => materialCheckedListBox1.Items.Remove(item));

                if (item != null)
                    _storateFiles.Remove(item.Value);
            }

            UpdateButtonsState();
            if (!materialCheckedListBox1.Items.Any())
                selectAllSwitch.Checked = false;
        }

        private void StrikeText(Func<MaterialCheckbox, bool> pred)
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
            const string filterDialog = "pdf files (*.pdf)|*.pdf| zip files (*.zip)|*.zip";

            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Registry
                    .GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders",
                        "{374DE290-123F-4565-9164-39C4925E467B}", string.Empty).ToString();
                openFileDialog.Filter = filterDialog;
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var filePath = openFileDialog.FileName;
                    _ext = filePath.Substring(filePath.Length - 3, 3);
                    switch (_ext)
                    {
                        case "zip":
                            await ExctractZip(filePath);
                            break;
                        case "pdf":
                        {
                            var print = new List<string> { filePath };
                            AddFilesPrint(print);
                            break;
                        }
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

        private void downloadButton_Click(object sender, EventArgs e)
        {
            DownloadNewVersion();
        }
    }
}