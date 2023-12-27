using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SeekBehaviour : SteeringBehaviour
{
    [SerializeField]
    private bool showGizmo = true;

    bool reachedLastTarget = true;

    [SerializeField]
    float dampingLen01, normRadius, reachRange, circleRadius, circleRadiusValue01, point1Value01, point2Value01;

    private ShapeFunctions shapeing;

    //for gizmo
    private Vector2 targetPositionCached;
    private float[] interestsTemp;
    private int dirSchemNum;

    private void Awake()
    {
        shapeing = new ShapeFunctions();
    }

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aidata)
    {
        dirSchemNum = aidata.dirSchemNum;

        if (reachedLastTarget)
        {
            if (aidata.targets == null || aidata.targets.Count == 0)
            {
                aidata.currentTarget = null;
                return (danger, interest);
            }
            else
            {
                reachedLastTarget = false;
                aidata.currentTarget = aidata.targets.OrderBy(target => Vector2.Distance(target.position, transform.position)).FirstOrDefault();
            }

        }


        if (aidata.currentTarget != null && aidata.targets != null && aidata.targets.Contains(aidata.currentTarget))
            targetPositionCached = aidata.currentTarget.position;


        float _dist = Vector2.Distance(transform.position, targetPositionCached);

        //float _distNorm = Mathf.Clamp01(-((_dist - _seekRadius) / (targetReachedThreshold - _seekRadius)) + 1);
        //float _distNorm = shapeing.DistanceNormilizeBetweenTwoRingsWithPoint(_dist, normRadius, circleRadius, new Vector2((normRadius + circleRadius) / 2, 0.2f));//old ver
        if (aidata.currentTarget != null) 
        {

            //Debug.Log(aidata.targets != null && aidata.targets.Contains(aidata.currentTarget));
            //chasing target
            if (aidata.targets != null && aidata.targets.Contains(aidata.currentTarget))
            {
                if (_dist < reachRange)
                {
                    reachedLastTarget = true;
                    aidata.currentTarget = null;
                    return (danger, interest);
                }

                float _distNorm = shapeing.DistanceNormilizeWithTwoPoints(_dist, normRadius, circleRadius, circleRadiusValue01, new Vector2((normRadius + circleRadius) / 2, point1Value01), new Vector2(circleRadius * 0.9f, point2Value01));
                Vector2 directionToTarget = (targetPositionCached - (Vector2)transform.position);

                for (int i = 0; i < interest.Length; i++)
                {
                    float directionsEquivalence = Vector2.Dot(directionToTarget.normalized, Directions.dirSchema[aidata.dirSchemNum][i]);

                    if (directionsEquivalence >= -1)
                    {
                        float result = shapeing.DampingShapeWithPoint(_distNorm, directionsEquivalence, dampingLen01, circleRadiusValue01);

                        if (result > interest[i])
                        {
                            interest[i] = result;
                        }
                    }
                }


            } //chasing target step
            else if (reachedLastTarget == false)
            {
                if (_dist < reachRange)
                {
                    reachedLastTarget = true;
                    aidata.currentTarget = null;
                    return (danger, interest);
                }

                float _distNorm = shapeing.DistanceNormilizeBetweenTwoRingsLinear(_dist, normRadius, reachRange);

                Vector2 directionToTarget = (targetPositionCached - (Vector2)transform.position);


                for (int i = 0; i < interest.Length; i++)
                {
                    float directionsEquivalence = Vector2.Dot(directionToTarget.normalized, Directions.dirSchema[aidata.dirSchemNum][i]);

                    if (directionsEquivalence >= -1)
                    {
                        float result = shapeing.DampingShape(_distNorm, directionsEquivalence, 1f);

                        if (result > interest[i])
                        {
                            interest[i] = result;
                        }
                    }
                }

            }

        }
        

            
        
        
        


        //Debug.Log("Dist: " + _dist + " Norm: " + _distNorm);
        

       /* for (int i = 0; i < interest.Length; i++) 
        {
            float directionsEquivalence = Vector2.Dot(directionToTarget.normalized, Directions.dirSchema[aidata.dirSchemNum][i]);
            
            if (directionsEquivalence >= -1) //-60?  should be fixed!!!!!!!!!
            {
                //float result = shapeing.DampingShape(_distNorm, directionsEquivalence, dampingLen01); // oldShaping
                float result = shapeing.DampingShapeWithPoint(_distNorm, directionsEquivalence, dampingLen01, circleRadiusValue01);

                if (result > interest[i])
                {
                    interest[i] = result;
                }


               *//* //PROB DOESNT WORK -- should be fixed!!
                float keepDistance = (1 - _distNorm) * directionsEquivalence / 1.5f;

                if (keepDistance > danger[i])
                {
                    danger[i] = keepDistance;
                }*/

              /*  keepDistance = (1 - _distNorm) * (-directionsEquivalence) / 1.15f;

                if (keepDistance > interest[i])
                {
                    interest[i] = keepDistance;
                }*//*

            }
                

            *//*float result;                   //TO DO: create class with shapes configurations
            if (directionsEquivalence > 0) 
            {
                if (directionsEquivalence - k >= 0) 
                {
                    result = directionsEquivalence + (-_distNorm + 1) * (1+k - 2 * directionsEquivalence);
                }
                else 
                {

                    result = directionsEquivalence + (-_distNorm + 1)*(1 - k);
                }
                //float result = 1.0f - Mathf.Abs(directionsEquivalence + 0.65f); //old
                if (i == 2) 
                {
                    Debug.Log("dir: " + directionsEquivalence + "   res: " + result);
                }

            }*//*
        }*/

        interestsTemp = interest;
        return (danger, interest);
    }

    public void OnDrawGizmos()
    {
        if (showGizmo == false)
            return;

        Gizmos.DrawSphere(targetPositionCached, 0.2f);

        if (Application.isPlaying && interestsTemp != null) 
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < interestsTemp.Length; i++) 
            {
                Gizmos.DrawRay(transform.position, Directions.dirSchema[dirSchemNum][i] * interestsTemp[i]);
            }
            if (reachedLastTarget == false) 
            {
                Gizmos.color = Color.red;

                Gizmos.DrawSphere(targetPositionCached, 0.1f);
            }
        }
    }
}
