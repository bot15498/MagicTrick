using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PropManagerGlobal : MonoBehaviour
{
    // This is the script that keepts track of what props the player currently has
    // The base prop manager manages props per briefcase
    // props is a dictionary representing the slot # / prop data. 
    public Dictionary<int, PlayableProp> props = new Dictionary<int, PlayableProp>();
    // hard code these for now I can't be bothered to think
    public PropManager PropTableManager;
    public PropManager PropShopManager;
    public GameObject propFieldPrefab;
    public GameObject propShopPrefab;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //public void UpdatePropSlots()
    //{
    //    // Update the prop managers to show the correct props in the correct slots
    //}

    //public void AddProp(PlayableProp toadd)
    //{
    //    // finds the next open slot, then adds the prop. 

    //    // Broadcast to all prop managers
    //}

    //public void UpdatePropSlots(PropManager masterManager, Dictionary<int, Prop> newProps)
    //{
    //    // Received a message from a prop manager that something has changed inside of it.
    //    // Get the current config for the prop manager
    //    foreach (PropManager propManager in propManagers)
    //    {
    //        if(propManager == masterManager)
    //        {
    //            continue;
    //        }

    //        propManager.RefreshProplistFromDict(newProps);
    //    }
    //}

    public void UpdateShopPropManager()
    {
        // Make it so the table manager updates the shop props
        PropTableManager.UpdatePropDict();
        PropShopManager.MakeCopyFromDict(PropTableManager.props);
        // Also save a copy here just in case
    }

    public void UpdateTablePropManager()
    {
        // Make it so the shop manager updates the table props
        PropShopManager.UpdatePropDict();
        PropTableManager.MakeCopyFromDict(PropShopManager.props);
        // Also save a copy here just in case
    }
}
