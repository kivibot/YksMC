using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace YksMC.Protocol.Tests.TestUtils
{
    public class ComparisonUtil
    {
        public static bool Compare(object first, object second)
        {
            if (first == null || second == null)
            {
                if (first == null && second == null)
                    return true;
                return false;
            }
            if (first.GetType() != second.GetType())
                return false;

            foreach(PropertyInfo property in first.GetType().GetRuntimeProperties())
            {
                if (!object.Equals(property.GetValue(first), property.GetValue(second)))
                    return false;
            }
            return true;
        }
    }
}
