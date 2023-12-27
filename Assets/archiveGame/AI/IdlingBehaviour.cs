using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdlingBehaviour : SteeringBehaviour
{
    [SerializeField]
    private float idlingRadius;
    [SerializeField]
    private Vector2 spawnPosition;

    private ShapeFunctions shapeing;

    [SerializeField]
    private bool showGizmos;

    private void Start()
    {
        shapeing = new ShapeFunctions();
        spawnPosition = transform.position;
    }

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aidata)
    {
        if (aidata.targets != null || aidata.GetTargetsCount() > 0) 
        {
            spawnPosition = transform.position;
            return (danger, interest);
        }
        else 
        {
            /*float _angle = Random.Range(0f, 360f);
            _angle = _angle * Mathf.PI / 180;
            var _x = Mathf.Cos(_angle) * Random.Range(0f, idlingRadius);
            var _y = Mathf.Sin(_angle) * Random.Range(0f, idlingRadius);
            Vector2 resultPoint = new Vector2(_x, _y) + spawnPosition;
            Vector2 dir = (resultPoint - (Vector2)transform.position);*/


            float _dist = Vector2.Distance(transform.position, spawnPosition);



            float _distNorm = shapeing.DistanceNormilizeBetweenTwoRingsLinear(_dist, idlingRadius, 0.1f);


            Vector2 directionToTarget = (spawnPosition - (Vector2)transform.position);

            for (int i = 0; i < interest.Length; i++)
            {
                float directionsEquivalence = Vector2.Dot(directionToTarget.normalized, Directions.dirSchema[aidata.dirSchemNum][i]);

                if (directionsEquivalence >= -1)
                {
                    float result = shapeing.DampingShapeWithPoint(_distNorm, directionsEquivalence, 0.0f, idlingRadius/2);

                    if (result > interest[i])
                    {
                        interest[i] = result;
                    }
                }
            }


            /*for (int i = 0; i < interest.Length; i++)
            {
                float directionsEquivalence = Vector2.Dot(dir, Directions.dirSchema[aidata.dirSchemNum][i]);

                //float result = Mathf.Clamp01(directionsEquivalence);

                float result = shapeing.DampingShape(distNorm, directionsEquivalence, 0);

                //result = shapeing.IdleShape(spawnPosition, transform.position, idlingRadius, Directions.dirSchema[aidata.dirSchemNum][i]);
                if (result > interest[i]) 
                {
                    interest[i] = result;
                }
            }*/
        }

        return (danger, interest);

    }


    private void OnDrawGizmos()
    {
        if (showGizmos == false)
            return;
        else 
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(spawnPosition, idlingRadius);
        }
    }
}
