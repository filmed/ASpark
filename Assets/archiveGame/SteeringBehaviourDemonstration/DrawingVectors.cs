using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingVectors : MonoBehaviour
{
    [SerializeField]
    private bool showDrawings = false;

    [SerializeField]
    private int dirSchemNum;

    [SerializeField]
    private float vectorMaxLength = 1f;

    [SerializeField]
    private Vector2[] dots;

    //[SerializeField]
    private ContextSolver contextSolver;

    [SerializeField]
    LineRenderer[] interestLines;
    [SerializeField]
    LineRenderer[] dangerLines;
    //[SerializeField]
    LineRenderer circleLine;

    public Color interestColor;
    public Color dangerColor;
    public GameObject linePrefab;



    private void Awake()
    {
       /* ObstacleAvoidanceBehavoiur obstacleAvoidanceBehavoiur = GetComponentInChildren<ObstacleAvoidanceBehavoiur>();
        SeekBehaviour seekBehaviour = GetComponentInChildren<SeekBehaviour>();*/

        contextSolver = GetComponentInChildren<ContextSolver>();
        circleLine = GetComponent<LineRenderer>();

        interestLines = InitLinrenderers(Directions.dirSchema[dirSchemNum].Count, linePrefab, Color.green, 0.05f);
        dangerLines = InitLinrenderers(Directions.dirSchema[dirSchemNum].Count, linePrefab, Color.red, 0.05f);

        Color testColor = Color.red;
        int m = 255;
        (testColor.r, testColor.g, testColor.b, testColor.a) = (36 / m, 255 / m, 58 / m, 0.1f);
        Debug.Log(testColor);
        DrawPolygon(circleLine, 16, vectorMaxLength, transform.position, 0.03f, 0.03f, testColor);
    }

    // Update is called once per frame
    void Update()
    {
        if (contextSolver.interest_draw != null && showDrawings)
        {

            for (int i = 0; i < contextSolver.interest_draw.Length; i++)
            {
                Vector3[] currInterestLine = new Vector3[2];
                Vector3[] currDangerLine = new Vector3[2];

                currInterestLine[0] = transform.position;
                currInterestLine[1] = transform.position + (Vector3)Directions.dirSchema[dirSchemNum][i] * contextSolver.interest_draw[i] * vectorMaxLength;

                currDangerLine[0] = transform.position;
                currDangerLine[1] = transform.position + (Vector3)Directions.dirSchema[dirSchemNum][i] * contextSolver.danger_draw[i] * vectorMaxLength;

                Color currDangerColor = dangerColor, currInterestColor = interestColor;

                /*currInterestColor.a = contextSolver.interest_draw[i] * contextSolver.interest_draw[i];
                currDangerColor.a = contextSolver.danger_draw[i] * contextSolver.danger_draw[i];*/

                currInterestColor.a = Mathf.Pow(contextSolver.interest_draw[i], 3);
                currDangerColor.a = Mathf.Pow(contextSolver.danger_draw[i], 3);

                //Debug.Log(i + " " + (contextSolver.interest_draw[i]));

                interestLines[i].startColor = currInterestColor;
                interestLines[i].endColor = currInterestColor;


                dangerLines[i].startColor = currDangerColor;
                dangerLines[i].endColor = currDangerColor;


                interestLines[i].SetPositions(currInterestLine);
                dangerLines[i].SetPositions(currDangerLine);
            }
            //DrawPolygon(circleLine, 16, vectorMaxLength, transform.position, 0.1f, 0.1f, Color.green);
        }

    }

    public LineRenderer[] InitLinrenderers(int _count, GameObject _linePrefab, Color _color, float _width) 
    {
        LineRenderer[] result_lines = new LineRenderer[_count];

        for (int i = 0; i < _count; i++) 
        {
            result_lines[i] = Instantiate(_linePrefab, gameObject.transform).GetComponent<LineRenderer>();
            result_lines[i].startColor = _color;
            result_lines[i].endColor = _color;
            result_lines[i].startWidth = _width;
            result_lines[i].endWidth = _width;
        }

        return result_lines;
        
    }

    void DrawPolygon(LineRenderer lineRenderer, int vertexNumber, float radius, Vector3 centerPos, float startWidth, float endWidth, Color _color)
    {
        lineRenderer.startWidth = startWidth;
        lineRenderer.endWidth = endWidth;

        lineRenderer.startColor = _color;
        lineRenderer.endColor = _color;

        lineRenderer.loop = true;
        float angle = 2 * Mathf.PI / vertexNumber;
        lineRenderer.positionCount = vertexNumber;

        for (int i = 0; i < vertexNumber; i++)
        {
            Matrix4x4 rotationMatrix = new Matrix4x4(new Vector4(Mathf.Cos(angle * i), Mathf.Sin(angle * i), 0, 0),
                                                     new Vector4(-1 * Mathf.Sin(angle * i), Mathf.Cos(angle * i), 0, 0),
                                       new Vector4(0, 0, 1, 0),
                                       new Vector4(0, 0, 0, 1));
            Vector3 initialRelativePosition = new Vector3(0, radius, 0);
            lineRenderer.SetPosition(i, centerPos + rotationMatrix.MultiplyPoint(initialRelativePosition));

        }
    }
}
