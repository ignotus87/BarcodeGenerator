using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using ZXing;

namespace BarcodeGenerator
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _barcodeContent;
        public string BarcodeContent
        {
            get { return _barcodeContent; }
            set
            {
                _barcodeContent = value;
                UpdateBarcodeBitmapImage();
            }
        }

        private BitmapImage _barcodeBitmap;
        public BitmapImage BarcodeBitmap
        {
            get { return _barcodeBitmap; }
            set
            {
                _barcodeBitmap = value;
                NotifyPropertyChanged();
            }
        }

        private void UpdateBarcodeBitmapImage()
        {
            BarcodeWriter writer = new BarcodeWriter();
            Bitmap bitmap = null;
            writer.Format = BarcodeFormat.CODE_128;
            writer.Options = new ZXing.Common.EncodingOptions()
            {
                PureBarcode = true,
                Height = 100,
                Width = 280,
                Margin = 10
            };

            try
            {
                bitmap = writer.Write(_barcodeContent ?? "");
            }
            catch { }

            BarcodeBitmap = ConvertBitmapToBitmapImage(bitmap);
        }

        private static BitmapImage ConvertBitmapToBitmapImage(Bitmap bitmap)
        {
            if (bitmap == null) { return null; }

            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }


        public MainWindowViewModel()
        {
            BarcodeContent = "Sample content";
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
