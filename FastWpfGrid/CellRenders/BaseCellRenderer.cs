using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FastWpfGrid.BlockElements;

namespace FastWpfGrid.CellRenders
{
    public class BaseCellRenderer: ICellRenderer
    {
        private CellRenderParameter parameters;
        private FastGridControl control;

        public BaseCellRenderer(IFastGridCell cell)
        {
            this.Cell = cell;
        }

        public IFastGridCell Cell
        {
            get;
            private set;
        }

        public virtual void Render(CellRenderParameter parameters)
        {
            this.parameters = parameters;
            this.control = this.parameters.Control;

            RenderCell(parameters);
        }

        private void RenderCell(CellRenderParameter parameters)
        {
            IFastGridCell cell = parameters.cell;
            IntRect rect = parameters.rect;
            Color? selectedTextColor = parameters.selectedTextColor;
            Color bgColor = parameters.bgColor;
            FastGridCellAddress cellAddr = parameters.cellAddr;

            var _mouseOverCell = this.control._mouseOverCell;

            bool isHoverCell = !cellAddr.IsEmpty && cellAddr == _mouseOverCell;

            if (isHoverCell)
            {
                this.control._mouseOverCellIsTrimmed = false;
                this.control.CurrentCellActiveRegions.Clear();
                this.control.CurrentHoverRegion = null;
            }

            if (cell == null) return;
            var rectContent = GetContentRect(rect);
            this.control._drawBuffer.DrawRectangle(rect, this.control.GridLineColor);
            this.control._drawBuffer.FillRectangle(rect.GrowSymmetrical(-1, -1), bgColor);

            int count = cell.BlockCount;
            int rightCount = cell.RightAlignBlockCount;
            int leftCount = count - rightCount;
            int leftPos = rectContent.Left;
            int rightPos = rectContent.Right;

            for (int i = count - 1; i >= count - rightCount; i--)
            {
                var block = cell.GetBlock(i);
                if (block == null) continue;
               
                int blockWi = this.RenderBlock(leftPos, rightPos, selectedTextColor, bgColor, rectContent, block, cellAddr, false, isHoverCell);
                rightPos -= blockWi;
            }

            for (int i = 0; i < leftCount && leftPos < rightPos; i++)
            {
                var block = cell.GetBlock(i);
                if (block == null) continue;
              
                int blockWi = this.RenderBlock(leftPos, rightPos, selectedTextColor, bgColor, rectContent, block, cellAddr, true, isHoverCell);
                leftPos += blockWi;
            }
            switch (cell.Decoration)
            {
                case CellDecoration.StrikeOutHorizontal:
                    this.control._drawBuffer.DrawLine(rect.Left, rect.Top + rect.Height / 2, rect.Right, rect.Top + rect.Height / 2, cell.DecorationColor ?? Colors.Black);
                    break;
            }
            if (isHoverCell)
            {
                this.control._mouseOverCellIsTrimmed = leftPos > rightPos;
            }
        }

        private IntRect GetContentRect(IntRect rect)
        {
            return rect.GrowSymmetrical(this.Cell.Padding.ToRect());
        }

        internal int RenderBlock(int leftPos, int rightPos, Color? selectedTextColor, Color bgColor,
            IntRect rectContent, IFastGridCellBlock block, FastGridCellAddress cellAddr, bool leftAlign, bool isHoverCell)
        {
            bool renderBlock = true;
            if (block.MouseHoverBehaviour == MouseHoverBehaviours.HideWhenMouseOut && !isHoverCell)
            {
                renderBlock = false;
            }

            int width = 0, top = 0, height = 0;

            switch (block.BlockType)
            {
                case FastGridBlockType.Text:

                    width = block.GetWidth(this.control._columnSizes.MaxSize);
                    height = block.GetHeight();

                    top = rectContent.Top + (int)Math.Round(rectContent.Height / 2.0 - height / 2.0);
                    break;
                case FastGridBlockType.Image:

                    top = rectContent.Top + (int)Math.Round(rectContent.Height / 2.0 - block.GetHeight() / 2.0);
                    height = block.GetHeight();
                    width = block.GetWidth(int.MaxValue);
                    break;
            }

            if (renderBlock && block.CommandParameter != null)
            {
                var activeRect = new IntRect(new IntPoint(leftAlign ? leftPos : rightPos - width, top), new IntSize(width, height)).GrowSymmetrical(1, 1);
                var region = new FastGridControl.ActiveRegion
                {
                    CommandParameter = block.CommandParameter,
                    Rect = activeRect,
                    Tooltip = block.ToolTip,
                };
                this.control.CurrentCellActiveRegions.Add(region);
                if (this.control._mouseCursorPoint.HasValue && activeRect.Contains(this.control._mouseCursorPoint.Value))
                {
                    this.control._drawBuffer.FillRectangle(activeRect, this.control.ActiveRegionHoverFillColor);
                    this.control.CurrentHoverRegion = region;
                }

                bool renderRectangle = true;
                if (block.MouseHoverBehaviour == MouseHoverBehaviours.HideButtonWhenMouseOut && !isHoverCell) renderRectangle = false;

                if (renderRectangle) this.control._drawBuffer.DrawRectangle(activeRect, this.control.ActiveRegionFrameColor);
            }

            switch (block.BlockType)
            {
                case FastGridBlockType.Text:
                    if (renderBlock)
                    {
                        var textBlock = block as TextBlockElement;
                        var textOrigin = new IntPoint(leftAlign ? leftPos : rightPos - width, top);
                        var font = this.control.GetFont(textBlock.IsBold, textBlock.IsItalic);
                        var fontColor = selectedTextColor ?? textBlock.FontColor ?? this.control.CellFontColor;
                        var bgColor2 = this.control.UseClearType ? bgColor : (Color?) null;
                        this.control._drawBuffer.DrawString(textOrigin.X, textOrigin.Y, rectContent, 
                            fontColor, bgColor2,
                                               font,
                                               textBlock.TextData);
                    }
                    break;
                case FastGridBlockType.Image:
                    if (renderBlock)
                    {
                        var imageBlock = block as ImageBlockElement;
                        var imgOrigin = new IntPoint(leftAlign ? leftPos : rightPos - imageBlock.ImageWidth, top);
                        var image = ImageBlockElement.GetImage(imageBlock.ImageSource);
                        this.control._drawBuffer.Blit(new Point(imgOrigin.X, imgOrigin.Y), image.Bitmap, new Rect(0, 0, imageBlock.ImageWidth, imageBlock.ImageHeight),
                                         image.KeyColor, image.BlendMode);
                    }
                    break;
            }

            return width;
        }

        public int GetCellContentHeight()
        {
            IFastGridCell cell = this.Cell;
            if (cell == null) return 0;
            var font = this.control.GetFont(false, false);
            int res = font.TextHeight;
            for (int i = 0; i < cell.BlockCount; i++)
            {
                var block = cell.GetBlock(i);
                if (block.BlockType != FastGridBlockType.Text) continue;
                string text = (block as TextBlockElement).TextData;
                if (text == null) continue;
                int hi = font.GetTextHeight(text);
                if (hi > res) res = hi;
            }
            return res;
        }

        public int GetCellContentWidth( int? maxSize = null)
        {
            IFastGridCell cell = this.Cell;
            if (cell == null) return 0;
            int count = cell.BlockCount;

            int witdh = 0;
            for (int i = 0; i < count; i++)
            {
                var block = cell.GetBlock(i);
                if (block == null)
                {
                    continue;
                }

                witdh += block.ActualWidth;
            }
            return witdh;
        }
    }
}
