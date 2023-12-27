using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacles : MonoBehaviour
{
    [SerializeField]
    private List<ObstacleObj> obstacles;

    // Start is called before the first frame update
    void Start()
    {
        for (float i = 90; i < 100; i+=1f) 
        {
            midPointCircleDraw(0, 0, i, 0.32f, obstacles[0].GetObstacleObj(), false, transform);
            AngledmidPointCircleDraw(0, 0, i, 0.32f, obstacles[0].GetObstacleObj(), transform);

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static void midPointCircleDraw(float x_centre, float y_centre, float r, float scl, GameObject obstacleObj, bool _shape, Transform _parent) 
    {
        
        float x = scl*r, y = 0, P;

        // Printing the initial point on the
        // axes after translation

        Instantiate(obstacleObj, new Vector2((x + x_centre), (y + y_centre)), Quaternion.identity).transform.parent = _parent;
        
     
        // When radius is zero only a single
        // point will be printed
        if (r > 0)
        {
            Instantiate(obstacleObj, new Vector2((x + x_centre), (-y + y_centre)), Quaternion.identity).transform.parent = _parent;
            
            Instantiate(obstacleObj, new Vector2((y + x_centre), (x + y_centre)), Quaternion.identity).transform.parent = _parent;
            
            Instantiate(obstacleObj, new Vector2((-y + x_centre), (x + y_centre)), Quaternion.identity).transform.parent = _parent;


            //Debug.Log("X: " + x + " Y: " + y);
            Instantiate(obstacleObj, new Vector2((-x + x_centre), (y + y_centre)), Quaternion.identity).transform.parent = _parent;
            Instantiate(obstacleObj, new Vector2((y + x_centre), (-x + y_centre)), Quaternion.identity).transform.parent = _parent;

        }

        // Initialising the value of P
        if (_shape)
            P = scl - r * scl;
        else 
            P = 1 - r;
        while (x > y)
        {
             
            y+=scl;
         
            // Mid-point is inside or on the perimeter
            if (P <= 0)
                P = P + 2 * y + 1; //??
         
            // Mid-point is outside the perimeter
            else
            {
                x-=scl;
                P = P + 2 * y - 2 * x + 1;
            }
         
            // All the perimeter points have already 
            // been printed
            if (x < y)
                break;

            // Printing the generated point and its 
            // reflection in the other octants after
            // translation

            Instantiate(obstacleObj, new Vector2((x + x_centre), (y + y_centre)), Quaternion.identity).transform.parent = _parent;
           
            Instantiate(obstacleObj, new Vector2((-x + x_centre), (y + y_centre)), Quaternion.identity).transform.parent = _parent;
            
            Instantiate(obstacleObj, new Vector2((x + x_centre), (-y + y_centre)), Quaternion.identity).transform.parent = _parent;
           
            Instantiate(obstacleObj, new Vector2((-x + x_centre), (-y + y_centre)), Quaternion.identity).transform.parent = _parent;
                
         
            // If the generated point is on the 
            // line x = y then the perimeter points
            // have already been printed
            if (x != y) 
            {
                Instantiate(obstacleObj, new Vector2((y + x_centre), (x + y_centre)), Quaternion.identity).transform.parent = _parent;

                Instantiate(obstacleObj, new Vector2((-y + x_centre), (x + y_centre)), Quaternion.identity).transform.parent = _parent;
              
                Instantiate(obstacleObj, new Vector2((y + x_centre), (-x + y_centre)), Quaternion.identity).transform.parent = _parent;
       
                Instantiate(obstacleObj, new Vector2((-y + x_centre), (-x + y_centre)), Quaternion.identity).transform.parent = _parent;

            }
        }

        //Debug.Log("X: " + x + " Y: " + y);

    }



    
    static void AngledmidPointCircleDraw(float x_centre, float y_centre, float r, float scl, GameObject obstacleObj, Transform _parent)
    {

        float x = scl * r, y = 0;

        // Printing the initial point on the
        // axes after translation

        Instantiate(obstacleObj, new Vector2((x + x_centre), (y + y_centre)), Quaternion.identity).transform.parent = _parent;


        // When radius is zero only a single
        // point will be printed
        if (r > 0)
        {
            Instantiate(obstacleObj, new Vector2((x + x_centre), (-y + y_centre)), Quaternion.identity).transform.parent = _parent;

            Instantiate(obstacleObj, new Vector2((y + x_centre), (x + y_centre)), Quaternion.identity).transform.parent = _parent;

            Instantiate(obstacleObj, new Vector2((-y + x_centre), (x + y_centre)), Quaternion.identity).transform.parent = _parent;


            //Debug.Log("X: " + x + " Y: " + y);
            Instantiate(obstacleObj, new Vector2((-x + x_centre), (y + y_centre)), Quaternion.identity).transform.parent = _parent;
            Instantiate(obstacleObj, new Vector2((y + x_centre), (-x + y_centre)), Quaternion.identity).transform.parent = _parent;

        }

        // Initialising the value of P
        float P = 1 * scl - r * scl; //?
        while (x > y)
        {

            y += scl;

            // Mid-point is inside or on the perimeter
            if (P <= 0)
                P = P + 2 * scl * y + 1; //??

            // Mid-point is outside the perimeter
            else
            {
                x -= scl;
                P = P + 2 * scl * y - 2 * scl * x + 1;
            }

            // All the perimeter points have already 
            // been printed
            if (x < y)
                break;

            // Printing the generated point and its 
            // reflection in the other octants after
            // translation

            Instantiate(obstacleObj, new Vector2((x + x_centre), (y + y_centre)), Quaternion.identity).transform.parent = _parent;

            Instantiate(obstacleObj, new Vector2((-x + x_centre), (y + y_centre)), Quaternion.identity).transform.parent = _parent;

            Instantiate(obstacleObj, new Vector2((x + x_centre), (-y + y_centre)), Quaternion.identity).transform.parent = _parent;

            Instantiate(obstacleObj, new Vector2((-x + x_centre), (-y + y_centre)), Quaternion.identity).transform.parent = _parent;


            // If the generated point is on the 
            // line x = y then the perimeter points
            // have already been printed
            if (x != y)
            {
                Instantiate(obstacleObj, new Vector2((y + x_centre), (x + y_centre)), Quaternion.identity).transform.parent = _parent;

                Instantiate(obstacleObj, new Vector2((-y + x_centre), (x + y_centre)), Quaternion.identity).transform.parent = _parent;

                Instantiate(obstacleObj, new Vector2((y + x_centre), (-x + y_centre)), Quaternion.identity).transform.parent = _parent;

                Instantiate(obstacleObj, new Vector2((-y + x_centre), (-x + y_centre)), Quaternion.identity).transform.parent = _parent;

            }
        }

        //Debug.Log("X: " + x + " Y: " + y);
    }







}


[System.Serializable]
class ObstacleObj
{
    [SerializeField]
    private GameObject obstaclesObjects;

    public GameObject GetObstacleObj() { return obstaclesObjects; }
}
