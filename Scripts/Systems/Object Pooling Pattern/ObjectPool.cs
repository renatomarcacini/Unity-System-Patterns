using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nato.Pool
{
    public interface IPoolObject
    {
        void OnGettingFromPool();
        void OnReturnToPool();
    }

    public static class ObjectPool<T> where T : MonoBehaviour, IPoolObject
    {
        private static Dictionary<Type, Queue<T>> poolDictionary = new Dictionary<Type, Queue<T>>();
        private static Dictionary<Type, Queue<T>> poolAuxDictionary = new Dictionary<Type, Queue<T>>();

        public static void Create(T prefab, int size)
        {
            Type type = typeof(T);
            if (!poolDictionary.ContainsKey(type))
            {
                poolDictionary[type] = new Queue<T>();
                for (int i = 0; i < size; i++)
                {
                    InstantiateObject(prefab, type, false);
                }
            }

            if (!poolAuxDictionary.ContainsKey(type))
            {
                poolAuxDictionary[type] = new Queue<T>();
                poolAuxDictionary[type].Enqueue(prefab);
            }
        }

        private static T InstantiateObject(T prefab, Type type, bool active)
        {
            T obj = UnityEngine.Object.Instantiate(prefab);
            obj.gameObject.SetActive(active);
            poolDictionary[type].Enqueue(obj);
            return obj;
        }


        public static T Get()
        {
            Type type = typeof(T);
            if (poolDictionary.ContainsKey(type) && poolDictionary[type].Count > 0)
            {
                T obj = poolDictionary[type].Dequeue();
                obj.gameObject.SetActive(true);
                obj.OnGettingFromPool();
                return obj;
            }
            else
            {
                T newObj = InstantiateObject(poolAuxDictionary[type].Peek(), type, true);
                T obj = poolDictionary[type].Dequeue();
                obj.OnGettingFromPool();
                Debug.Log(poolDictionary[type].Count);
                return obj;
            }
        }

        public static void Return(T obj)
        {
            Type type = typeof(T);
            obj.gameObject.SetActive(false);
            if (poolDictionary.ContainsKey(type))
            {
                poolDictionary[type].Enqueue(obj);
                obj.OnReturnToPool();
            }
            else
            {
                Debug.LogWarning("Pool not created for type: " + typeof(T).Name);
            }
        }
    }
}