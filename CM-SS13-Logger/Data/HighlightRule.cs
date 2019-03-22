using System.ComponentModel;
using System.Drawing;

namespace CM_SS13_Logger
{
    public class HighlightRule
    {
        [DisplayName("Expression")]
        public string Expression { get; set; }

        [DisplayName("Foreground Color")]
        public KnownColor ForegroundColor { get; set; } = KnownColor.Black;

        [DisplayName("Background Color")]
        public KnownColor BackgroundColor { get; set; } = KnownColor.White;

        [DisplayName("Bold")]
        public bool Bold { get; set; }

        [DisplayName("Italic")]
        public bool Italic { get; set; }

        [DisplayName("Underline")]
        public bool Underline { get; set; }
    }
}
