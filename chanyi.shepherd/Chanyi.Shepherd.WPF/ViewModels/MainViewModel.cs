using System;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Configuration;
using System.Windows.Threading;
using System.IO;
using System.Net.NetworkInformation;
using System.Windows.Controls.Primitives;
using Microsoft.Win32;
using System.Net.Sockets;

using Microsoft.Practices.Prism.Commands;
using ICSharpCode.SharpZipLib.Zip;

using Chanyi.Shepherd.WPF.Views;
using Chanyi.Shepherd.WPF.Views.View;
using Chanyi.Shepherd.WPF.UserControls;
using Chanyi.Shepherd.WPF.Properties;
using Chanyi.Shepherd.WPF.Views.Help;
using Chanyi.Utility.Common;
using Chanyi.Shepherd.WPF.Helper;
using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.WPF.ProductService;
using Chanyi.FtpUtility;
using Chanyi.SecurityUtility;


namespace Chanyi.Shepherd.WPF.ViewModels
{
    public class MainViewModel : FormViewModel
    {
        public MainViewModel(Grid pane, StatusBar bar, ProgressBar pgr)
        {
            this.Pane = pane;
            this.Pane.Children.Clear();
            this.Pane.Children.Add(new StartUC());//加载起始页

            this.StatusBar = bar;
            this.ProgressBar = pgr;

            Action initialize = () =>
            {
                this.InitializeStatusBar();
#if DEBUG
                return;
#endif
                this.ValidateSerialNumber();
                this.AutoUpdate();
            };
            initialize.BeginInvoke(ar => initialize.EndInvoke(ar as IAsyncResult), initialize);
        }

        /// <summary>
        /// 内容面板容器
        /// </summary>
        public Grid Pane { get; set; }

        public StatusBar StatusBar { get; set; }

        private string operationDesc;
        public override string OperationDesc
        {
            get { return operationDesc; }
            set
            {
                operationDesc = value;
                this.RaisePropertyChanged("OperationDesc");
            }
        }

        public ProgressBar ProgressBar { get; set; }

        private int progressBarValue;
        public override int ProgressBarValue
        {
            get { return progressBarValue; }
            set
            {
                if (value >= 0)
                    this.ShowProgress();
                if (value >= 100)
                    this.HideProgress();
                progressBarValue = value;
                this.RaisePropertyChanged("ProgressBarValue");
            }
        }

        #region 通知消息

        private string notification = ConfigurationManager.AppSettings["loaddingNotification"];
        private string defaultNotification = ConfigurationManager.AppSettings["defaultNotification"];

        public string Notification
        {
            get { return notification; }
            set
            {
                notification = value;
                this.RaisePropertyChanged("Notification");
            }
        }

        List<string> notifications = new List<string>();
        void InitializeNotification()
        {
            LoadNotification();
            while (true)
            {
                if (notifications.Count() <= 0)
                    continue;


                this.UIDispatcher.Invoke(new Action(() =>
                {
                    DispatcherTimer timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromSeconds(3);
                    Random rdm = new Random();
                    timer.Tick += (sender, e) => this.Notification = notifications[rdm.Next(0, notifications.Count())];
                    timer.Start();
                }), null);
                break;
            }
        }

        public void LoadNotification()
        {
            Action notify = () =>
            {
                var delivery = this.Service.GetPreDeliverySerialNumber();
                var ablactation = this.Service.GetPreAblactationSerialNumber();
                var feed = this.Service.GetFeedRemaindful();
                var medicine = this.Service.GetMedicineRemaindful();

                string deliveryNotification = ConfigurationManager.AppSettings["deliveryNotification"];
                string ablactationNotification = ConfigurationManager.AppSettings["ablactationNotification"];
                string feedNotification = ConfigurationManager.AppSettings["feedNotification"];
                string medicineNotification = ConfigurationManager.AppSettings["medicineNotification"];

                delivery.ForEach(d => notifications.Add(string.Format(deliveryNotification, d)));
                ablactation.ForEach(a => notifications.Add(string.Format(ablactationNotification, a)));
                feed.ForEach(f => notifications.Add(string.Format(feedNotification, f.Name, f.Type, f.Area)));
                medicine.ForEach(m => notifications.Add(string.Format(medicineNotification, m.Name, m.Manufacturer)));
                if (notifications.Count() <= 0)
                    this.notifications.Add(defaultNotification);
            };
            notify.BeginInvoke(ar => notify.EndInvoke(ar as IAsyncResult), notify);
        }


