using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Constants;

namespace YksMC.Protocol.Models.Attributes
{
    public class ConditionalAttribute : Attribute
    {
        public string Field { get; set; }
        public Condition Condition { get; set; }
        public object[] Values { get; set; }

        public ConditionalAttribute(string field, Condition condition, params object[] values)
        {
            Field = field;
            Condition = condition;
            Values = values;
        }

        public ConditionalAttribute(string field, Condition condition, object value)
            : this(field, condition, new object[] { value })
        {
        }
    }
}
