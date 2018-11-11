using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Chanyi.Shepherd.QueryModel.Model.System;
using Chanyi.Shepherd.WPF.UserControls;

namespace Chanyi.Shepherd.WPF.ViewModels.View
{
    class NotificationViewModel : BaseViewModel
    {
        public NotificationViewModel(ProgressRing prDelivery, ProgressRing prAblactation, ProgressRing prFeed, ProgressRing prMedicine)
        {
            this.prDelivery = prDelivery;
            this.prAblactation = prAblactation;
            this.prFeed = prFeed;
            this.prMedicine = prMedicine;
            this.InitializeBindData();
        }

        ProgressRing prDelivery, prAblactation, prFeed, prMedicine;

        protected override void InitializeBindData()
        {
            Action initialize = () =>
            {
                var delivery = this.Service.GetPreDeliveryRemaindful();
                var ablactation = this.Service.GetPreAblactationRemaindful();
                var feed = this.Service.GetFeedRemaindful();
                var medicine = this.Service.GetMedicineRemaindful();

                this.UIDispatcher.Invoke(new Action(() =>
                {
                    delivery.ForEach(d => this.Delivery.Add(d));
                    this.prDelivery.Hide();
                    ablactation.ForEach(a => this.Ablactation.Add(a));
                    this.prAblactation.Hide();
                    feed.ForEach(f => this.Feed.Add(f));
                    this.prFeed.Hide();
                    medicine.ForEach(m => this.Medicine.Add(m));
                    this.prMedicine.Hide();
                }), null);
            };
            initialize.BeginInvoke(ar => initialize.EndInvoke(ar as IAsyncResult), initialize);
        }

        private ObservableCollection<PreDeliveryRemaindful> delivery = new ObservableCollection<PreDeliveryRemaindful>();
        public ObservableCollection<PreDeliveryRemaindful> Delivery { get { return delivery; } }

        private ObservableCollection<PreAblactationRemaindful> ablactation = new ObservableCollection<PreAblactationRemaindful>();
        public ObservableCollection<PreAblactationRemaindful> Ablactation { get { return ablactation; } }

        private ObservableCollection<FeedRemaindful> feed = new ObservableCollection<FeedRemaindful>();
        public ObservableCollection<FeedRemaindful> Feed { get { return feed; } }

        private ObservableCollection<MedicineRemaindful> medicine = new ObservableCollection<MedicineRemaindful>();
        public ObservableCollection<MedicineRemaindful> Medicine { get { return medicine; } }
    }
}
