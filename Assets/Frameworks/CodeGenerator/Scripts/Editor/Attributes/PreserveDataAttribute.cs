using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HandyPackage.CodeGeneration
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class PreserveDataAttribute : Attribute
    {
        
    }
}

