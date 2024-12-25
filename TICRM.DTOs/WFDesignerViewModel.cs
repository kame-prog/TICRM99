using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TICRM.DTOs
{
    public class WFDesignerViewModel
    {

        public class ModelData
        {
            public string position { get; set; }
        }

        public class NodeDataArray
        {
            public string text { get; set; }
            public string key { get; set; }
            public string figure { get; set; }
            public string fill { get; set; }
            public string loc { get; set; }
        }

        public class LinkDataArray
        {
            public string from { get; set; }
            public string to { get; set; }
            public int iterate { get; set; }
        }

        public class workflowDesigner
        {
            public string @class { get; set; }
            public string nodeKeyProperty { get; set; }
            public string linkKeyProperty { get; set; }
            public ModelData modelData { get; set; }
            public List<WorkFlowNodeDTO> nodeDataArray { get; set; }
            public List<LinkDataArray> linkDataArray { get; set; }

            public string Name { get; set; }
            public string TriggerCondition { get; set; }
            public Nullable<System.DateTime> TriggerIn { get; set; }
            public Nullable<System.DateTime> TriggerOut { get; set; }
            public string TargetOn { get; set; }
            public string Description { get; set; }
            public string WorkFlowStatus { get; set; }
            public string AppliedTo { get; set; }
            public Nullable<int> Frequency { get; set; }
            public Nullable<int> FrequencyOut { get; set; }
            public Nullable<int> Priority { get; set; }
            public string Action { get; set; }
            public string AccountId { get; set; }
        }

    }
}
