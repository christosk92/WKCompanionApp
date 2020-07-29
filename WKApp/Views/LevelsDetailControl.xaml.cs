using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace WKApp.Views
{
    public partial class LevelsDetailControl : UserControl
    {
        public String MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as String; }
            set { SetValue(MasterMenuItemProperty, value); }
        }

        public static readonly DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem", typeof(String), typeof(LevelsDetailControl), new PropertyMetadata(null, OnMasterMenuItemPropertyChanged));
        public static string DataType;
        public LevelsDetailControl()
        {
            InitializeComponent();
        }

        private async static void OnMasterMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as LevelsDetailControl;
            string param = e.NewValue as String;
            if (e.OldValue != null)
            {
                control.mainFrame.Navigate(typeof(LevelsContentGridPage), $"{DataType}!{param}");
            }
        }
    }
}
