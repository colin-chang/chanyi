using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.IServices;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;


namespace Chanyi.Shepherd.WPF.ViewModels
{
    /// <summary>
    /// 表单窗口ViewModel
    /// </summary>
    public abstract class FormViewModel : BaseViewModel
    {

        /// <summary>
        /// 带校验的提交数据命令
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        protected DelegateCommand GetSubmitCommand(Action action)
        {
            return new DelegateCommand(() =>
            {
                if (!IsValid)
                    return;

                action();
            });
        }

        /// <summary>
        /// 提交数据命令
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        protected DelegateCommand<T> GetSubmitCommand<T>(Action<T> action) where T : UIElement
        {
            return GetSubmitCommand<T>(null, null, action);
        }

        protected DelegateCommand<T> GetSubmitCommand<T>(Action beforeValidate, Action<T> action) where T : UIElement
        {
            return GetSubmitCommand<T>(null, beforeValidate, action);
        }

        protected DelegateCommand<T> GetSubmitCommand<T>(Func<bool> verifyRule, Action<T> action) where T : UIElement
        {
            return GetSubmitCommand<T>(verifyRule, null, action);
        }

        protected DelegateCommand<T> GetSubmitCommand<T>(Func<bool> verifyRule, Action beforeValidate, Action<T> action) where T : UIElement
        {
            return new DelegateCommand<T>(p =>
            {
                //获取焦点
                p.Focus();
                if (beforeValidate != null)
                    beforeValidate();

                if (verifyRule == null)
                {
                    if (!IsValid)
                        return;
                }
                else
                {
                    if (!ConditionalVerify(verifyRule))
                        return;
                }

                action(p);
            });
        }

        bool ConditionalVerify(Func<bool> verifyRule)
        {

            if (errors.Count() > 0)
                return false;

            foreach (var p in this.GetType().GetProperties())
            {
                if (!p.CanWrite)
                    continue;
                if (Attribute.GetCustomAttributes(p, typeof(ValidationAttribute), true).Count() <= 0)
                    continue;
                if (!p.IsDefined(typeof(ConditionalVerifyAttribute), true))
                    p.SetValue(this, p.GetValue(this, null), null);
                else
                {
                    if (!verifyRule())
                        continue;

                    p.SetValue(this, p.GetValue(this, null), null);
                }
            }

            return errors.Count() <= 0;
        }

        /// <summary>
        /// 处理服务返回的未成功状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result">服务返回结果</param>
        /// <returns></returns>
        protected override bool ValidateFailedServiceResult<T>(ServiceResult<T> result)
        {
            if (typeof(T) == typeof(bool) && Convert.ToBoolean(result.Result))
                return true;

            if (result.Status == ResultStatus.WANING)
            {
                serviceError[ResultStatus.WANING.ToString()] = result.Message;
                RaisePropertyChanged("Error");
                return false;
            }
            serviceError.Remove(ResultStatus.WANING.ToString());

            if (result.Status == ResultStatus.Unknown || result.Status == ResultStatus.ERROR)
            {
                MessageBox.Show(string.Format("程序异常：点击确定关闭此窗口。\r\n错误码：{0},请联系软件提供商", result.Code), "程序异常", MessageBoxButton.OK, MessageBoxImage.Error);
                this.CurrentWindow.DialogResult = false;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 初始化绑定展示数据
        /// </summary>
        protected virtual void InitializeBindItem()
        {
        }

        public virtual DelegateCommand CancelCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    this.CurrentWindow.DialogResult = false;
                });
            }
        }
    }
}
