using System;
using System.Collections.Generic;
using UnityEngine;


public class EventBusService : IService
{
    private Dictionary<string, List<Delegate>> _subscribers = new Dictionary<string, List<Delegate>>();

    public EventBusService()
    {
        Register();
    }

    public void Subscribe(string key, Action callback)
    {
        if (!_subscribers.ContainsKey(key))
            _subscribers[key] = new List<Delegate>();

        if (!_subscribers[key].Contains(callback))
        {
            _subscribers[key].Add(callback);
        }

    }

    public void Subscribe<T>(string key, Action<T> callback)
    {
        if (!_subscribers.ContainsKey(key))
        {
            _subscribers[key] = new List<Delegate> { callback };
        }
        else
        {
            if (!_subscribers[key].Contains(callback))
            {
                _subscribers[key].Add(callback);
            }
        }
    }

    public void Subscribe<T1, T2>(string key, Action<T1, T2> callback)
    {
        if (!_subscribers.ContainsKey(key))
        {
            _subscribers[key] = new List<Delegate> { callback };
        }
        else
        {
            if (!_subscribers[key].Contains(callback))
            {
                _subscribers[key].Add(callback);
            }
        }
    }



    public void UnSusbcribe(string key, Action callback)
    {
        if (!_subscribers.ContainsKey(key))
        {
            Debug.LogError($"Can not Unsuscribe the key: {key} not exist on Susbcribers Dictionary");
            return;
        }

        _subscribers[key].Remove(callback);
        if (_subscribers[key].Count == 0)
            _subscribers.Remove(key);

    }

    public void UnSusbcribe<T>(string key, Action<T> callback)
    {
        if (!_subscribers.ContainsKey(key))
        {
            Debug.LogError($"Can not Unsuscribe the key: {key} not exist on Susbcribers Dictionary");
            return;
        }

        _subscribers[key].Remove(callback);
        if (_subscribers[key].Count == 0)
            _subscribers.Remove(key);

    }

    public void UnSusbcribe<T1, T2>(string key, Action<T1, T2> callback)
    {
        if (!_subscribers.ContainsKey(key))
        {
            Debug.LogError($"Can not Unsuscribe the key: {key} not exist on Susbcribers Dictionary");
            return;
        }

        _subscribers[key].Remove(callback);
        if (_subscribers[key].Count == 0)
            _subscribers.Remove(key);

    }


    public void Broadcast(string key)
    {

        if (!_subscribers.ContainsKey(key))
        {
            Debug.LogWarning($"Can not Broadcast Event the key: {key} not exist on Susbcribers Dictionary");
            return;
        }

        var subs = new List<Delegate>(_subscribers[key]);
        foreach (var s in subs)
        {
            try
            {
                (s as Action)?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error invoking {s.Method.Name}: {ex.Message}");
            }
        }
    }

    public void Broadcast<T>(string key, T eventData)
    {
        if (!_subscribers.ContainsKey(key))
        {
            Debug.LogWarning($"Can not Broadcast Event the key: {key} not exist on Susbcribers Dictionary");
            return;
        }

        var subs = new List<Delegate>(_subscribers[key]);
        foreach (var s in subs)
        {
            try
            {
                (s as Action<T>)?.Invoke(eventData);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error invoking {s.Method.Name}: {ex.Message}");
            }
        }

    }

    public void Broadcast<T1, T2>(string key, T1 eventData1, T2 eventData2)
    {
        if (!_subscribers.ContainsKey(key))
        {
            Debug.LogWarning($"Can not Broadcast Event the key: {key} not exist on Susbcribers Dictionary");
            return;
        }

        var subs = new List<Delegate>(_subscribers[key]);
        foreach (var s in subs)
        {
            try
            {
                (s as Action<T1, T2>)?.Invoke(eventData1, eventData2);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error invoking {s.Method.Name}: {ex.Message}");
            }
        }
    }




    public void Register()
    {
        _subscribers = new Dictionary<string, List<Delegate>>();
        ServiceLocator.Instance.Register(this);
    }

    public void Unregister()
    {
        ServiceLocator.Instance.Unregister(this);
    }
}