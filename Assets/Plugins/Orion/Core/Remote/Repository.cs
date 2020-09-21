using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Orion
{
    public static class Repository
    {
        public static IReadOnlyDictionary<Token, object> Objects => objects;
        public static IReadOnlyDictionary<Token, List<object>> Stacks => stacks;
        
        private static Dictionary<Token, object> objects = new Dictionary<Token, object>();
        private static Dictionary<Token, List<object>> stacks = new Dictionary<Token, List<object>>();

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

        public static bool Unregister(Token token) => objects.Remove(token);
        public static bool UnregisterStack(Token token) => stacks.Remove(token);
        public static bool RemoveFromStack(Token token, object value)
        {
            try
            {
                return stacks[token].Remove(value);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
                Debug.Break();
                
                throw;
            }
        }

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
    }
}