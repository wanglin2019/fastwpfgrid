using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastWpfGrid.CellRenders;

namespace FastWpfGrid
{
    public class FastGridRowHeaderCell: FastGridCellImpl
    {
       public int RowIndex
       {
           get;
           private set;
       }
        public FastGridRowHeaderCell(int rowIndex)
        {
            this.RowIndex = rowIndex;
            this.AddTextBlock(this.RowIndex + 1);
        }

        public static IFastGridCell CreateCell(int rowIndex)
        {
            return new FastGridRowHeaderCell(rowIndex);
        }

        public override ICellRenderer Renderer
        {
            get
            {
                return new RowHeaderCellRenderer(this);
            }
        }
    }
}
