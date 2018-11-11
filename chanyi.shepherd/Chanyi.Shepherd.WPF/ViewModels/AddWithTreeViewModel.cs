using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System;

using Microsoft.Practices.Prism.Commands;
using AutoMapper;

using Chanyi.Shepherd.WPF.Model;

namespace Chanyi.Shepherd.WPF.ViewModels
{
    abstract class AddWithTreeViewModel : AddViewModel
    {
        private ObservableCollection<NodeData> treeData;
        /// <summary>
        /// TreeView数据源
        /// </summary>
        public ObservableCollection<NodeData> TreeData
        {
            get
            {
                if (treeData == null)
                    treeData = new ObservableCollection<NodeData>();
                return treeData;
            }
            set
            {
                treeData = value;
                this.RaisePropertyChanged("TreeData");
            }
        }

        /// <summary>
        /// 树节点点击命令
        /// </summary>
        public abstract DelegateCommand<string> NodeClickCommand { get; }

        /// <summary>
        /// 查找指定ID的节点
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nodes"></param>
        /// <returns></returns>
        protected NodeData GetNodeById(string id)
        {
            foreach (NodeData node in this.TreeData)
            {
                if (node.Id == id)
                    return node;
                foreach (NodeData n in node.Children)
                {
                    if (n.Id == id)
                        return n;
                }
            }
            return null;
            #region 递归版
            //if (nodes == null)
            //    return null;
            //foreach (var node in nodes)
            //{
            //    if (string.Equals(node.Id, id))
            //        return node;
            //    var n = GetNodeById(id, node.Children);
            //    if (n != null) return n;
            //}
            //return null;
            #endregion
        }

        /// <summary>
        /// 设置节点选中状态
        /// </summary>
        /// <param name="node"></param>
        protected void SetNodeChecked(NodeData node, Action<NodeData> setSelectedItems)
        {
            TreeData.Each(n =>
            {
                if (n.Children.Count() > 0 && n.Children.Contains(node))
                {
                    int count = n.Children.Where(c => c.IsChecked == true).Count();
                    if (count <= 0)
                        n.IsChecked = false;
                    else if (count < n.Children.Count())
                        n.IsChecked = null;
                    else
                        n.IsChecked = true;
                }
            });

            setSelectedItems(node);
        }

        public virtual DelegateCommand<string> RemoveSymptopmCommand
        {
            get
            {
                return new DelegateCommand<string>(id =>
                {
                });
            }
        }
    }
}