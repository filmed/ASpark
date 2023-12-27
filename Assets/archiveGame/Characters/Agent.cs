using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Agent : MonoBehaviour
{
    private AgentAnimations agentAnimations;
    private AgentMover agentMover;
    private WeaponParent weaponParent;

    private Vector2 pointerInput, movementInput;

    public Vector2 PointerInput { get => pointerInput; set => pointerInput = value; }
    public Vector2 MovementInput { get => movementInput; set => movementInput = value; }

 
    private void Awake()
    {
        agentAnimations = GetComponentInChildren<AgentAnimations>();
        weaponParent = GetComponentInChildren<WeaponParent>();
        agentMover = GetComponentInChildren<AgentMover>();
    }

    // Update is called once per frame
    void Update()
    {
        agentMover.MovementInput = MovementInput;
        weaponParent.mousePos = pointerInput;
        
        AnimateCharacter();
    }

    public void PerformAttack() 
    {
        agentAnimations.AgentAttackAnimation();
        weaponParent.Attack();
    }

  

    private void AnimateCharacter() 
    {
        agentAnimations.AgentMovingAnimation(MovementInput);
    }

}
