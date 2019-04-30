using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastWpfGrid.CellRenders
{
    public interface ICellRenderer
    {
        IFastGridCell Cell
        {
            get;
        }

        void Render(CellRenderParameter parameters);

        int GetCellContentHeight();
        int GetCellContentWidth(int? maxSize = null);
    }
}
