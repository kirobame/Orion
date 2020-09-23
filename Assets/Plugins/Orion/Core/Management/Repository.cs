using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Orion
{
    public static class Repository
    {
        public static IReadOnlyDictionary<Token, object> Objects => objects;
        public static IReadOnlyDictionary<Token, List<object>> Stacks => stacks;

        public static IReadOnlyDictionary<Token, IReferencer> Referencers => referencers;
        
        private static Dictionary<Token, object> objects = new Dictionary<Token, object>();
        private static Dictionary<Token, List<object>> stacks = new Dictionary<Token, List<object>>();

        private static Dictionary<Token, IReferencer> referencers = new Dictionary<Token, IReferencer>();

        #region Registration

        public static void Register(IReferencer referencer)
        {
            try
            {
                referencers.Add(referencer.Token, referencer);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
                Debug.Break();

                throw;
            }
            Register(referencer.Token, referencer.Value);
        }
        public static void Register(Token token, object value)
        {
            try
            {
                objects.Add(token, value);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
                Debug.Break();

                throw;
            }
        }

        public static void RegisterStack(IReferencer referencer)
        {
            try
            {
                objects.Add(referencer.Token, referencer);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
                Debug.Break();

                throw;
            }
            RegisterStack(referencer.Token, referencer.Value);
        }
        public static void RegisterStack(Token token, object value)
        {
            if (stacks.TryGetValue(token, out var list)) list.Add(value);
            else stacks.Add(token, new List<object>() {value});
        }
        public static void RegisterStack(Token token, IEnumerable<object> values)
        {
            if (stacks.TryGetValue(token, out var list)) list.AddRange(values);
            else stacks.Add(token, new List<object>(values));
        }
        #endregion
        
        #region Unregistration

        public static bool Unregister(IReferencer referencer)
        {
            return referencers.Remove(referencer.Token) && Unregister(referencer.Token);
        }
        public static bool Unregister(Token token) => objects.Remove(token);

        public static bool UnregisterStack(IReferencer referencer)
        {
            return referencers.Remove(referencer.Token) && UnregisterStack(referencer.Token);
        }
        public static bool UnregisterStack(Token token) => stacks.Remove(token);

        public static bool RemoveFromStack(IReferencer referencer)
        {
            return referencers.Remove(referencer.Token) && RemoveFromStack(referencer.Token, referencer.Value);
        }
        public static bool RemoveFromStack(Token token, object value)
        {
            try
            {
                var removalSuccess = stacks[token].Remove(value);
                if (removalSuccess && stacks[token].Count == 0) stacks.Remove(token);

                return removalSuccess;
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
                Debug.Break();
                
                throw;
            }
        }
        #endregion

        public static T Get<T>(Token token) => (T)objects[token];
        public static IEnumerable<T> GetStack<T>(Token token) => stacks[token].Cast<T>();

        public static bool TryGet<T>(Token token, out T value)
        {
            if (objects.TryGetValue(token, out var rawValue))
            {
                value = (T)rawValue;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }
        public static bool TryGetStack<T>(Token token, out IEnumerable<T> values)
        {
            if (stacks.TryGetValue(token, out var list))
            {
                values = list.Cast<T>();
                return true;
            }
            else
            {
                values = default;
                return false;
            }
        }

        public static void Set<T>(Token token, T value)
        {
            objects[token] = value;
            if (referencers.TryGetValue(token, out var referencer)) referencer.SetValue(value as Object);
        }
        public static bool TrySet<T>(Token token, T value)
        {
            if (objects.ContainsKey(token))
            {
                if (referencers.TryGetValue(token, out var referencer)) referencer.SetValue(value as Object);
                
                objects[token] = value;
                return true;
            }

            return false;
        }
    }
}