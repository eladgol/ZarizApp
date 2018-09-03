using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace ZarizNavigation.Controls
{
    public partial class LabelAndTextEntry : Xamarin.Forms.Grid
    {
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(LabelAndTextEntry), default(string), Xamarin.Forms.BindingMode.TwoWay);
        public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(nameof(IsPassword), typeof(string), typeof(LabelAndTextEntry), default(string), Xamarin.Forms.BindingMode.OneWay);
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(LabelAndTextEntry), default(string), Xamarin.Forms.BindingMode.OneWayToSource);

        public string Title
        {
            get
            {
                return (string)GetValue(TitleProperty);
            }

            set
            {
                SetValue(TitleProperty, value);
            }

        }

        public string IsPassword
        {
            get
            {
                return (string)GetValue(IsPasswordProperty);
            }

            set
            {
                SetValue(IsPasswordProperty, value);
            }

        }
        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }

            set
            {
                SetValue(TextProperty, value);
            }

        }
        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == TitleProperty.PropertyName)
            {
                OnEntryUnfocused(this, new EventArgs());
            }
            if (propertyName == IsPasswordProperty.PropertyName)
            {
                if (IsPassword == "true")
                {
                    nEntry.IsPassword = true;
                }
            }
            if (propertyName == TextProperty.PropertyName)
            {

            }
        }
        public LabelAndTextEntry()
        {
            InitializeComponent();
           
            
            nEntry.TextChanged += OnTextChanged;
            OnEntryUnfocused(this, new EventArgs());
        }
        async void OnEntryFocused(object sender, EventArgs e)
        {
            //usernameLabel.IsVisible = true;
            nLabel.Text = Title;
            nEntry.Placeholder = string.Empty;
        }
        async void OnEntryUnfocused(object sender, EventArgs e)
        {
            nEntry.Placeholder = Title;
            nLabel.Text = string.Empty;
            //usernameLabel.IsVisible = false; 
        }
        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Text = e.NewTextValue;
        }

    }
}
