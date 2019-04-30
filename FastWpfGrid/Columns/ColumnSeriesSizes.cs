using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastWpfGrid.Columns;

namespace FastWpfGrid
{
    /// <summary>
    /// Manager to hold column/row sizes and indexes
    /// Terminology: 
    /// ModelIndex - index in model
    /// ScrollIndex - index in scroll are (=RealIndex-FrozenCount)
    /// FrozenIndex - index in frozen area
    /// RealIndex - index in frozen and scroll area (first are frozen items, than scroll items)
    /// Grid uses mostly RealIndex
    /// </summary>
    public class ColumnSeriesSizes
    {

        private FastGridColumnCollection _columns;

        
        private int _count;
        public int DefaultSize;
        public int? MaxSize;
        //private List<int> _hiddenAndFrozenModelIndexes;
        //private List<int> _frozenModelIndexes;

        // these items are updated in BuildIndex()
        private List<SeriesSizeItem> _scrollItems = new List<SeriesSizeItem>();
        //private Dictionary<int, SeriesSizeItem> _itemByIndex = new Dictionary<int, SeriesSizeItem>();
        private List<int> _positions = new List<int>();
        private List<int> _scrollIndexes = new List<int>();
        private List<SeriesSizeItem> _frozenItems = new List<SeriesSizeItem>();
        public Func<int> ScrollAreaWidthCalculator;

        public ColumnSeriesSizes(Func<int> ScrollAreaWidthCalculator)
        {
            this.ScrollAreaWidthCalculator = ScrollAreaWidthCalculator;
        }

        public int ScrollAreaWidth
        {
            get
            {
                var width = ScrollAreaWidthCalculator();
                return width;
            }
        }

        public int Count
        {
            get { return _count; }
            private set { _count = value; }
        }

        public int FirstVisibleScrollColumnIndex
        {
            get;
            set;
        }

        public int FirstVisibleScrollColumnDisplayIndex
        {
            get
            {
                var ret = this.FrozenCount + this.FirstVisibleScrollColumnIndex;
                return ret;
            }
        }
        public int LastVisibleScrollColumnDisplayIndex
        {
            get
            {
                var ret = FirstVisibleScrollColumnDisplayIndex + VisibleScrollColumnCount;
                return ret;
            }
        }
        public int ScrollCount
        {
            get
            {
                return _count - this.Columns.GetHiddenColumns().Count;
            }
        }

        public int FrozenCount
        {
            get
            {
                return this.Columns.GetFrozenColumns().Count;
            }
        }

        public int FrozenSize
        {
            get { return _frozenItems.Sum(x => x.Size); }
        }

        public int RealCount
        {
            get { return FrozenCount + ScrollCount; }
        }

        public FastGridColumnCollection Columns
        {
            get
            {
                return this._columns;
            }
        }

        public void Clear()
        {
            _scrollItems.Clear();
            //_itemByIndex.Clear();
            _positions.Clear();
            _scrollIndexes.Clear();
            _frozenItems.Clear();
        }

        public void InitColumns(FastGridColumnCollection columns)
        {
            this.Clear();

            this._columns = columns;
            this.Count = columns.Count;
            SetExtraordinaryRealColumns();
        }

        public void SetExtraordinaryRealColumns()
        {
            BuildIndex();
        }

        public void PutSizeOverride(int modelIndex, int size)
        {
            if (MaxSize.HasValue && size > MaxSize)
            {
                size = MaxSize.Value;
            }

            this.Columns.SetWidthByIndex(modelIndex, size);
        }

        public void BuildIndex()
        {
            _scrollItems.Clear();
            //_itemByIndex.Clear();

            _scrollIndexes = Columns.Select(x => x.DisplayIndex - FrozenCount).Where(x => x >= 0).ToList();
            _scrollIndexes.Sort();

            int lastScrollIndex = -1;
            int lastEndPosition = 0;

            foreach (int scrollIndex in _scrollIndexes)
            {
                var displayIndex = FrozenCount + scrollIndex;
                var column = this.Columns.FirstOrDefault(x => x.DisplayIndex == displayIndex);
             
                var item = new SeriesSizeItem
                {
                    DisplayIndex = displayIndex,
                    ScrollIndex = scrollIndex,
                    ModelIndex = column.Index,
                    Size = column.Width,
                    Position = lastEndPosition + (scrollIndex - lastScrollIndex - 1) * DefaultSize,
                };
                _scrollItems.Add(item);
                //_itemByIndex[index] = item;
                lastScrollIndex = scrollIndex;
                lastEndPosition = item.EndPosition;
            }

            _positions = _scrollItems.Select(x => x.Position).ToList();


            _frozenItems.Clear();
            int lastpos = 0;

            var frozenColumns = this.Columns.GetFrozenColumns();
            for (int i = 0; i < frozenColumns.Count; i++)
            {
                var column = frozenColumns[i];

                var item = new SeriesSizeItem
                {
                    DisplayIndex = column.DisplayIndex,
                    FrozenIndex = i,
                    ModelIndex = column.Index,
                    Size = column.Width,
                    Position = lastpos,
                };
                _frozenItems.Add(item);
                lastpos += column.Width;
            }
        }

