using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjSpike : Projectile
{
    [SerializeField]
    private float durability;

    [SerializeField]
    private float rotateSpeed;

    [SerializeField]
    private float pushDistance;

    private int rotateDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, flyingTime);

        int[] rotateDirections = { -1, 1 };
        rotateDir = rotateDirections[Random.Range(0, 2)];
       
    }

    private void FixedUpdate()
    {
        rb.velocity = direction * speed;
        transform.Rotate(Vector3.forward, Time.fixedDeltaTime * rotateSpeed * rotateDir);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("collision");



        if (collision.transform.root.TryGetComponent<Health>(out Health hittable))
        {
            if (sourceLayer != collision.gameObject.layer) 
            {
                hittable.GetHit(damage, gameObject);
                durability -= 1;
                if (durability <= 0)
                    Destroy(gameObject);
            }

        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6) //collides with obstacle
        {
            Destroy(gameObject);
        }

    }

    public override void ModifyDirection(Vector2 _direction, float basicAttackScatter)
    {
        direction = Weapon.DoScatter(_direction, scatter + basicAttackScatter);
    }

    public override void Init(Vector2 _firePoint, int _source)
    {
        sourceLayer = _source;
        Instantiate(gameObject, _firePoint, Quaternion.identity);
    }

}
