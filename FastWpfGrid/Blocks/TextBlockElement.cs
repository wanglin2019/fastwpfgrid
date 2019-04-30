using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FastWpfGrid.BlockElements
{
    public class TextBlockElement: FastGridBlockImpl
    {
        public TextBlockElement(IFastGridCell cell):base(cell)
        {
            this.FontStyle = new FontStyle();
            this.FontStyle.FontName = "Arial";
            this.FontStyle.EmSize = 12;
        }

        public Color? FontColor { get; set; }
        public bool IsItalic {
            get
            {
                return this.FontStyle.IsItalic;
            }
            set
            {
                this.FontStyle.IsItalic = value;
            }
        }

        public bool IsBold
        {
            get
            {
                return this.FontStyle.IsBold;
            }
            set
            {
                this.FontStyle.IsBold = value;
            }
        }

        public string TextData { get;
            set;
        }

        public FontStyle FontStyle
        {
            get;
          
            set;
        }

        public override FastGridBlockType BlockType
        {
            get
            {
                return FastGridBlockType.Text;
            }
        }

        public override int GetWidth(int? maxSize = null)
        {
            string text = this.TextData;
            var font = GetFont(this.IsBold, this.IsItalic);
            return font.GetTextWidth(text, maxSize);
        }

        public override int GetHeight()
        {
            string text = this.TextData;
            var font = GetFont(this.IsBold, this.IsItalic);
            return font.GetTextHeight(text);
        }

        private Dictionary<Tuple<bool, bool>, GlyphFont> _glyphFonts = new Dictionary<Tuple<bool, bool>, GlyphFont>();
        public GlyphFont GetFont(bool isBold, bool isItalic)
        {
            var key = Tuple.Create(isBold, isItalic);
            if (!_glyphFonts.ContainsKey(key))
            {
                var font = LetterGlyphTool.GetFont(new PortableFontDesc(FontStyle.FontName, FontStyle.EmSize, FontStyle.IsBold, FontStyle.IsItalic, FontStyle.IsClearType));
                _glyphFonts[key] = font;
            }
            return _glyphFonts[key];
        }
    }
}
