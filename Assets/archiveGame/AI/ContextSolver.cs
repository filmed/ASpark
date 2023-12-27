using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextSolver : MonoBehaviour //TO DO: менять вектор движения с наименьшей потерей веса и наименьшим изменением угла(dot)
{
    [SerializeField]
    private bool showGizmos = true;

    //for gizmo
    float[] interestGizmo = null;
    Vector2 resultDirection = Vector2.zero;
    Vector2 maxDirection = Vector2.zero;
    Vector2 prevDir = Vector2.one;
    int prevDirNumber = 0;

    private float rayLength = 1;



    public float[] danger_draw  { get; set; }
    public float[] interest_draw { get; set; }


    private void Start()
    {
        //interestGizmo = new float[8];
        //danger_draw = new float[8];
        //interest_draw = new float[8];
    }

    public Vector2 GetDirectionToMove(List<SteeringBehaviour> behaviours, AIData aidata) 
    {
        float[] interest = new float[Directions.dirSchema[aidata.dirSchemNum].Count];
        float[] danger = new float[Directions.dirSchema[aidata.dirSchemNum].Count];
        
        foreach (SteeringBehaviour behaviour in behaviours) 
        {
            (danger, interest) = behaviour.GetSteering(danger, interest, aidata);

            danger_draw = danger;
            interest_draw = interest;
        }

        for (int i = 0; i < Directions.dirSchema[aidata.dirSchemNum].Count; i++) 
        {
            interest[i] = Mathf.Clamp01(interest[i] - danger[i]);
        }

        interestGizmo = interest;

        //get the avg direction
        Vector2 outputDirection = Vector2.zero;
        Vector2 maximumDirection = Vector2.zero;
        int pNum = 0; ;
        for (int i = 0; i < Directions.dirSchema[aidata.dirSchemNum].Count; i++) 
        {
            //maximumDirection = maximumDirection.magnitude > (Directions.eightDirections[i] * interest[i]).magnitude ? Directions.eightDirections[i] * interest[i] : maximumDirection;

            if (interest[i] > maximumDirection.magnitude) 
            {
                maximumDirection = interest[i] * Directions.dirSchema[aidata.dirSchemNum][i];
                pNum = i;
            }
           
            outputDirection += Directions.dirSchema[aidata.dirSchemNum][i] * interest[i];
        }

        //resultDirection =  outputDirection.normalized; should be returned?
        
        if ((maximumDirection.magnitude - interest[prevDirNumber] <= 0.2f) && (Vector2.Dot(maximumDirection.normalized, prevDir.normalized) < 0.5f)) 
        {
            resultDirection = prevDir.normalized;
        }
        else 
        {
            resultDirection = maximumDirection.normalized;
            prevDir = resultDirection;
            prevDirNumber = pNum;
        }
        
        maxDirection = maximumDirection.normalized;

        

        return resultDirection;
        //return maxDirection;
    }

    private void OnDrawGizmos()
    {
        if (showGizmos == false)
            return;
        if (Application.isPlaying && interestGizmo != null) 
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, resultDirection * 2.5f*rayLength);
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, maxDirection * 1.5f*rayLength);
        }
    }
}
