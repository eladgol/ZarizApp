using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Linq;
namespace ZarizNavigation.Controls
{
    public class SelectableItem : BindableObject
    {
        public static readonly BindableProperty DataProperty =
            BindableProperty.Create(
                nameof(Data),
                typeof(object),
                typeof(SelectableItem),
                (object)null);

        public static readonly BindableProperty IsSelectedProperty =
            BindableProperty.Create(
                nameof(IsSelected),
                typeof(bool),
                typeof(SelectableItem),
                false);

        public SelectableItem(object data)
        {
            Data = data;
            IsSelected = false;
        }

        public SelectableItem(object data, bool isSelected)
        {
            Data = data;
            IsSelected = isSelected;
        }

        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
    }

    public class SelectableItem<T> : SelectableItem
    {
        public SelectableItem(T data)
            : base(data)
        {
        }

        public SelectableItem(T data, bool isSelected)
            : base(data, isSelected)
        {
        }

        // this is safe as we are just returning the base value
        public new T Data
        {
            get { return (T)base.Data; }
            set { base.Data = value; }
        }
    }
    public partial class MultiSelectView : ContentView
    {
        public static ObservableCollection<SelectableItem<string>> items { get; set; }
        public MultiSelectView()
        {
            items = new ObservableCollection<SelectableItem<string>> () { 
                new SelectableItem<string>("Speaker"), 
                new SelectableItem<string>("Pen"),
                new SelectableItem<string>("Lamp"),
                new SelectableItem<string>("Monitor"),
                new SelectableItem<string>("Bag"),
                new SelectableItem<string>("Book"),
                new SelectableItem<string>("Cap"),
                new SelectableItem<string>("Tote"),
                new SelectableItem<string>("Floss"),
                new SelectableItem<string>("Phone") };

            InitializeComponent();
        }
        void DisplayAlert(string title, string message, string cancel)
        {
            string[] values = { title, message, cancel};
            MessagingCenter.Send<MultiSelectView, string[]>(this, "DisplayAlert", values);
        }
        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as SelectableItem;
            if (item != null)
            {
                // toggle the selection property
                item.IsSelected = !item.IsSelected;
            }

            // deselect the item
            ((ListView)sender).SelectedItem = null;
        }

        //void OnSelection(object sender, SelectedItemChangedEventArgs e)
        //{
        //    if (e.SelectedItem == null)
        //    {
        //        return;
        //    }
        //    DisplayAlert("Item Selected", e.SelectedItem.ToString(), "Ok");
        //}
        void OnRefresh(object sender, EventArgs e)
        {
            var list = (ListView)sender;
            //put your refreshing logic here
            var itemList = items.Reverse().ToList();
            items.Clear();
            foreach (var s in itemList)
            {
                items.Add(s);
            }
            //make sure to end the refresh state
            list.IsRefreshing = false;
        }

        void OnTap(object sender, ItemTappedEventArgs e)
        {
            DisplayAlert("Item Tapped", e.Item.ToString(), "Ok");
        }

        //void OnMore(object sender, EventArgs e)
        //{
        //    var item = (MenuItem)sender;
        //    DisplayAlert("More Context Action", item.CommandParameter + " more context action", "OK");
        //}

        //void OnDelete(object sender, EventArgs e)
        //{
        //    var item = (MenuItem)sender;
        //    items.Remove(item.CommandParameter.ToString());
        //}
    }
    public class textViewCell : ViewCell
    {
        public textViewCell()
        {
            StackLayout layout = new StackLayout();
            layout.Padding = new Thickness(15, 0);
            Label label = new Label();

            label.SetBinding(Label.TextProperty, ".");
            layout.Children.Add(label);

            //var moreAction = new MenuItem { Text = "More" };
            //moreAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
            //moreAction.Clicked += OnMore;

            //var deleteAction = new MenuItem { Text = "Delete", IsDestructive = true }; // red background
            //deleteAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
            //deleteAction.Clicked += OnDelete;

            //this.ContextActions.Add(moreAction);
            //this.ContextActions.Add(deleteAction);
            View = layout;
        }

        //void OnMore(object sender, EventArgs e)
        //{
        //    var item = (MenuItem)sender;
        //    //Do something here... e.g. Navigation.pushAsync(new specialPage(item.commandParameter));
        //    //page.DisplayAlert("More Context Action", item.CommandParameter + " more context action",  "OK");
        //}

        //void OnDelete(object sender, EventArgs e)
        //{
        //    var item = (MenuItem)sender;
        //    MultiSelectView.items.Remove(item.CommandParameter.ToString());
        //}
    }
}
