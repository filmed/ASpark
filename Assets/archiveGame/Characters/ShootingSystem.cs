using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSystem : MonoBehaviour
{

    private PlayerInputActions controlsShooting;

    public GameObject model;
    public GameObject bulletSpawn;
    public GunSystem gun;

    public float fireTimer;
    public Vector2 direction;

    private void Awake()
    {
        controlsShooting = new PlayerInputActions();
    }

    private void OnEnable()
    {
        controlsShooting.Enable();
    }
    private void OnDisable()
    {
        controlsShooting.Disable();
    }



    Animator animator;
    Vector2 fireDirection;

    // Start is called before the first frame update
    void Start()
    {
        animator = model.GetComponent<Animator>();
        if (gun != null) 
        {
            fireTimer = gun.basicAttackTime;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {


        if (controlsShooting.Player.Shoot.IsPressed() && fireTimer <= 0 && gun != null)
        {
            fireDirection = (new Vector2(bulletSpawn.transform.position.x, bulletSpawn.transform.position.y) - new Vector2(model.transform.position.x, model.transform.position.y)).normalized;

            Debug.Log(fireDirection +" "+ new Vector2(bulletSpawn.transform.position.x, bulletSpawn.transform.position.y));
            Debug.DrawRay(bulletSpawn.transform.position, fireDirection * 10f, Color.red);
            gun.nextShoot(fireDirection, new Vector2(bulletSpawn.transform.position.x, bulletSpawn.transform.position.y));
            fireTimer = gun.basicAttackTime;
        }
        else if (fireTimer > 0)
        {
            fireTimer -= Time.fixedDeltaTime;
        }
    }

    void OnFire()
    {   
        //PlayerController.direction1

        //Debug.Log("fire");
        animator.SetTrigger("playerAttackTrigger");
    }

    
}
