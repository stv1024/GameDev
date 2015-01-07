using UnityEngine;
using System.Collections;
using Fairwood.Math;

#if UNITY_EDITOR
    using UnityEditor;
#endif

/// <summary>
/// 管理丝的碰撞器
/// </summary>
public class SilkSegment
{
    public Silk MySilk;
    public int IndexOnSilk;

    public DistanceJoint2D Joint;

    private float _jointLength;
    /// <summary>
    /// 如果有Joint则自动更改Joint的长度，否则只记录
    /// </summary>
    public float JointLength
    {
        get { return _jointLength; }
        set
        {
            Debug.Log("set JointLength:" + value);
            _jointLength = value;
            if (Joint)
            {
                //var limit = Joint.linearLimit;
                //limit.limit = _jointLength;
                //Joint.linearLimit = limit;
            }
        }
    }

    /// <summary>
    /// 创建时必须手动立即执行这个，相当于构造器，但是Awake()不能填参数
    /// </summary>
    /// <param name="mySilk"></param>
    /// <param name="indexOnSilk"></param>
    public void Initialize(Silk mySilk, int indexOnSilk)
    {
        MySilk = mySilk;
        IndexOnSilk = indexOnSilk;
        var center = MySilk.transform.InverseTransformPoint((FarPoint.Position + NearPoint.Position)*0.5f);

        _lastLine.x = NearPoint.Position.x;
        _lastLine.y = NearPoint.Position.y;
        _lastLine.z = FarPoint.Position.x;
        _lastLine.w = FarPoint.Position.y;
    }

    private Vector4 _lastLine;

    public bool Terminated = false;

    public void Update(float dt)
    {
        var dir = (FarPoint.Position - NearPoint.Position).normalized;
        //var ray = new Ray2D(NearPoint.Position, dir);
        var hit = Physics2D.Raycast(NearPoint.Position, dir, (FarPoint.Position - NearPoint.Position).magnitude,
                            LayerManager.LayerMask.SilkJoint);
        if (hit)
        {
            SilkDebug.DrawCross(hit.point, 0.3f, Color.red);

            var curLine = new Vector4(NearPoint.Position.x, NearPoint.Position.y, FarPoint.Position.x, FarPoint.Position.y);
            LineCollisionHit lineHit;
            var bl = LineCollision.TryGetLineCollisionPoint(_lastLine, curLine, out lineHit, LayerManager.LayerMask.SilkJoint,
                                                   PhysicsConfig.SurfaceLayerThickness);
            SilkDebug.DrawLine(_lastLine, Color.green);

            SilkDebug.DrawCross(lineHit.Point, 0.4f, Color.black);

            var jointItem = lineHit.Collider.gameObject.layer == LayerManager.Layer.Ground
                                ? null
                                : lineHit.Collider.rigidbody2D;
            MySilk.SeperateSegment(IndexOnSilk, lineHit.Point, jointItem);
        }

        _lastLine.x = NearPoint.Position.x;
        _lastLine.y = NearPoint.Position.y;
        _lastLine.z = FarPoint.Position.x;
        _lastLine.w = FarPoint.Position.y;
    }

    /// <summary>
    /// 删除旧的Joint，创建新的Joint，根据远近两个Point，所以要先设置Point，再设置Segment
    /// </summary>
    public void CreateJoint(float length)
    {
        if (Joint) Object.Destroy(Joint);
        if (FarPoint.Item) //优先在远点创建
        {
            Joint = FarPoint.Item.gameObject.AddComponent<DistanceJoint2D>();
            ResetJoint();
            JointLength = length;
            var farPointOriPos = FarPoint.Item.transform.position;
            var farPointNeedDisplacement = NearPoint.Position - FarPoint.Position; //远点的临时位移
            FarPoint.Item.transform.position += farPointNeedDisplacement.ToVector3();
            Joint.connectedBody = NearPoint.Item ? NearPoint.Item.rigidbody2D : null; //兼容链接到地形
            FarPoint.Item.transform.position = farPointOriPos;
        }
        else if (NearPoint.Item) //那就只能在近点创建了
        {
            Joint = NearPoint.Item.gameObject.AddComponent<DistanceJoint2D>();
            ResetJoint();
            JointLength = length;
            var nearPointOriPos = NearPoint.Item.transform.position;
            var nearPointNeedDisplacement = FarPoint.Position - NearPoint.Position; //近点的临时位移
            NearPoint.Item.transform.position += nearPointNeedDisplacement.ToVector3();
            Joint.connectedBody = FarPoint.Item ? FarPoint.Item.rigidbody2D : null; //兼容链接到地形
            NearPoint.Item.transform.position = nearPointOriPos;
        }
        else
        {
            JointLength = length;
        }
    }

    /// <summary>
    /// 使Joint达到默认状态
    /// </summary>
    /// <param name="length">铰链长度</param>
    private void ResetJoint()
    {
        //Joint.axis = new Vector3(0, 0, 1);
        //Joint.xMotion = ConfigurableJointMotion.Limited;
        //Joint.yMotion = ConfigurableJointMotion.Limited;
        //Joint.zMotion = ConfigurableJointMotion.Limited;
        //var limit = Joint.linearLimit;
        //limit.limit = _jointLength;
        //limit.bounciness = 0.7f;
        //limit.spring = 100;
        //limit.damper = 10;
        //Joint.linearLimit = limit;
    }

    public void OnDestroy()
    {
        Object.Destroy(Joint);
    }
    #region Utilities

    public SilkPoint FarPoint
    {
        get { return MySilk.Points[IndexOnSilk]; }
    }

    public SilkPoint NearPoint
    {
        get { return MySilk.Points[IndexOnSilk + 1]; }
    }

    public float Distance
    {
        get { return (MySilk.Points[IndexOnSilk].Position - MySilk.Points[IndexOnSilk + 1].Position).magnitude; }
    }

    #endregion
    
    #region Inspector
#if UNITY_EDITOR
    public string DebugName { get { return MySilk.name + " " + IndexOnSilk; } }
    public void OnInspectorGUI()
    {
        var foldout = EditorGUILayout.Foldout(SilkDebug.GetFoldoutValue(DebugName), DebugName);
        SilkDebug.SetFoldoutValue(DebugName, foldout);
        if (foldout)
        {
            EditorGUILayout.ObjectField("MySilk", MySilk, typeof (Silk), true);
            EditorGUILayout.IntField("IndexOnSilk", IndexOnSilk);
            EditorGUILayout.ObjectField("Joint", Joint, typeof (ConfigurableJoint), true);
            EditorGUILayout.FloatField("_jointLength", _jointLength);
        }
    }
#endif
    #endregion
}