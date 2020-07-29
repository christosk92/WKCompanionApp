using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WKApp.Models;
using WKApp.Services;
using WKApp.ViewModels;

namespace WKApp.Views
{
    public sealed partial class LevelsContentGridDetailPage : Page
    {
        private LevelsContentGridDetailViewModel ViewModel
        {
            get { return ViewModelLocator.Current.LevelsContentGridDetailViewModel; }
        }

        public NavigationServiceEx NavigationService => ViewModelLocator.Current.NavigationService;

        public LevelsContentGridDetailPage()
        {
            InitializeComponent();
        }
        public void SetMnemonics(DataGridData item)
        {
            string input = item.Data.MeaningMnemonic;
            string inputReading = item.Data.ReadingMnemonic;

            var meaningText = CreateMnemonicTextBlock(input);
            mainRelative.Children.Add(meaningText);
            if (!String.IsNullOrEmpty(inputReading))
            {
                var readingText = CreateMnemonicTextBlock(inputReading);
                subRel.Children.Add(readingText);
                RelativePanel.SetBelow(readingText, ReadingSubd);
            }
            RelativePanel.SetBelow(meaningText, MeaningSub);
        }
        public TextBlock CreateMnemonicTextBlock(string input)
        {
            var textBlock = new TextBlock();
            textBlock.Inlines.Clear();

            var extractedTags = input.Tags();
            string recursiveString = input;
            List<string> res = new List<string>();
            if (extractedTags.Count > 0)
            {
                foreach (var p in extractedTags)
                {
                    string[] recursArr = recursiveString.Split(p, 2);
                    res.Add(recursArr[0]);
                    res.Add(p);
                    recursiveString = recursArr[1];
                }
                var list = res.Where(x => !x.Contains("</")).ToList();
                for (int i = 0; i < list.Count - 1; i++)
                {
                    if (list[i].Contains("<"))
                    {
                        //is a tag..
                        Run run1 = new Run();
                        run1.Foreground = new SolidColorBrush(Colors.MediumPurple);
                        //find the next..
                        run1.Text = list[i + 1];
                        textBlock.Inlines.Add(run1);
                        i++;
                    }
                    else
                    {
                        Run run1 = new Run();
                        run1.Text = list[i];
                        textBlock.Inlines.Add(run1);
                    }
                }
            }
            else
            {
                textBlock.Text = input;
            }
            textBlock.FontSize = 16;
            textBlock.TextWrapping = Windows.UI.Xaml.TextWrapping.WrapWholeWords;
            return textBlock;
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is DataGridData main)
            {
                await ViewModel.InitializeAsync(main);
            }
        }
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            if (e.NavigationMode == NavigationMode.Back)
            {
                NavigationService.Frame.SetListDataItemForNextConnectedAnimation(ViewModel.Item);
            }
        }
    }
}
