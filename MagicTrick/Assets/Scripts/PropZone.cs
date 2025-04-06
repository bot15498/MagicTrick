using UnityEngine;

public class PropZone : MonoBehaviour, IPropZone
{
    public Prop CurrentProp { get; protected set; }

    public virtual void AssignProp(Prop newProp)
    {
        if (newProp == null) return;

        // If this slot already has a prop and it's not the same one
        if (CurrentProp != null && CurrentProp != newProp)
        {
            Prop oldProp = CurrentProp;

            // Swap oldProp to newProp's previous slot
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

        // Clear this prop’s current zone first to prevent stacking
        newProp.ClearCurrentSlot();

        // Re-parent and set position
        newProp.transform.SetParent(transform, false);
        newProp.transform.localPosition = Vector3.zero;

        CurrentProp = newProp;
        newProp.currentDropZone = this;
        newProp.previousSlotGroup = this;
    }

    public virtual void ClearSlot(Prop prop)
    {
        if (prop == null) return;

        if (CurrentProp == prop)
        {
            CurrentProp = null;
        }

        if (prop.currentDropZone == this)
        {
            prop.currentDropZone = null;
        }
    }

    public virtual void ClearSlot()
    {
        if (CurrentProp != null)
        {
            CurrentProp.currentDropZone = null;
            CurrentProp = null;
        }
    }
}
