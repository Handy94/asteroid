using System;
using UnityEngine;

namespace HandyPackage
{
    [Serializable]
    public class KeyboardInputListener : InputListener
    {
        public KeyCode keyCode;
        public KeyboardTriggerType keyboardTriggerType;

        protected override bool IsInputTriggered()
        {
            bool isInput = false;
            var val = keyCode;
            if (keyboardTriggerType.HasFlag(KeyboardTriggerType.DOWN))
            {
                isInput |= Input.GetKeyDown(keyCode);
            }
            if (keyboardTriggerType.HasFlag(KeyboardTriggerType.PRESSED))
            {
                isInput |= Input.GetKey(keyCode);
            }
            if (keyboardTriggerType.HasFlag(KeyboardTriggerType.UP))
            {
                isInput |= Input.GetKeyUp(keyCode);
            }
            return isInput;
        }

        [Flags]
        public enum KeyboardTriggerType
        {
            DOWN = 1,
            PRESSED = 1 << 1,
            UP = 1 << 2,
        }
    }

}
