using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{


    private PlayerInputActions controls;


    public float moveSpeed = 1f;
    public float collisionsOffset = 0.02f;
    public ContactFilter2D movementFilter;
    public GameObject playerSprite;
    public GameObject fireTarget;
    public float targetMaxOrbit = 2;
    public float targetMinOrbit = 0.5f;
    public float targetSpeed = 1f;
    public float glidingRange = 50f;
    public float angle = 0;

    public float smoothTime = 0.5f;
    public Vector2 velocity;

    public static Vector2 direction1;
    public static Vector2 direction2;


    Vector2 movementInput;
    Vector2 mousePos;
    Animator animator;

    Rigidbody2D rb;
    Rigidbody2D rbTarget;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();


    private void Awake()
    {
        controls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rbTarget = fireTarget.GetComponent<Rigidbody2D>();
        animator = playerSprite.GetComponent<Animator>();

       // controls.Player.Shoot.performed += _ => Shooting();

    }

    void FixedUpdate()
    {
        //if (controls.Player.Shoot.IsPressed()) 
        //{
            
        //}
        
        

        if (movementInput != Vector2.zero)
        {
            bool isMoved = TryMove(movementInput);

            if (!isMoved)
            {
                isMoved = TryMove(new Vector2(movementInput.x, 0));
            }

            if (!isMoved)
            {
                isMoved = TryMove(new Vector2(0, movementInput.y));
            } 

            animator.SetBool("isMoving", isMoved);
        }
        else 
        {
            animator.SetBool("isMoving", false);
        }

         mousePos = GetMousePos();
         /*float distance = Vector3.Distance(playerSprite.transform.position, fireTarget.transform.position);
         Debug.Log(distance);
         if (distance < targetMaxOrbit)
         {
             //fireTarget.transform.position = mousePos;
             //rbTarget.MovePosition(rbTarget.position + mousePos.normalized * targetMoveSpeed * Time.fixedDeltaTime);
             fireTarget.transform.position = Vector2.SmoothDamp(fireTarget.transform.position, mousePos, ref velocity, smoothTime, targetSpeed);
         }
         else if (distance > targetMinOrbit)
         {
             fireTarget.transform.position = Vector2.SmoothDamp(mousePos, fireTarget.transform.position, ref velocity, smoothTime, targetSpeed);
         }
         else 
         {
             fireTarget.transform.position = new Vector2(playerSprite.transform.position.x + targetMinOrbit + (targetMaxOrbit - targetMinOrbit) /  2, playerSprite.transform.position.y);
         }*/


        //MoveToAngle(fireTarget, angle, targetSpeed, targetMaxOrbit, playerSprite.transform.position);
        //transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);
        

        direction1 = (mousePos - new Vector2(playerSprite.transform.position.x, playerSprite.transform.position.y)).normalized;
        direction2 = (new Vector2(fireTarget.transform.position.x, fireTarget.transform.position.y) - new Vector2(playerSprite.transform.position.x, playerSprite.transform.position.y)).normalized;
        //Debug.Log(direction1);
        //Debug.DrawRay(playerSprite.transform.position, direction1 * 20f, Color.blue);
        //Debug.DrawRay(playerSprite.transform.position, direction2 * 20f, Color.green);
        //Debug.Log(Vector2.SignedAngle(direction2, direction1));
        
        //angle = Vector2.SignedAngle(direction2, direction1);
        
        float newAngle  = Vector2.SignedAngle(Vector2.right, direction1);
        //RotateToAngle(fireTarget, 180, targetSpeed, targetMaxOrbit, playerSprite.transform.position);

        //перед вызовом вращени€ вернуть прицел на рассто€ние радиуса от цента относительно вектора currentDirection (direction2)

        RotateToAngle(fireTarget, newAngle, targetSpeed, targetMaxOrbit, playerSprite.transform.position);    // should be returned
        
        fireTarget.transform.localPosition = Vector2.SmoothDamp(fireTarget.transform.localPosition, direction1*(targetMaxOrbit + glidingRange), ref velocity, smoothTime, targetSpeed); //should be returned

    }


    public void RotateToAngle(GameObject _obj, float _angle, float _speed, float _radius, Vector2 _center)
    {
        //float _directionAngle = _angle/Mathf.Abs(_angle);
        _angle = _angle * Mathf.PI / 180;
        var _x = Mathf.Cos(_angle) * _radius;
        var _y = Mathf.Sin(_angle) * _radius;
        _obj.transform.position = new Vector2(_x, _y) + _center;
    }


    public Vector3 GetMousePos() 
    {
        Vector3 _position;

        _position = Mouse.current.position.ReadValue();
        Vector3 Worldpos = Camera.main.ScreenToWorldPoint(_position);

        return Worldpos;
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
                moveSpeed * Time.fixedDeltaTime + collisionsOffset
                );
            if (count > 0) 
            {
                Debug.Log(count);
            }
            
            if (count == 0) 
            {
                _result = true;
                //fireTarget.transform.localPosition = direction1 * targetMaxOrbit;
                rb.MovePosition(rb.position + _direction * moveSpeed * Time.fixedDeltaTime);
            }
        }
        else 
        {
            _result = false;
        }
        return _result;
    }

    void OnMove(InputValue _movementValue) 
    {
        movementInput = _movementValue.Get<Vector2>();
    }

    void Shooting() 
    {
        Debug.Log("fireShow");
    }

/*    void OnFire() 
    {
        Debug.Log("fire");
        animator.SetTrigger("playerAttackTrigger");

    }*/
}
