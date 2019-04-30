using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FastWpfGrid.BlockElements
{
    public class ImageBlockElement: FastGridBlockImpl
    {
        public ImageBlockElement(IFastGridCell cell):base(cell)
        {
            
        }

        public string ImageSource { get;
            set;
        }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }

        private static Dictionary<string, ImageHolder> _imageCache = new Dictionary<string, ImageHolder>();

        public static ImageHolder GetImage(string source)
        {
            lock (_imageCache)
            {
                if (_imageCache.ContainsKey(source)) return _imageCache[source];
            }

            string packUri = "pack://application:,,,/" + Assembly.GetEntryAssembly().GetName().Name + ";component/" + source.TrimStart('/');
            BitmapImage bmImage = new BitmapImage();
            bmImage.BeginInit();
            bmImage.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
            bmImage.UriSource = new Uri(packUri, UriKind.Absolute);
            bmImage.EndInit();
            var wbmp = new WriteableBitmap(bmImage);

            if (wbmp.Format != PixelFormats.Bgra32)
                wbmp = new WriteableBitmap(new FormatConvertedBitmap(wbmp, PixelFormats.Bgra32, null, 0));

            var image = new ImageHolder(wbmp, bmImage);
            lock (_imageCache)
            {
                _imageCache[source] = image;
            }
            return image;
        }


        public override FastGridBlockType BlockType
        {
            get
            {
                return FastGridBlockType.Image;
            }
        }

        public override int GetWidth(int? maxSize = null)
        {
            return this.ImageWidth;
        }

        public override int GetHeight()
        {
            return this.ImageHeight;
        }
    }
}
