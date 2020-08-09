namespace HandyPackage
{
    using System.Collections.Generic;
    using UnityEngine;

    public class MonoApplicationManager : MonoBehaviour
    {
        private List<IMonoApplicationFocus> _applicationFocus = new List<IMonoApplicationFocus>();
        private List<IMonoApplicationPause> _applicationPause = new List<IMonoApplicationPause>();
        private List<IMonoApplicationQuit> _applicationQuit = new List<IMonoApplicationQuit>();

        public void RegisterOnApplicationFocus(IMonoApplicationFocus applicationFocus)
        {
            if (_applicationFocus.Contains(applicationFocus)) return;
            _applicationFocus.Add(applicationFocus);
        }

        public void RegisterOnApplicationPause(IMonoApplicationPause applicationPause)
        {
            if (_applicationPause.Contains(applicationPause)) return;
            _applicationPause.Add(applicationPause);
        }

        public void RegisterOnApplicationQuit(IMonoApplicationQuit applicationQuit)
        {
            if (_applicationQuit.Contains(applicationQuit)) return;
            _applicationQuit.Add(applicationQuit);
        }

        public void RemoveOnApplicationFocus(IMonoApplicationFocus applicationFocus)
        {
            if (!_applicationFocus.Contains(applicationFocus)) return;
            _applicationFocus.Remove(applicationFocus);
        }

        public void RemoveOnApplicationPause(IMonoApplicationPause applicationPause)
        {
            if (!_applicationPause.Contains(applicationPause)) return;
            _applicationPause.Remove(applicationPause);
        }

        public void RemoveOnApplicationQuit(IMonoApplicationQuit applicationQuit)
        {
            if (!_applicationQuit.Contains(applicationQuit)) return;
            _applicationQuit.Remove(applicationQuit);
        }

        public void OnApplicationFocus(bool focusStatus)
        {
            int count = _applicationFocus.Count;
            for (int i = 0; i < count; i++)
            {
                _applicationFocus[i].OnApplicationFocus(focusStatus);
            }
        }

        public void OnApplicationPause(bool pauseStatus)
        {
            int count = _applicationPause.Count;
            for (int i = 0; i < count; i++)
            {
                _applicationPause[i].OnApplicationPause(pauseStatus);
            }
        }

        public void OnApplicationQuit()
        {
            int count = _applicationQuit.Count;
            for (int i = 0; i < count; i++)
            {
                _applicationQuit[i].OnApplicationQuit();
            }
        }
    }
}