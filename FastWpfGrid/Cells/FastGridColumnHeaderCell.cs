using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastWpfGrid.CellRenders;
using FastWpfGrid.Columns;

namespace FastWpfGrid
{
    public class FastGridColumnHeaderCell: FastGridCellImpl
    {
        public FastGridColumn Column
        {
            get;
            private set;
        }

        public FastGridColumnHeaderCell(FastGridColumn column)
        {
            this.Column = column;
        }

        public override ICellRenderer Renderer
        {
            get
            {
                return new ColumnHeaderCellRenderer(this);
            }
        }
    }
}
