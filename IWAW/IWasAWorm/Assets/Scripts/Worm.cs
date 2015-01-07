using Fairwood.Math;
using UnityEngine;
using Fairwood;

public class Worm : MonoBehaviour
{
    public GameObject SilkPrefab;

    public float ShootSpeed = 100;
    public float ShootRange = 200;

    public Silk SilkInMouth;

    public Vector2 MouthPosLocal;
    public Vector2 MouthPosWorld
    {
        get { return transform.TransformPoint(MouthPosLocal); }
    }

    void Awake()
    {
        rigidbody.centerOfMass = Vector3.zero;//将质心固定为原点
        rigidbody.inertiaTensor = new Vector3(1, 1, 1);//将惯性张量固定为有限值
    }

    public void Spit(Vector2 dir)
    {
        if (dir.x == 0 && dir.y == 0)
        {
            Debug.LogError("dir == 0");
            return;
        }
        if (SilkInMouth)//嘴里有丝
        {
            
        }
        else//嘴里无丝
        {
            var go = Instantiate(SilkPrefab) as GameObject;
            go.transform.ResetAs(SilkPrefab.transform);
            //var go = new GameObject("Silk " + Time.frameCount);
            SilkInMouth = go.GetComponent<Silk>();
            SilkInMouth.NewSpit(
                transform.TransformPoint(MouthPosLocal).ToVector2(), dir.normalized*ShootSpeed, this, ShootRange);
        }
    }

    public void BreakUpHeldSilk()
    {
        Debug.Log("BreakUpHeldSilk");
        if (!SilkInMouth) return;
        SilkInMouth.BreakUp();
        SilkInMouth = null;
    }

    public void LengthenSilk(float dl)
    {
        if (dl == 0) return;
        if (!SilkInMouth) return;
        SilkInMouth.Lengthen(dl);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.T))
        {
            var str = "{COM:" + rigidbody.centerOfMass + ", Tensor:" + rigidbody.inertiaTensor + "}";
            Debug.LogWarning(str);
        }
    }
}