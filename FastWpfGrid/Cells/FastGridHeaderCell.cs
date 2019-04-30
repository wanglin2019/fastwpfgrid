using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastWpfGrid.CellRenders;

namespace FastWpfGrid
{
    public class FastGridHeaderCell: FastGridCellImpl
    {
        public static readonly FastGridHeaderCell Default = new FastGridHeaderCell("");

        public object Header
        {
            get;
            set;
        }

        public FastGridHeaderCell(object header)
        {
            this.Header = header;
            if (this.Header != null)
            {
                var str = this.Header.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    this.AddTextBlock(this.Header.ToString());
                }
            }
        }

        public override ICellRenderer Renderer
        {
            get
            {
                return new HeaderCellRenderer(this);
            }
        }
    }
}
