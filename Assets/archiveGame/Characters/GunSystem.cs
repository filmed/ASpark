using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSystem : MonoBehaviour
{
    // Start is called before the first frame update

    public float basicAttackTime;
    public float basicAttackScatter;
    public float basicAttackBulletCount;

    public List<GameObject> bullets;
    public int current = 0;

    //public List<Bullet> bullets1;

    public GunSystem(float _basicAttackTime, float _basicAttackScatter, float _basicAttackBulletCount, List<GameObject> _bullets) 
    {
        basicAttackTime = _basicAttackTime;
        basicAttackScatter = _basicAttackScatter;
        basicAttackBulletCount = _basicAttackBulletCount;
        bullets = _bullets;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void nextShoot(Vector2 _direction, Vector2 _firePoint) 
    {
        Rigidbody2D[] currShotRB = new Rigidbody2D[(int)basicAttackBulletCount];
        Bullet[] currShotBullet = new Bullet[(int)basicAttackBulletCount];

        for (int i = 0; i < basicAttackBulletCount; i++) 
        {
            GameObject bullet = Instantiate(bullets[current], _firePoint, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            Bullet b = bullet.GetComponent<Bullet>();

            Destroy(bullet, b.basicFlyingTime);

            
            currShotRB[i] = rb;
            currShotBullet[i] = b;
            current = (current + 1) % bullets.Count;
        }
        for (int i = 0; i < basicAttackBulletCount; i++) 
        {
            currShotRB[i].AddForce(doScatter(_direction) *  currShotBullet[i].basicBulletSpeed, ForceMode2D.Impulse);
        }








    }
    public Vector2 doScatter(Vector2 _direction)
    {
        //float _directionAngle = _angle/Mathf.Abs(_angle);
        float _angle = Random.Range(-basicAttackScatter / 2, basicAttackScatter / 2); 
        _angle = _angle * Mathf.PI / 180;

        return new Vector2(
            _direction.x * Mathf.Cos(_angle) - _direction.y * Mathf.Sin(_angle),
           _direction.x * Mathf.Sin(_angle) + _direction.y * Mathf.Cos(_angle)
        ).normalized;
    }
}
