using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FastWpfGrid.CellRenders;
using FastWpfGrid.Columns;

namespace FastWpfGrid
{
    public class FastGridContentCell: FastGridCellImpl
    {
        public FastGridColumn Column
        {
            get;
            private set;
        }

        private FastGridThickness padding;
        public override FastGridThickness Padding
        {
            get
            {
                if (padding == null)
                {
                    return this.Column.ContentCellPadding;
                }

                return padding;
            }
            set
            {
                padding = value;
            }
        }

        public FastGridContentCell(FastGridColumn column)
        {
            this.Column = column;
        }

        public override ICellRenderer Renderer
        {
            get
            {
                return new ContentCellRenderer(this);
            }
        }
    }
}
