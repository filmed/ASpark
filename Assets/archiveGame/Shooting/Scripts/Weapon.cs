using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float basicAttackTime;
    public float basicAttackScatter;
    public float basicAttackBulletCount;
    public float reloadingTimer = 0;
    public float bulletsCount;

    public Projectile[] bullets;
    public bool[] upgradeList;
    public int current = 0;

    private void Awake()
    {
        //    bullets = new Projectile[(int)bulletsCount];
        
    }
    private void Start()
    {
        
    }

    public void nextShoot(Vector2 _firePoint, Vector2 _direction, int _source)
    {
        //Debug.Log("Source set: " +_source);

        //Projectile[] _currentProjectiles = new Projectile[(int)basicAttackBulletCount];


        for (int i = 0; i < (int)basicAttackBulletCount; i++) 
        {
            var currBullet = bullets[current];
            currBullet.ModifyDirection(_direction, basicAttackScatter);
            currBullet.Init(_firePoint, _source);


            //_currentProjectiles[i] = currBullet.Init(currBullet, _firePoint);

            //_currentProjectiles[i] = Instantiate(bullets[current], _firePoint, Quaternion.identity);
            //_currentProjectiles[i].direction = DoScatter(_direction, basicAttackScatter + _currentProjectiles[i].scatter);
            
            current = (current + 1) % bullets.Length;
        }

        //return _currentProjectiles;
    }

    public static Vector2 DoScatter(Vector2 _direction, float _scatter)
    {
        //float _directionAngle = _angle/Mathf.Abs(_angle);
        float _angle = Random.Range(-_scatter / 2, _scatter / 2);
        _angle = _angle * Mathf.PI / 180;

        return new Vector2(
            _direction.x * Mathf.Cos(_angle) - _direction.y * Mathf.Sin(_angle),
           _direction.x * Mathf.Sin(_angle) + _direction.y * Mathf.Cos(_angle)
        ).normalized;
    }
}
