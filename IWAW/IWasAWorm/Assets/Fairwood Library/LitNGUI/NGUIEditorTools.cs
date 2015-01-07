//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Tools for the editor
/// </summary>

public class NGUIEditorTools
{
    static public void RegisterUndo(string name, params Object[] objects)
    {
        if (objects != null && objects.Length > 0)
        {
            foreach (Object obj in objects)
            {
                if (obj == null) continue;
                Undo.RegisterUndo(obj, name);
                EditorUtility.SetDirty(obj);
            }
        }
        else
        {
            Undo.RegisterSceneUndo(name);
        }
    }
}