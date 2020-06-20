using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Component = UnityEngine.Component;

namespace UsingTheirs.RemoteInspector
{

    public static class HandlerApplyGameObject
    {
        public static string HandlerMain(string jsonRequest)
        {
            try
            {
                Logger.Log("[ApplyGameObjectHandler] req:{0}", jsonRequest);

                var req = JsonUtility.FromJson<ChangeGameObjectReq>(jsonRequest);
                var changedGo = req.gameObject;
                var go = HelperGameObjectFinder.FindGameObject(changedGo.instanceId);
                if (go == null)
                    return ErrorString("GameObject Not Found");

                go.SetActive(changedGo.active);

                var changedComponent = changedGo.component;
                if (changedComponent == null || changedComponent.instanceId == 0)
                    return ErrorString("");

                var component = FindComponent(changedComponent.instanceId, go);
                if (component == null)
                    return ErrorString("Component Not Found");

                if (changedComponent.hasEnabled)
                    SetComponentEnabled(component, changedComponent.enabled);

                var changedField = changedComponent.field;
                if (changedField != null && !string.IsNullOrEmpty(changedField.name))
                {
                    var fi = FindFieldInfo(changedField.name, component);
                    if (fi == null)
                        return ErrorString( string.Format("Field '{0}' Not Found", changedField.name ));

                    object value = SerializationCustom.Deserialize(fi.FieldType, changedField.value);
                    fi.SetValue(component, value);
                }

                var changedProperty = changedComponent.property;
                if (changedProperty != null && !string.IsNullOrEmpty(changedProperty.name))
                {
                    var pi = FindPropertyInfo(changedProperty.name, component);
                    if (pi == null)
                        return ErrorString( string.Format("Property '{0}' Not Found", changedProperty.name ));

                    object value = SerializationCustom.Deserialize(pi.PropertyType, changedProperty.value);
                    pi.SetValue(component, value, null);
                }
                
                var changedArrayField = changedComponent.arrayField;
                if (changedArrayField != null && !string.IsNullOrEmpty(changedArrayField.name))
                {
                    var fi = FindFieldInfo(changedArrayField.name, component);
                    if (fi == null)
                        return ErrorString( string.Format("Array Field '{0}' Not Found", changedArrayField.name ));

                    object list = fi.GetValue(component);
                    
                    Type elementType = SerializationConfig.GetArrayElementType(fi.FieldType);

                    var ilist = (IList) list;
                    ArrayUtil.ApplyChange(ref ilist, changedArrayField.type, changedArrayField.length,
                        changedArrayField.index, changedArrayField.value, fi.FieldType, elementType);

                    fi.SetValue(component, ilist);
                }

                var changedArrayProperty = changedComponent.arrayProperty;
                if (changedArrayProperty != null && !string.IsNullOrEmpty(changedArrayProperty.name))
                {
                    var pi = FindPropertyInfo(changedArrayProperty.name, component);
                    if (pi == null)
                        return ErrorString( string.Format("Array Property '{0}' Not Found", changedArrayProperty.name ));

                    object list = pi.GetValue(component, null);
                    
                    Type elementType = SerializationConfig.GetArrayElementType(pi.PropertyType);

                    var ilist = (IList)list;
                    ArrayUtil.ApplyChange(ref ilist, changedArrayProperty.type, changedArrayProperty.length,
                        changedArrayProperty.index, changedArrayProperty.value, pi.PropertyType, elementType);

                    pi.SetValue(component, ilist, null);
                }

                return ErrorString("");
            }
            catch(Exception e)
            {
                return ErrorString(e.ToString());
            }
        }

        public static string ErrorString(string reason)
        {
            var res = new ChangeGameObjectRes();
            res.error = reason;
            return JsonUtility.ToJson(res);
        }

        static void SetComponentEnabled(Component component, bool enabled)
        {
            var enabledProperty = component.GetType().GetProperty("enabled", BindingFlags.Instance | BindingFlags.Public);
            if (enabledProperty == null)
            {
                Logger.LogError("[HandlerApplyGameObject] No enabled property");
                return;
            }

            enabledProperty.SetValue(component, enabled, null);
        }

        private static FieldInfo FindFieldInfo(string name, Component component)
        {
            return component.GetType().GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        private static PropertyInfo FindPropertyInfo(string name, Component component)
        {
            return component.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        public static Component FindComponent(int instanceId, GameObject go)
        {
            var components = go.GetComponents<Component>();
            for (int i = 0; i < components.Length; ++i)
            {
                if (components[i].GetInstanceID() == instanceId)
                    return components[i];
            }
            return null;
        }

    }

}