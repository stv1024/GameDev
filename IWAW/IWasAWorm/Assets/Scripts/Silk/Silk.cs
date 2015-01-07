using System;
using System.Collections.Generic;
using System.Linq;
using Fairwood.Math;
using UnityEngine;
using System.Collections;
using Scripts.Silk;

/// <summary>
/// ��Զ��Ҫ��Ϊ�������������壬��ֹ����ɱ
/// </summary>
public class Silk : MonoBehaviour
{
    /// <summary>
    /// 0��Զ�ˣ�1�ǽ���
    /// </summary>
    public readonly SilkEnd[] Ends = new SilkEnd[2];
    /// <summary>
    /// �߶��б���Points��һ����������һ����0����Զ��
    /// </summary>
    public readonly List<SilkSegment> Segments = new List<SilkSegment>();

    /// <summary>
    /// �����б��������˵㣬0��Զ
    /// </summary>
    public readonly List<SilkPoint> Points = new List<SilkPoint>();
    /// <summary>
    /// �������е㣬��������
    /// </summary>
    //public readonly List<Vector2> VertexPositions = new List<Vector2>(); 

    void Awake()
    {
        name = "Silk " + Time.frameCount;

        var farEnd = new SilkEnd(this);
        Ends[0] = farEnd;

        var nearEnd = new SilkEnd(this);
        Ends[1] = nearEnd;

        Points.Clear();
        Points.Add(farEnd);
        Points.Add(nearEnd);

        //var segmentGO = new GameObject("Segment of " + name);
        //segmentGO.transform.parent = transform;
        var segment0 = new SilkSegment();
        segment0.Initialize(this, 0);
        Segments.Add(segment0);
    }

    public void NewSpit(Vector2 respawnPos, Vector2 velocity, Worm worm, float range)
    {
        //transform.localPosition = respawnPos;
        //rigidbody.velocity = velocity;

        FarEnd.State = SilkEndState.Spit;
        FarEnd.Item = FarEnd.TempMass.rigidbody2D;
        FarEnd.Anchor = Vector2.zero;
        NearEnd.State = SilkEndState.Held;
        NearEnd.Item = worm.rigidbody2D;
        NearEnd.Anchor = worm.MouthPosLocal;

        _lastFarEndPosWorld = respawnPos;//��respawnPos���

        var tempMass = FarEnd.TempMass;
        tempMass.SetActive(true);//����С�����������������
        tempMass.transform.parent = null;
        tempMass.transform.localPosition = respawnPos;
        tempMass.rigidbody2D.velocity = velocity;
        tempMass.rigidbody2D.gravityScale = 0;

        _respawnPos = respawnPos;
        _range = range;
    }

    public void Lengthen(float dl)
    {
        if (NearEnd.State != SilkEndState.Held) return;
        Segments[Segments.Count - 1].JointLength += dl;
        //var limit = Segments[Segments.Count - 1].Joint.linearLimit;
        //limit.limit += dl;
        //Segments[Segments.Count - 1].Joint.linearLimit = limit;
    }

    /// <summary>
    /// ҧ��
    /// </summary>
    public void BreakUp()
    {
        if (NearEnd.State != SilkEndState.Held) return;
        NearEnd.State = SilkEndState.Free;
        NearEnd.TempMass.transform.position = NearEnd.Position;
        NearEnd.TempMass.SetActive(true);
        NearEnd.Item = NearEnd.TempMass.rigidbody2D;
        var lastSegment = Segments[Segments.Count - 1];
        //lastSegment.CreateJoint(lastSegment.Joint ? lastSegment.Joint.linearLimit.limit : lastSegment.Distance);
    }
    /// <summary>
    /// ˫˿�ϲ�
    /// </summary>
    public void Combine()
    {
        throw new NotImplementedException();
    }
    /// <summary>
    /// ����i���۶ϳ����ڣ��µĽ��ڵ�i��֮�����
    /// </summary>
    /// <param name="i"></param>
    /// <param name="hitPoint"></param>
    /// <param name="jointItem"></param>
    public void SeperateSegment(int i, Vector2 hitPoint, Rigidbody2D jointItem = null)
    {
        var nearPartDistance = (hitPoint - Points[i + 1].Position).magnitude;
        var farPartDistance = (hitPoint - Points[i].Position).magnitude;
        var totalDistance = nearPartDistance + farPartDistance;
        Debug.Log("nPD:" + nearPartDistance + ";fPD:" + farPartDistance + ";tD:" + totalDistance);
        var nearPartLength = Segments[i].JointLength * nearPartDistance / totalDistance;
        var farPartLength = Segments[i].JointLength * farPartDistance / totalDistance;

        var newMidPoint = new SilkMidPoint(this, i + 1, Points[i + 1].Position, hitPoint, Points[i].Position);
        Points.Insert(i + 1, newMidPoint);

        //var newSegmentGO = new GameObject("Segment of " + name);
        //newSegmentGO.transform.parent = transform;
        var newSegment = new SilkSegment();
        newSegment.Initialize(this, i + 1);
        Segments.Insert(i + 1, newSegment);//�����۵�˿�ĺ������

        newMidPoint.Item = jointItem;
        if (newMidPoint.Item)
        {
            newMidPoint.Anchor = newMidPoint.Item.transform.InverseTransformPoint(hitPoint);
        }
        else
        {
            newMidPoint.Anchor = hitPoint;
        }

        newSegment.CreateJoint(nearPartLength);

        Segments[i].CreateJoint(farPartLength);
    }
    /// <summary>
    /// �ϲ���i�ں͵�i+1�ڣ�����Ĳ���i+1�Ŀ�λ
    /// </summary>
    /// <param name="i"></param>
    public void CombineSegments(int i)
    {
        var jointLength = Segments[i].JointLength + Segments[i + 1].JointLength;
        Debug.Log("combinedlength:"+jointLength);
        Points.RemoveAt(i + 1);
        //Destroy(Segments[i + 1].gameObject);
        Segments.RemoveAt(i + 1);
        Segments[i].CreateJoint(jointLength);
    }

