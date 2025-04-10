using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoryManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class SlotTimeline
{
    public Dictionary<int, ActionContainer> History = new Dictionary<int, ActionContainer>();

}