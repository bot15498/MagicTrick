using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoryManager : MonoBehaviour
{
    public List<SlotTimeline> slotTimelines;

    void Awake()
    {
        // hard code this to 3 slots for now.
        slotTimelines = new List<SlotTimeline>
        {
            new SlotTimeline(),
            new SlotTimeline(),
            new SlotTimeline()
        };
    }

    void Update()
    {

    }
}

public class SlotTimeline
{
    public Dictionary<int, ActionContainer> History;

    public SlotTimeline()
    {
        History = new Dictionary<int, ActionContainer>();
        History[0] = new ActionContainer();
    }

    public void AdvanceTime()
    {
        // This is really bad space wise I think but n is never a 2 digit number so it's probably fine
        Dictionary<int, ActionContainer> temp = new Dictionary<int, ActionContainer>();
        foreach (var item in History)
        {
            temp[item.Key - 1] = item.Value;
        }
        if (!temp.ContainsKey(0))
        {
            temp[0] = new ActionContainer();
        }
        History.Clear();
        History = temp;
    }

    public void AddToTimeline(ActionContainer toadd, int time)
    {
        if (!History.ContainsKey(time))
        {
            History[time] = toadd;
        }
        else
        {
            History[time] += toadd;
        }
    }

}