
using System.Collections.Generic;
using UnityEngine.Scripting;

[Preserve]
public class FloorThreshold 
{
    public float MinY { get; private set; }
    public float MaxY { get; private set; }
    public float YPosition { get; private set; }

    public FloorThreshold(float minY, float maxY, float yPosition)
    {
        MinY = minY;
        MaxY = maxY;
        YPosition = yPosition;
    }
}

