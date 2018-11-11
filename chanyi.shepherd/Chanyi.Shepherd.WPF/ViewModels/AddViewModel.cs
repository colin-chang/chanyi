using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.IServices;
using System.Windows.Threading;

namespace Chanyi.Shepherd.WPF.ViewModels
{
    public abstract class AddViewModel : FormViewModel
    {

        /// <summary>
        /// CoboBox默认文本
        /// </summary>
        protected string defaultSelection = ConfigurationManager.AppSettings["formDefaultSelection"];

        /// <summary>
        /// 当期窗口状态
        /// </summary>
        protected bool dialogResult = false;

        /// <summary>
        /// 错误控件
        /// </summary>
        protected UIElement errorControl;

        /// <summary>
        /// 是否继续添加
        /// </summary>
        protected bool isContinue = false;

        protected override bool ValidateFailedServiceResult<T>(ServiceResult<T> result)
        {
            bool b = base.ValidateFailedServiceResult<T>(result);
            if (b) this.dialogResult = true;
            return b;
        }

        protected override void InitializeBindData()
        {
            if (this.IsInDesignMode) return;

            Action init = () =>
            {
                ToggleError(Visibility.Hidden);
                this.InitializeBindItem();
                base.InitializeBindData();
                this.ClearErrors();
                ToggleError(Visibility.Visible);
            };
            init.BeginInvoke(ar => init.EndInvoke(ar as IAsyncResult), null);
        }

        void ToggleError(Visibility visibility)
        {
            this.UIDispatcher.Invoke(new Action(() =>
            {
                if (this.errorControl != null)
                    this.errorControl.Visibility = visibility;
            }), DispatcherPriority.Send, null);
        }

        /// <summary>
        /// 是否继续添加数据
        /// </summary>
        /// <typeparam name="T">ViewModel</typeparam>
        /// <param name="msg">成功消息</param>
        /// <returns></returns>
        protected bool Continue2Add(string msg)
        {
            bool result = false;
            Func<bool> goon = () =>
            {
                if (MessageBox.Show(this.CurrentWindow, msg + ",是否继续添加！", "添加成功", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    this.CurrentWindow.Closing -= CurrentWindowClosing;
                    this.CurrentWindow.Closing += CurrentWindowClosing;

                    this.isContinue = true;
                    this.InitializeBindData();
                    return result = true;
                }
                return result = false;
            };

            if (this.UIDispatcher.Thread == Thread.CurrentThread)
                goon();
            else
                this.UIDispatcher.Invoke(new Action(() => goon()), null);
            return result;
        }

        void CurrentWindowClosing(object sender, EventArgs e)
        {
            this.CurrentWindow.DialogResult = this.dialogResult;
        }

        /// <summary>
        /// 清除错误
        /// </summary>
        protected void ClearErrors()
        {
            this.errors.Clear();
            this.RaisePropertyChanged("Error");
        }

        public override DelegateCommand CancelCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    this.CurrentWindow.DialogResult = this.dialogResult;
                });
            }
        }
    }
}
