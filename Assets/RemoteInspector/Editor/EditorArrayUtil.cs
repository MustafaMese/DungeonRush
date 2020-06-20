using System;
using System.Collections.Generic;
using System.Linq;

namespace UsingTheirs.RemoteInspector
{
    public class EditorArrayUtil
    {
        public static void ApplyChange(List<string> value, InputFieldDrawer.ArrayChangeType changeType, int length,
            int index, string newValue, Type elementType)
        {
            if (changeType == InputFieldDrawer.ArrayChangeType.Resize)
            {
                ResizeArray(value, length, elementType);
            }
            else if (changeType == InputFieldDrawer.ArrayChangeType.Modify)
            {
                value[index] = newValue;
            }
            else if (changeType == InputFieldDrawer.ArrayChangeType.Insert)
            {
                value.Insert( index, value[index]);
            }
            else if (changeType == InputFieldDrawer.ArrayChangeType.Delete)
            {
                value.RemoveAt(index);
            }
            else if (changeType == InputFieldDrawer.ArrayChangeType.Append)
            {
                value.Add(newValue);
            }
            else
            {
                throw new Exception(string.Format("[RemoteInspector] Invalid ArrayChangeType:{0}", changeType));
            }
        }
        
        public static void ResizeArray(List<string> value, int newCount, Type elementType)
        {
            int oldCount = value.Count;
            
            if (newCount < oldCount)
            {
                int removeCount = oldCount - newCount;
                value.RemoveRange(newCount, removeCount);
            }
            else if (newCount > oldCount)
            {
                value.Capacity = newCount; // Optimization

                string initValue;
                if (oldCount > 0)
                    initValue = value[oldCount - 1];
                else
                    initValue = GetDefault(elementType);

                int addCount = newCount - oldCount;
                value.AddRange(Enumerable.Repeat(initValue, addCount));
            }
        }

        private static string GetDefault(Type elementType)
        {
            object obj = SerializationCustom.GetDefaultValue(elementType);
            return SerializationCustom.Serialize(elementType, obj);
        }

        public static ChangedArray CreateChangedArray(string arrayName, List<string> arrayValue, 
            InputFieldDrawer.ArrayChangeType changeType, int length, int index, string newValue)
        {
            var changedArray = new ChangedArray();
            changedArray.name = arrayName;
            changedArray.type = ConvertArrayChangeType(changeType);
            changedArray.length = length;
            changedArray.index = index;
            changedArray.value = newValue;
            return changedArray;
        }

        public static ArrayChangeType ConvertArrayChangeType(InputFieldDrawer.ArrayChangeType type)
        {
            switch (type)
            {
                case InputFieldDrawer.ArrayChangeType.Resize: return ArrayChangeType.Resize;
                case InputFieldDrawer.ArrayChangeType.Insert: return ArrayChangeType.Insert;
                case InputFieldDrawer.ArrayChangeType.Delete: return ArrayChangeType.Delete;
                case InputFieldDrawer.ArrayChangeType.Modify: return ArrayChangeType.Modify;
                case InputFieldDrawer.ArrayChangeType.Append: return ArrayChangeType.Append;
            }
            
            throw new Exception(string.Format("[RemoteInspector] Invalid ArrayChangeType: {0}", type));
        }
    }
}