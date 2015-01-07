using Fairwood.Math;
using UnityEngine;
using System.Collections;

/// <summary>
/// 有则必然有Joint连接着
/// </summary>
public class SilkPoint
{
    public readonly Silk MySilk;
    /// <summary>
    /// 没有则是绑定在地形
    /// </summary>
    public Rigidbody2D Item;
    /// <summary>
    /// 没有Item则是世界坐标
    /// </summary>
    public Vector2 Anchor;

    public Vector2 Position
    {
        get { return (Item ? Item.transform.TransformPoint(Anchor).ToVector2() : Anchor); }
    }

    public SilkPoint(Silk silk)
    {
        MySilk = silk;
    }
}