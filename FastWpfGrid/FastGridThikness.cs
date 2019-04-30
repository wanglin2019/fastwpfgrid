using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace FastWpfGrid
{
   
    public struct FastGridThickness : IEquatable<FastGridThickness>
    {
        private int _Left;
        private int _Top;
        private int _Right;
        private int _Bottom;

        /// <summary>初始化 <see cref="T:System.Windows.Thickness" /> 结构的新实例，此结构的各边使用指定的统一长度。</summary>
        /// <param name="uniformLength">应用到边框所有四个边的统一长度。</param>
        public FastGridThickness(int uniformLength)
        {
            this._Left = this._Top = this._Right = this._Bottom = uniformLength;
        }

        /// <summary>初始化 <see cref="T:System.Windows.Thickness" /> 结构的新实例，该矩形结构的各边应用了特定的长度（作为 <see cref="T:System.int" /> 提供）。</summary>
        /// <param name="left">矩形左边的粗细。</param>
        /// <param name="top">矩形顶边的粗细。</param>
        /// <param name="right">矩形右边的粗细。</param>
        /// <param name="bottom">矩形底边的粗细。</param>
        public FastGridThickness(int left, int top, int right, int bottom)
        {
            this._Left = left;
            this._Top = top;
            this._Right = right;
            this._Bottom = bottom;
        }

        /// <summary>获取或设置边框左边的宽度（以像素为单位）。</summary>
        /// <returns>一个 <see cref="T:System.int" />，表示此  <see cref="T:System.Windows.Thickness" /> 实例的边框左边的宽度（以像素为单位）。1 像素等于 1/96 英寸。默认值为 0。</returns>
        public int Left
        {
            get
            {
                return this._Left;
            }
            set
            {
                this._Left = value;
            }
        }

        /// <summary>获取或设置边框顶边的宽度（以像素为单位）。</summary>
        /// <returns>
        /// <see cref="T:System.Windows.Thickness" /> 的此实例的 <see cref="T:System.int" />，表示边框顶边的宽度（以像素为单位）。1 像素等于 1/96 英寸。默认值为 0。</returns>
        public int Top
        {
            get
            {
                return this._Top;
            }
            set
            {
                this._Top = value;
            }
        }

        /// <summary>获取或设置边框右边的宽度（以像素为单位）。</summary>
        /// <returns>
        /// <see cref="T:System.Windows.Thickness" /> 的此实例的 <see cref="T:System.int" />，表示边框右边的宽度（以像素为单位）。1 像素等于 1/96 英寸。默认值为 0。</returns>
        public int Right
        {
            get
            {
                return this._Right;
            }
            set
            {
                this._Right = value;
            }
        }

        /// <summary>获取或设置边框底边的宽度（以像素为单位）。</summary>
        /// <returns>
        /// <see cref="T:System.Windows.Thickness" /> 的此实例的 <see cref="T:System.int" />，表示边框底边的宽度（以像素为单位）。1 像素等于 1/96 英寸。默认值为 0。</returns>
        public int Bottom
        {
            get
            {
                return this._Bottom;
            }
            set
            {
                this._Bottom = value;
            }
        }


        /// <summary>比较此 <see cref="T:System.Windows.Thickness" /> 结构与另一个 <see cref="T:System.Object" /> 是否相等。</summary>
        /// <returns>如果两个对象相等，则为 true；否则为 false。</returns>
        /// <param name="obj">要比较的对象。</param>
        public override bool Equals(object obj)
        {
            if (obj is FastGridThickness)
                return this == (FastGridThickness)obj;
            return false;
        }

        /// <summary>比较此 <see cref="T:System.Windows.Thickness" /> 与另一个 <see cref="T:System.Windows.Thickness" /> 结构是否相等。</summary>
        /// <returns>如果 <see cref="T:System.Windows.Thickness" /> 的两个实例相等，则为 true；否则为 false。</returns>
        /// <param name="thickness">要对其进行比较以看是否相等的 <see cref="T:System.Windows.Thickness" /> 的一个实例。</param>
        public bool Equals(FastGridThickness thickness)
        {
            return this == thickness;
        }

        /// <summary>返回结构的哈希代码。</summary>
        /// <returns>此 <see cref="T:System.Windows.Thickness" /> 实例的哈希代码。</returns>
        public override int GetHashCode()
        {
            return this._Left.GetHashCode() ^ this._Top.GetHashCode() ^ this._Right.GetHashCode() ^ this._Bottom.GetHashCode();
        }

        /// <summary>比较两个 <see cref="T:System.Windows.Thickness" /> 结构的值是否相等。</summary>
        /// <returns>如果 <see cref="T:System.Windows.Thickness" /> 的两个实例相等，则为 true；否则为 false。</returns>
        /// <param name="t1">要比较的第一个结构。</param>
        /// <param name="t2">要比较的另一个结构。</param>
        public static bool operator ==(FastGridThickness t1, FastGridThickness t2)
        {
            if (t1.Left == t2.Left
                && t1.Top == t2.Top
                && t1.Right == t2.Right
                && t1.Bottom == t2.Bottom)
            {
                return true;
            }

            return false;
        }

        /// <summary>比较两个 <see cref="T:System.Windows.Thickness" /> 结构是否不相等。</summary>
        /// <returns>如果 <see cref="T:System.Windows.Thickness" /> 的两个实例不相等，则为 true；否则为 false。</returns>
        /// <param name="t1">要比较的第一个结构。</param>
        /// <param name="t2">要比较的另一个结构。</param>
        public static bool operator !=(FastGridThickness t1, FastGridThickness t2)
        {
            return !(t1 == t2);
        }

        public IntRect ToRect()
        {
            var rect = new IntRect();
            rect.Left = this.Left;
            rect.Top = this.Top;
            rect.Right = this.Right;
            rect.Bottom = this.Bottom;
            return rect;
        }

        public int Width
        {
            get
            {
                return this.Left + this.Right;
            }
        }

        public int Height
        {
            get
            {
                return this.Top + this.Bottom;
            }
        }
    }
}
