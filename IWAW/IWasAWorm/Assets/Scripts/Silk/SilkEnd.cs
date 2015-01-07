using Fairwood.Math;
using UnityEngine;
using System.Collections;
using Scripts.Silk;

public class SilkEnd : SilkPoint
{
    public SilkEndState State = SilkEndState.Held;


    /// <summary>
    /// Spit��Free״̬����Ҫ����ʱС��������ǰ��
    /// </summary>
    public GameObject TempMass;

    public SilkEnd(Silk silk) : base(silk)
    {
        TempMass = new GameObject("End TempMass of " + silk.name, typeof (Rigidbody2D));
        TempMass.layer = LayerManager.Layer.Silk;
        TempMass.transform.parent = silk.transform;//ʹ��ʱ�ٷų���
        TempMass.rigidbody.mass = Silk.TempMass;
        TempMass.rigidbody.drag = 1f;
        TempMass.rigidbody.centerOfMass = Vector3.zero;
        TempMass.rigidbody.inertiaTensor = new Vector3(0.1f, 0.1f, 0.1f);
        TempMass.SetActive(false);
    }
}