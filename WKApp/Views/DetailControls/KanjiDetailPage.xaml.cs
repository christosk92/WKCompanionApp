using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using WKApp.Core.Helpers;
using WKApp.Helpers;
using WKApp.Models;
using WKApp.ViewModels;
using WKApp.ViewModels.DetailControls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WKApp.Views.DetailControls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class KanjiDetailPage : UserControl
    {
        public DataGridData KanjiMenuItem
        {
            get { return GetValue(KanjiMenuItemProperty) as DataGridData; }
            set { SetValue(KanjiMenuItemProperty, value); }
        }

        public static readonly DependencyProperty KanjiMenuItemProperty = DependencyProperty.Register("KanjiMenuItem", typeof(String), typeof(LevelsDetailControl), new PropertyMetadata(null, OnMasterMenuItemPropertyChanged));
        private Nihongo<Kanji> KanjiData => Singleton<Nihongo<Kanji>>.Instance;

        private KanjiDetailViewModel ViewModel
        {
            get { return ViewModelLocator.Current.KanjiDetailViewModel; }
        }

        public KanjiDetailPage()
        {
            this.InitializeComponent();
        }
        private static async Task<String> DownloadImage(string url, String fileName)
        {
            const String imagesSubdirectory = "DownloadedImages";
            var rootFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(imagesSubdirectory, CreationCollisionOption.OpenIfExists);
            String newPath = String.Format("ms-appdata:///local/{0}/{1}", imagesSubdirectory, fileName);
            var basicProps = await rootFolder.GetBasicPropertiesAsync();
            if (await rootFolder.FileExistsAsync(fileName) && basicProps.Size > 0)
                return newPath;
            var storageFile = await rootFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    byte[] buffer = await client.GetByteArrayAsync(url);
                    using (Stream stream = await storageFile.OpenStreamForWriteAsync())
                        stream.Write(buffer, 0, buffer.Length);
                }
            }
            catch (Exception)
            {
                return null;
            }

            // Use this path to load image

            return newPath;
        }

        private async static void OnMasterMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as KanjiDetailPage;
            var param = e.NewValue as DataGridData;
            var objx = control.KanjiData.Ids.First(y => y.Id == param.Id);
            var objy = control.KanjiData.Data.First(y => y.id.ToString() == param.Id);
            control.ViewModel.InitializeAsync(objx);
            control.SetMnemonics(objy);

            var svgurl = Path.Combine("https://dl1-acc.filmfetch.net/clients/win32/data/dark/kanji", $"{objx.Title.ToCode().ToLower()}.svg");
            var path = await DownloadImage(svgurl, $"{objx.Title.ToCode().ToLower()}.svg");
            if (path != null)
            {
                StorageFile file =
                     await StorageFile.GetFileFromApplicationUriAsync(new Uri(path));
                string text = await Windows.Storage.FileIO.ReadTextAsync(file);
                control.webV.NavigateToString(text.Replace("]>", ""));
            }
        }

        public void SetMnemonics(Kanji item)
        {
            string input = item.data.meaning_mnemonic;
            string inputReading = item.data.reading_mnemonic;

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

        private void itemHero_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ToggleThemeTeachingTip1.IsOpen = true;
        }

        private void itemHero_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ToggleThemeTeachingTip1.IsOpen = false;

        }
    }
}
