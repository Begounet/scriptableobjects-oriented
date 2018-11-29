using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOTextureSampler", menuName = "SO/Advanced/Texture Sampler")]
public class SOTextureSampler : SOBaseVariable
{
    [SerializeField]
    private Texture2D _lookupTable;
    public Texture2D LookupTable
    {
        get { return _lookupTable; }
        set { _lookupTable = value; }
    }

    public bool IsTextureValid { get { return _lookupTable != null; } }
    public int TextureWidth { get { return IsTextureValid ? _lookupTable.width : 0; } }
    public int TextureHeight { get { return IsTextureValid ? _lookupTable.height : 0; } }

    public Color SampleColor(int x, int y)
    {
        if (_lookupTable == null)
        {
            return Color.black;
        }

        x = Mathf.Clamp(x, 0, _lookupTable.width - 1);
        y = Mathf.Clamp(y, 0, _lookupTable.height - 1);

        return _lookupTable.GetPixel(x, y);
    }
}
