using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace FastWpfGrid
{
    public enum FastGridBlockType
    {
        Text,
        Image,
    }

    public enum MouseHoverBehaviours
    {
        HideWhenMouseOut,
        HideButtonWhenMouseOut,
        ShowAllWhenMouseOut,
    }

    public interface IFastGridCellBlock
    {
        FastGridBlockType BlockType { get; }


        MouseHoverBehaviours MouseHoverBehaviour { get; }
        object CommandParameter { get; }
        string ToolTip { get; }


        IFastGridCell Cell
        {
            get;
        }

        void Render();

        int GetWidth(int? maxSize = null);
        int GetHeight();

        FastGridThickness Padding
        {
            get;
            set;
        }
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
