using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using FastWpfGrid.BlockElements;
using FastWpfGrid.CellRenders;

namespace FastWpfGrid
{
    public abstract class FastGridCellImpl : IFastGridCell
    {
        public Color? BackgroundColor { get; set; }
        public CellDecoration Decoration { get; set; }
        public Color? DecorationColor { get; set; }

        public virtual FastGridThickness Padding
        {
            get;
            set;
        }

        public object GetCellValue()
        {
            return null;
        }

        public void SetCellValue(object value)
        {
            
        }

        public string ToolTipText { get; set; }
        public TooltipVisibilityMode ToolTipVisibility { get; set; }

        public abstract ICellRenderer Renderer
        {
            get;
        }

        public int GetCellContentHeight()
        {
            return this.Renderer.GetCellContentHeight();
        }

        public int GetCellContentWidth(int? maxSize = null)
        {
            return this.Renderer.GetCellContentWidth(maxSize);
        }

        public int ActualWidth
        {
            get
            {
                var width = this.GetCellContentWidth() + this.Padding.Width;
                return width;
            }
        }

        public int ActualHeight
        {
            get
            {
                return this.GetCellContentHeight() + this.Padding.Height;
            }
        }

        public List<FastGridBlockImpl> Blocks = new List<FastGridBlockImpl>();

        public int BlockCount
        {
            get { return Blocks.Count; }
        }

        public int RightAlignBlockCount { get; set; }

        public IFastGridCellBlock GetBlock(int blockIndex)
        {
            return Blocks[blockIndex];
        }

        public IEnumerable<FastGridBlockImpl> SetBlocks
        {
            set
            {
                Blocks.Clear();
                Blocks.AddRange(value);
            }
        }

        public FastGridBlockImpl AddImageBlock(string image, int width = 16, int height = 16)
        {
            var res = new ImageBlockElement(this)
            {
                ImageWidth = width,
                ImageHeight = height,
                ImageSource = image,
            };
            Blocks.Add(res);
            return res;
        }

        public FastGridBlockImpl AddTextBlock(object text)
        {
            var res = new TextBlockElement(this)
            {
                TextData = text == null ? null : text.ToString(),

            };
            Blocks.Add(res);
            return res;
        }
    }
}
