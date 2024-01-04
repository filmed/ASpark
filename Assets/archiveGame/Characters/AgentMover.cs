using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class AgentMover : MonoBehaviour 
{
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;
    public Vector2 MovementInput { get; set; }
    private Vector2 currentInput;
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    public Vector2 ImpulseVector = Vector2.zero; 

    [SerializeField]
    private float currentSpeed = 0, maxSpeed = 2, acceleration = 50, deacceleration = 100, collisionsOffset = 0.02f, distance;

    [SerializeField]
    private ContactFilter2D movementFilter;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }
    private void FixedUpdate()
    {
        if (ImpulseVector.magnitude > 0) 
        {
            ImpulseVector = ImpulseVector * 0.5f;
        }
        if (MovementInput != Vector2.zero)
        {
            currentInput = MovementInput;
            currentSpeed += acceleration * maxSpeed * Time.fixedDeltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);

            bool isMoved = TryMove(MovementInput);
            
            if (!isMoved)
            {
                isMoved = TryMove(new Vector2(MovementInput.x, 0));
            }

            if (!isMoved)
            {
                isMoved = TryMove(new Vector2(0, MovementInput.y));
            }

            if (!isMoved) //congrats, it works (even with moveSpeed = 50  0__0), but looks awful... :/
            {
                //get pos of closest colider wich has the closest point to player collider center. Prob could use rb.position instead of (Vector2)transform.position + boxCollider.offset, but i didnt try :) 
                RaycastHit2D hit = castCollisions.OrderBy(target => Vector2.Distance(target.collider.ClosestPoint((Vector2)transform.position + circleCollider.offset), (Vector2)transform.position + circleCollider.offset)).FirstOrDefault();
                //get distance between player collider center and wall's collider edge 
                distance = Vector2.Distance(hit.collider.ClosestPoint((Vector2)transform.position + circleCollider.offset), (Vector2)transform.position + circleCollider.offset);

                //get vector which we have to move(direction * fixed_distance). TO DO: Fix collider system to sphere, or calculate better for box collider (now calculates half of diagonal ==> it equals to radius) 
                Vector2 toMove = MovementInput * (distance - collisionsOffset - circleCollider.radius);
                rb.MovePosition(rb.position + toMove);
               
            }

              
        }
        else 
        {
            currentSpeed -= deacceleration * maxSpeed * Time.fixedDeltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);

            bool isGlided = TryMove(currentInput);

            if (!isGlided && castCollisions.Count > 0) 
            {
                //get pos of closest colider wich has the closest point to player collider center. Prob could use rb.position instead of (Vector2)transform.position + boxCollider.offset, but i didnt try :) 
                RaycastHit2D hit = castCollisions.OrderBy(target => Vector2.Distance(target.collider.ClosestPoint((Vector2)transform.position + circleCollider.offset), (Vector2)transform.position + circleCollider.offset)).FirstOrDefault();
                //get distance between player collider center and wall's collider edge 
                distance = Vector2.Distance(hit.collider.ClosestPoint((Vector2)transform.position + circleCollider.offset), (Vector2)transform.position + circleCollider.offset);

                //get vector which we have to move(direction * fixed_distance). TO DO: Fix collider system to sphere, or calculate better for box collider (now calculates half of diagonal ==> it equals to radius) 
                Vector2 toMove = MovementInput * (distance - collisionsOffset - circleCollider.radius);
                rb.MovePosition(rb.position + toMove);
            }

            


        }
    }


    public bool TryMove(Vector2 _direction)
    {
        
        bool _result = false;
        if (_direction != Vector2.zero)
        {
            int count = rb.Cast(
                _direction,
                movementFilter,
                castCollisions,
                currentSpeed * Time.fixedDeltaTime + collisionsOffset
                ) ;
            Debug.DrawRay(transform.position + new Vector3(circleCollider.offset.x, circleCollider.offset.y, 0), _direction * (currentSpeed * Time.fixedDeltaTime + collisionsOffset), Color.blue, 1f);
            


            if (count == 0)
            {
                Debug.DrawRay(transform.position + new Vector3(circleCollider.offset.x, circleCollider.offset.y, 0), _direction * (currentSpeed * Time.fixedDeltaTime + collisionsOffset), Color.red, 5f);
                
                rb.MovePosition(rb.position+ImpulseVector +  _direction * currentSpeed * Time.fixedDeltaTime);
                _result = true;
            }
        }
        else
        {
            _result = false;
        }
        return _result;
    }
}
