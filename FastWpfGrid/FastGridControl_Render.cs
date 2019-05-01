using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FastWpfGrid.CellRenders;

namespace FastWpfGrid
{
    partial class FastGridControl
    {
        private void RenderGrid()
        {
            var start = DateTime.Now;
            if (_drawBuffer == null)
            {
                ClearInvalidation();
                return;
            }
            using (_drawBuffer.GetBitmapContext())
            {
                if (this._resizingColumn != null)
                {
                    var ddd = 0;
                }
                int colsToRender = _columnSizes.VisibleScrollColumnCount;
                int rowsToRender = VisibleRowCount;

                if (_invalidatedCells.Count > 250)
                {
                    _isInvalidatedAll = true;
                }

                if (!_isInvalidated || _isInvalidatedAll)
                {
                    _drawBuffer.Clear(Colors.White);
                }

                if (ShouldDrawGridHeader())
                {
                    RenderGridHeader();
                }

                // render frozen rows
                for (int row = 0; row < _rowSizes.FrozenCount; row++)
                {
                    for (int col = 0; col < _columnSizes.FrozenCount; col++)
                    {
                        if (!ShouldDrawCell(row, col)) continue;
                        RenderCell(row, col);
                    }

                    for (int col = _columnSizes.FirstVisibleScrollColumnDisplayIndex; 
                        col < _columnSizes.LastVisibleScrollColumnDisplayIndex; col++)
                    {
                        if (!ShouldDrawCell(row, col))
                        {
                            continue;
                        }

                        RenderCell(row, col);
                    }
                }

                // render cells
                for (int row = _rowSizes.FrozenCount+ FirstVisibleRowScrollIndex; 
                    row < _rowSizes.FrozenCount+ FirstVisibleRowScrollIndex + rowsToRender; row++)
                {
                    for (int col = 0; col < _columnSizes.FrozenCount; col++)
                    {
                        if (!ShouldDrawCell(row, col)) continue;
                        RenderCell(row, col);
                    }

                    for (int col = _columnSizes.FirstVisibleScrollColumnDisplayIndex;
                        col < _columnSizes.LastVisibleScrollColumnDisplayIndex; col++)
                    {
                        if (row < 0 || col < 0 || row >= _realRowCount || col >= _realColumnCount) continue;
                        if (!ShouldDrawCell(row, col)) continue;
                        RenderCell(row, col);
                    }
                }

                // render frozen row headers
                for (int row = 0; row < _rowSizes.FrozenCount; row++)
                {
                    if (!ShouldDrawRowHeader(row)) continue;
                    RenderRowHeader(row);
                }

                // render row headers
                for (int row = _rowSizes.FrozenCount+ FirstVisibleRowScrollIndex; 
                    row < _rowSizes.FrozenCount+ FirstVisibleRowScrollIndex + rowsToRender; row++)
                {
                    if (row < 0 || row >= _realRowCount) continue;
                    if (!ShouldDrawRowHeader(row)) continue;
                    RenderRowHeader(row);
                }

                // render frozen column headers
                for (int col = 0; col < _columnSizes.FrozenCount; col++)
                {
                    if (!ShouldDrawColumnHeader(col)) continue;
                    RenderColumnHeader(col);
                }


                // render column headers
                for (int col =  _columnSizes.FirstVisibleScrollColumnDisplayIndex;
                    col <  _columnSizes.LastVisibleScrollColumnDisplayIndex; col++)
                {
                    if (col < 0 || col >= _realColumnCount) continue;
                    if (!ShouldDrawColumnHeader(col)) continue;
                    RenderColumnHeader(col);
                }
            }

            if (_isInvalidatedAll)
            {
                Debug.WriteLine("Render full grid: {0} ms", Math.Round((DateTime.Now - start).TotalMilliseconds));
            }
            ClearInvalidation();
        }

        private void RenderGridHeader()
        {
            var cell = GridHeaderCell;
            var rect = GetGridHeaderRect();
            RenderCell(cell, rect, null, HeaderBackground, FastGridCellAddress.GridHeader);
        }

        private void RenderColumnHeader(int col)
        {
            var cell = GetColumnHeader(col);

            Color? selectedBgColor = null;
            if (col == _currentCell.Column) selectedBgColor = HeaderCurrentBackground;

            var rect = GetColumnHeaderRect(col);
            Debug.WriteLine($"column:{col}, start:{rect.Left}, width:{rect.Width}");

            Color? cellBackground = null;
            if (cell != null) cellBackground = cell.BackgroundColor;

            Color? hoverColor = null;
            if (col == _mouseOverColumnHeader) hoverColor = MouseOverRowColor;

            RenderCell(cell, rect, null, hoverColor ?? selectedBgColor ?? cellBackground ?? HeaderBackground, new FastGridCellAddress(null, col));
        }

