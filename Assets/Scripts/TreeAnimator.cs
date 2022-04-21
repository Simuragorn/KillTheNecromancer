using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAnimator : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int idleIndex = Random.Range(1, 4);
        animator.SetTrigger("Idle_" + idleIndex);
    }

}
