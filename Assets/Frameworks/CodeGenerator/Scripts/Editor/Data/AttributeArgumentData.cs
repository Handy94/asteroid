using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandyPackage.CodeGeneration
{
    public class AttributeArgumentData
    {
        public string m_ArgumentName;
        public string m_ArgumentType;
        public string m_ArgumentValue;
        public bool m_IsPartOfConstructor = true;

        public AttributeArgumentData(string argName, string argType, string argValue, bool isPartOfConstructor = true)
        {
            m_ArgumentName = argName;
            m_ArgumentType = argType;
            m_ArgumentValue = argValue;
            m_IsPartOfConstructor = isPartOfConstructor;
        }
    }
}