    private Vector2 _respawnPos;
    private float _range;
    private Vector2 _lastFarEndPosWorld;
    void Update()
    {
        for (int i = 0; i < Points.Count;)
        {
            var silkMidPoint = Points[i] as SilkMidPoint;
            if (silkMidPoint == null)
            {
                i++;
                continue;
            }
            if (!silkMidPoint.CheckHandChange()) i++;
        }
        if (FarEnd.State == SilkEndState.Held)
        {
            Debug.LogError("�������³���");
        }
        if (FarEnd.State == SilkEndState.Spit && NearEnd.State == SilkEndState.Held)
        {
            var curFarEndPosWorld = FarEnd.Position;
            if (curFarEndPosWorld != _lastFarEndPosWorld)
            {
                var d = (curFarEndPosWorld - _lastFarEndPosWorld).magnitude;
                var ray = new Ray2D(_lastFarEndPosWorld, curFarEndPosWorld - _lastFarEndPosWorld);
                Debug.DrawRay(_lastFarEndPosWorld, curFarEndPosWorld - _lastFarEndPosWorld, Color.red);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, d, LayerManager.LayerMask.SilkJoint);
                if (hit)//���������ˣ�Ҫ����
                {//todo:����ˮҪ���٣���ô��
                    if (hit.collider.gameObject.layer == LayerManager.Layer.Ground) //����������
                    {
                        var silkDir = (FarEnd.Position - NearEnd.Position).normalized;
                        FarEnd.Item.transform.position = hit.point.ToVector3(FarEnd.Item.transform.position.z) -
                                                         (silkDir*PhysicsConfig.SurfaceLayerThickness).ToVector3();
                        #region FarEnd.Spit->Fixed
                        FarEnd.State = SilkEndState.Fixed;
                        FarEnd.Anchor = FarEnd.Position;
                        FarEnd.Item = null;
                        FarEnd.TempMass.SetActive(false);
                        Segments[0].CreateJoint(Segments[0].Distance);

                        #endregion
                    }
                    else//Ӧ��ֻ����������
                    {
                        
                    }
                }
                else if ((curFarEndPosWorld-_respawnPos).magnitude>=_range)//�ﵽ���
                {
                    #region FarEnd.Spit->Free
                    FarEnd.State = SilkEndState.Free;
                    FarEnd.TempMass.rigidbody2D.gravityScale = 1;
                    FarEnd.TempMass.rigidbody2D.drag = 2;
                    FarEnd.TempMass.AddComponent<SphereCollider>().radius = 0;
                    FarEnd.TempMass.SetActive(true);
                    FarEnd.Item = FarEnd.TempMass.rigidbody2D;
                    FarEnd.Anchor = Vector2.zero;
                    Segments[0].CreateJoint(Segments[0].Distance);
                    var segmentDir = (Segments[0].FarPoint.Position - Segments[0].NearPoint.Position).normalized;
                    FarEnd.TempMass.rigidbody2D.velocity -= Vector2.Dot(FarEnd.TempMass.rigidbody2D.velocity, segmentDir) *
                                                          segmentDir;

                    #endregion
                }
            }
            _lastFarEndPosWorld = curFarEndPosWorld;
        }
        var dt = Time.fixedDeltaTime;
        var oldSegments = new List<SilkSegment>(Segments);
        foreach (var silkSegment in oldSegments.Where(seg=>!seg.Terminated))
        {
            silkSegment.Update(dt);
        }
    }


    #region Utilities
    /// <summary>
    /// ǰ��
    /// </summary>
    public SilkEnd FarEnd { get { return Ends[0]; } }
    /// <summary>
    /// ĩ��
    /// </summary>
    public SilkEnd NearEnd { get { return Ends[1]; } }

    /// <summary>
    /// �������
    /// </summary>
    /// <returns>��ǰ����Ķ������꣬��������</returns>
    public Vector2[] GetBrokenLine()
    {
        var pnts = new Vector2[Points.Count];
        for (var i = 0; i < Points.Count; i++)
        {
            pnts[i] = Points[i].Position;
        }
        return pnts;
    }
    #endregion

    #region Const

    public const float TempMass = 1e-3f;

    #endregion
}
