using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastWpfGrid
{
    public class FastGridColumnCollection : ObservableCollection<FastGridColumn>
    {

        public FastGridColumnCollection()
        {
            
        }

        public FastGridColumn GetColumn(int index)
        {
            if (index < 0 || index > this.Count - 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            return this[index];
        }

        public IFastGridCell GetHeaderCell(int index)
        {
            var column = this.GetColumn(index);
            return column.GenerateHeaderCell();
        }

        public List<FastGridColumn> GetHiddenColumns()
        {
            return this.Where(x => x.IsHidden).ToList();
        }

        public List<FastGridColumn> GetFrozenColumns()
        {
            return this.Where(x => x.IsFrozen && x.IsHidden==false).OrderBy(x=>x.DisplayIndex).ToList();
        }
        public HashSet<int> GetHiddenColumnsIndex()
        {
            return new HashSet<int>(this.Where(x => x.IsHidden).Select(x => x.Index));
        }

        public HashSet<int> GetFrozenColumnsIndex()
        {
            return new HashSet<int>(this.Where(x => x.IsFrozen&&x.IsHidden==false).Select(x => x.Index));
        }

        public void SetHiddenColumns(List<int> index)
        {
            var items = this.Where(x => index.Contains(x.Index));
            foreach (var fastGridColumn in items)
            {
                fastGridColumn.IsHidden = true;
            }
        }

        public void SetFrozenColumns(List<int> index)
        {
            var items = this.Where(x => index.Contains(x.Index));
            foreach (var fastGridColumn in items)
            {
                fastGridColumn.IsFrozen = true;
            }
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
            
            for (var i = 0; i < this.Count; i++)
            {
                var column = this[i];
                column.Owner = this;
                column.Index = i;
            }

            var frozenColumns = this.Where(x => x.IsFrozen && x.IsHidden == false).OrderBy(x => x.Index);

            var normalColumns = this.Where(x => x.IsFrozen == false && x.IsHidden == false).OrderBy(x => x.Index);

            var displayIndex = 0;
            foreach (var column in frozenColumns)
            {
                column.DisplayIndex = displayIndex;
                displayIndex += 1;
            }

            foreach (var column in normalColumns)
            {
                column.DisplayIndex = displayIndex;
                displayIndex += 1;
            }
        }

        public void SetWidthByIndex(int index, int width)
        {
            var column = this.FirstOrDefault(x => x.Index == index);
            if (column != null)
            {
                //自适应宽度
                //if (width > column.Width)
                //{
                //    column.Width = width;
                //}

                column.Width = width;
            }
        }
        public void SetWidthByDisplayIndex(int displayIndex, int width)
        {
            var column = this.FirstOrDefault(x => x.DisplayIndex == displayIndex);
            if (column != null)
            {
                //自适应宽度
                //if (width > column.Width)
                //{
                //    column.Width = width;
                //}

                column.Width = width;
            }
        }
    }
}
