using UnityEngine;
using System.Collections;

public class SilkRenderer : MonoBehaviour
{
    public Silk RenderedSilk;
    public Vector2[] BrokenLine;
    private void Awake()
    {
        if (!RenderedSilk) RenderedSilk = GetComponent<Silk>();
        if (!RenderedSilk)
        {
            Debug.LogWarning("no silk to render!");
            enabled = false;
        }
    }

    private void Update()
    {
        if (!RenderedSilk)
        {
            enabled = false;
            return;
        }
        var pnts = RenderedSilk.GetBrokenLine();
        for (var i = 0; i < pnts.Length - 1; i++)
        {
            Debug.DrawLine(pnts[i], pnts[i + 1], Color.white);
        }
        BrokenLine = pnts;
    }
}