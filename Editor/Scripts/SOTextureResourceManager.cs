using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOTextureResourceManager
{
    public enum Resource
    {
        IconBackground,

        IconBroadcaster,
        IconListener,

        IconConstant,
        IconRuntime,

        IconRead,
        IconWrite,
        IconReadWrite,
    }

    public static SOTextureResourceManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SOTextureResourceManager();
            }
            return (_instance);
        }
    }

    private static SOTextureResourceManager _instance;

    private Dictionary<Resource, Texture> _loadedTexture;

    private SOTextureResourceManager()
    {
        _loadedTexture = new Dictionary<Resource, Texture>();

        _loadedTexture.Add(Resource.IconBackground, Resources.Load<Texture>("IconBackground"));

        _loadedTexture.Add(Resource.IconBroadcaster, Resources.Load<Texture>("IconBroadcaster"));
        _loadedTexture.Add(Resource.IconListener, Resources.Load<Texture>("IconListener"));

        _loadedTexture.Add(Resource.IconConstant, Resources.Load<Texture>("IconConstant"));
        _loadedTexture.Add(Resource.IconRuntime, Resources.Load<Texture>("IconRuntime"));

        _loadedTexture.Add(Resource.IconRead, Resources.Load<Texture>("IconRead"));
        _loadedTexture.Add(Resource.IconWrite, Resources.Load<Texture>("IconWrite"));
        _loadedTexture.Add(Resource.IconReadWrite, Resources.Load<Texture>("IconReadWrite"));
    }

    public static Texture  GetTexture(Resource resource)
    {
        Texture texture;
        if (Instance._loadedTexture.TryGetValue(resource, out texture))
        {
            return (texture);
        }

        return (null);
    }
}
