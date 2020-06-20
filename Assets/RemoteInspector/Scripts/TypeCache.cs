using UnityEngine;
using System;
using System.Collections.Generic;

namespace UsingTheirs.RemoteInspector
{

    public static class TypeCache
    {
        public static Dictionary<string, Type> cachedTypes = new Dictionary<string, Type>();

        public static Type GetFromCacheOrFind(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                Logger.LogError("[GetFromCacheOrFind] Empty Type Name");
                return null;
            }

            Type cachedType;
            if (cachedTypes.TryGetValue(typeName, out cachedType))
            {
                return cachedType;
            }

            //cachedType = Type.GetType(typeName);

            var asmArray = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var asm in asmArray)
            {
                cachedType = asm.GetType(typeName);
                if (cachedType != null)
                    break;
            }

            if (cachedType != null)
            {
                cachedTypes.Add(typeName, cachedType);
            }
            else
            {
                Logger.LogError("[GetFromCacheOrFind] Type Not Found : " + typeName);
            }

            return cachedType;
        }
        
    }

}