using System;
using System.Collections;
using System.Collections.Generic;

public class PubSub
{
    private static Dictionary<Type, List<Action<object>>> listeners = new Dictionary<Type, List<Action<object>>>();

    public static void RegisterListener<T>(Action<object> listener) where T : class
    {
        if (!listeners.ContainsKey(typeof(T)))
            listeners.Add(typeof(T), new List<Action<object>>());

        listeners[typeof(T)].Add(listener);
    }

    public static bool UnregisterListener<T> (Action<object> listener) where T : class
    {
        if (listeners.ContainsKey(typeof(T)) && listeners[typeof(T)].Contains(listener))
        {
            listeners[typeof(T)].Remove(listener);
            return true;
        }

        return false;
    }

    public static void Publish<T>(T publishedEvent) where T : class
    {
        if (!listeners.ContainsKey(typeof(T)))
            return;

        foreach (Action<object> action in listeners[typeof(T)])
        {
            action.Invoke(publishedEvent);
        }
    }

    public static void ClearListeners()
    {
        listeners.Clear();
    }
}
