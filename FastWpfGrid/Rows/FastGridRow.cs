using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastWpfGrid
{
    public class FastGridRow
    {
        public int Index
        {
            get;
            set;
        }

        public int DisplayIndex
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }

        public bool IsFrozen
        {
            get;
            set;
        }

        public bool IsHidden
        {
            get;
            set;
        }

        public object DataItem
        {
            get;
            set;
        }

        public bool IsSelected
        {
            get;
            set;
        }

        public object Header
        {
            get;
            set;
        }

        private IFastGridCell _headerCell;
        public IFastGridCell HeaderCell
        {
            get
            {
                if (_headerCell == null)
                {
                    _headerCell = GenerateHeaderCell();
                }

                return _headerCell;
            }
            set
            {
                _headerCell = value;
            }
        }

        private List<IFastGridCell> _contentCells = new List<IFastGridCell>();
        public List<IFastGridCell> ContentCells
        {
            get
            {
                return _contentCells;
            }
        }

        public IFastGridCell GenerateHeaderCell()
        {
           var cell = new FastGridHeaderCell(this.Header);
           return cell;
        }


    }

}
