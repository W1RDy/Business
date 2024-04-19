using UnityEngine;

static class Vector2Extension
{
    public static Vector2 Sum(this Vector2 vector, float number)
    {
        return new Vector2(vector.x + number, vector.y + number);
    }
}