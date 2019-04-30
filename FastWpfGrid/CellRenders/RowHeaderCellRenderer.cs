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
    public class RowHeaderCellRenderer : BaseCellRenderer
    {
        public RowHeaderCellRenderer(IFastGridCell cell) : base(cell)
        {
        }
    }
}
