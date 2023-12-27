using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAnimations : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void AgentMovingAnimation(Vector2 _movementInput) 
    {
        if (_movementInput.magnitude > 0)
        {
            //Vector2.Dot(_movementInput, Vector2.right) // TO DO for detect directions 
            animator.SetBool("isMoving", true);
        }
        else 
        {
            animator.SetBool("isMoving", false);
        }
    }

    public void AgentAttackAnimation() 
    {
        animator.SetTrigger("agentAttackTrigger");
    }

}
