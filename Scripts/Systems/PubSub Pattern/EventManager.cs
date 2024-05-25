using System.Collections.Generic;
using System;

namespace Nato.PubSub
{

    public static class EventManager<T>
    {
        private static List<Action<T>> eventListeners = new List<Action<T>>();

        public static void Subscribe(Action<T> listener)
        {
            if (!eventListeners.Contains(listener))
            {
                eventListeners.Add(listener);
            }
        }

        public static void Unsubscribe(Action<T> listener)
        {
            if (eventListeners.Contains(listener))
            {
                eventListeners.Remove(listener);
            }
        }

        public static void Publish(T eventData)
        {
            foreach (var listener in eventListeners)
            {
                listener.Invoke(eventData);
            }
        }

    }

    public static class EventManager
    {
        private static Dictionary<string, List<Action<object>>> eventDictionary = new Dictionary<string, List<Action<object>>>();

        public static void Subscribe(string eventType, Action<object> listener)
        {
            if (!eventDictionary.ContainsKey(eventType))
            {
                eventDictionary[eventType] = new List<Action<object>>();
            }
            eventDictionary[eventType].Add(listener);
        }

        public static void Unsubscribe(string eventType, Action<object> listener)
        {
            if (eventDictionary.ContainsKey(eventType))
            {
                eventDictionary[eventType].Remove(listener);
                if (eventDictionary[eventType].Count == 0)
                {
                    eventDictionary.Remove(eventType);
                }
            }
        }

        public static void Publish(string eventType, object eventData)
        {
            if (eventDictionary.ContainsKey(eventType))
            {
                foreach (var listener in eventDictionary[eventType])
                {
                    listener.Invoke(eventData);
                }
            }
        }
    }

}

