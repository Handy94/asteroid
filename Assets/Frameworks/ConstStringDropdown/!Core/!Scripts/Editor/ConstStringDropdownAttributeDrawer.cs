#if UNITY_EDITOR
namespace HandyPackage.Editor
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEditor;

    [CustomPropertyDrawer(typeof(ConstStringDropdownAttribute))]
    public class ConstStringDropdownAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ConstStringDropdownAttribute attr = attribute as ConstStringDropdownAttribute;

            string val = property.stringValue;
            int selectedIdx = attr.IndexOf(val);
            List<string> options = null;
            switch (attr.showMode)
            {
                case ConstStringDropdownShowMode.BOTH:
                    options = attr.constList.Select(x => $"{x.Name} ({x.GetRawConstantValue().ToString()})").ToList();
                    break;
                case ConstStringDropdownShowMode.VARIABLE_NAME_ONLY:
                    options = attr.constList.Select(x => x.Name).ToList();
                    break;
                case ConstStringDropdownShowMode.VALUE_ONLY:
                    options = attr.constList.Select(x => x.GetRawConstantValue().ToString()).ToList();
                    break;
            }

            bool isValueExist = selectedIdx >= 0;
            if (!isValueExist)
            {
                options.Insert(0, $"*UNKNOWN_CONSTANT* ({val})");
                selectedIdx = 0;
            }

            EditorGUI.BeginChangeCheck();
            int idx = EditorGUI.Popup(position, label.text, selectedIdx, options.ToArray());
            if (EditorGUI.EndChangeCheck())
            {
                selectedIdx = (isValueExist) ? idx : idx - 1;
                if (selectedIdx >= 0) property.stringValue = attr.GetConstantValue(selectedIdx);
            }
        }
    }
}
#endif