
using UnityEngine;

public interface IPropZone
{
    Prop CurrentProp { get; }
    void AssignProp(Prop prop);
    void ClearSlot(Prop prop);
}