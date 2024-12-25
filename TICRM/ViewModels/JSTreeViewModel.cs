using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TICRM.ViewModels
{
    public class JSTreeViewModel
    {


        public class State
        {
            public bool opened { get; set; }
            public bool disabled { get; set; }
            public bool selected { get; set; }
        }

        public class LiAttr
        {
        }

        public class AAttr
        {
            public string href { get; set; }
        }

        public class Node
        {
            public string id { get; set; }
            public string text { get; set; }
            public string icon { get; set; }
            public State state { get; set; }
            //public List<Node> children { get; set; }
            public List<object> children { get; set; }
            public LiAttr li_attr { get; set; }
            public AAttr a_attr { get; set; }
        }



    }
}