        public int GetScrollIndexOnPosition(int position)
        {
            int itemOrder = _positions.BinarySearch(position);
            if (itemOrder >= 0) return itemOrder;
            itemOrder = ~itemOrder; // bitwise complement - index is next larger index
            if (itemOrder == 0) return position / DefaultSize;
            if (position <= _scrollItems[itemOrder - 1].EndPosition) return _scrollItems[itemOrder - 1].ScrollIndex;
            return (position - _scrollItems[itemOrder - 1].Position) / DefaultSize + _scrollItems[itemOrder - 1].ScrollIndex;
        }

        public int GetFrozenIndexOnPosition(int position)
        {
            foreach (var item in _frozenItems)
            {
                if (position >= item.Position && position <= item.EndPosition) return item.FrozenIndex;
            }
            return -1;
        }

        public int GetSizeSum(int startScrollIndex, int endScrollIndex)
        {
            int order1 = _scrollIndexes.BinarySearch(startScrollIndex);
            int order2 = _scrollIndexes.BinarySearch(endScrollIndex);

            int count = endScrollIndex - startScrollIndex;


            if (order1 < 0) order1 = ~order1;
            if (order2 < 0) order2 = ~order2;

            int result = 0;

            for (int i = order1; i <= order2; i++)
            {
                if (i < 0) continue;
                if (i >= _scrollItems.Count) continue;
                var item = _scrollItems[i];
                if (item.ScrollIndex < startScrollIndex) continue;
                if (item.ScrollIndex >= endScrollIndex) continue;

                result += item.Size;
                count--;
            }

            result += count * DefaultSize;
            return result;
        }


        public int GetSizeByScrollIndex(int scrollIndex)
        {
            return GetSizeByRealIndex(scrollIndex + FrozenCount);
        }

        public int GetSizeByRealIndex(int realIndex)
        {
            var column = this.Columns.FirstOrDefault(x => x.DisplayIndex == realIndex);
            return column.Width;
        }


        public int GetScroll(int sourceScrollIndex, int targetScrollIndex)
        {
            if (sourceScrollIndex < targetScrollIndex)
            {
                return -Enumerable.Range(sourceScrollIndex, targetScrollIndex - sourceScrollIndex).Select(GetSizeByScrollIndex).Sum();
            }
            else
            {
                return Enumerable.Range(targetScrollIndex, sourceScrollIndex - targetScrollIndex).Select(GetSizeByScrollIndex).Sum();
            }
        }

        public bool ModelIndexIsInScrollArea(int modelIndex)
        {
            var realIndex = ModelToReal(modelIndex);
            return realIndex >= FrozenCount;
        }

        public int GetTotalScrollSizeSum()
        {
            //var scrollSizeOverrides = _sizeOverridesByModelIndex.Where(x => ModelIndexIsInScrollArea(x.Key)).ToList();
            //return scrollSizeOverrides.Select(x => x.Value).Sum() + (Count - scrollSizeOverrides.Count) * DefaultSize;
            return 0;
        }

        public int GetPositionByRealIndex(int realIndex)
        {
            if (realIndex < 0) return 0;
            if (realIndex < FrozenCount) return _frozenItems[realIndex].Position;
            return GetPositionByScrollIndex(realIndex - FrozenCount);
        }

        public int GetPositionByScrollIndex(int scrollIndex)
        {
            int order = _scrollIndexes.BinarySearch(scrollIndex);
            if (order >= 0) return _scrollItems[order].Position;
            order = ~order;
            order--;
            if (order < 0) return scrollIndex * DefaultSize;
            return _scrollItems[order].EndPosition + (scrollIndex - _scrollItems[order].ScrollIndex - 1) * DefaultSize;
        }

        public int GetVisibleScrollCount(int firstVisibleIndex, int viewportSize)
        {
            int res = 0;
            int index = firstVisibleIndex;
            int count = 0;
            while (res < viewportSize && index <= Count)
            {
                res += GetSizeByScrollIndex(index);
                index++;
                count++;
            }
            return count;
        }

        public int GetVisibleScrollCountReversed(int lastVisibleIndex, int viewportSize)
        {
            int res = 0;
            int index = lastVisibleIndex;
            int count = 0;
            while (res < viewportSize && index >= 0)
            {
                res += GetSizeByScrollIndex(index);
                index--;
                count++;
            }
            return count;
        }

