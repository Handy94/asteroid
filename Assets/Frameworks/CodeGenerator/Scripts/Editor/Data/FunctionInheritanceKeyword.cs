using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandyPackage.CodeGeneration
{
    public enum FunctionInheritanceKeyword
    {
        // if function is static, it cannot be virtual or override
        NONE, STATIC, VIRTUAL, OVERRIDE
    }
}


