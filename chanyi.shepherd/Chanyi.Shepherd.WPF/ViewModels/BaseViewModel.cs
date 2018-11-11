using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

using Chanyi.Shepherd.IServices;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using System.Runtime.InteropServices;
using Chanyi.Shepherd.WPF.Helper;
using Chanyi.Shepherd.QueryModel.Model.System;

namespace Chanyi.Shepherd.WPF.ViewModels
{
    public abstract class BaseViewModel : NotificationObject, IDataErrorInfo
    {
        /// <summary>
        /// 当前登录用户ID
        /// </summary>
        public string UserId
        {
            get
            {
                return Application.Current.Properties["Uid"] as string;
            }
            set
            {
                Application.Current.Properties["Uid"] = value;
            }
        }

        public List<Permission> Permissions
        {
            get
            {
                return Application.Current.Properties["Permissions"] as List<Permission>;
            }
            set
            {
                Application.Current.Properties["Permissions"] = value;
            }
        }


        /// <summary>
        /// 当前活动窗口
        /// </summary>
        protected Window CurrentWindow
        {
            get
            {
                return Application.Current.Windows.OfType<Window>().Where(w => w.IsActive).FirstOrDefault();
            }
        }

        [DllImport("wininet")]
        private extern static bool InternetGetConnectedState(out int connectionDescription, int reservedValue);
        /// <summary>
        /// 网络是否可用
        /// </summary>
        public bool NetworkAvailable
        {
            get
            {
                int i = 0;
                return InternetGetConnectedState(out i, 0);
            }
        }

        /// <summary>
        /// 是否支持此操作系统
        /// </summary>
        public bool IsOSSupported { get { return CompatibilityHelper.IsOSSupported(); } }

        /// <summary>
        /// 当前ViewModel的Type
        /// </summary>
        public Type CurrentType { get { return this.GetType(); } }

        public BaseViewModel()
        {
            //初始化服务（用于提升性能）
            Action InitializeService = () =>
            {
                //初始化Spring注入
                IService service = this.Service;
            };
            if (!IsInDesignMode)
                InitializeService.BeginInvoke(ar => InitializeService.EndInvoke(ar as IAsyncResult), InitializeService);
        }

        /// <summary>
        /// 判断是否处于设计器模式
        /// </summary>
        protected bool IsInDesignMode
        {
            get { return (bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue; }
        }

        /// <summary>
        /// 处理服务返回的未成功状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual bool ValidateFailedServiceResult<T>(ServiceResult<T> result)
        {
            return true;
        }

        /// <summary>
        /// 服务对象
        /// </summary>
        protected IService Service { get { return this.GetService(); } }

        /// <summary>
        /// 全局共享内存KeyValue表
        /// </summary>
        protected IDictionary Session { get { return Application.Current.Properties; } }

        /// <summary>
        /// 主线程UI操作队列
        /// </summary>
        protected Dispatcher UIDispatcher { get { return (Session["UIDispatcher"] as Dispatcher); } }

        /// <summary>
        /// 初始化绑定展示内容
        /// </summary>
        protected virtual void InitializeBindData()
        {
            Reset();
        }

        /// <summary>
        /// 重置当前ViewModel
        /// </summary>
        /// <typeparam name="T">ViewModel类型</typeparam>
        protected virtual void Reset()
        {
            var defaultModel = Activator.CreateInstance(this.CurrentType, (object)true);
            this.GetType().GetProperties().Where(p => p.CanWrite && Attribute.IsDefined(p, typeof(EntityPropertyAttribute))).ToList().ForEach(p => p.SetValue(this, this.CurrentType.GetProperty(p.Name).GetValue(defaultModel, null), null));
        }

        #region 数据校验


        /// <summary>
        /// 客服端错误消息集合
        /// </summary>
        protected Dictionary<string, string> errors = new Dictionary<string, string>();

        /// <summary>
        /// 服务错误集合
        /// </summary>
        protected Dictionary<string, string> serviceError = new Dictionary<string, string>();

        protected void SetError(string key, string value)
        {
            SetError(key, value, false);
        }

        protected void SetError(string key, string value, bool fromService)
        {
            if (!fromService)
            {
                if (!string.IsNullOrWhiteSpace(value))
                    this.errors[key] = value;
                else
                    this.errors.Remove(key);

            }
            else
            {
                if (!string.IsNullOrWhiteSpace(value))
                    this.serviceError[key] = value;
                else
                    this.serviceError.Remove(key);
            }
            this.RaisePropertyChanged("Error");
        }

        public string Error
        {
            get
            {
                if (errors.Count() > 0)
                    return errors.FirstOrDefault().Value;
                if (serviceError.Count() > 0)
                    return serviceError.FirstOrDefault().Value;
                return string.Empty;
            }
        }

        public string this[string columnName]
        {
            get
            {
                var vc = new ValidationContext(this, null, null);
                vc.MemberName = columnName;
                var res = new List<ValidationResult>();
                var result = Validator.TryValidateProperty(this.GetType().GetProperty(columnName).GetValue(this, null), vc, res);
                if (res.Count > 0)
                    return string.Join(Environment.NewLine, res.Select(r => r.ErrorMessage).ToArray());
                return string.Empty;
            }
        }

        /// <summary>
        /// 客户端是否存在错误
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (errors.Count() > 0) return false;

                this.GetType().GetProperties().ToList().ForEach(p =>
                {
                    if (p.CanWrite && Attribute.GetCustomAttributes(p, typeof(ValidationAttribute), true).Count() > 0)
                        p.SetValue(this, p.GetValue(this, null), null);
                });
                return errors.Count() <= 0;
            }
        }

        /// <summary>
        /// 属性Set校验
        /// </summary>
        /// <param name="propName"></param>
        protected void Validate(string propName)
        {
            this.RaisePropertyChanged(propName);
            if (string.IsNullOrWhiteSpace(this[propName]))
                errors.Remove(propName);
            else
                errors[propName] = this[propName];
            this.RaisePropertyChanged("Error");
        }

        #endregion

        #region 公共操作

        public MainViewModel MainViewModel
        {
            get
            {
                return this.UIDispatcher.Invoke(new Func<object>(() => Application.Current.MainWindow.DataContext), null) as MainViewModel;
            }
        }

        /// <summary>
        /// 更新状态栏通知消息
        /// </summary>
        protected void UpdateNotification()
        {
            if (MainViewModel != null)
                MainViewModel.LoadNotification();
        }

        /// <summary>
        /// 显示状态栏进度条
        /// </summary>
        public void ShowProgress()
        {
            this.UIDispatcher.Invoke(new Action(() => MainViewModel.ProgressBar.Visibility = Visibility.Visible), DispatcherPriority.Send, null);
        }

        /// <summary>
        /// 隐藏状态栏进度条
        /// </summary>
        public void HideProgress()
        {
            this.UIDispatcher.Invoke(new Action(() => MainViewModel.ProgressBar.Visibility = Visibility.Collapsed), DispatcherPriority.Send, null);
        }

        public virtual string OperationDesc
        {
            get
            {
                return MainViewModel.OperationDesc;
            }
            set
            {
                MainViewModel.OperationDesc = value;
            }
        }

        public virtual int ProgressBarValue
        {
            get
            {
                return MainViewModel.ProgressBarValue;
            }
            set
            {
                MainViewModel.ProgressBarValue = value;
            }
        }

        #endregion
    }
}