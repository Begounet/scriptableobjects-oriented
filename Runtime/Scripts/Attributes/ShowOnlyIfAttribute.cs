using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    [System.AttributeUsage(System.AttributeTargets.Field, Inherited = true)]
    public class ShowOnlyIfAttribute : PropertyAttribute
    {
        public string propertyDependencyName;
        public bool expectedValue = true;

        public ShowOnlyIfAttribute(string PropertyDependencyName, bool ExpectedValue = true)
        {
            propertyDependencyName = PropertyDependencyName;
            expectedValue = ExpectedValue;
        }
    }
}