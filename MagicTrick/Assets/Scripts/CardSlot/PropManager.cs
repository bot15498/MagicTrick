using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PropManager : MonoBehaviour
{
    [Header("Prop Prefabs to Spawn")]
    [SerializeField] private List<GameObject> propPrefabs;
    [SerializeField] private GameObject propPrefab;

    [Header("Prop Slots in Scene")]
    [SerializeField] private List<PropSlot> propSlots;

    [Header("Spawned Props")]
    public Dictionary<int, Prop> props = new Dictionary<int, Prop>();

    void Start()
    {
        for (int i = 0; i < propSlots.Count; i++)
        {
            props[i] = null;
        }
        AssignPropsToSlots();
    }

    void AssignPropsToSlots()
    {
        int count = Mathf.Min(propPrefabs.Count, propSlots.Count);

        for (int i = 0; i < count; i++)
        {
            // Instantiate under the scene root (not the slot) to avoid transform nesting conflicts
            GameObject newPropObj = Instantiate(propPrefabs[i], transform);
            newPropObj.transform.localPosition = Vector3.zero;

            Prop prop = newPropObj.GetComponentInChildren<Prop>();
            prop.UpdateVisual();
            if (prop != null)
            {
                // Prevent stacking by ensuring the prop is not assigned elsewhere
                prop.ClearCurrentSlot();

                // Assign to slot
                propSlots[i].AssignProp(prop);

                // Track
                props[i] = prop;
            }
            else
            {
                Debug.LogWarning($"Prefab at index {i} does not contain a Prop component in its children.");
            }
        }
    }

    public void UpdatePropDict()
    {
        props.Clear();
        for (int i = 0; i < propSlots.Count; i++)
        {
            if (propSlots[i].CurrentProp != null)
            {
                props[i] = propSlots[i].CurrentProp;
            }
            else
            {
                props[i] = null;
            }
        }
    }

    public void AssignPropToEmptySlot(Prop toadd)
    {
        // Thisis for when the prop object alrady instantiated
        UpdatePropDict();
        for (int i = 0; i < propSlots.Count; i++)
        {
            if (props[i] == null)
            {
                props[i] = toadd;
                propSlots[i].AssignProp(toadd);
            }
        }
    }

    public void AssignPropToEmptySlot(PlayableProp toadd)
    {
        // Make a new object, then add it
        GameObject newPropObj = Instantiate(propPrefab, transform);
        newPropObj.transform.localPosition = Vector3.zero;

        Prop prop = newPropObj.GetComponentInChildren<Prop>();
        prop.PropData = toadd;
        prop.UpdateVisual();
        if (prop != null)
        {
            // Prevent stacking by ensuring the prop is not assigned elsewhere
            prop.ClearCurrentSlot();
        }
        AssignPropToEmptySlot(prop);
    }

    public void MakeCopyFromDict(Dictionary<int, Prop> newProps)
    {
        // Clear out the current props
        for (int i = 0; i < propSlots.Count; i++)
        {
            if (propSlots[i].CurrentProp != null)
            {
                GameObject propobj = propSlots[i].CurrentProp.gameObject;
                Destroy(propobj);
            }
            propSlots[i].ClearSlot();

            // Make copies from the given props
            if (newProps[i] != null)
            {
                GameObject newPropObj = Instantiate(propPrefab, transform);
                newPropObj.transform.localPosition = Vector3.zero;
                Prop newProp = newPropObj.GetComponentInChildren<Prop>();
                newProp.PropData = newProps[i].PropData;
                newProp.UpdateVisual();
                propSlots[i].AssignProp(newProp);
            }
        }

    }

    public bool IsFull()
    {
        int propcount = props.Values.Where(x => x != null).Count();
        return propSlots.Count == propcount;
    }

    public List<PlayableProp> GetPropList()
    {
        List<PlayableProp> toreturn = new List<PlayableProp>();
        for (int i = 0; i < propSlots.Count; i++)
        {
            if (propSlots[i].CurrentProp != null)
            {
                toreturn.Add(propSlots[i].CurrentProp.PropData);
            }
        }
        return toreturn;
    }

    public void SetPropContainerActive(bool isActive)
    {
        foreach (var propslot in propSlots)
        {
            propslot.gameObject.SetActive(isActive);
            if (!isActive)
            {
                foreach (var slot in propSlots)
                {
                    if (slot.CurrentProp != null)
                    {
                        GameObject propobj = slot.CurrentProp.gameObject;
                        Destroy(propobj);
                    }
                }
            }
        }
    }

    public void ForceUpdatePropVisual()
    {
        foreach (var propslot in propSlots)
        {
            if(propslot.CurrentProp != null)
            {
                propslot.CurrentProp.propVisual.transform.position = propslot.CurrentProp.transform.position;
            }
        }
    }

    public void ClearProps()
    {
        // Clear out the current props
        for (int i = 0; i < propSlots.Count; i++)
        {
            if (propSlots[i].CurrentProp != null)
            {
                GameObject propobj = propSlots[i].CurrentProp.gameObject;
                Destroy(propobj);
            }
            propSlots[i].ClearSlot();
        }
    }
}
