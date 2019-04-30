using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using FastWpfGrid;
using SelectionChangedEventArgs = System.Windows.Controls.SelectionChangedEventArgs;

namespace FastWpfGridTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private GridModel1 _model1;
        //private GridModel2 _model2;
        //private GridModel3 _model3;

        public MainWindow()
        {
            InitializeComponent();
            
            grid1.Columns = new FastGridColumnCollection();
            grid1.Columns.Add(new FastGridColumn(){ Header = "46",Path = "A"});
            grid1.Columns.Add(new FastGridColumn() { Header = "45",Path = "B"});
            grid1.Columns.Add(new FastGridColumn() { Header = "44", Path = "C" });
            grid1.Columns.Add(new FastGridColumn() { Header = "43", Path = "D" });
            grid1.Columns.Add(new FastGridColumn() { Header = "42", Path = "E" });
            grid1.Columns.Add(new FastGridColumn() { Header = "41", Path = "F" });
            grid1.Columns.Add(new FastGridColumn() { Header = "40", Path = "A" });
            grid1.Columns.Add(new FastGridColumn() { Header = "39", Path = "B" });
            grid1.Columns.Add(new FastGridColumn() { Header = "38", Path = "C" });
            grid1.Columns.Add(new FastGridColumn() { Header = "37", Path = "D" });
            grid1.Columns.Add(new FastGridColumn() { Header = "36", Path = "E" });
            grid1.Columns.Add(new FastGridColumn() { Header = "35", Path = "F" });
            grid1.Columns.Add(new FastGridColumn() { Header = "34", Path = "A" });
            grid1.Columns.Add(new FastGridColumn() { Header = "33", Path = "B" });
            grid1.Columns.Add(new FastGridColumn() { Header = "32", Path = "C" });
            grid1.Columns.Add(new FastGridColumn() { Header = "31", Path = "D" });
            grid1.Columns.Add(new FastGridColumn() { Header = "30", Path = "E" });
            grid1.Columns.Add(new FastGridColumn() { Header = "29", Path = "F" });
            grid1.Columns.Add(new FastGridColumn() { Header = "28", Path = "A" });
            grid1.Columns.Add(new FastGridColumn() { Header = "27", Path = "B" });
            grid1.Columns.Add(new FastGridColumn() { Header = "26", Path = "C" });
            grid1.Columns.Add(new FastGridColumn() { Header = "25", Path = "D" });
            grid1.Columns.Add(new FastGridColumn() { Header = "24", Path = "E" });
            grid1.Columns.Add(new FastGridColumn() { Header = "23", Path = "F" });
            grid1.Columns.Add(new FastGridColumn() { Header = "22", Path = "A" });
            grid1.Columns.Add(new FastGridColumn() { Header = "21", Path = "B" });
            grid1.Columns.Add(new FastGridColumn() { Header = "20", Path = "C" });
            grid1.Columns.Add(new FastGridColumn() { Header = "19", Path = "D" });
            grid1.Columns.Add(new FastGridColumn() { Header = "18", Path = "E" });
            grid1.Columns.Add(new FastGridColumn() { Header = "17", Path = "F" });
            grid1.Columns.Add(new FastGridColumn() { Header = "16", Path = "A" });
            grid1.Columns.Add(new FastGridColumn() { Header = "15", Path = "B" });
            grid1.Columns.Add(new FastGridColumn() { Header = "14", Path = "C" });
            grid1.Columns.Add(new FastGridColumn() { Header = "13", Path = "D" });
            grid1.Columns.Add(new FastGridColumn() { Header = "12", Path = "E" });
            grid1.Columns.Add(new FastGridColumn() { Header = "11", Path = "F" });
            grid1.Columns.Add(new FastGridColumn() { Header = "10", Path = "A" });
            grid1.Columns.Add(new FastGridColumn() { Header = "9", Path = "B" });
            grid1.Columns.Add(new FastGridColumn() { Header = "8", Path = "C" });
            grid1.Columns.Add(new FastGridColumn() { Header = "7", Path = "D" });
            grid1.Columns.Add(new FastGridColumn() { Header = "6", Path = "E" });
            grid1.Columns.Add(new FastGridColumn() { Header = "5", Path = "F" });
            grid1.Columns.Add(new FastGridColumn() { Header = "4", Path = "A" });
            grid1.Columns.Add(new FastGridColumn() { Header = "3", Path = "B" });
            grid1.Columns.Add(new FastGridColumn() { Header = "2", Path = "C" });
            grid1.Columns.Add(new FastGridColumn() { Header = "1", Path = "D" });
            grid1.Columns.Add(new FastGridColumn() { Header = "z", Path = "E" });
            grid1.Columns.Add(new FastGridColumn() { Header = "y", Path = "F" });
            grid1.Columns.Add(new FastGridColumn() { Header = "x", Path = "A" });
            grid1.Columns.Add(new FastGridColumn() { Header = "w", Path = "B" });
            grid1.Columns.Add(new FastGridColumn() { Header = "v", Path = "C" });
            grid1.Columns.Add(new FastGridColumn() { Header = "u", Path = "D" });
            grid1.Columns.Add(new FastGridColumn() { Header = "t", Path = "E" });
            grid1.Columns.Add(new FastGridColumn() { Header = "s", Path = "F" });
            grid1.Columns.Add(new FastGridColumn() { Header = "r", Path = "A" });
            grid1.Columns.Add(new FastGridColumn() { Header = "q", Path = "B" });
            grid1.Columns.Add(new FastGridColumn() { Header = "p", Path = "C" });
            grid1.Columns.Add(new FastGridColumn() { Header = "o", Path = "D" });
            grid1.Columns.Add(new FastGridColumn() { Header = "n", Path = "E" });
            grid1.Columns.Add(new FastGridColumn() { Header = "m", Path = "F" });
            grid1.Columns.Add(new FastGridColumn() { Header = "l", Path = "A" });
            grid1.Columns.Add(new FastGridColumn() { Header = "k", Path = "B" });
            grid1.Columns.Add(new FastGridColumn() { Header = "g", Path = "C" });
            grid1.Columns.Add(new FastGridColumn() { Header = "i", Path = "D" });
            grid1.Columns.Add(new FastGridColumn() { Header = "h", Path = "E" });
            grid1.Columns.Add(new FastGridColumn() { Header = "g", Path = "F" });
            grid1.Columns.Add(new FastGridColumn() { Header = "f", Path = "A" });
            grid1.Columns.Add(new FastGridColumn() { Header = "e", Path = "B" });
            grid1.Columns.Add(new FastGridColumn() { Header = "d", Path = "C" });
            grid1.Columns.Add(new FastGridColumn() { Header = "c", Path = "D" });
            grid1.Columns.Add(new FastGridColumn() { Header = "b", Path = "E" ,IsHidden = true});
            grid1.Columns.Add(new FastGridColumn() { Header = "a", Path = "F",IsFrozen = true});

            
            grid1.ItemsSource = DataItem.Items();

            grid1.NotifyRefresh();

            //grid1.Model = _model1 = new GridModel1();
            //grid2.Model = _model2 = new GridModel2();
            //grid3.Model = _model3 = new GridModel3();
        }

        private void tabChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tab.SelectedIndex == 2) Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, (Action) SetBitmap);
        }

        private void SetBitmap()
        {
            int width = (int) textbox.ActualWidth;
            int height = (int) textbox.ActualHeight;

            if (width <= 0 || height <= 0) return;
            WriteableBitmap buffer;

            var bgColor = Color.FromRgb(0xF9, 0xF9, 0xF9);

            textImage1.Width = width;
            textImage1.Height = height;
            buffer = BitmapFactory.New(width, height);
            buffer.Clear(bgColor);
            //buffer.DrawString(0, 0, Colors.Black, new Typeface("Arial"), 12, "ABC 123 Gfijdr");
            buffer.DrawString(0, 0, Colors.Black, new PortableFontDesc(isbold: true), "Transparency");
            buffer.DrawLine(0, 10, 50, 10, Colors.Black);
            buffer.DrawLine(50, 0, 50, 40, Colors.Black);
            textImage1.Source = buffer;

            textImage2.Width = width;
            textImage2.Height = height;
            buffer = BitmapFactory.New(width, height);
            buffer.Clear(bgColor);
            buffer.DrawString(0, 0, Colors.Black, bgColor, new PortableFontDesc(cleartype: true), "ABC 123 Gfijdr");
            textImage2.Source = buffer;

            textImage3.Width = width;
            textImage3.Height = height;


            var text = new FormattedText("ABC l 123 Gfijdr",
                                         CultureInfo.InvariantCulture,
                                         FlowDirection.LeftToRight,
                                         new Typeface("Arial"),
                                         12,
                                         new SolidColorBrush(Colors.Black));


            var drawingVisual = new DrawingVisual();
            RenderOptions.SetBitmapScalingMode(drawingVisual, BitmapScalingMode.NearestNeighbor);
            RenderOptions.SetClearTypeHint(drawingVisual, ClearTypeHint.Enabled);
            RenderOptions.SetEdgeMode(drawingVisual, EdgeMode.Aliased);
            TextOptions.SetTextRenderingMode(drawingVisual, TextRenderingMode.ClearType);
            TextOptions.SetTextFormattingMode(drawingVisual, TextFormattingMode.Display);
            TextOptions.SetTextHintingMode(drawingVisual, TextHintingMode.Fixed);

            using (var drawingContext = drawingVisual.RenderOpen())
            {

                //var brush = new VisualBrush(new TextBlock {Text = "ABC 123 Gfijdr"});
                //drawingContext.DrawRectangle(brush, null, new Rect(0, 0, width, height));

                //Double halfPenWidth = 0.5;

                //var face = new Typeface("Arial");
                //GlyphTypeface gt;
                //face.TryGetGlyphTypeface(out gt);
                //var indexes = new ushort[] {gt.CharacterToGlyphMap['A']};
                //var widths = new double[] {gt.AdvanceWidths['A']*12};

                //var glyphRun = new GlyphRun(gt, 0, false, 12, indexes, new Point(0, 14), widths, null, null, null, null, null, null);
                //var rect = glyphRun.ComputeAlignmentBox();

                //Double halfPenWidth = 0.5;
                ////var rect = new Rect(0, 0, width, height);
                //GuidelineSet guidelines = new GuidelineSet();
                //guidelines.GuidelinesX.Add(rect.Left + halfPenWidth);
                //guidelines.GuidelinesX.Add(rect.Right + halfPenWidth);
                //guidelines.GuidelinesY.Add(rect.Top + halfPenWidth);
                //guidelines.GuidelinesY.Add(rect.Bottom + halfPenWidth);

                drawingContext.DrawRectangle(new SolidColorBrush(bgColor), null, new Rect(0, 0, width, height));

                drawingContext.DrawText(text, new Point(0, 0));
                drawingContext.DrawLine(new Pen(Brushes.Black, 1), new Point(0.5, 10.5), new Point(50.5, 10.5));
                drawingContext.DrawLine(new Pen(Brushes.Black, 1), new Point(35.5, 0.5), new Point(35.5, 40.5));

                //drawingContext.PushGuidelineSet(guidelines);
                //drawingContext.DrawGlyphRun(Brushes.Black, glyphRun);
                //drawingContext.Pop();
            }
            //drawingVisual.Transform = new ScaleTransform(0.9, 0.9);
            //drawingVisual.Transform = new ScaleTransform(1, 1);
            //drawingVisual.Transform = new TranslateTransform(0, -0.25);

            var tb = new TextBlock { Text = "ABC l 123 Gfijdr" };
            tb.Measure(new Size(width, height));
            tb.Arrange(new Rect(new Point(0, 0), new Size(width, height)));

            var bmp = new RenderTargetBitmap(width, height, DpiDetector.DpiXKoef * 96, DpiDetector.DpiYKoef * 96, PixelFormats.Pbgra32);

            RenderOptions.SetBitmapScalingMode(bmp, BitmapScalingMode.NearestNeighbor);
            RenderOptions.SetClearTypeHint(bmp, ClearTypeHint.Enabled);
            RenderOptions.SetEdgeMode(bmp, EdgeMode.Aliased);
            TextOptions.SetTextRenderingMode(bmp, TextRenderingMode.ClearType);
            TextOptions.SetTextFormattingMode(bmp, TextFormattingMode.Display);
            TextOptions.SetTextHintingMode(bmp, TextHintingMode.Fixed);

            //RenderOptions.SetCachingHint(bmp, CachingHint.Cache);
            //RenderOptions.SetBitmapScalingMode(bmp, BitmapScalingMode.LowQuality);
            bmp.Render(drawingVisual);
            //bmp.Render(tb);
            //bmp.Freeze();


            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));

            var ms = new MemoryStream();
            encoder.Save(ms);

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();

            encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            using (var fs = System.IO.File.OpenWrite("c:/test/file1.png"))
            {
                encoder.Save(fs);
            }

            textImage3.Stretch = Stretch.None;
            textImage3.Source = bmp;
        }

        private void columnsCfgChanged(object sender, RoutedEventArgs e)
        {
            var hidden = new HashSet<int>();
            var frozen = new HashSet<int>();

            if (chbHideColumn3.IsChecked == true) hidden.Add(2);
            if (chbFreezeColumn5.IsChecked == true) frozen.Add(4);

            //_model1.SetColumnArrange(hidden, frozen);
        }

        private void grid1_SelectedCellsChanged(object sender, FastWpfGrid.SelectionChangedEventArgs e)
        {
            var view = (FastWpfGrid.IFastGridView)grid1;
            if (view.GetSelectedModelCells().Count > 1)
            {
                view.ShowSelectionMenu(new string[] { "CMD1", "CMD2" });
            }
            else
            {
                view.ShowSelectionMenu(null);
            }
        }
    }
}