        public void InvalidateAfterScroll(int oldFirstVisible, int newFirstVisible, Action<int> invalidate, int viewportSize)
        {
            //int oldFirstVisible = FirstVisibleColumn;
            //FirstVisibleColumn = column;
            //int visibleCols = VisibleColumnCount;

            if (newFirstVisible > oldFirstVisible)
            {
                int oldVisibleCount = GetVisibleScrollCount(oldFirstVisible, viewportSize);
                int newVisibleCount = GetVisibleScrollCount(newFirstVisible, viewportSize);

                for (int i = oldFirstVisible + oldVisibleCount - 1; i <= newFirstVisible + newVisibleCount; i++)
                {
                    invalidate(i + FrozenCount);
                }
            }
            else
            {
                for (int i = newFirstVisible; i <= oldFirstVisible; i++)
                {
                    invalidate(i + FrozenCount);
                }
            }
        }

        public bool IsWholeInView(int firstVisibleIndex, int index, int viewportSize)
        {
            int res = 0;
            int testedIndex = firstVisibleIndex;
            while (res < viewportSize && testedIndex < Count)
            {
                res += GetSizeByScrollIndex(testedIndex);
                if (testedIndex == index) return res <= viewportSize;
                testedIndex++;
            }
            return false;
        }

        public int ScrollInView(int firstVisibleIndex, int scrollIndex, int viewportSize)
        {
            if (IsWholeInView(firstVisibleIndex, scrollIndex, viewportSize))
            {
                return firstVisibleIndex;
            }

            if (scrollIndex < firstVisibleIndex)
            {
                return scrollIndex;
            }

            // scroll to the end
            int res = 0;
            int testedIndex = scrollIndex;
            while (res < viewportSize && testedIndex >= 0)
            {
                int size = GetSizeByScrollIndex(testedIndex);
                if (res + size > viewportSize) return testedIndex + 1;
                testedIndex--;
                res += size;
            }

            if (res >= viewportSize && testedIndex < scrollIndex) return testedIndex + 1;
            return firstVisibleIndex;
            //if (testedIndex < index) return testedIndex + 1;
            //return index;
        }

        public void Resize(int realIndex, int newSize)
        {
            this.Columns.SetWidthByDisplayIndex(realIndex, newSize);

            BuildIndex();
        }


        public int RealToModel(int displayIndex)
        {
            var column = this.Columns.FirstOrDefault(x => x.DisplayIndex == displayIndex);
            return column.Index;
        }

        public int ModelToReal(int modelIndex)
        {
            var column = this.Columns.FirstOrDefault(x => x.Index == modelIndex);
            return column.DisplayIndex;
        }

        public int GetFrozenPosition(int frozenIndex)
        {
            return _frozenItems[frozenIndex].Position;
        }

        /// <summary>
        /// Determines whether [has size override] [the specified model index].
        /// </summary>
        /// <param name="modelIndex">Index of the model.</param>
        /// <returns>
        ///   <c>true</c> if [has size override] [the specified model index]; otherwise, <c>false</c>.
        /// </returns>
        public bool HasSizeOverride(int modelIndex)
        {
            return false;
            //return _sizeOverridesByModelIndex.ContainsKey(modelIndex);
        }

        public bool IsVisible(int testedRealIndex, int firstVisibleScrollIndex, int viewportSize)
        {
            if (testedRealIndex < 0) return false;
            if (testedRealIndex >= 0 && testedRealIndex < FrozenCount) return true;
            int scrollIndex = testedRealIndex - FrozenCount;
            int onPageIndex = scrollIndex - firstVisibleScrollIndex;
            return onPageIndex >= 0 && onPageIndex < GetVisibleScrollCount(firstVisibleScrollIndex, viewportSize);
        }

        public int? GetSeriesIndexOnPosition(double position, int headerSize, int firstVisible)
        {
            var series = this;
            if (position <= headerSize) return null;
            int frozenSize = series.FrozenSize;
            if (position <= headerSize + frozenSize) return series.GetFrozenIndexOnPosition((int)Math.Round(position - headerSize));
            return series.GetScrollIndexOnPosition(
                       (int)Math.Round(position - headerSize - frozenSize) + series.GetPositionByScrollIndex(firstVisible)
                   ) + series.FrozenCount;
        }
        /// <summary>
        /// 当前可见的列数量
        /// </summary>
        public int VisibleScrollColumnCount
        {
            get { return this.GetVisibleScrollCount(this.FirstVisibleScrollColumnIndex, this.ScrollAreaWidth); }
        }
    }
}
