using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using WKApp.ViewModels;

namespace WKApp.Views
{
    public sealed partial class LevelsContentGridPage : Page
    {
        private LevelsContentGridViewModel ViewModel
        {
            get { return ViewModelLocator.Current.LevelsContentGridViewModel; }
        }

        public LevelsContentGridPage()
        {
            InitializeComponent();
        }
        public static T FindControl<T>(UIElement parent, Type targetType, string ControlName) where T : FrameworkElement
        {

            if (parent == null) return null;

            if (parent.GetType() == targetType && ((T)parent).Name == ControlName)
            {
                return (T)parent;
            }
            T result = null;
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                UIElement child = (UIElement)VisualTreeHelper.GetChild(parent, i);

                if (FindControl<T>(child, targetType, ControlName) != null)
                {
                    result = FindControl<T>(child, targetType, ControlName);
                    break;
                }
            }
            return result;
        }
        private void itemThumbnail_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var txt = FindControl<TextBlock>(sender as Control, typeof(TextBlock), "HoverButton");
            var txt2 = FindControl<TextBlock>(sender as Control, typeof(TextBlock), "HoverButton2");

            txt.Opacity = 1; txt2.Opacity = 1;

        }

        private void itemThumbnail_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var txt = FindControl<TextBlock>(sender as Control, typeof(TextBlock), "HoverButton");
            var txt2 = FindControl<TextBlock>(sender as Control, typeof(TextBlock), "HoverButton2");

            txt.Opacity = 0; txt2.Opacity = 0;

        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //$"{DataType}-{param}"
            if (e.Parameter is string)
            {
                var param = e.Parameter.ToString().Split("!");
                var dataType = param[0];
                var level = param[1];
                await ViewModel.LoadData(level, dataType);
            }
        }
    }
}
