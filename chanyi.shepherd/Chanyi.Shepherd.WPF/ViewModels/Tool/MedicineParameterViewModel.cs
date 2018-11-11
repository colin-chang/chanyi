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
    class MedicineParameterViewModel:AddViewModel
    {
         public MedicineParameterViewModel()
        {
            Mapper.CreateMap<MedicineCritical, UpdateCritical>();
            this.InitializeBindData();
        }
         public MedicineParameterViewModel(bool withinInitilization) { }

        protected override void InitializeBindItem()
        {
            var medicineParameters = this.Service.GetMedicineCritical();
            this.UIDispatcher.Invoke(new Action(() =>
            {
                medicineParameters.ForEach(fp => this.medicineParameters.Add(fp));
            }), null);
        }


        private ObservableCollection<MedicineCritical> medicineParameters = new ObservableCollection<MedicineCritical>();
        public ObservableCollection<MedicineCritical> MedicineParameters { get { return medicineParameters; } }
        
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
                    if (this.MedicineParameters.Where(bf => !bf.Value.IsFloat()).Count() > 0)
                    {
                        MessageBox.Show("临界值输入不合法!", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    List<UpdateCritical> updateFeedParameter = new List<UpdateCritical>();
                    this.MedicineParameters.Each(fp => updateFeedParameter.Add(Mapper.Map<UpdateCritical>(fp)));
                    updateFeedParameter.Each(up => {
                        up.Type = SettingsEnum.FeedRemaindful;
                        up.OperatorId = this.UserId;
                    });

                    var result = this.Service.UpdateCritical(updateFeedParameter);

                    if (!ValidateFailedServiceResult<bool>(result))
                        return;
                    this.UpdateNotification();
                    MessageBox.Show("药品临界值设置成功", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
    }
}

