using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSysem : MonoBehaviour
{
    
    public List<Enemy> enemies;
    [SerializeField]
    private int curWave = 1;
    [SerializeField]
    private int costMultiplier;
    //[SerializeField]
    private Transform spawnAreaTransform;
    [SerializeField]
    private float areaRadius;
    [SerializeField]
    private float timeBetweenWaves;
    private float waveTimer;

    [SerializeField] //TO DO: get form player
    private float playerVisionArea;
    [SerializeField] //TO DO: get form player
    private Transform playerTransform;

    //FOR GIZMOS
    [SerializeField]
    private bool showGizmo = false;


    private int waveValue = 0;

    private void Start()
    {
        waveTimer = 10;
        spawnAreaTransform = transform;
    }
    private void FixedUpdate()
    {

        if (waveTimer > 0) waveTimer -= Time.fixedDeltaTime;
        else 
        {
            GenerateWave();
            waveTimer = timeBetweenWaves;
            Debug.Log("Wave: " + (curWave++).ToString());
        }
        
    }


    public void GenerateWave() 
    {
        waveValue = costMultiplier * curWave;
        SpawnEnemies(waveValue);
    }

    public int SpawnEnemies(int _waveValue)
    {
        int _count = 0;
        if (playerTransform is not null) 
        {

            List<Enemy> availableToSpawn = new List<Enemy>();

            int tries = 100 * _waveValue;
            while (_waveValue > 0 && tries > 0) 
            {
                int enemyId = Random.Range(0, enemies.Count);

                if (_waveValue - enemies[enemyId].GetCost() >= 0)
                {
                    availableToSpawn.Add(enemies[enemyId]);
                    _waveValue -= enemies[enemyId].GetCost();
                }
                else tries--;

            }
        
            /* foreach (Enemy enemy in enemies) 
            {
                int _cost = enemy.GetCost();
                if (_waveValue >= _cost)
                {
                    _waveValue -= _cost;
                    _cost++;
                    availableToSpawn.Add(enemy);
                }
                else break;
            }*/

            foreach (Enemy enemy in availableToSpawn) 
            {
                Vector2 _posToSpawn;
                do
                {
                    _posToSpawn = RotateToAngle(Random.Range(0, 360), Random.Range(0, areaRadius), spawnAreaTransform.position);
                } while ((Mathf.Pow((_posToSpawn.x - playerTransform.position.x), 2) + Mathf.Pow((_posToSpawn.y - playerTransform.position.y), 2)) <= Mathf.Pow(playerVisionArea, 2));

                Instantiate(enemy.GetEnemyObject(), _posToSpawn, Quaternion.identity);
            }

        }

        return _count;
    }

    public int GetCurrentWave() { return curWave;}

    public Vector2 RotateToAngle(float _angle, float _range, Vector2 _center)
    {
        _angle = _angle * Mathf.PI / 180;
        var _x = Mathf.Cos(_angle) * _range;
        var _y = Mathf.Sin(_angle) * _range;
        return (new Vector2(_x, _y) + _center);
    }




    public void OnDrawGizmos()
    {
        if (showGizmo == false || !Application.isPlaying)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(spawnAreaTransform.position, areaRadius);


    }



}
[System.Serializable]
public class Enemy 
{
    [SerializeField]
    private GameObject enemyObject;
    [SerializeField]
    private int cost;

    public int GetCost() { return cost; }
    public GameObject GetEnemyObject() { return enemyObject; }


}



