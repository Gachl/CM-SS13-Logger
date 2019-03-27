using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CM_SS13_Logger
{
    public class HighlightRule
    {
        [DisplayName("Expression")]
        public string Expression { get; set; }

        [DisplayName("Foreground Color")]
        public KnownColor? ForegroundColor { get; set; } = KnownColor.Black;

        [DisplayName("Background Color")]
        public KnownColor? BackgroundColor { get; set; } = KnownColor.White;

        [DisplayName("Bold")]
        public CheckState Bold { get; set; }

        [DisplayName("Italic")]
        public CheckState Italic { get; set; }

        [DisplayName("Underline")]
        public CheckState Underline { get; set; }

        [DisplayName("Stop at this rule")]
        public bool Stop { get; set; }
    }
}
