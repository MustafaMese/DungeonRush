using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UsingTheirs.RemoteInspector
{

    public static class SerializationConfig
    {
        public static bool IsSerializable(System.Type type, string name, string componentName)
        {
            // backing fields for automatic properties
            if (name.StartsWith("<"))
                return false;

            if (name == "tag"
            || name == "name"
            || name == "hideFlags"
            || name == "enabled"
            || name == "isActiveAndEnabled"
            || name == "useGUILayout"
            )
                return false;
            
            if (componentName == "MeshRenderer" || componentName == "SkinnedMeshRenderer")
            {
                if (name == "material"
                    || name == "sharedMaterial"
                    )
                    return false;
            }


            if (type == typeof(string)
            || type == typeof(bool)
            || type == typeof(int)
            || type == typeof(float)
            || type == typeof(double)
            || type == typeof(long)
            || type == typeof(Bounds)
            || type == typeof(Vector2)
            || type == typeof(Vector3)
            || type == typeof(Vector4)
            || type == typeof(Color)
            || type == typeof(Rect)
            || type.IsEnum
            || type == typeof(Material)
            || type == typeof(Texture2D)
            || type == typeof(Texture3D)
            || type == typeof(Cubemap)
            || type == typeof(Texture2DArray)
            || type == typeof(CubemapArray)
#if UNITY_2017_2_OR_NEWER
        || type == typeof(BoundsInt)
            || type == typeof(RectInt)
            || type == typeof(Vector2Int)
            || type == typeof(Vector3Int)
#endif
        )
                return true;

            //Debug.Log("Non-supported type : " + type.Name);
            return false;
        }

        public static bool IsSerializableArray(System.Type type, string name, string componentName)
        {
            if (componentName == "MeshRenderer" || componentName == "SkinnedMeshRenderer")
            {
                if (name == "materials")
                    return false;
            }
            
            var elementType = GetArrayElementType(type);
            if (elementType == null)
                return false;
            
            return IsSerializable(GetArrayElementType(type), string.Empty, string.Empty);
        }

        public static System.Type GetArrayElementType(System.Type type)
        {
            if (IsGenericList(type))
            {
                return type.GetGenericArguments().Single();
            } 
            else if (type.IsArray)
            {
                return type.GetElementType();
            }

            return null;
        }

        public static bool IsGenericList(System.Type type)
        {
            return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>));
        }
    }


}