        private void RenderRowHeader(int row)
        {
            var cell = GetRowHeader(row);

            Color? selectedBgColor = null;
            if (row == _currentCell.Row) selectedBgColor = HeaderCurrentBackground;

            var rect = GetRowHeaderRect(row);
            Color? cellBackground = null;
            if (cell != null) cellBackground = cell.BackgroundColor;

            Color? hoverColor = null;
            if (row == _mouseOverRowHeader) hoverColor = MouseOverRowColor;

            RenderCell(cell, rect, null, hoverColor ?? selectedBgColor ?? cellBackground ?? HeaderBackground, new FastGridCellAddress(row, null));
        }

        private void RenderCell(int row, int col)
        {
            var rect = GetCellRect(row, col);
            
            var cell = GetCell(row, col);
            if (cell == null)
            {
                Debugger.Break();
            }

            
            Color? selectedBgColor = null;
            Color? selectedTextColor = null;
            Color? hoverRowColor = null;
            if (_currentCell.TestCell(row, col) || _selectedCells.Contains(new FastGridCellAddress(row, col)))
            {
                selectedBgColor = _isLimitedSelection ? LimitedSelectedColor : SelectedColor;
                selectedTextColor = _isLimitedSelection ? LimitedSelectedTextColor : SelectedTextColor;
            }
            if (row == _mouseOverRow)
            {
                hoverRowColor = MouseOverRowColor;
            }


            Color? cellBackground = null;
            if (cell != null) cellBackground = cell.BackgroundColor;

            RenderCell(cell, rect, selectedTextColor, selectedBgColor
                                                      ?? hoverRowColor
                                                      ?? cellBackground
                                                      ?? GetAlternateBackground(row),
                                                      new FastGridCellAddress(row, col));
        }

        private void RenderCell(IFastGridCell cell, IntRect rect, Color? selectedTextColor, Color bgColor, FastGridCellAddress cellAddr)
        {
            var parameter = new CellRenderParameter()
            {
                Control = this,
                cell = cell,
                rect = rect,
                selectedTextColor = selectedTextColor,
                bgColor = bgColor,
                cellAddr = cellAddr,
            };
            cell.Renderer.Render(parameter);
        }

        private void ScrollContent(int row, int column)
        {
            if (row == FirstVisibleRowScrollIndex && column == _columnSizes.FirstVisibleScrollColumnIndex)
            {
                return;
            }

            if (row != FirstVisibleRowScrollIndex && !_isInvalidatedAll && column == _columnSizes.FirstVisibleScrollColumnIndex
                && Math.Abs(row - FirstVisibleRowScrollIndex) * 2 < VisibleRowCount)
            {
                using (var ctx = CreateInvalidationContext())
                {
                    int scrollY = _rowSizes.GetScroll(FirstVisibleRowScrollIndex, row);
                    _rowSizes.InvalidateAfterScroll(FirstVisibleRowScrollIndex, row, InvalidateRow, GridScrollAreaHeight);
                    FirstVisibleRowScrollIndex = row;

                    _drawBuffer.ScrollY(scrollY, GetScrollRect());
                    _drawBuffer.ScrollY(scrollY, GetRowHeadersScrollRect());
                    if (_columnSizes.FrozenCount > 0) _drawBuffer.ScrollY(scrollY, GetFrozenColumnsRect());
                }
                // if row heights are changed, invalidate all
                if (CountVisibleRowHeights())
                {
                    InvalidateAll();
                }
                OnScrolledModelRows();
                return;
            }

            if (column != _columnSizes.FirstVisibleScrollColumnIndex && !_isInvalidatedAll && row == FirstVisibleRowScrollIndex
                && Math.Abs(column - _columnSizes.FirstVisibleScrollColumnIndex) * 2 < _columnSizes.VisibleScrollColumnCount)
            {
                using (var ctx = CreateInvalidationContext())
                {
                    int scrollX = _columnSizes.GetScroll(_columnSizes.FirstVisibleScrollColumnIndex, column);
                    _columnSizes.InvalidateAfterScroll(_columnSizes.FirstVisibleScrollColumnIndex, column, InvalidateColumn, GridScrollAreaWidth);
                    _columnSizes.FirstVisibleScrollColumnIndex = column;

                    _drawBuffer.ScrollX(scrollX, GetScrollRect());
                    _drawBuffer.ScrollX(scrollX, GetColumnHeadersScrollRect());
                    if (_rowSizes.FrozenCount > 0) _drawBuffer.ScrollX(scrollX, GetFrozenRowsRect());
                }
                OnScrolledModelColumns();
                return;
            }

            bool changedRow = FirstVisibleRowScrollIndex != row;
            bool changedCol = _columnSizes.FirstVisibleScrollColumnIndex != column;

            // render all
            using (var ctx = CreateInvalidationContext())
            {
                FirstVisibleRowScrollIndex = row;
                _columnSizes.FirstVisibleScrollColumnIndex = column;
                CountVisibleRowHeights();
                InvalidateAll();
            }

            if (changedRow)
            {
                OnScrolledModelRows();
            }
            if (changedCol)
            {
                OnScrolledModelColumns();
            }
        }
    }
}
