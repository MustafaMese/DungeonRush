using System;
using UnityEngine;

namespace UsingTheirs.RemoteInspector
{
    // For Client
    
    public class ObjectRef
    {
        public string name;
        public int instanceID;
    }
    
    public class MaterialRef : ObjectRef
    {
        public string shaderName;
    }
    
    public class Texture2DRef : ObjectRef
    {
        public TextureFormat format;
        public int width, height;
    }
    
    public class Texture3DRef : ObjectRef
    {
        public TextureFormat format;
        public int width, height, depth;
    }
    
    public class TextureCubeRef : ObjectRef
    {
        public TextureFormat format;
        public int width;
        public int height;
    }
    
    public class Texture2DArrayRef : ObjectRef
    {
        public TextureFormat format;
        public int width, height, depth;
    }
    public class TextureCubeArrayRef : ObjectRef
    {
        public TextureFormat format;
        public int width, height, count;
    }
}