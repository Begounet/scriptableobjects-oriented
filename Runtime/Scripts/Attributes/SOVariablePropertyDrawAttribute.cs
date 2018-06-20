using System;

public class SOVariablePropertyDrawAttribute : Attribute
{
    public bool drawProperties;

    public SOVariablePropertyDrawAttribute(bool shouldDrawProperties)
    {
        drawProperties = shouldDrawProperties;
    }
}
