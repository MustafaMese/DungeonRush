using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace UsingTheirs.RemoteInspector
{

    static public class SerializationCustom
    {
        public static List<string> SerializeArray(Type elementType, IList list)
        {
            if (list == null || list.Count == 0)
                return new List<string>();
            
            var ret = new List<string>(list.Count);
            foreach (var value in list)
            {
                ret.Add( Serialize(elementType, value));
            }

            return ret;
        }
        
        public static List<object> DeserializeArray(Type elementType, List<string> serialized)
        {
            var ret = new List<object>(serialized.Count);
            foreach (var value in serialized)
            {
                ret.Add( Deserialize(elementType, value));
            }

            return ret;
        }
        
        
        public static string Serialize(Type t, object obj)
        {
            if (t == typeof(Vector3)) return Serialize_Vector3(obj);
            else if (t == typeof(string)) return Serialize_string(obj);
            else if (t == typeof(Vector2)) return Serialize_Vector2(obj);
            else if (t == typeof(Vector4)) return Serialize_Vector4(obj);
            else if (t == typeof(Bounds)) return Serialize_Bounds(obj);
            else if (t == typeof(Color)) return Serialize_Color(obj);
            else if (t == typeof(Rect)) return Serialize_Rect(obj);
            else if (t == typeof(Material)) return Serialize_Material(obj);
            else if (t == typeof(Texture2D)) return Serialize_Texture2D(obj);
            else if (t == typeof(Texture3D)) return Serialize_Texture3D(obj);
            else if (t == typeof(Cubemap)) return Serialize_Cubemap(obj);
            else if (t == typeof(Texture2DArray)) return Serialize_Texture2DArray(obj);
            else if (t == typeof(CubemapArray)) return Serialize_CubemapArray(obj);
#if UNITY_2017_2_OR_NEWER
            else if (t == typeof(BoundsInt)) return Serialize_BoundsInt(obj);
            else if (t == typeof(RectInt)) return Serialize_RectInt(obj);
            else if (t == typeof(Vector2Int)) return Serialize_Vector2Int(obj);
            else if (t == typeof(Vector3Int)) return Serialize_Vector3Int(obj);
#endif
            else return obj.ToString();
        }

        public static object Deserialize(Type t, string serialized)
        {
            if (t == typeof(Vector3)) return Deserialize_Vector3(serialized);
            else if (t == typeof(string)) return Deserialize_string(serialized);
            else if (t.IsEnum) return Deserialize_Enum(t, serialized);
            else if (t == typeof(Vector2)) return Deserialize_Vector2(serialized);
            else if (t == typeof(Vector4)) return Deserialize_Vector4(serialized);
            else if (t == typeof(Bounds)) return Deserialize_Bounds(serialized);
            else if (t == typeof(Color)) return Deserialize_Color(serialized);
            else if (t == typeof(Rect)) return Deserialize_Rect(serialized);
            else if (t == typeof(Material)) return Deserialize_Material(serialized);
            else if (t == typeof(Texture2D)) return Deserialize_Texture2D(serialized);
            else if (t == typeof(Texture3D)) return Deserialize_Texture3D(serialized);
            else if (t == typeof(Cubemap)) return Deserialize_Cubemap(serialized);
            else if (t == typeof(Texture2DArray)) return Deserialize_Texture2DArray(serialized);
            else if (t == typeof(CubemapArray)) return Deserialize_CubemapArray(serialized);
#if UNITY_2017_2_OR_NEWER
            else if (t == typeof(BoundsInt)) return Deserialize_BoundsInt(serialized);
            else if (t == typeof(RectInt)) return Deserialize_RectInt(serialized);
            else if (t == typeof(Vector2Int)) return Deserialize_Vector2Int(serialized);
            else if (t == typeof(Vector3Int)) return Deserialize_Vector3Int(serialized);
#endif
            else
            {
                try
                {
                    TypeConverter tc = TypeDescriptor.GetConverter(t);
                    object value = tc.ConvertFromString(serialized);
                    return value;
                }
                catch (Exception e)
                {
                    Logger.LogError("[SerializationCustom.Deserialize] Failed to Deserialize. Type:{0}, Value:{1}, Exception:{2}",
                        t, serialized, e);
                    return GetDefaultValue(t);
                }
            }
        }

        public static object GetDefaultValue(Type type)
        {
            if (type.IsValueType)
                return Activator.CreateInstance(type);
            return null;
        }

        // String
        public static string Serialize_string(object obj)
        {
            if (obj == null)
                return string.Empty;

            return (string)obj;
        }
        static object Deserialize_string(string serialized)
        {
            return serialized;
        }

        // Vector3
        public static string Serialize_Vector3(object obj)
        {
            var v = (Vector3)obj;
            return string.Format("{0};{1};{2}", v.x, v.y, v.z);
        }
        static object Deserialize_Vector3(string serialized)
        {
            var split = serialized.Split(';');
            var v = new Vector3();
            v.x = Convert.ToSingle(split[0]);
            v.y = Convert.ToSingle(split[1]);
            v.z = Convert.ToSingle(split[2]);
            return v;
        }

        // Enum
        static object Deserialize_Enum(Type t, string serialized)
        {
            object obj = Enum.Parse(t, serialized);
            return obj;
        }

        // Vector2
        public static string Serialize_Vector2(object obj)
        {
            var v = (Vector2)obj;
            return string.Format("{0};{1}", v.x, v.y);
        }
        static object Deserialize_Vector2(string serialized)
        {
            var split = serialized.Split(';');
            var v = new Vector2();
            v.x = Convert.ToSingle(split[0]);
            v.y = Convert.ToSingle(split[1]);
            return v;
        }

        // Vector4
        public static string Serialize_Vector4(object obj)
        {
            var v = (Vector4)obj;
            return string.Format("{0};{1};{2};{3}", v.x, v.y, v.z, v.w);
        }
        static object Deserialize_Vector4(string serialized)
        {
            var split = serialized.Split(';');
            var v = new Vector4();
            v.x = Convert.ToSingle(split[0]);
            v.y = Convert.ToSingle(split[1]);
            v.z = Convert.ToSingle(split[2]);
            v.z = Convert.ToSingle(split[3]);
            return v;
        }

        // Bounds
        public static string Serialize_Bounds(object obj)
        {
            var b = (Bounds)obj;
            var c = b.center;
            var s = b.size;
            return string.Format("{0};{1};{2};{3};{4};{5}", c.x, c.y, c.z, s.x, s.y, s.z);
        }
        static object Deserialize_Bounds(string serialized)
        {
            var split = serialized.Split(';');
            var c = new Vector3();
            c.x = Convert.ToSingle(split[0]);
            c.y = Convert.ToSingle(split[1]);
            c.z = Convert.ToSingle(split[2]);
            var s = new Vector3();
            s.x = Convert.ToSingle(split[3]);
            s.y = Convert.ToSingle(split[4]);
            s.z = Convert.ToSingle(split[5]);

            var b = new Bounds(c, s);
            return b;
        }

        // Color
        public static string Serialize_Color(object obj)
        {
            var c = (Color)obj;
            return string.Format("{0};{1};{2};{3}", c.r, c.g, c.b, c.a);
        }
        static object Deserialize_Color(string serialized)
        {
            var split = serialized.Split(';');
            var c = new Color();
            c.r = Convert.ToSingle(split[0]);
            c.g = Convert.ToSingle(split[1]);
            c.b = Convert.ToSingle(split[2]);
            c.a = Convert.ToSingle(split[3]);
            return c;
        }

        // Rect
        public static string Serialize_Rect(object obj)
        {
            var r = (Rect)obj;
            return string.Format("{0};{1};{2};{3}", r.x, r.y, r.width, r.height);
        }
        static object Deserialize_Rect(string serialized)
        {
            var split = serialized.Split(';');
            var r = new Rect();
            r.x = Convert.ToSingle(split[0]);
            r.y = Convert.ToSingle(split[1]);
            r.width = Convert.ToSingle(split[2]);
            r.height = Convert.ToSingle(split[3]);
            return r;
        }
        
        // Material
        public static string Serialize_Material(object obj)
        {
            if (obj != null)
            {
                var m = (Material)obj;
                return string.Format("{0};{1};{2}",  m.GetInstanceID(), m.name, m.shader.name);
            }
            else
            {
                return "0;";
            }
        }
        static object Deserialize_Material(string serialized)
        {
            var split = serialized.Split(';');
            var m = new MaterialRef();
            m.instanceID = Convert.ToInt32(split[0]);
            if (m.instanceID != 0)
            {
                m.name = split[1];
                m.shaderName = split[2];
            }
            return m;
        }
        
        // Texture2D
        public static string Serialize_Texture2D(object obj)
        {
            if (obj != null && ((Texture2D)obj).GetInstanceID() != 0)
            {
                var t = (Texture2D)obj;
                return string.Format("{0};{1};{2};{3};{4}", t.GetInstanceID(), t.name,
                    SerializationCustom.Serialize(typeof(TextureFormat), t.format), t.width, t.height);
            }
            else
            {
                return "0;";
            }
        }
        
        static object Deserialize_Texture2D(string serialized)
        {
            var split = serialized.Split(';');
            var t = new Texture2DRef();
            t.instanceID = Convert.ToInt32(split[0]);
            if (t.instanceID != 0)
            {
                t.name = split[1];
                t.format = (TextureFormat)SerializationCustom.Deserialize(typeof(TextureFormat), split[2]);
                t.width = Convert.ToInt32(split[3]);
                t.height = Convert.ToInt32(split[4]);
            }
            
            return t;
        }
        
        // Texture3D
        public static string Serialize_Texture3D(object obj)
        {
            if (obj != null && ((Texture3D)obj).GetInstanceID() != 0)
            {
                var t = (Texture3D)obj;
                return string.Format("{0};{1};{2};{3};{4};{5}", t.GetInstanceID(), t.name,
                    SerializationCustom.Serialize(typeof(TextureFormat), t.format), t.width, t.height, t.depth);
            }
            else
            {
                return "0;";
            }
        }
        
        static object Deserialize_Texture3D(string serialized)
        {
            var split = serialized.Split(';');
            var t = new Texture3DRef();
            t.instanceID = Convert.ToInt32(split[0]);
            if (t.instanceID != 0)
            {
                t.name = split[1];
                t.format = (TextureFormat)SerializationCustom.Deserialize(typeof(TextureFormat), split[2]);
                t.width = Convert.ToInt32(split[3]);
                t.height = Convert.ToInt32(split[4]);
                t.depth = Convert.ToInt32(split[5]);
            }
            
            return t;
        }
        
        // Cubemap
        public static string Serialize_Cubemap(object obj)
        {
            if (obj != null && ((Cubemap)obj).GetInstanceID() != 0)
            {
                var t = (Cubemap)obj;
                return string.Format("{0};{1};{2};{3};{4}", t.GetInstanceID(), t.name,
                    SerializationCustom.Serialize(typeof(TextureFormat), t.format), t.width, t.height);
            }
            else
            {
                return "0;";
            }
        }
        
        static object Deserialize_Cubemap(string serialized)
        {
            var split = serialized.Split(';');
            var t = new TextureCubeRef();
            t.instanceID = Convert.ToInt32(split[0]);
            if (t.instanceID != 0)
            {
                t.name = split[1];
                t.format = (TextureFormat)SerializationCustom.Deserialize(typeof(TextureFormat), split[2]);
                t.width = Convert.ToInt32(split[3]);
                t.height = Convert.ToInt32(split[4]);
            }
            
            return t;
        }
        
        // Texture2DArray
        public static string Serialize_Texture2DArray(object obj)
        {
            if (obj != null && ((Texture2DArray)obj).GetInstanceID() != 0)
            {
                var t = (Texture2DArray)obj;
                return string.Format("{0};{1};{2};{3};{4};{5}", t.GetInstanceID(), t.name,
                    SerializationCustom.Serialize(typeof(TextureFormat), t.format), t.width, t.height, t.depth);
            }
            else
            {
                return "0;";
            }
        }
        
        static object Deserialize_Texture2DArray(string serialized)
        {
            var split = serialized.Split(';');
            var t = new Texture2DArrayRef();
            t.instanceID = Convert.ToInt32(split[0]);
            if (t.instanceID != 0)
            {
                t.name = split[1];
                t.format = (TextureFormat)SerializationCustom.Deserialize(typeof(TextureFormat), split[2]);
                t.width = Convert.ToInt32(split[3]);
                t.height = Convert.ToInt32(split[4]);
                t.depth = Convert.ToInt32(split[5]);
            }
            
            return t;
        }
        
        // CubemapArray
        public static string Serialize_CubemapArray(object obj)
        {
            if (obj != null && ((CubemapArray)obj).GetInstanceID() != 0)
            {
                var t = (CubemapArray)obj;
                return string.Format("{0};{1};{2};{3};{4};{5}", t.GetInstanceID(), t.name,
                    SerializationCustom.Serialize(typeof(TextureFormat), t.format), t.width, t.height, t.cubemapCount);
            }
            else
            {
                return "0;";
            }
        }
        
        static object Deserialize_CubemapArray(string serialized)
        {
            var split = serialized.Split(';');
            var t = new TextureCubeArrayRef();
            t.instanceID = Convert.ToInt32(split[0]);
            if (t.instanceID != 0)
            {
                t.name = split[1];
                t.format = (TextureFormat)SerializationCustom.Deserialize(typeof(TextureFormat), split[2]);
                t.width = Convert.ToInt32(split[3]);
                t.height = Convert.ToInt32(split[4]);
                t.count = Convert.ToInt32(split[5]);
            }
            
            return t;
        }

#if UNITY_2017_2_OR_NEWER
        // BoundsInt
        public static string Serialize_BoundsInt(object obj)
        {
            var b = (BoundsInt)obj;
            var c = b.center;
            var s = b.size;
            return string.Format("{0};{1};{2};{3};{4};{5}", c.x, c.y, c.z, s.x, s.y, s.z);
        }
        static object Deserialize_BoundsInt(string serialized)
        {
            var split = serialized.Split(';');
            var c = new Vector3Int();
            c.x = Convert.ToInt32(split[0]);
            c.y = Convert.ToInt32(split[1]);
            c.z = Convert.ToInt32(split[2]);
            var s = new Vector3Int();
            s.x = Convert.ToInt32(split[3]);
            s.y = Convert.ToInt32(split[4]);
            s.z = Convert.ToInt32(split[5]);

            var b = new BoundsInt(c, s);
            return b;
        }

        // RectInt
        public static string Serialize_RectInt(object obj)
        {
            var r = (RectInt)obj;
            return string.Format("{0};{1};{2};{3}", r.x, r.y, r.width, r.height);
        }
        static object Deserialize_RectInt(string serialized)
        {
            var split = serialized.Split(';');
            var r = new RectInt();
            r.x = Convert.ToInt32(split[0]);
            r.y = Convert.ToInt32(split[1]);
            r.width = Convert.ToInt32(split[2]);
            r.height = Convert.ToInt32(split[3]);
            return r;
        }

        // Vector2Int
        public static string Serialize_Vector2Int(object obj)
        {
            var v = (Vector2Int)obj;
            return string.Format("{0};{1}", v.x, v.y);
        }
        static object Deserialize_Vector2Int(string serialized)
        {
            var split = serialized.Split(';');
            var v = new Vector2Int();
            v.x = Convert.ToInt32(split[0]);
            v.y = Convert.ToInt32(split[1]);
            return v;
        }

        // Vector3Int
        public static string Serialize_Vector3Int(object obj)
        {
            var v = (Vector3Int)obj;
            return string.Format("{0};{1};{2}", v.x, v.y, v.z);
        }
        static object Deserialize_Vector3Int(string serialized)
        {
            var split = serialized.Split(';');
            var v = new Vector3Int();
            v.x = Convert.ToInt32(split[0]);
            v.y = Convert.ToInt32(split[1]);
            v.z = Convert.ToInt32(split[2]);
            return v;
        }
#endif
    }

}