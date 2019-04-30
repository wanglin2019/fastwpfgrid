using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using FastWpfGrid.CellRenders;

namespace FastWpfGrid
{
    public enum CellDecoration
    {
        None,
        StrikeOutHorizontal,
    }

    public enum TooltipVisibilityMode
    {
        Always,
        OnlyWhenTrimmed,
    }

    public interface IFastGridCell
    {
        Color? BackgroundColor { get; }

        int BlockCount { get; }
        int RightAlignBlockCount { get; }
        IFastGridCellBlock GetBlock(int blockIndex);
        CellDecoration Decoration { get; }
        Color? DecorationColor { get; }

        FastGridThickness Padding
        {
            get;
            set;
        }

        object GetCellValue();
        void SetCellValue(object value);

        string ToolTipText { get; }
        TooltipVisibilityMode ToolTipVisibility { get; }

        ICellRenderer Renderer
        {
            get;
        }

        int GetCellContentHeight();
        int GetCellContentWidth(int? columnSizesMaxSize=null);

        int ActualWidth
        {
            get;
        }

        int ActualHeight
        {
            get;
        }
    }
}
