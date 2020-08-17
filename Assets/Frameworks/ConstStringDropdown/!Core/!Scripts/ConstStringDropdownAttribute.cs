namespace HandyPackage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Linq;
    using UnityEngine;

    public class ConstStringDropdownAttribute : PropertyAttribute
    {
        public readonly Type _classContainerType;
        public ConstStringDropdownShowMode showMode = ConstStringDropdownShowMode.BOTH;

        public List<FieldInfo> constList;

        public ConstStringDropdownAttribute(Type classContainerType)
        {
            _classContainerType = classContainerType;

            constList = _classContainerType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                                               .Where(x => x.IsLiteral && !x.IsInitOnly)
                                               .Where(x => x.FieldType.Equals(typeof(string)))
                                               .ToList();
            var nestedConst = _classContainerType.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic)
                                                 .SelectMany(t => t.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy))
                                               .Where(x => x.IsLiteral && !x.IsInitOnly)
                                               .Where(x => x.FieldType.Equals(typeof(string)))
                                               .ToList();
            constList.AddRange(nestedConst);
        }

        public ConstStringDropdownAttribute(Type classContainerType, ConstStringDropdownShowMode showMode) : this(classContainerType)
        {
            this.showMode = showMode;
        }

        public int IndexOf(string value)
        {
            var findObj = constList.Find(x => value.Equals(x.GetRawConstantValue().ToString()));
            if (findObj != null)
            {
                return constList.IndexOf(findObj);
            }
            return -1;
        }

        public string GetConstantValue(int index)
        {
            if (index >= constList.Count)
                return string.Empty;
            return constList[index].GetRawConstantValue().ToString();
        }
    }
}
