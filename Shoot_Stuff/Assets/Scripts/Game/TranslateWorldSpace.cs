/*==============================================
 * Translate World Space
 * Normalizes world positions and distances to
 * floats between 0 and 1 for use with Csound
 * =============================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TranslateWorldSpace {

    static float worldX = 0.0f;
    static float worldY = 0.0f;
    static float worldZ = 0.0f;
    // static float longestDistance = 0.0f;

    public static void setWorldSize(Vector3 worldSize)
    {
        worldX = worldSize.x;
        worldY = worldSize.y;
        worldZ = worldSize.z;

        // Calculate longest line that can be drawn in world box (square root of each side to the power of 2
        // longestDistance = Mathf.Sqrt(Mathf.Pow(worldX, 2) + Mathf.Pow(worldY, 2) + Mathf.Pow(worldZ, 2));
    }

    public enum Axis { x, y, z };

    public static float normalizePosition(Axis axisToUse, float position)
    {
        switch (axisToUse)
        {
            case Axis.x:
                return (position + (worldX / 2)) / worldX;
            case Axis.y:
                return (position + (worldY / 2)) / worldY;
            case Axis.z:
                return (position + (worldZ / 2)) / worldZ;
        }

        Debug.Log("Couldn't normalize position - Returning 0f");
        return 0.0f;
    }

    public static float normalizeDistance(float distance)
    {
        return Mathf.Clamp01((worldX - distance) / worldX);
    }
}
