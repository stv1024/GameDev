using Fairwood.Math;
using UnityEngine;
using System.Collections;
using Scripts.Silk;

public class SilkEnd : SilkPoint
{
    public SilkEndState State = SilkEndState.Held;


    /// <summary>
    /// Spit、Free状态下需要个临时小质量绑在前端
    /// </summary>
    public GameObject TempMass;

    public SilkEnd(Silk silk) : base(silk)
    {
        TempMass = new GameObject("End TempMass of " + silk.name, typeof (Rigidbody2D));
        TempMass.layer = LayerManager.Layer.Silk;
        TempMass.transform.parent = silk.transform;//使用时再放出来
        TempMass.rigidbody.mass = Silk.TempMass;
        TempMass.rigidbody.drag = 1f;
        TempMass.rigidbody.centerOfMass = Vector3.zero;
        TempMass.rigidbody.inertiaTensor = new Vector3(0.1f, 0.1f, 0.1f);
        TempMass.SetActive(false);
    }
}