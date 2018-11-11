using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Chanyi.Shepherd.WPF.Model
{
    /// <summary>
    /// 系谱里专用的按钮
    /// </summary>
    public class FamilyTreeButton : Button
    {
        public FamilyTreeButton(float width, float height, string content, string tag, float left, float top)
        {
            this.Width = width;
            this.Height = height;
            this.Content = content.Length > 4 ? (content.Substring(0, 4) + "...") : content;
            this.Tag = tag;
            this.ToolTip = content;
            this.BorderThickness = new Thickness(0);
            this.Cursor = Cursors.Hand;
            this.Margin = new Thickness(left, top, 0, 0);
            //this.Foreground = Brushes.YellowGreen;

            this.SetBinding(Button.CommandProperty, new Binding("LoadCommand"));
            this.CommandParameter = this.Tag;
        }

        string SubButtonContent(string content)
        {

            return content;
        }
    }
}