        public DelegateCommand NotificationCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NotificationWindow win = new NotificationWindow();
                    win.ShowDialog();
                });
            }
        }

        #endregion

        void InitializeStatusBar()
        {
            InitializeNotification();
        }

        #region 菜单栏

        //展示视图
        public DelegateCommand<MenuItem> MenuItemCommand
        {
            get
            {
                return new DelegateCommand<MenuItem>(mi =>
                {
                    var xml = mi.Header as XmlElement;
                    var ele = xml.Attributes;

                    string viewsPath = Assembly.GetEntryAssembly().FullName.Split(',')[0] + ".Views.";
                    Type type = Type.GetType(viewsPath + ele["View"].Value);
                    if (typeof(Window).IsAssignableFrom(type))
                    {
                        Window win = Activator.CreateInstance(type) as Window;
                        win.Owner = Application.Current.MainWindow;
                        win.ShowDialog();
                    }
                    else if (typeof(UserControl).IsAssignableFrom(type))
                    {
                        object uc;
                        if (xml.HasAttribute("Header") && xml.HasAttribute("Icon"))
                        {
                            string header = ele["Header"].Value;
                            string icon = ele["Icon"].Value;
                            string intro = xml.InnerText;
                            uc = Activator.CreateInstance(type, header, icon, intro);
                        }
                        else
                            uc = Activator.CreateInstance(type);
                        this.Pane.Children.Clear();
                        this.Pane.Children.Add(uc as UserControl);
                    }
                });
            }
        }

        //执行操作
        public DelegateCommand<string> MenuItemOperationCommand
        {
            get
            {
                return new DelegateCommand<string>(operationKey =>
                {
                    var func = this.GetType().GetMethod(operationKey);
                    if (func == null)
                        return;
                    func.Invoke(this, null);
                });
            }
        }

        #region 菜单栏操作

        //退出
        public void Exit()
        {
            this.CurrentWindow.Close();
        }

        //切换状态栏
        public void ToggleStatusBar()
        {
            this.StatusBar.Visibility = this.StatusBar.IsVisible ? Visibility.Collapsed : Visibility.Visible;
        }

        //查看帮助
        public void ViewDocument()
        {
            LoaddingWindow loadding = new LoaddingWindow();
            //关闭加载窗口
            Action loaded = () => this.UIDispatcher.Invoke(new Action(() => loadding.Close()), null);

            Action load = () =>
            {
                bool handed = false;
                if (this.NetworkAvailable)
                {
                    string ip = ConfigurationManager.AppSettings["supportHost"];
                    try
                    {
                        PingReply reply = new Ping().Send(ip);
                        if (reply.Status == IPStatus.Success)
                        {
                            loaded();
                            Process.Start(ConfigurationManager.AppSettings["instructionUrl"]);
                            handed = true;
                        }
                    }
                    catch
                    {
                        handed = false;
                    }
                }
                if (!handed)
                {
                    loaded();
                    Process.Start(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\Document.chm"));
                }
            };
            load.BeginInvoke(ar => load.EndInvoke(ar as IAsyncResult), load);
            loadding.ShowDialog();
        }

        #region 检测更新

        string newVersion = string.Empty;

        public void CheckUpdate()
        {
            Action checkUpdate = () =>
            {
                try
                {
                    this.ProgressBarValue = 0;
                    this.OperationDesc = "正在检查网络…";
                    if (!this.NetworkAvailable)
                    {
                        this.OperationDesc = "网络不可用";
                        return;
                    }
                    if (!this.NetworkAvailable)
                        return;

                    string ip = ConfigurationManager.AppSettings["supportHost"];
                    bool netsuc = false;
                    try
                    {
                        PingReply reply = new Ping().Send(ip);
                        if (reply.Status == IPStatus.Success)
                            netsuc = true;
                    }
                    catch
                    {
                        netsuc = false;
                    }
                    if (!netsuc)
                        return;
                    using (var query = new QueryServiceClient())
                    {
                        this.OperationDesc = "正在检索新版本…";
                        newVersion = string.IsNullOrWhiteSpace(newVersion) ? query.GetProductLatestVersion(Resources.ProductId) : newVersion;
                        this.ProgressBarValue += 10;
                        if (string.Compare(Assembly.GetEntryAssembly().GetName().Version.ToString(), newVersion) >= 0)
                        {
                            this.ProgressBarValue = 100;
                            this.OperationDesc = "已是最新版本";
                            return;
                        }
                        this.UIDispatcher.Invoke(new Action(() =>
                        {
                            CheckUpdateWindow win = new CheckUpdateWindow(newVersion) { Owner = Application.Current.MainWindow };
                            if (win.ShowDialog() != true)
                            {
                                this.ProgressBarValue = 100;
                                this.OperationDesc = string.Empty;
                                return;
                            }
                            Action<string> download = Download;
                            download.BeginInvoke(newVersion, ar => download.EndInvoke(ar as IAsyncResult), download);
                        }), null);
                    }
                }
                catch (Exception ex)
                {
                    this.OperationDesc = string.Empty;
                    this.HideProgress();
                    this.UIDispatcher.Invoke(new Action(() => MessageBox.Show(Application.Current.MainWindow, ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error)), null);
                }
            };
            checkUpdate.BeginInvoke(ar => checkUpdate.EndInvoke(ar as IAsyncResult), checkUpdate);
        }

        private void Download(string version)
        {
            this.OperationDesc = "连接服务器…";
            string host = ConfigurationManager.AppSettings["ftpHost"];
            string uid = ConfigurationManager.AppSettings["ftpUid"];
            string pwd = ConfigurationManager.AppSettings["ftpPwd"];
            FtpClient ftp = new FtpClient(host, uid, pwd);
            this.OperationDesc = "连接服务器成功…";
            this.ProgressBarValue += 10;

            string updatePath = ConfigurationManager.AppSettings["updatePath"];
            string productName = ConfigurationManager.AppSettings["productName"];
            string romatePath = Path.Combine(updatePath, productName + version + ".zip");
            this.OperationDesc = "获取更新信息…";
            long fileSize = ftp.GetFileSize(romatePath);
            this.ProgressBarValue += 30;
            string localPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Download", productName + version + ".zip");
            if (!Directory.Exists(Path.GetDirectoryName(localPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(localPath));
            if (System.IO.File.Exists(localPath))
                System.IO.File.Delete(localPath);
            this.OperationDesc = "开始下载…";
            int completedLength = 0;
            bool downloadSuccess = ftp.Download(romatePath, localPath, length => this.ProgressBarValue += Convert.ToInt32((completedLength += length) * 1.0 / fileSize * 25));
            //this.ProgressBarValue += 25;       
            if (downloadSuccess)
            {
                this.OperationDesc = "下载完成";
                UnZipFile(localPath);
            }
        }

        private void UnZipFile(string path)
        {
            try
            {
                this.OperationDesc = "解压文件…";
                using (ZipInputStream stream = new ZipInputStream(System.IO.File.OpenRead(path)))
                {
                    ZipEntry theEntry;
                    string excPath = string.Empty;
                    while ((theEntry = stream.GetNextEntry()) != null)
                    {
                        string zipfilename = System.IO.Path.GetFileName(theEntry.Name);
                        if (string.IsNullOrEmpty(zipfilename))
                            return;
                        string fileName = Path.Combine(Path.GetDirectoryName(path), zipfilename);
                        string exts = ConfigurationManager.AppSettings["supportExtentsion"];
                        exts.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(ext =>
                        {
                            if (string.Equals(ext, Path.GetExtension(zipfilename), StringComparison.CurrentCultureIgnoreCase))
                                excPath = fileName;
                        });

                        using (FileStream fs = System.IO.File.OpenWrite(fileName))
                        {
                            int size = 1024 * 2;
                            byte[] buffer = new byte[size];
                            while ((size = stream.Read(buffer, 0, buffer.Length)) > 0)
                                fs.Write(buffer, 0, size);
                        }
                    }
                    this.OperationDesc = "解压完成成功";
                    this.ProgressBarValue += 20;
                    Setup(excPath);
                }
            }
            catch (Exception ex)
            {
                this.OperationDesc = string.Empty;
                this.ProgressBarValue = 100;
                throw new Exception("程序异常：" + ex.Message);
            }
        }

        private void Setup(string path)
        {
            try
            {
                this.OperationDesc = "启动更新…";
                this.ProgressBarValue += 5;
                this.UIDispatcher.Invoke(new Action(() =>
                {
                    if (MessageBox.Show(Application.Current.MainWindow, "更新需要关闭当前应用程序，是否继续？", "提醒", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                    {
                        this.OperationDesc = string.Empty;
                        return;
                    }
                    new Process { StartInfo = new ProcessStartInfo(path) }.Start();
                    Application.Current.Shutdown();
                }), null);
            }
            catch (Exception ex)
            {
                this.OperationDesc = string.Empty;
                this.ProgressBarValue = 100;
                throw new Exception("安装更新失败：" + ex.Message);
            }
        }

        #endregion

        #endregion

        #endregion

        //工具栏
        public DelegateCommand<Type> ToolBarCommand
        {
            get
            {
                return new DelegateCommand<Type>(t =>
                {
                    if (!typeof(Window).IsAssignableFrom(t))
                        return;

                    Window win;
                    win = t.GetConstructor(new Type[] { }) != null ? Activator.CreateInstance(t) as Window : t.GetConstructors().FirstOrDefault().Invoke(new object[] { InOutWarehouseDirectionEnum.Out }) as Window;
                    win.Owner = Application.Current.MainWindow;
                    win.ShowDialog();
                });
            }
        }

        //左边栏
        public DelegateCommand<XmlElement> LSideBarCommand
        {
            get
            {
                return new DelegateCommand<XmlElement>(xml =>
                {
                    var ele = xml.Attributes;

                    string viewsPath = Assembly.GetEntryAssembly().FullName.Split(',')[0] + ".Views.";
                    Type type = Type.GetType(viewsPath + ele["View"].Value);
                    if (type == this.Pane.Children[0].GetType())
                        return;

                    if (typeof(Window).IsAssignableFrom(type))
                    {
                        Window win = Activator.CreateInstance(type) as Window;
                        win.Owner = Application.Current.MainWindow;
                        win.ShowDialog();
                    }
                    else if (typeof(UserControl).IsAssignableFrom(type))
                    {
                        string header = ele["Header"].Value;
                        string icon = ele["Icon"].Value;
                        string intro = xml.InnerText;
                        var uc = Activator.CreateInstance(type, header, icon, intro);
                        this.Pane.Children.Clear();
                        this.Pane.Children.Add(uc as UserControl);
                    }
                });
            }
        }

        //检测软件注册状态
        void ValidateSerialNumber()
        {
            string sn = Settings.Default.SerialNumber;
            string rsn = string.Empty;
            bool hsn = !string.IsNullOrWhiteSpace(sn);
            sn = hsn ? sn : new RegistryHelper("Shepherd")["SerialNumber"];

            if (string.IsNullOrWhiteSpace(sn) || !LicenseHelper.ValidateLicense(Resources.SerialId, sn, Resources.PubKey))
            {
                this.UIDispatcher.Invoke(new Action(() =>
                {
                    RegisterWindow win = new RegisterWindow();
                    win.Owner = Application.Current.MainWindow;
                    if (win.ShowDialog() == false)
                        Application.Current.Shutdown();
                }), null);
            }
            else
            {
                if (hsn)
                    return;

                Settings.Default.SerialNumber = rsn;
                Settings.Default.Save();
            }
        }

        /// <summary>
        /// 自动更新
        /// </summary>
        void AutoUpdate()
        {
            Action autoUpdate = () =>
            {
                if (!Settings.Default.AutoUpdate)
                    return;
                if (!this.NetworkAvailable)
                    return;
                using (var query = new QueryServiceClient())
                {
                    try
                    {
                        newVersion = query.GetProductLatestVersion(Resources.ProductId);
                        if (string.Compare(Settings.Default.SkipVersion, newVersion) >= 0)
                            return;

                        this.CheckUpdate();
                    }
                    catch (SocketException ex)
                    {
                        //网络连接失败
                        if (ex.ErrorCode == 10060)
                            return;
                    }
                    catch (Exception ex)
                    {

                    }
                }
            };

            autoUpdate.BeginInvoke(ar => autoUpdate.EndInvoke(ar as IAsyncResult), autoUpdate);
        }
    }
}
