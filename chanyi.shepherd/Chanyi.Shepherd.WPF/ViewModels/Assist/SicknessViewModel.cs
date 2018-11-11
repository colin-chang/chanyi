using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.WPF.Model;


namespace Chanyi.Shepherd.WPF.ViewModels.Assist
{
    abstract class SicknessViewModel : FormViewModel
    {
        private string keyWord;
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string KeyWord
        {
            get { return keyWord; }
            set
            {
                keyWord = value;
                this.RaisePropertyChanged("KeyWord");
            }
        }

        /// <summary>
        /// 搜素命令
        /// </summary>
        public abstract DelegateCommand<Button> SearchCommand { get; }

        protected abstract override void InitializeBindData();

        private ObservableCollection<Node> treeData;
        /// <summary>
        /// TreeView数据源
        /// </summary>
        public ObservableCollection<Node> TreeData
        {
            get { return treeData; }
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
        protected Node GetNodeById(string id)
        {
            foreach (Node node in this.TreeData)
            {
                if (node.Id == id)
                    return node;
                foreach (Node n in node.Children)
                {
                    if (n.Id == id)
                        return n;
                }
            }
            return null;

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
        }
    }
}
