using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    public Vector2 mousePos { get; set; }
    private Vector2 mouseDir, weaponDir, velocity;
    private Transform playerTransform;
    public Animator animator;
    public Weapon weapon;
    private int source;
    private GameObject obj;

    [SerializeField]
    private float targetMinOrbit, targetMaxOrbit, glidingRange, smoothTime, targetSpeed;

    private void Awake()
    {
        playerTransform = gameObject.transform.root.GetComponent<Transform>(); //didnt use getcomponentinpatent cuz it calls first time the same gameobject!!!! 
        obj = gameObject.transform.root.Find("HitBox").gameObject;

        source = obj.layer;
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        mouseDir = (mousePos - new Vector2(playerTransform.position.x, playerTransform.position.y)).normalized;
        weaponDir = (new Vector2(transform.position.x, transform.position.y) - new Vector2(playerTransform.position.x, playerTransform.position.y)).normalized;
        
        float angleToRotate = Vector2.SignedAngle(Vector2.right, mouseDir);

        //RotateToAngle(180, targetMaxOrbit, playerTransform.position);

        RotateToAngle(angleToRotate, targetMaxOrbit, playerTransform.position);


        transform.localPosition = Vector2.SmoothDamp(transform.localPosition, mouseDir * (targetMaxOrbit + glidingRange), ref velocity, smoothTime, targetSpeed);


        if (weapon != null) 
        {
            if (weapon.reloadingTimer > 0) weapon.reloadingTimer -= Time.fixedDeltaTime;
        }
    }

    public void RotateToAngle(float _angle, float _radius, Vector2 _center)
    {
        _angle = _angle * Mathf.PI / 180;
        var _x = Mathf.Cos(_angle) * _radius;
        var _y = Mathf.Sin(_angle) * _radius;
        transform.position = new Vector2(_x, _y) + _center;
    }

    public void Attack() 
    {
        if (weapon.reloadingTimer <= 0) 
        {
            weapon.nextShoot(transform.position, weaponDir, source);
            weapon.reloadingTimer = weapon.basicAttackTime;
        }
    }
}
