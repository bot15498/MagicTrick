using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActAction : ScriptableObject
{
    public abstract void DoAction(GameManager gameManager);
}
