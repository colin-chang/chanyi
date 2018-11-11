using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.Prism.Commands;

namespace Chanyi.Shepherd.WPF.ViewModels
{
    abstract class EditViewModel : FormViewModel
    {
        /// <summary>
        /// 查询要编辑的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected abstract object GetEditModel();

        protected override void InitializeBindData()
        {
            Action initializeBindData = () =>
            {
                if (this.IsInDesignMode) return;
                this.InitializeBindItem();

                var model = this.GetEditModel();
                if (model == null) return;
                var ps = this.GetType().GetProperties();
                model.GetType().GetProperties().ToList().ForEach(p =>
                {
                    var prop = ps.Where(pi => pi.Name == p.Name).FirstOrDefault();
                    if (prop != null)
                    {
                        var modelType = p.PropertyType;
                        var thisType = prop.PropertyType;
                        var value = p.GetValue(model, null);
                        if (modelType == thisType)
                            prop.SetValue(this, value, null);
                        if (thisType == typeof(string) && (value is ValueType))
                            prop.SetValue(this, value.ToString(), null);
                    }
                });
            };
            initializeBindData.BeginInvoke(ar => initializeBindData.EndInvoke(ar as IAsyncResult),initializeBindData);
        }
    }
}
