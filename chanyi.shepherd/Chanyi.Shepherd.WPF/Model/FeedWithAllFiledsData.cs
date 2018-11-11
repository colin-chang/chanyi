using System.ComponentModel;
using Chanyi.Shepherd.QueryModel.Model.Input;


namespace Chanyi.Shepherd.WPF.Model
{
    class FeedWithAllFiledsData: FeedWithAllFileds,INotifyPropertyChanged
    {
        private bool isChecked;

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                this.RaisePropertyChanged("IsChecked");
            }
        }

        private float dosage;

        public float Dosage
        {
            get { return dosage; }
            set
            {
                dosage = value;
                this.RaisePropertyChanged("Dosage");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); 
        }
    }
}
