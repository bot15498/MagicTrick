using UnityEngine;
using UnityEngine.Events;

public class PropPlayZone : PropZone
{
    [Header("Events")]
    public UnityEvent<Prop> OnPropReceived = new UnityEvent<Prop>();

    [Header("Debug / Reference")]
    public GameObject currentPropObject; // Reference to the GameObject in the zone

    public override void AssignProp(Prop newProp)
    {
        if (newProp == null) return;

        // If this zone already has a prop (that isn’t the incoming one), swap it
        if (CurrentProp != null && CurrentProp != newProp)
        {
            Prop oldProp = CurrentProp;

            if (newProp.previousSlotGroup != null && newProp.previousSlotGroup != this)
            {
                newProp.previousSlotGroup.ClearSlot(oldProp);
                newProp.previousSlotGroup.AssignProp(oldProp);
            }
            else
            {
                oldProp.ClearCurrentSlot();
                oldProp.ReturnToHand();
            }
        }

        newProp.ClearCurrentSlot();

        newProp.transform.SetParent(transform, false);
        newProp.transform.localPosition = Vector3.zero;

        CurrentProp = newProp;
        currentPropObject = newProp.gameObject; // Update public GameObject

        newProp.currentDropZone = this;
        newProp.previousSlotGroup = this;

        OnPropReceived.Invoke(newProp);
    }

    public override void ClearSlot(Prop prop)
    {
        base.ClearSlot(prop);

        if (CurrentProp == prop)
        {
            currentPropObject = null;
        }
    }

    public override void ClearSlot()
    {
        base.ClearSlot();
        currentPropObject = null;
    }
}
