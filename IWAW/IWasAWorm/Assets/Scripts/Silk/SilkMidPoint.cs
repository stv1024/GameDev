using Fairwood.Math;
using UnityEngine;
using System.Collections;
using Scripts.Silk;

public class SilkMidPoint : SilkPoint
{
    private int _indexOnSilk;
    private HandType _hand;

    public SilkMidPoint(Silk silk, int indexOnSilk, Vector2 nearerPoint, Vector2 myPos, Vector2 fartherPoint) : base(silk)
    {
        _indexOnSilk = indexOnSilk;
        var f = fartherPoint - myPos;
        var n = nearerPoint - myPos;
        _hand = (f.x*n.y - f.y*n.x) > 0 ? HandType.Right : HandType.Left;
    }

    /// <summary>
    /// 由Silk在Update里每帧调用，检查手性是否改变，变了则要把前后两截合并成一节直线
    /// </summary>
    public bool CheckHandChange()
    {
        //检测是否改变了手性

        var f = FartherPoint - Position;
        var n = NearerPoint - Position;
        var curHand = (f.x * n.y - f.y * n.x) > 0 ? HandType.Right : HandType.Left;
        if (curHand != _hand)
        {
            //手性改变，双截合并
            MySilk.CombineSegments(_indexOnSilk - 1);
            return true;
        }
        return false;
    }

    #region Utilities
    Vector2 NearerPoint { get { return MySilk.Points[_indexOnSilk + 1].Position; } }
    Vector2 FartherPoint { get { return MySilk.Points[_indexOnSilk - 1].Position; } }
    #endregion

    /// <summary>
    /// 手性，从近端向远端看去，符合哪只手的螺旋定则
    /// </summary>
    public enum HandType
    {
        Left,
        Right
    }
}