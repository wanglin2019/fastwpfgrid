using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FastWpfGrid
{
    public class FastGridColumn
    {
        public int Index
        {
            get;
            set;
        }

        private int displayIndex = -1;
        public int DisplayIndex
        {
            get
            {
                if (this.IsHidden)
                {
                    return -1;
                }

                return displayIndex;
            }
            set
            {
                this.displayIndex = value;
            }
        }

        public string Header
        {
            get;
            set;
        }

        public string Path
        {
            get;
            set;
        }

        public int Width
        {
            get;
            set;
        }

        public int MinWidth
        {
            get;
            set;
        }

        public int? MaxWidth
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

        public bool IsReadOnly
        {
            get;
            set;
        }

        private FastGridThickness _contentCellPadding;
        public FastGridThickness ContentCellPadding
        {
            get
            {
                return _contentCellPadding;
            }
            set
            {
                _contentCellPadding = value;
            }
        }

        private IFastGridCell _headerCell;
        public IFastGridCell HeaderCell
        {
            get
            {
                if (_headerCell == null)
                {
                    var cell = new FastGridColumnHeaderCell(this);
                    cell.AddTextBlock(this.Header);

                    _headerCell = cell;
                }

                return _headerCell;
            }
            set
            {
                this._headerCell = value;
            }
        }

        public IFastGridCell GenerateHeaderCell()
        {
            return HeaderCell;
        }

        public IFastGridCell GenerateContentCell(int rowIndex, object item)
        {
            var propertyName = this.Path;

            var propertyInfo = item.GetType().GetProperty(propertyName);
            if (propertyInfo != null)
            {
                var v = propertyInfo.GetValue(item, null);
                var cellImpl = new FastGridContentCell(this);
                if (v != null)
                {
                    cellImpl.AddTextBlock(v.ToString());
                }
                else
                {
                    cellImpl.AddTextBlock(string.Empty);
                }
                
                return cellImpl;
            }

            return null;
        }

        public int ClampColumnWidth(int newWidth, int? gridScrollAreaWidth=null)
        {
            if (newWidth < MinWidth)
            {
                return MinWidth;
            }

            if (MaxWidth.HasValue && newWidth > MaxWidth.Value)
            {
                return MaxWidth.Value;
            }

            if (gridScrollAreaWidth.HasValue && newWidth > gridScrollAreaWidth.Value)
            {
                return gridScrollAreaWidth.Value;
            }

            return newWidth;

            //if (newWidth >= MinWidth && newWidth <= MaxWidth)
            //{
            //    return true;
            //}

            //return false;
        }

        public FastGridColumnCollection Owner
        {
            get;
            set;
        }

        public int ScrollIndex
        {
            get
            {
                return this.DisplayIndex - Owner.GetFrozenColumns().Count;
            }
        }
    }

}
