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
    /// ��Silk��Update��ÿ֡���ã���������Ƿ�ı䣬������Ҫ��ǰ�����غϲ���һ��ֱ��
    /// </summary>
    public bool CheckHandChange()
    {
        //����Ƿ�ı�������

        var f = FartherPoint - Position;
        var n = NearerPoint - Position;
        var curHand = (f.x * n.y - f.y * n.x) > 0 ? HandType.Right : HandType.Left;
        if (curHand != _hand)
        {
            //���Ըı䣬˫�غϲ�
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
    /// ���ԣ��ӽ�����Զ�˿�ȥ��������ֻ�ֵ���������
    /// </summary>
    public enum HandType
    {
        Left,
        Right
    }
}