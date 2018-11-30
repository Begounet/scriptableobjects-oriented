using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    [System.AttributeUsage(System.AttributeTargets.Field, Inherited = true)]
    public class ShowOnlyIfTrueAttribute : PropertyAttribute
    {
        public string propertyDependencyName;

        public ShowOnlyIfTrueAttribute(string PropertyDependencyName)
        {
            propertyDependencyName = PropertyDependencyName;
        }
    }
}