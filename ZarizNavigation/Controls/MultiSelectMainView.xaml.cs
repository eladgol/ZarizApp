using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;

namespace ZarizNavigation.Controls
{
    public partial class MultiSelectMainView : ContentView
    {
        public SelectableObservableCollection<string> Items { get; }
        private bool enableMultiSelect;
        public bool EnableMultiSelect
        {
            get { return enableMultiSelect; }
            set
            {
                enableMultiSelect = value;
                OnPropertyChanged();
            }
        }

        public MultiSelectMainView()
        {
            var initialItems = new[] {
                "One",
                "Two",
                "Three",
                "Four",
                "Five"
            };

            enableMultiSelect = true;
            Items = new SelectableObservableCollection<string>(initialItems);
            AddItemCommand = new Command(OnAddItem);
            RemoveSelectedCommand = new Command(OnRemoveSelected);
            ToggleSelectionCommand = new Command(OnToggleSelection);

            BindingContext = this;
        }


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
