using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Reflection;

namespace UsingTheirs.RemoteInspector
{

    public static class HelperRemoteGameObjectCreator
    {
        public static RemoteGameObject CreateRemoteGameObject(GameObject gameObject, bool includeComponents, bool includeChildren)
        {
            var item = new RemoteGameObject();
            item.name = gameObject.name;
            item.instanceId = gameObject.GetInstanceID();
            item.active = gameObject.activeSelf;
            if (includeComponents)
                item.components = CreateRemoteComponentArray(gameObject);

            if (includeChildren)
            {
                var tr = gameObject.transform;
                item.childGameObjects = new List<RemoteGameObject>(tr.childCount);
                for (int i = 0; i < tr.childCount; ++i)
                {
                    var go = CreateRemoteGameObject(tr.GetChild(i).gameObject, includeComponents, includeChildren);
                    item.childGameObjects.Add(go);
                }
            }

            return item;
        }

        static RemoteComponent[] CreateRemoteComponentArray(GameObject gameObject)
        {
            var components = gameObject.GetComponents<Component>();
            var remoteComponents = new RemoteComponent[components.Length];
            for (int i = 0; i < components.Length; ++i)
            {
                remoteComponents[i] = new RemoteComponent();
                remoteComponents[i].type = components[i].GetType().Name;
                remoteComponents[i].instanceId = components[i].GetInstanceID();
                remoteComponents[i].fields = CreateRemoteFieldArray(components[i]);
                remoteComponents[i].arrayFields = CreateRemoteArrayFieldArray(components[i]);
                remoteComponents[i].properties = CreateRemotePropertyArray(components[i]);
                remoteComponents[i].arrayProperties = CreateRemoteArrayPropertyArray(components[i]);

                GetComponentEnabled(components[i], out remoteComponents[i].hasEnabled, out remoteComponents[i].enabled);
            }
            return remoteComponents;
        }

        static void GetComponentEnabled(Component component, out bool hasEnabled, out bool enabled)
        {
            var enabledProperty = component.GetType().GetProperty("enabled", BindingFlags.Instance | BindingFlags.Public);
            hasEnabled = enabledProperty != null;
            enabled = enabledProperty != null ? (bool)enabledProperty.GetValue(component, null) : false;
        }

        static RemoteField[] CreateRemoteFieldArray(Component component)
        {
            
            var fields = component.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var remoteFields = new List<RemoteField>(fields.Length);
            for (int i = 0; i < fields.Length; ++i)
            {
                //Debug.Log("[Field] name:" + fields[i].Name + ", type:" + fields[i].FieldType.Name);

                if (!SerializationConfig.IsSerializable(fields[i].FieldType, fields[i].Name, component.GetType().Name))
                    continue;

                var remoteField = new RemoteField();
                remoteField.type = fields[i].FieldType.FullName;
                remoteField.name = fields[i].Name;
                remoteField.value = SerializationCustom.Serialize(fields[i].FieldType, fields[i].GetValue(component));
                remoteField.isPublic = fields[i].IsPublic;
                remoteFields.Add(remoteField);
            }
            return remoteFields.ToArray();
        }
        
        static RemoteArrayField[] CreateRemoteArrayFieldArray(Component component)
        {
            var fields = component.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var remoteArrayFields = new List<RemoteArrayField>(fields.Length );
            for (int i = 0; i < fields.Length; ++i)
            {
                if (!SerializationConfig.IsSerializableArray(fields[i].FieldType, fields[i].Name, component.GetType().Name))
                    continue;

                var elementType = SerializationConfig.GetArrayElementType(fields[i].FieldType);

                var remoteArrayField = new RemoteArrayField();
                remoteArrayField.elementType = elementType.FullName;
                remoteArrayField.name = fields[i].Name;
                remoteArrayField.value = SerializationCustom.SerializeArray( elementType, (IList)fields[i].GetValue(component));
                remoteArrayField.isPublic = fields[i].IsPublic;
                remoteArrayFields.Add(remoteArrayField);
            }
            return remoteArrayFields.ToArray();
        }

        static RemoteProperty[] CreateRemotePropertyArray(Component component)
        {
            var properties = component.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var remoteProperties = new List<RemoteProperty>(properties.Length);
            for (int i = 0; i < properties.Length; ++i)
            {
                //Debug.Log("[Property] name:" + properties[i].Name + ", type:" + properties[i].PropertyType.Name + ", canRead:" + properties[i].CanRead + ", canWrite:" + properties[i].CanWrite);

                if (!SerializationConfig.IsSerializable(properties[i].PropertyType, properties[i].Name, component.GetType().Name))
                    continue;

                var remoteProperty = CreateRemoteProperty(component, properties[i]);
                remoteProperties.Add(remoteProperty);
            }
            return remoteProperties.ToArray();
        }
        
        static RemoteProperty CreateRemoteProperty(Component component, PropertyInfo property)
        {
            var remoteProperty = new RemoteProperty();
            remoteProperty.type = property.PropertyType.FullName;
            remoteProperty.name = property.Name;
            remoteProperty.canRead = property.CanRead;
            remoteProperty.canWrite = property.CanWrite;

            if (remoteProperty.canRead)
            {
                var getMethod = property.GetGetMethod(true);
                remoteProperty.isPublicRead = getMethod.IsPublic;
                remoteProperty.value = SerializationCustom.Serialize(property.PropertyType, property.GetValue(component, null));
            }

            if (remoteProperty.canWrite)
            {
                var setMethod = property.GetSetMethod(true);
                remoteProperty.isPublicWrite = setMethod.IsPublic;
            }

            return remoteProperty;
        }
        
        static RemoteArrayProperty[] CreateRemoteArrayPropertyArray(Component component)
        {
            var properties = component.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var remoteArrayProperties = new List<RemoteArrayProperty>(properties.Length);
            for (int i = 0; i < properties.Length; ++i)
            {
                //Debug.Log("[Property] name:" + properties[i].Name + ", type:" + properties[i].PropertyType.Name + ", canRead:" + properties[i].CanRead + ", canWrite:" + properties[i].CanWrite);

                if (!SerializationConfig.IsSerializableArray(properties[i].PropertyType, properties[i].Name,
                    component.GetType().Name))
                    continue;
                
                var remoteArrayProperty = CreateRemoteArrayProperty(component, properties[i]);
                remoteArrayProperties.Add(remoteArrayProperty);
            }
            return remoteArrayProperties.ToArray();
        }

        
        static RemoteArrayProperty CreateRemoteArrayProperty(Component component, PropertyInfo property)
        {
            var elementType = SerializationConfig.GetArrayElementType(property.PropertyType);
            
            var remoteArrayProperty = new RemoteArrayProperty();
            remoteArrayProperty.elementType = elementType.FullName;
            remoteArrayProperty.name = property.Name;
            remoteArrayProperty.canRead = property.CanRead;
            remoteArrayProperty.canWrite = property.CanWrite;

            if (remoteArrayProperty.canRead)
            {
                var getMethod = property.GetGetMethod(true);
                remoteArrayProperty.isPublicRead = getMethod.IsPublic;
                remoteArrayProperty.value = SerializationCustom.SerializeArray(elementType, (IList)property.GetValue(component, null));
            }

            if (remoteArrayProperty.canWrite)
            {
                var setMethod = property.GetSetMethod(true);
                remoteArrayProperty.isPublicWrite = setMethod.IsPublic;
            }

            return remoteArrayProperty;
        }
    }

}