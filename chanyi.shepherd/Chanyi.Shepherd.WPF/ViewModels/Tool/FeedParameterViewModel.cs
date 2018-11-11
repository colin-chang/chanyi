using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

using Microsoft.Practices.Prism.Commands;
using AutoMapper;

using Chanyi.Shepherd.QueryModel.Model.System;
using Chanyi.Shepherd.QueryModel.UpdateModel.System;
using Chanyi.Shepherd.QueryModel;
using Chanyi.Utility.Common;

namespace Chanyi.Shepherd.WPF.ViewModels.Tool
{
    class FeedParameterViewModel : AddViewModel
    {
        public FeedParameterViewModel()
        {
            Mapper.CreateMap<FeedCritical, UpdateCritical>();
            this.InitializeBindData();
        }
        public FeedParameterViewModel(bool withinInitilization) { }

        protected override void InitializeBindItem()
        {
            var feedParameters = this.Service.GetFeedCritical();
            this.UIDispatcher.Invoke(new Action(() =>
            {
                feedParameters.ForEach(fp => this.feedParameters.Add(fp));
            }), null);
        }


        private ObservableCollection<FeedCritical> feedParameters = new ObservableCollection<FeedCritical>();
        public ObservableCollection<FeedCritical> FeedParameters { get { return feedParameters; } }

        public DelegateCommand SubmitCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (!this.IsValid)
                    {
                        MessageBox.Show(this.Error, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;

                    }
                    if (this.FeedParameters.Where(bf => !bf.Value.IsFloat()).Count() > 0)
                    {
                        MessageBox.Show("临界值输入不合法!", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    List<UpdateCritical> updateFeedParameter = new List<UpdateCritical>();
                    this.FeedParameters.Each(fp => updateFeedParameter.Add(Mapper.Map<UpdateCritical>(fp)));
                    updateFeedParameter.Each(up =>
                    {
                        up.Type = SettingsEnum.FeedRemaindful;
                        up.OperatorId = this.UserId;
                    });

                    var result = this.Service.UpdateCritical(updateFeedParameter);

                    if (!ValidateFailedServiceResult<bool>(result))
                        return;
                    this.UpdateNotification();
                    MessageBox.Show("饲料临界值设置成功", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
    }
}
