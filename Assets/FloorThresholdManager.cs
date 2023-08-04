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
            new FloorThreshold(-7f, float.MinValue, -0.05f), // Ground floor
     // new FloorThreshold(float.MinValue, -0.4f, -0.05f), // Ground floor
            new FloorThreshold(-0.7f, 2.6f, 0.8f), // First floor
            new FloorThreshold(2.6f, 5.8f, 3.76f), // Second floor
            new FloorThreshold(5.8f, 9f, 6.73f), // Third floor
        };
    }

    /*FloorThresholdManager:
     * We want to separate ground floor from other vales when we use it in CheckYPosMousClick in PlayerController*/

}

