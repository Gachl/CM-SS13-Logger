using System.ComponentModel;

namespace CM_SS13_Logger
{
    public class ParseRule
    {
        [DisplayName("Regular Expression")]
        public string Expression { get; set; } = "^(?<Message>.*)$";

        [DisplayName("Multiline")]
        public bool Multiline { get; set; } = false;
    }
}
