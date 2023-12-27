using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private List<SteeringBehaviour> steeringBehaviours;
    [SerializeField]
    private List<SteeringBehaviour> idlingBehaviours;

    [SerializeField]
    private List<Detector> detectors;

    [SerializeField]
    private AIData aiData;

    [SerializeField]
    private float detectionDelay = 0.05f, aiUpdateDelay = 0.06f, attackDelay = 1f;

    [SerializeField]
    private float attackDistance = 0.5f;

    public UnityEvent OnAttackPressed;
    public UnityEvent<Vector2> OnMovementInput, OnPointerInput;

    [SerializeField]
    private Vector2 movementInput, weaponDir;

    [SerializeField]
    private ContextSolver movementDirectionSolver;

    [SerializeField]
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();


    [SerializeField]
    private ContactFilter2D filter2D;

    [SerializeField]
    private LayerMask layermask;

    private WeaponParent wp;

    bool following = false, idling = false;
    int currDir = 0;

    private void Start()
    {
        wp = GetComponentInChildren<WeaponParent>();
        InvokeRepeating("PerformDetection", 0, detectionDelay);
    }

    private void PerformDetection() 
    {
        foreach (Detector detector in detectors) 
        {
            detector.Detect(aiData);
        }
    }

    private void Update()
    {
        if (aiData.currentTarget != null)
        {
            OnPointerInput?.Invoke(aiData.currentTarget.position);

            if (following == false)
            {
                following = true;
                StartCoroutine(ChaseAndAttack());
            }
        }
        else if (aiData.GetTargetsCount() > 0) 
        {
            aiData.currentTarget = aiData.targets[0];
        }
        else if (idling == false && aiData.currentTarget == null)
        {
            idling = true;
            following = false;

            StartCoroutine(Idle());

        }

        OnMovementInput?.Invoke(movementInput);
    }

    private IEnumerator Idle()
    {
        movementInput = Vector2.zero;
        yield return new WaitForSeconds(aiUpdateDelay);
        
    }

        private IEnumerator ChaseAndAttack() 
    {
        if (aiData.currentTarget == null) 
        {
            //stopping 
            //Debug.Log("Stopping");
            movementInput = Vector2.zero;
            following = false;
            yield return null;
        }
        else 
        {
            //Debug.Log(ag.PointerInput);
            Vector2 weaponDir = ((Vector2)wp.transform.position - (Vector2)transform.position).normalized;
            float distance = Vector2.Distance(aiData.currentTarget.position, transform.position);
            RaycastHit2D hit = Physics2D.Raycast(wp.transform.position, weaponDir, 2 * distance);


            
            
            //Debug.DrawLine(transform.position, (Vector2)transform.position +( attackDistance * weaponDir), Color.red, 0.5f);
            if (distance <= attackDistance && hit.collider != null && (layermask == (layermask | (1 << hit.collider.gameObject.layer)))) // TEST
            {
                //attack
                //Debug.Log("Attack");
                movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);
                OnAttackPressed?.Invoke();
                yield return new WaitForSeconds(attackDelay);
                StartCoroutine(ChaseAndAttack());
            }
            else 
            {
                //chase
                //Debug.Log("Chase");
                movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);
                yield return new WaitForSeconds(aiUpdateDelay);
                StartCoroutine(ChaseAndAttack());
            }
        }
    }
}
