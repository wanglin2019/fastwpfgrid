using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastWpfGrid
{
    public class FastGridRowCollection : ObservableCollection<FastGridRow>
    {
        public List<FastGridRow> GetHiddenRows()
        {
            return this.Where(x => x.IsHidden).ToList();
        }

        public List<FastGridRow> GetFrozenRows()
        {
            return this.Where(x => x.IsFrozen).ToList();
        }
        public HashSet<int> GetHiddenRowsIndex()
        {
            return new HashSet<int>(this.Where(x => x.IsHidden).Select(x => x.Index));
        }

        public HashSet<int> GetFrozenRowsIndex()
        {
            return new HashSet<int>(this.Where(x => x.IsFrozen).Select(x => x.Index));
        }

        public void SetHiddenRows(HashSet<int> index)
        {
            var items = this.Where(x => index.Contains(x.Index));
            foreach (var fastGridRow in items)
            {
                fastGridRow.IsHidden = true;
            }
        }

        public void SetFrozenRows(HashSet<int> index)
        {
            var items = this.Where(x => index.Contains(x.Index));
            foreach (var fastGridRow in items)
            {
                fastGridRow.IsFrozen = true;
            }
        }

        public IFastGridCell GetContentCell(int rowIndex, int columnIndex)
        {
            var row = this[rowIndex];
            return row.ContentCells[columnIndex];
        }
    }
}
