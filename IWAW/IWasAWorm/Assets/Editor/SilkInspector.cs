using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Silk))]
public class SilkInspector : Editor
{
    private static bool _extendSegments = false;
    private static bool _extendPoints = false;
    public override void OnInspectorGUI()
    {
        Debug.Log("OnInspectorGUI " + Time.timeSinceLevelLoad);
        var cs = (Silk) target;
        DrawDefaultInspector();
        if (cs.FarEnd == null || cs.NearEnd == null) return;
        EditorGUILayout.LabelField(string.Format("State(far,near):({0},{1})", cs.FarEnd.State, cs.NearEnd.State));
        EditorGUILayout.ObjectField("FarItem", cs.FarEnd.Item, typeof(Rigidbody), true);
        EditorGUILayout.LabelField(string.Format("Anchor:{0}, Pos:{1}", cs.FarEnd.Anchor, cs.FarEnd.Position));
        EditorGUILayout.ObjectField("NearItem", cs.NearEnd.Item, typeof(Rigidbody), true);
        EditorGUILayout.LabelField(string.Format("Anchor:{0}, Pos:{1}", cs.NearEnd.Anchor, cs.NearEnd.Position));
        _extendPoints = EditorGUILayout.Foldout(_extendPoints, "Points");
        if (_extendPoints)
        {
            for (var i = 0; i < cs.Points.Count; i++)
            {
                EditorGUILayout.LabelField(string.Format("[{0}]:Pos:{1}", i, cs.Points[i].Position));
            }
        }
        _extendSegments = EditorGUILayout.Foldout(_extendSegments, "Segments");
        if (_extendSegments)
        {
            for (var i = 0; i < cs.Segments.Count; i++)
            {
                cs.Segments[i].OnInspectorGUI();
            }
        }
    }

    void OnSceneGUI()
    {
        Debug.Log("OnSceneGUI " + Time.timeSinceLevelLoad);
    }

    void OnGUI()
    {
        Debug.Log("OnGUI " + Time.timeSinceLevelLoad);
    }

    void OnDrawGizmos()
    {
        Debug.Log("OnDrawGizmos " + Time.timeSinceLevelLoad);
    }

    void Update()
    {
        Debug.Log("Update " + Time.timeSinceLevelLoad);
    }
}