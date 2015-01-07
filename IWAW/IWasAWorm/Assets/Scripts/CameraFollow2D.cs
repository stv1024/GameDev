using UnityEngine;
using Fairwood.Math;

public class CameraFollow2D : MonoBehaviour
{
    public Transform Target;
    public float K = 2;

    private void Update()
    {
        if (!Target)
        {
            enabled = false;
            return;
        }
        transform.position += (Target.position - transform.position).SetV3Z(0)*K*Time.deltaTime;
    }
}