using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

using ZarizNavigation.Controls;
using System;

namespace ZarizNavigation
{
    public partial class MultiSelectDemo : ContentView
    {
        private bool enableMultiSelect;

        public MultiSelectDemo()
        {
            InitializeComponent();

            var initialItems = new[] {
                        "אוכל", "חינוך", "נקיון", "שיפוצים", "הדרכה", "גינון",
        "הובלה", "כלים כבדים", "ביביסיטר","עבודות בית", "אדמיניסטרציה", "שיווק", "רסר",
        "הפקה"
            };

            enableMultiSelect = true;
            Items = new SelectableObservableCollection<string>(initialItems);
            AddItemCommand = new Command(OnAddItem);
            RemoveSelectedCommand = new Command(OnRemoveSelected);
            ToggleSelectionCommand = new Command(OnToggleSelection);

            BindingContext = this;
        }

        public bool EnableMultiSelect
        {
            get { return enableMultiSelect; }
            set
            {
                enableMultiSelect = value;
                OnPropertyChanged();
            }
        }

        public SelectableObservableCollection<string> Items { get; }

        public ICommand AddItemCommand { get; }

        public ICommand RemoveSelectedCommand { get; }

        public ICommand ToggleSelectionCommand { get; }

        private void OnAddItem()
        {
            Items.Add(Guid.NewGuid().ToString());
        }

        private void OnRemoveSelected()
        {
            var selectedItems = Items.SelectedItems.ToArray();
            foreach (var item in selectedItems)
            {
                Items.Remove(item);
            }
        }

        private void OnToggleSelection()
        {
            foreach (var item in Items)
            {
                item.IsSelected = !item.IsSelected;
            }
        }
    }
}