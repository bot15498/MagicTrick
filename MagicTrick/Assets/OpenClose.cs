using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenClose : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject toggleTarget;
    bool isopen;

    void Start()
    {
        isopen = false;
        toggleTarget.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void toggletarget()
    {
        isopen = !isopen;
        toggleTarget.SetActive(isopen);
    }

}
