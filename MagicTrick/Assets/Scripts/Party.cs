using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(fileName = "New Party", menuName = "Party")]
public class Party : ScriptableObject
{
    public string PartyName;
    [TextArea(5,10)]
    public string PartyDescription;
    public Sprite PartyInvitationImage;
    public List<Round> Rounds = new List<Round>();

#if UNITY_EDITOR
    [ContextMenu("Make New Round")]
    private void AddNewRound()
    {
        Round newround = ScriptableObject.CreateInstance<Round>();
        newround.ScoreRequired = 0;

        Rounds.Add(newround);

        AssetDatabase.AddObjectToAsset(newround, this);
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(newround);
    }
#endif
}
