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
        
        public int? MaxSize;
        //private List<int> _hiddenAndFrozenModelIndexes;
        //private List<int> _frozenModelIndexes;

        // these items are updated in BuildIndex()
        private List<SeriesSizeItem> _scrollItems = new List<SeriesSizeItem>();
        //private Dictionary<int, SeriesSizeItem> _itemByIndex = new Dictionary<int, SeriesSizeItem>();
        private List<int> _positions = new List<int>();
        private List<int> _scrollIndexes = new List<int>();
        
        public Func<int> ScrollAreaWidthCalculator;

        private List<SeriesSizeItem> _allPositions = new List<SeriesSizeItem>();

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
                var ret = FirstVisibleScrollColumnDisplayIndex + VisibleScrollColumnCount-1;
                return ret;
            }
        }
        public int ScrollCount
        {
            get
            {
                return _count - this.Columns.GetHiddenColumns().Count - this.Columns.GetFrozenColumns().Count;
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
            get
            {
                var frozenColumns = this.Columns.GetFrozenColumns();
                return frozenColumns.Sum(x => x.Width);
            }
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
            
            _allPositions.Clear();
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
            _allPositions.Clear();
            int lastEndPosition = 0;
            foreach (var column in this.Columns.Where(x => x.IsHidden == false).OrderBy(x => x.DisplayIndex))
            {
                _allPositions.Add(new SeriesSizeItem
                {
                    DisplayIndex = column.DisplayIndex,
                    ModelIndex = column.Index,
                    Size = column.Width,
                    Position = lastEndPosition,
                });
                lastEndPosition += column.Width;
            }


            

            _scrollItems.Clear();
            //_itemByIndex.Clear();

            _scrollIndexes = Columns.Select(x => x.DisplayIndex - FrozenCount).Where(x => x >= 0).ToList();
            _scrollIndexes.Sort();

            lastEndPosition = FrozenSize;
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
                    Position = lastEndPosition ,
                };
                _scrollItems.Add(item);
             
                lastEndPosition = item.EndPosition;
            }

            _positions = _scrollItems.Select(x => x.Position).ToList();

        }

        public int GetScrollIndexOnPosition(int position)
        {
            var displayIndex = GetDisplayIndexOnPosition(position);
            return displayIndex - FrozenCount;
        }

        public int GetFrozenIndexOnPosition(int position)
        {
            foreach (var item in _allPositions)
            {
                if (position > item.EndPosition)
                {
                    return -1;
                }
                if (position >= item.Position && position <= item.EndPosition)
                {
                    return item.DisplayIndex;
                }
            }
            return -1;
        }

        public int GetSizeSumBetweenScrollIndex(int startScrollIndex, int endScrollIndex,bool includedEndScrollIndex=false)
        {
            int result = 0;

            var startDisplayIndex = FrozenCount + startScrollIndex;
            var endDisplayIndex = FrozenCount + endScrollIndex;
            endDisplayIndex = includedEndScrollIndex ? endDisplayIndex : endDisplayIndex - 1;
            for (var i = startDisplayIndex; i <= endDisplayIndex; i++)
            {
                var item = _allPositions[i];
                result += item.Size;
            }

            return result;
        }


        public int GetSizeByScrollIndex(int scrollIndex)
        {
            return GetSizeByRealIndex(scrollIndex + FrozenCount);
        }

        public int GetSizeByRealIndex(int displayIndex)
        {
            var item = _allPositions[displayIndex];
            return item.Size;
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


        public int GetPositionByRealIndex(int realIndex)
        {
            if (realIndex < 0) return 0;
            if (realIndex < FrozenCount)
            {
                return _allPositions[realIndex].Position;
            }
            return GetPositionByScrollIndex(realIndex - FrozenCount);
        }

        public int GetPositionByScrollIndex(int scrollIndex)
        {
            int order = _scrollIndexes.BinarySearch(scrollIndex);
            if (order >= 0) return _scrollItems[order].Position;
            order = ~order;
            order--;
            if (order < 0) return scrollIndex;
            return _scrollItems[order].EndPosition + (scrollIndex - _scrollItems[order].ScrollIndex - 1);
        }

        public int GetVisibleScrollCount(int firstVisibleIndex, int viewportSize)
        {
            int displayIndex = FrozenCount+ firstVisibleIndex;
            int count = 0;

            foreach (var item in _allPositions)
            {
                if (item.DisplayIndex < displayIndex)
                {
                    continue;
                }
                if (item.EndPosition >= viewportSize)
                {
                    break;
                }
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
            if (column == null)
            {
                return -1;
            }
            return column.Index;
        }

        public int GetVisibleColumnLeftPosition(int visibleDisplayIndex)
        {
            if (visibleDisplayIndex < FrozenCount)
            {
                return _allPositions[visibleDisplayIndex].Position;
            }
            if (visibleDisplayIndex < FirstVisibleScrollColumnDisplayIndex)
            {
                throw new ArgumentException("the column is invisible");
            }

            return FrozenSize +
                   GetSizeSumBetweenScrollIndex(FirstVisibleScrollColumnIndex, visibleDisplayIndex - FrozenCount);
        }

        public bool IsVisible(int testedRealIndex, int firstVisibleScrollIndex, int viewportSize)
        {
            if (testedRealIndex < 0) return false;
            if (testedRealIndex >= 0 && testedRealIndex < FrozenCount) return true;
            int scrollIndex = testedRealIndex - FrozenCount;
            int onPageIndex = scrollIndex - firstVisibleScrollIndex;
            return onPageIndex >= 0 && onPageIndex < GetVisibleScrollCount(firstVisibleScrollIndex, viewportSize);
        }

      
        /// <summary>
        /// 当前可见的列数量
        /// </summary>
        public int VisibleScrollColumnCount
        {
            get { return this.GetVisibleScrollCount(this.FirstVisibleScrollColumnIndex, this.ScrollAreaWidth); }
        }

        public int GetDisplayIndexOnPosition(int position)
        {
            if (position < 0)
            {
                return -1;
            }
            var frozenSize = this.FrozenSize;

            var firstVisibileScrollColumnStartPosition =
                _allPositions[this.FirstVisibleScrollColumnDisplayIndex].Position;

            var isInFrozenRange = frozenSize > 0 && position <= frozenSize;

            foreach (var item in _allPositions)
            {
                if (isInFrozenRange)
                {
                    if (item.EndPosition > frozenSize)
                    {
                        return -1;
                    }

                    var begin = item.Position;
                    var end = item.EndPosition;
                    if (position >= begin && position <= end)
                    {
                        return item.DisplayIndex;
                    }
                }
                else
                {
                    if (item.EndPosition <= frozenSize)
                    {
                        continue;
                    }

                    var begin = (frozenSize + item.Position - firstVisibileScrollColumnStartPosition);
                    var end = (frozenSize + item.EndPosition - firstVisibileScrollColumnStartPosition);
                    if (position >= begin && position <= end)
                    {
                        return item.DisplayIndex;
                    }
                }
            }
            return -1;
        }
        public int GetResizingColumnDisplayIndexOnPosition(int position,int ColumnResizeTheresold)
        {
            if (position < 0)
            {
                return -1;
            }
            var frozenSize = this.FrozenSize;
          
            var firstVisibileScrollColumnStartPosition =
                _allPositions[this.FirstVisibleScrollColumnDisplayIndex].Position;

            var isInFrozenRange = frozenSize > 0 && position <= (frozenSize + ColumnResizeTheresold);

            foreach (var item in _allPositions)
            {
                if (isInFrozenRange)
                {
                    if (item.EndPosition > frozenSize)
                    {
                        return -1;
                    }

                    var begin = item.Position;
                    var end = item.EndPosition;
                    if (position >= begin - ColumnResizeTheresold && position <= begin + ColumnResizeTheresold)
                    {
                        return item.DisplayIndex - 1;
                    }

                    if (position >= end - ColumnResizeTheresold && position <= end + ColumnResizeTheresold)
                    {
                        return item.DisplayIndex;
                    }
                }
                else
                {
                    if (item.EndPosition <= frozenSize)
                    {
                        continue;
                    }
                    var begin = (frozenSize + item.Position - firstVisibileScrollColumnStartPosition);
                    var end = (frozenSize + item.EndPosition - firstVisibileScrollColumnStartPosition);
                    if (position >= begin - ColumnResizeTheresold && position <= begin + ColumnResizeTheresold)
                    {
                        return item.DisplayIndex - 1;
                    }

                    if (position >= end - ColumnResizeTheresold && position <= end + ColumnResizeTheresold)
                    {
                        return item.DisplayIndex;
                    }
                }
            }
            return -1;
        }
    }
}
