using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FastWpfGrid.BlockElements
{
    public class BlockRenderParameter
    {
        public int leftPos
        {
            get;set;
        }

        public int rightPos
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

        public IntRect rectContent
        {
            get;
            set;
        }

        public IFastGridCellBlock block
        {
            get;
            set;
        }

        public FastGridCellAddress cellAddr
        {
            get;
            set;
        }

        public bool leftAlign
        {
            get;
            set;
        }

        public bool isHoverCell
        {
            get;
            set;
        }

    }
}
