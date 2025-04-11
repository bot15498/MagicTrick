using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class ActAction : ScriptableObject
{
    public abstract ActionContainer AddAction(ActionContainer container, int slot, GameManager manager);

#if UNITY_EDITOR
    [ContextMenu("Delete This")]
    private void DeleteThis()
    {
        Undo.DestroyObjectImmediate(this);
        AssetDatabase.SaveAssets();
    }
#endif
}
