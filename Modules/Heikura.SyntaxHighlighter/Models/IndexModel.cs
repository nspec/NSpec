using System.Collections.Generic;

namespace Heikura.Orchard.Modules.SyntaxHighlighter.Models
{
    public class IndexModel {
        public string Theme {
            get;
            set;
        }

        public List<string> Themes {
            get;
            set;
        }
    }
}