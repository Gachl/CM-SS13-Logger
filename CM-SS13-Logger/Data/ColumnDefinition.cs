using System.ComponentModel;

namespace CM_SS13_Logger
{
    public class ColumnDefinition
    {
        [DisplayName("Name")]
        public string ColumnName { get; set; }

        [DisplayName("Label")]
        public string ColumnLabel { get; set; }

        [Browsable(false)]
        public int Width { get; set; }
    }
}
