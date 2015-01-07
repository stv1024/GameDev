using Fairwood.Math;
using UnityEngine;
using System.Collections;

/// <summary>
/// �����Ȼ��Joint������
/// </summary>
public class SilkPoint
{
    public readonly Silk MySilk;
    /// <summary>
    /// û�����ǰ��ڵ���
    /// </summary>
    public Rigidbody2D Item;
    /// <summary>
    /// û��Item������������
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