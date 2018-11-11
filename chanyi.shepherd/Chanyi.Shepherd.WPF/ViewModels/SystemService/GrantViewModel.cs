using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel.Model;
using Chanyi.Shepherd.IServices;

namespace Chanyi.Shepherd.WPF.ViewModels.SystemService
{
    abstract class GrantViewModel<T> : AddViewModel where T : BaseModel
    {
        protected override abstract void InitializeBindItem();

        /// <summary>
        /// 主体对象ID 如：UserId
        /// </summary>
        public string ObjectId { get; set; }

        public Action<bool> GrantFailed { get; set; }

        public abstract string Title { get; }

        private string keyword;

        public string Keyword
        {
            get { return keyword; }
            set
            {
                keyword = value;
                this.RaisePropertyChanged("Keyword");
            }
        }



        private ObservableCollection<T> sourceItems = new ObservableCollection<T>();
        public ObservableCollection<T> SourceItems { get { return sourceItems; } }

        private ObservableCollection<T> targetItems = new ObservableCollection<T>();
        public ObservableCollection<T> TargetItems { get { return targetItems; } }

        public DelegateCommand<string> GrantCommand
        {
            get
            {
                return new DelegateCommand<string>(id =>
                {
                    var selectedItem = this.SourceItems.Where(ap => ap.Id == id).FirstOrDefault();
                    if (selectedItem == null)
                        return;

                    var current = this.TargetItems.Where(op => op.Id == id).FirstOrDefault();
                    if (current != null)
                        return;

                    this.TargetItems.Insert(0, selectedItem);
                    this.SourceItems.Remove(selectedItem);
                });
            }
        }

        public DelegateCommand<string> RemoveCommand
        {
            get
            {
                return new DelegateCommand<string>(id =>
                {
                    var current = this.TargetItems.Where(op => op.Id == id).FirstOrDefault();
                    if (current == null)
                        return;

                    var avperm = this.SourceItems.Where(ap => ap.Id == id).FirstOrDefault();
                    if (avperm != null)
                        return;

                    this.SourceItems.Insert(0, current);
                    this.TargetItems.Remove(current);
                });
            }
        }

        public DelegateCommand SearchCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    List<T> availablePerms = this.GetSourceItemsMethod()(this.Keyword, this.ObjectId);
                    this.UIDispatcher.Invoke(new Action(() =>
                    {
                        this.SourceItems.Clear();
                        availablePerms.ForEach(p => this.SourceItems.Add(p));
                    }));
                });
            }
        }


        public DelegateCommand SubmitCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    var result = this.GetGrantMethod()(this.TargetItems.Select(p => p.Id).ToList(), this.ObjectId);
                    if (this.GrantFailed != null)
                        GrantFailed(!result.Result);
                    if (result.Result)
                        this.Permissions = this.Service.GetAllPermissionByUserId(this.UserId);
                    this.CurrentWindow.DialogResult = result.Result;
                });
            }
        }

        /// <summary>
        /// 获取源数据方法
        /// </summary>
        /// <returns></returns>
        protected abstract Func<string, string, List<T>> GetSourceItemsMethod();

        /// <summary>
        /// 获取赋权限方法
        /// </summary>
        /// <returns></returns>
        protected abstract Func<List<string>, string, ServiceResult<bool>> GetGrantMethod();
    }
}
