using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using System.Windows.Media;
using System;
using System.Windows.Threading;
using System.Reflection;
using System.Collections;
using System.Linq;

using Chanyi.Shepherd.WPF.Views.View;
using Chanyi.Shepherd.WPF.Views;
using Chanyi.Shepherd.WPF.ViewModels;
using System.Collections.Generic;

namespace Chanyi.Shepherd.WPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //主线程操作UI的线程队列


            Application.Current.Properties["UIDispatcher"] = this.Dispatcher;

            //用户登录
            AccountWindow win = new AccountWindow();
            if (win.ShowDialog() != true)
            {
                this.Close();
                return;
            }



            InitializeComponent();
            this.ViewModel = new MainViewModel(this.pane, this.stusBar, this.pgr);
            this.DataContext = this.ViewModel;
        }

        public MainViewModel ViewModel { get; set; }

        private void MenuItemClick(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            MenuItem mi = e.OriginalSource as MenuItem;
            if (mi == null)
                return;

            var xml = mi.Header as XmlElement;
            if (xml == null)
                return;

            if (!xml.HasAttribute("View")) //执行操作
            {
                if (xml.HasAttribute("Operation"))
                    this.ViewModel.MenuItemOperationCommand.Execute(xml.Attributes["Operation"].Value);
            }
            else    //展示视图
            {
                if (!mi.HasItems)
                    this.ViewModel.MenuItemCommand.Execute(mi);
            }
        }

        private void Menu_Loaded(object sender, RoutedEventArgs e)
        {
            var menu = sender as Menu;
            if (menu == null)
                return;

            FilterMenuPermissions(menu.Items, disableMenuItems);
            disableMenuItems.ForEach(n => n.ParentNode.RemoveChild(n));
        }

        private void ToolBarClick(object sender, RoutedEventArgs e)
        {
            Button btn = e.OriginalSource as Button;
            this.ViewModel.ToolBarCommand.Execute(Assembly.GetExecutingAssembly().GetType(string.Join(".", Assembly.GetExecutingAssembly().GetName().Name, "Views", btn.Tag as string)));
        }

        private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
        {
            //delay the call to open the new window until the item selection code finishes up
            Dispatcher.BeginInvoke(new Action(() =>
            {
                e.Handled = true;
                TreeViewItem item = e.OriginalSource as TreeViewItem;
                if (item == null)
                    return;
                item.IsSelected = false;
                var xml = item.Header as XmlElement;
                if (xml == null)
                    return;
                if (IsLastLevel(xml))
                    this.ViewModel.LSideBarCommand.Execute(xml);
            }), DispatcherPriority.Background, null);
        }

        private void TreeView_Loaded(object sender, RoutedEventArgs e)
        {
            var tree = sender as TreeView;
            if (tree == null)
                return;

            FilterMenuPermissions(tree.Items, disableTreeViewItems);
            disableTreeViewItems.ForEach(n => n.ParentNode.RemoveChild(n));
        }

        List<XmlElement> disableTreeViewItems = new List<XmlElement>();

        List<XmlElement> disableMenuItems = new List<XmlElement>();

        /// <summary>
        /// 过滤没有权限的节点
        /// </summary>
        /// <param name="treeViewItems"></param>
        void FilterMenuPermissions(IEnumerable treeViewItems, List<XmlElement> disNodes)
        {
            foreach (var item in treeViewItems)
            {
                var tvi = item as XmlElement;
                if (tvi == null)
                    continue;

                if (!IsLastLevel(tvi))
                {
                    var nodes = tvi.ChildNodes;
                    FilterMenuPermissions(nodes, disNodes);//处理子节点

                    //处理完子节点处理自己,LsideBar.xml节点之间不能加注释
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        if (!disNodes.Contains(nodes[i]))
                            break;
                        if (i == nodes.Count - 1)
                            disNodes.Add(tvi);
                    }
                }
                else
                {
                    //过滤掉不需要权限控制的菜单项
                    string isNeedPermission = tvi.GetAttribute("IsNeedPermission");
                    if (!string.IsNullOrEmpty(isNeedPermission) && isNeedPermission == "False")
                        continue;

                    string view = tvi.GetAttribute("View");
                    bool isEnabled = this.ViewModel.Permissions.Where(p => p.URL == view).Count() > 0;
                    if (!isEnabled)
                        disNodes.Add(tvi);
                }
            }
        }

        bool IsLastLevel(XmlElement xml)
        {
            if (!xml.HasChildNodes)
                return true;
            if (xml.ChildNodes.Count == 1 && xml.ChildNodes[0] is XmlText)
                return true;
            return false;
        }

    }
}
