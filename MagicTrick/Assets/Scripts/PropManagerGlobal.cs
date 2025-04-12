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
