using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected Rigidbody2D rb;
    [SerializeField]
    protected Vector2 direction = Vector2.zero;
    [SerializeField]
    protected float speed = 0;
    [SerializeField]
    protected float scatter;
    [SerializeField]
    protected float launchingTime;
    [SerializeField]
    protected float flyingTime;
    [SerializeField]
    protected float damage;
    [SerializeField]
    protected int sourceLayer;
    [SerializeField]
    protected float knockBack;

    public abstract void Init(Vector2 _firePoint, int _source);
    public abstract void ModifyDirection(Vector2 _direction, float basicAttackScatter);

    public Vector2 GetDirection() { return direction; }
}
