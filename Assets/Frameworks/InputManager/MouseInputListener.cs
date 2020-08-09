using System;
using UnityEngine;

namespace HandyPackage
{
    [Serializable]
    public class MouseInputListener : InputListener<MouseInputResult>
    {
        public MouseButton mouseInput;
        public MouseTriggerType mouseTriggerType = MouseTriggerType.DOWN;

        private static MouseInputResult nullMouseInputResult = new MouseInputResult();

        protected override bool IsInputTriggered(out MouseInputResult mouseInputResult)
        {
            bool isInput = false;

            if (mouseTriggerType.HasFlag(MouseTriggerType.DOWN))
            {
                isInput |= Input.GetMouseButtonDown((int)mouseInput);
            }
            if (mouseTriggerType.HasFlag(MouseTriggerType.PRESSED))
            {
                isInput |= Input.GetMouseButton((int)mouseInput);
            }
            if (mouseTriggerType.HasFlag(MouseTriggerType.UP))
            {
                isInput |= Input.GetMouseButtonUp((int)mouseInput);
            }
            if (isInput)
            {
                mouseInputResult = new MouseInputResult()
                {
                    mousePosition = Input.mousePosition
                };
            }
            else
            {
                mouseInputResult = nullMouseInputResult;
            }
            return isInput;
        }
    }

    public enum MouseButton
    {
        LEFT_CLICK = 0,
        RIGHT_CLICK = 1,
        MIDDLE_CLICK = 2
    }

    [Flags]
    public enum MouseTriggerType
    {
        DOWN = 1,
        PRESSED = 1 << 1,
        UP = 1 << 2
    }

    public struct MouseInputResult
    {
        public Vector2 mousePosition;
    }

}
