using System.Collections.ObjectModel;
using System.ComponentModel;
using AutoMapper;

namespace Chanyi.Shepherd.WPF.Model
{
    class NodeData : INotifyPropertyChanged
    {
        public NodeData()
        {
            this.IsChecked = false;
            this.IsSelected = false;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        private ObservableCollection<NodeData> children;

        public ObservableCollection<NodeData> Children
        {
            get
            {
                if (children == null)
                    children = new ObservableCollection<NodeData>();
                return children;
            }
            set
            {
                this.children = value;
            }
        }

        private bool? isChecked;

        public virtual bool? IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                if (IsChecked != null)
                    this.Children.Each(c => c.IsChecked = this.IsChecked);
                this.RaisePropertyChanged("IsChecked");
            }
        }

        private bool isSelected;

        public virtual bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                this.RaisePropertyChanged("IsSelected");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    class Node : NodeData
    {
        private ObservableCollection<Node> children;

        public new ObservableCollection<Node> Children
        {
            get
            {
                if (children == null)
                    children = new ObservableCollection<Node>();
                return children;
            }
            set
            {
                this.children = value;
            }
        }

        private bool? isChecked;

        public override bool? IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                if (IsChecked != null)
                    this.Children.Each(c => c.IsChecked = this.IsChecked);
                this.RaisePropertyChanged("IsChecked");
            }
        }


        private long? relevancy;

        public long? Relevancy
        {
            get { return relevancy; }
            set
            {
                relevancy = value;
                this.RaisePropertyChanged("Relevancy");
            }
        }
    }
}
