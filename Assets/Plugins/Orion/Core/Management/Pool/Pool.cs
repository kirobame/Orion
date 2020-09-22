using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Orion
{
    public abstract class Pool : MonoBehaviour { public abstract void Stock(Object instance); }
    
    public abstract class Pool<T> : Pool, IProvider<T> where T : Poolable
    {
        [SerializeField] private T prefab;
        [ShowInInspector, ReadOnly] protected List<T> instances = new List<T>();
    
        private Queue<T> availableInstances = new Queue<T>();
        private HashSet<T> usedInstances = new HashSet<T>();

        void Awake()
        {
            instances.AddRange(GetComponentsInChildren<T>(true));
            foreach (var instance in instances) availableInstances.Enqueue(instance);
        }

        public T RequestSingle() => Request(1).First();
        public T[] Request(int count)
        {
            for (var i = 0; i < count - availableInstances.Count; i++)
            {
                var instance = Instantiate(prefab, transform);
                instance.SetOrigin(this);
                
                instances.Add(instance);
                availableInstances.Enqueue(instance);
            }
        
            var request = new T[count];
            for (var i = 0; i < count; i++)
            {
                var instance = availableInstances.Dequeue();
                
                Claim(instance);
                request[i] = instance;
            }

            return request;
        }

        public T[] RequestSpecific(int count, Func<T, bool> predicate) => RequestSpecific(prefab, count, predicate);
        public T[] RequestSpecific(T prefab, int count, Func<T, bool> predicate)
        {
            var request = new T[count];
            var index = 0;

            var toRequeue = new List<T>();
            while (availableInstances.Count > 0 && index < count)
            {
                var instance = availableInstances.Dequeue();
                
                if (predicate(instance) == true)
                {
                    Claim(instance);
                    request[index] = instance;
                    
                    index++;
                }
                else toRequeue.Add(instance);
            }

            foreach (var instance in toRequeue) availableInstances.Enqueue(instance);

            for (var i = 0; i < count - index; i++)
            {
                var instance = Instantiate(prefab);
                
                instance.gameObject.SetActive(true);
                instance.SetOrigin(this);
                
                instances.Add(instance);
                usedInstances.Add(instance);

                request[index + i] = instance;
            }

            return request;
        }

        public override void Stock(Object instance)
        {
            if (this == null || !gameObject.activeInHierarchy) return;
            
            var castedInstance = instance as T;
            if (castedInstance == null) return;
        
            castedInstance.Reboot();
            castedInstance.gameObject.SetActive(false);
            
            var routine = new WaitForEndOfFrame().ToRoutine();
            routine.Append(new ActionRoutine() {action = () => castedInstance.transform.SetParent(transform)});

            StartCoroutine(routine.Call);

            availableInstances.Enqueue(castedInstance);
            if (!usedInstances.Remove(castedInstance)) instances.Add(castedInstance);
        }

        private void Claim(T instance)
        {
            instance.SetOrigin(this);
            
            instance.gameObject.SetActive(true);
            instance.transform.SetParent(null);
            
            usedInstances.Add(instance);
        }
        
        object IProvider.GetInstance() => RequestSingle();
        T IProvider<T>.GetInstance() => RequestSingle();
    }
}