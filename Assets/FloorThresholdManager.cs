using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorThresholdManager
{
    public static List<FloorThreshold> floorThresholds;
    static FloorThresholdManager()
    {
        // Add the floor thresholds and their corresponding Y positions here
        floorThresholds = new List<FloorThreshold>
        {
            new FloorThreshold(float.MinValue, 0f, -0.05f), // Ground floor
            new FloorThreshold(0.71f, 3.3f, 0.8f), // First floor
            new FloorThreshold(3.8f, 6f, 3.76f), // Second floor
            new FloorThreshold(6.52f, 9f, 6.73f), // Third floor
        };
    }

}

