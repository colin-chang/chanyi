using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace System.Windows
{
    public static class PanelExpand
    {
        /// <summary>
        /// 设置所有可编辑ComboBox不合法内容默认选中第一项
        /// </summary>
        /// <param name="win"></param>
        /// <param name="cb"></param>
        public static void ResetComboBox(this Window win, ComboBox cb)
        {
            if (!cb.IsEditable)
                return;

            string defaultSelection = ConfigurationManager.AppSettings["formDefaultSelection"];
            string text = !string.IsNullOrWhiteSpace(cb.Text) ? cb.Text : defaultSelection;

            //防止可编辑的ComboBox默认选中模糊匹配的第一项
            var item = cb.Items.OfType<object>().Select(i => i.GetType().GetProperty(cb.DisplayMemberPath).GetValue(i, null).ToString().Trim().ToLower()).Where(t => string.Equals(t, cb.Text.Trim().ToLower())).FirstOrDefault();
            if (text != defaultSelection && item == null)
            {
                cb.SelectedValue = null;
                cb.Text = text;
            }

            if (cb.SelectedValue == null)
                cb.SelectedIndex = 0;
        }

        /// <summary>
        /// 设定可编辑的ComboBox不包含在Items中选中值
        /// </summary>
        /// <param name="uc"></param>
        /// <param name="cb"></param>
        public static void SetComboBox(this ContentControl uc, ComboBox cb)
        {
            if (!cb.IsEditable)
                return;

            string asKey = typeof(UserControl).IsAssignableFrom(uc.GetType()) ? "listDefaultSelection" : "formDefaultSelection";
            string defaultSelection =ConfigurationManager.AppSettings[asKey];
            string text = !string.IsNullOrWhiteSpace(cb.Text) ? cb.Text : defaultSelection;

            //防止可编辑的ComboBox默认选中模糊匹配的第一项
            var item = cb.Items.OfType<object>().Select(i => i.GetType().GetProperty(cb.DisplayMemberPath).GetValue(i, null).ToString().Trim().ToLower()).Where(t => string.Equals(t, cb.Text.Trim().ToLower())).FirstOrDefault();
            if (text != defaultSelection && item == null)
            {
                cb.SelectedValue = Guid.NewGuid().ToString();
                cb.Text = text;
            }

            //防止可编辑的CoboBox默认不区分大小写匹配
            if (cb.SelectedItem != null)
            {
                string selectText = cb.SelectedItem.GetType().GetProperty(cb.DisplayMemberPath).GetValue(cb.SelectedItem, null).ToString().Trim();
                string displayText = cb.Text.Trim();
                if (!string.Equals(selectText, displayText) && string.Equals(selectText.ToLower(), displayText.ToLower()))
                {
                    cb.SelectedItem = cb.Items.OfType<object>().Where(i => i.GetType().GetProperty(cb.DisplayMemberPath).GetValue(i, null).ToString().Trim() == displayText).FirstOrDefault();
                }
            }

            //防止可编辑的CoboBox写入不存在的值不更新SelectedValue
            if (cb.SelectedValue == null)
            {
                if (text != defaultSelection)
                    cb.SelectedValue = Guid.NewGuid().ToString();
                else
                    cb.SelectedValue = null;
                cb.Text = text;
            }
        }
    }
}
