using System.Collections.Generic;
using UnityEngine;

public class PropManager : MonoBehaviour
{
    [Header("Prop Prefabs to Spawn")]
    [SerializeField] private List<GameObject> propPrefabs;

    [Header("Prop Slots in Scene")]
    [SerializeField] private List<PropSlot> propSlots;

    [Header("Spawned Props")]
    public List<Prop> props = new List<Prop>();

    void Start()
    {
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
            if (prop != null)
            {
                // Prevent stacking by ensuring the prop is not assigned elsewhere
                prop.ClearCurrentSlot();

                // Assign to slot
                propSlots[i].AssignProp(prop);

                // Track
                props.Add(prop);
            }
            else
            {
                Debug.LogWarning($"Prefab at index {i} does not contain a Prop component in its children.");
            }
        }
    }
}
