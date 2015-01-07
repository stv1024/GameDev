using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 丝的Debug方法
/// </summary>
public static class SilkDebug
{
    public static void DrawCross(Vector3 pos, float radius, Color color, float duration = 0f)
    {
        Debug.DrawRay(pos, Vector3.right * radius, color, duration);
        Debug.DrawRay(pos, Vector3.up * radius, color, duration);
        Debug.DrawRay(pos, Vector3.left * radius, color, duration);
        Debug.DrawRay(pos, Vector3.down * radius, color, duration);
    }

    public static void DrawLine(Vector4 line, Color color, float duration = 0f)
    {
        Debug.DrawLine(new Vector3(line.x, line.y), new Vector3(line.z, line.w), color, duration);
    }


    public static Dictionary<string, bool> FoldoutDict = new Dictionary<string, bool>();
    public static bool GetFoldoutValue(string key)
    {
        bool value;
        return FoldoutDict.TryGetValue(key, out value) && value;
    }
    public static void SetFoldoutValue(string key, bool value)
    {
        FoldoutDict[key] = value;
    }
}