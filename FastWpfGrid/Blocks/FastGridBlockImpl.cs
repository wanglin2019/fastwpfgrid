using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastWpfGrid.BlockElements
{
    public abstract class FastGridBlockImpl : IFastGridCellBlock
    {
        public abstract FastGridBlockType BlockType
        {
            get;
        }

        public MouseHoverBehaviours MouseHoverBehaviour
        {
            get;
        }

        public object CommandParameter
        {
            get;
        }

        public string ToolTip
        {
            get;
        }

        public IFastGridCell Cell
        {
            get;
        }

        public FastGridBlockImpl(IFastGridCell cell)
        {
            this.Cell = cell;
            MouseHoverBehaviour = MouseHoverBehaviours.ShowAllWhenMouseOut;
        }

        public void Render()
        {
            
        }

        public abstract int GetWidth(int? maxSize = null);

        public abstract int GetHeight();

        public FastGridThickness Padding
        {
            get;
            set;
        }

        public virtual int ActualWidth
        {
            get
            {
                return this.GetWidth() + this.Padding.Left + this.Padding.Right;
            }
        }

        public virtual int ActualHeight
        {
            get
            {
                return this.GetHeight() + this.Padding.Top + this.Padding.Bottom;
            }
        }
    }
}
