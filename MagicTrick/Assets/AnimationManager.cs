using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour

{


    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void playAnimation(string animationName)
    {
        anim.Play(animationName);
        StartCoroutine(WaitForAnimation(animationName));
    }

    IEnumerator WaitForAnimation(string stateName)
    {
        yield return null; 
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        float clipLength = stateInfo.length;

        yield return new WaitForSeconds(clipLength);
        TrickClipDone();

    }


    void TrickClipDone()
    {
        //Add stuff to scream at game manager
    }
}
