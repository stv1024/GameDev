using Fairwood.Math;
using UnityEngine;

/// <summary>
/// 虫子的物理层
/// </summary>
public class WormPhysics : MonoBehaviour
{
    private Vector2 _lastPos;
    private SphereCollider _sphereCollider;
    void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
    }
    void FixedUpdate()
    {
        var curPos = transform.position.ToVector2();
        RaycastHit hit;
        if (Physics.SphereCast(_lastPos, _sphereCollider.radius, (curPos - _lastPos).normalized, out hit,
                               (curPos - _lastPos).magnitude, LayerManager.LayerMask.Ground))
        {
            SilkDebug.DrawCross(_lastPos, 10, Color.cyan, 1);
            SilkDebug.DrawCross(curPos, 10, Color.cyan, 1);
            Debug.DrawLine(_lastPos, curPos, Color.gray, 1);
            Debug.Break();
        }
        _lastPos = curPos;
    }
}