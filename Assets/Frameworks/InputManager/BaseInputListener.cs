using System;

namespace HandyPackage
{
    [Serializable]
    public abstract class BaseInputListener : IInputListener
    {
        public abstract void CheckForInput();
    }

    [Serializable]
    public abstract class InputListener : BaseInputListener
    {
        private Action _actionOnInput;

        protected abstract bool IsInputTriggered();

        public override void CheckForInput()
        {
            if (IsInputTriggered())
            {
                _actionOnInput.Invoke();
            }
        }

        public void SetAction(Action action)
        {
            this._actionOnInput = action;
        }
    }

    [Serializable]
    public abstract class InputListener<T> : BaseInputListener
    {
        private Action<T> _actionOnInput;

        protected abstract bool IsInputTriggered(out T inputResult);

        public override void CheckForInput()
        {
            if (IsInputTriggered(out T inputResult))
            {
                _actionOnInput.Invoke(inputResult);
            }
        }

        public void SetAction(Action<T> action)
        {
            this._actionOnInput = action;
        }
    }

}
