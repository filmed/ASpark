using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObstacleAvoidanceBehavoiur : SteeringBehaviour 
{
    [SerializeField]
    private float radius = 2f, agentColliderSize = 0.16f;

    [SerializeField]
    private bool showGizmo = true;

    [SerializeField]
    private bool showDrawing = true;

    private ShapeFunctions shaping;

    [SerializeField]
    float dampingLen01, normRadius, reachRange;
    

    //for gizmo
    float[] dangerResultTemp = null;
    private int dirSchemNum;

    private void Awake()
    {
        shaping = new ShapeFunctions();
    }

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aidata) 
    {
        dirSchemNum = aidata.dirSchemNum;
        foreach (Collider2D obstacleCollider in aidata.obstacles) 
        {
            Vector2 directionToObtacle = obstacleCollider.ClosestPoint(transform.position) - (Vector2)transform.position;


            Debug.DrawRay(transform.position, directionToObtacle, Color.blue, 1.5f);
            
            float distanceToObstacle = directionToObtacle.magnitude; //returns length of the vector
            float distNorm = shaping.DistanceNormilizeBetweenTwoRingsLinear(distanceToObstacle, reachRange, normRadius);

            //float weight = distanceToObstacle <= agentColliderSize ? 1 : (radius - distanceToObstacle) / radius; //old
            float weight = shaping.DistanceNormilizeBetweenTwoRingsLinear(distanceToObstacle, agentColliderSize, radius);



            Vector2 directionToObstacleNormalized = directionToObtacle.normalized;

            for (int i = 0; i < Directions.dirSchema[aidata.dirSchemNum].Count; i++) 
            {
                float directionsEquivalence = Vector2.Dot(directionToObstacleNormalized, Directions.dirSchema[aidata.dirSchemNum][i]);

               // var w = 1.0 - Mathf.Abs(directionsEquivalence - 0.65f);

                if (directionsEquivalence > 0) 
                {

                    //float result = directionsEquivalence * weight //old

                    float result = shaping.DampingShape(distNorm, directionsEquivalence, dampingLen01);

                    result = result * weight; // weight of range to obtacle

                    //displacing algorithm

                    if (result > danger[i]) 
                    {
                        danger[i] = result;
                    }
                }

            }
        }

        dangerResultTemp = danger;

        return (danger, interest);
    }

    private void OnDrawGizmos()
    {
        if (showGizmo == false)
            return;

        if (Application.isPlaying && dangerResultTemp != null) 
        {
            Gizmos.color = Color.black;
            for (int i = 0; i < dangerResultTemp.Length; i++) 
            {
                Gizmos.DrawRay(
                    transform.position,
                   Directions.dirSchema[dirSchemNum][i] * dangerResultTemp[i]
                    );
            }
        }
        else 
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }




}

public static class Directions 
{
    

    public static List<Vector2> eightDirections = new List<Vector2> {
        new Vector2(0,1).normalized,
        new Vector2(1,1).normalized,
        new Vector2(1,0).normalized,
        new Vector2(1,-1).normalized,
        new Vector2(0,-1).normalized,
        new Vector2(-1,-1).normalized,
        new Vector2(-1,0).normalized,
        new Vector2(-1,1).normalized,
    };

    public static List<Vector2> sixteenDirections = new List<Vector2> {
        new Vector2(1,Mathf.Sqrt(2) - 1).normalized,
        new Vector2(Mathf.Sqrt(2) - 1,1).normalized,
        new Vector2(-Mathf.Sqrt(2) + 1, 1).normalized,
        new Vector2(-1, Mathf.Sqrt(2) - 1).normalized,
        new Vector2(-1,-Mathf.Sqrt(2) + 1).normalized,
        new Vector2(-Mathf.Sqrt(2) + 1,-1).normalized,
        new Vector2(Mathf.Sqrt(2) - 1,-1).normalized,
        new Vector2(1,-Mathf.Sqrt(2) + 1).normalized,

        new Vector2(0,1).normalized,
        new Vector2(1,1).normalized,
        new Vector2(1,0).normalized,
        new Vector2(1,-1).normalized,
        new Vector2(0,-1).normalized,
        new Vector2(-1,-1).normalized,
        new Vector2(-1,0).normalized,
        new Vector2(-1,1).normalized,
    };

    public static List<List<Vector2>> dirSchema = new List<List<Vector2>>
    {
        eightDirections,
        sixteenDirections,
    };
}
