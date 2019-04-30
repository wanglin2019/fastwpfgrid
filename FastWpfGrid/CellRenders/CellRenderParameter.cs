using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FastWpfGrid.CellRenders
{
    public class CellRenderParameter
    {
        public FastGridControl Control
        {
            get;
            set;
        }

        public IFastGridCell cell
        {
            get;
            set;
        }

  

        public IntRect rect
        {
            get;
            set;
        }

        public Color? selectedTextColor
        {
            get;
            set;
        }

        public Color bgColor
        {
            get;
            set;
        }

        public FastGridCellAddress cellAddr
        {
            get;
            set;
        }
    }
}
