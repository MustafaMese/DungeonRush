using System;
using System.Collections;

namespace UsingTheirs.RemoteInspector
{
    public static class ArrayUtil
    {
        public static void ApplyChange(ref IList value, ArrayChangeType changeType, int length,
            int index, string newValue, Type listType, Type elementType)
        {
            if (changeType == ArrayChangeType.Resize)
            {
                if (SerializationConfig.IsGenericList(listType))
                    ResizeArrayImpl(value, length, elementType);
                else
                    ResizeArrayReadonlyImpl(ref value, length, elementType);
            }
            else if (changeType == ArrayChangeType.Modify)
            {
                object obj = SerializationCustom.Deserialize(elementType, newValue);
                value[index] = obj;
            }
            else if (changeType == ArrayChangeType.Insert)
            {
                if (SerializationConfig.IsGenericList(listType))
                    value.Insert(index, value[index]);
                else
                    InsertElementReadonlyImpl(ref value, index, elementType);
            }
            else if (changeType == ArrayChangeType.Delete)
            {
                if (SerializationConfig.IsGenericList(listType))
                    value.RemoveAt(index);
                else
                    RemoveElementReadonlyImpl(ref value, index, elementType);
            }
            else if (changeType == ArrayChangeType.Append)
            {
                object newObject = SerializationCustom.Deserialize(elementType, newValue);
                if (SerializationConfig.IsGenericList(listType))
                {
                    value.Add(newObject);
                }
                else
                {
                    InsertElementReadonlyImpl(ref value, value.Count, elementType, newObject);
                }

            }
            else
            {
                throw new Exception(string.Format("[RemoteInspector] Invalid ArrayChangeType:{0}", changeType));
            }
        }

        private static void RemoveElementReadonlyImpl(ref IList value, int index, Type elementType)
        {
            int oldCount = value.Count;
            int newCount = oldCount - 1;
            var newArray = Array.CreateInstance(elementType, newCount);

            for (int i = 0; i < index; ++i)
                newArray.SetValue(value[i], i);

            for (int i = index; i < newCount; ++i)
                newArray.SetValue(value[i+1], i);

            value = newArray;
        }
        
        private static void InsertElementReadonlyImpl(ref IList value, int index, Type elementType, object newObject = null)
        {
            if (newObject == null)
                newObject = value[index];
            
            int oldCount = value.Count;
            int newCount = oldCount + 1;
            var newArray = Array.CreateInstance(elementType, newCount);
            
            for (int i = 0; i < index; ++i)
                newArray.SetValue(value[i], i);
            
            newArray.SetValue(newObject, index);

            for (int i = index + 1; i < newCount; ++i)
                newArray.SetValue(value[i-1], i);

            value = newArray;
        }

        public static void ResizeArrayImpl(IList value, int newCount, Type elementType)
        {
            int oldCount = value.Count;
            
            if (newCount < oldCount)
            {
                int removeCount = oldCount - newCount;
                for (int i = 0; i < removeCount; ++i)
                {
                    //Debug.Log( string.Format("newCount:{0}, i:{1}, removeCount:{2}, oldCount:{3}", 
                        //newCount, i, removeCount, oldCount));
                    value.RemoveAt(newCount);
                }
            }
            else if (newCount > oldCount)
            {
                object defaultObj;
                if (oldCount > 0)
                    defaultObj = value[oldCount - 1];
                else
                    defaultObj = SerializationCustom.GetDefaultValue(elementType);
                   
                int addCount = newCount - oldCount;
                for (int i = 0; i < addCount; ++i)
                    value.Add(defaultObj);
            }
        }
        public static void ResizeArrayReadonlyImpl(ref IList value, int newCount, Type elementType)
        {
            var newArray = Array.CreateInstance(elementType, newCount);
            int oldCount = value.Count;
            
            if (newCount < oldCount)
            {
                for (int i = 0; i < newCount; ++i)
                {
                    newArray.SetValue(value[i], i);
                }
            }
            else if (newCount > oldCount)
            {
                value.CopyTo( newArray, 0);
                
                object defaultObj;
                if (oldCount > 0)
                    defaultObj = value[oldCount - 1];
                else
                    defaultObj = SerializationCustom.GetDefaultValue(elementType);

                for (int i = oldCount; i < newCount; ++i)
                    newArray.SetValue(defaultObj, i);
            }

            value = newArray;
        }
    }
}