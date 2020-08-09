namespace Asteroid
{
    using HandyPackage;
    using System.Collections.Generic;
    using Debug = UnityEngine.Debug;

    public class InputManager : ITickable
    {
        private List<IInputListener> _inputListeners = new List<IInputListener>();

        public void Tick()
        {
            int count = _inputListeners.Count;
            for (int i = 0; i < count; i++)
            {
                _inputListeners[i]?.CheckForInput();
            }
        }

        public void RegisterInputListener(IInputListener inputListener)
        {
            _inputListeners.Add(inputListener);
        }

        public void RemoveInputListener(IInputListener inputListener)
        {
            if (!_inputListeners.Contains(inputListener))
            {
                Debug.LogError($"InputListener not found : {inputListener.GetType().Name}");
                return;
            }
            _inputListeners.Remove(inputListener);
        }
    }
}
