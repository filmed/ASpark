using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeFunctions : MonoBehaviour
{
    public float DampingShape(float _distNorm01, float _directionsEquivalence, float _dampingLen01) 
    {
        float result;
        if (_directionsEquivalence - _dampingLen01 >= 0)
        {
            result = _directionsEquivalence + (-_distNorm01 + 1) * (1 + _dampingLen01 - 2 * _directionsEquivalence);
        }
        else
        {

            result = _directionsEquivalence + (-_distNorm01 + 1) * (1 - _dampingLen01);
        }
        return result;
    }

    public float DampingShapeWithPoint(float _distNorm01, float _directionsEquivalence, float _dampingLen01, float _keepValue01)
    {
        float result;

        if (_keepValue01 == 0 || _keepValue01 == 1)
        {
            return -1;
        }

        if (_distNorm01 >= _keepValue01)
        {
            if (_directionsEquivalence - _dampingLen01 >= 0) 
            {
                result = _directionsEquivalence + (_distNorm01 - 1) / (_keepValue01 - 1) * (-2 * _directionsEquivalence + 1 + _dampingLen01);
            }
            else 
            {
                result = _directionsEquivalence + (_distNorm01 - 1) / (_keepValue01 - 1) * (1 - _dampingLen01);
            }
        }
        else
        {
            if (_directionsEquivalence - _dampingLen01 >= 0)
            {
                result = _directionsEquivalence + (_distNorm01 - _keepValue01) / (-_keepValue01) * (- 1 - _dampingLen01) - 2 * _directionsEquivalence + 1 + _dampingLen01;
            }
            else
            {
                result = _directionsEquivalence + (_distNorm01 - _keepValue01) / (-_keepValue01) * (-2*_directionsEquivalence - 1 + _dampingLen01) + 1 - _dampingLen01;
            }

        }
        return result;
    }

    //Point should be between rings for X and between 0 and 1 for Y else returns -1
    public float DistanceNormilizeBetweenTwoRingsWithPoint(float _distance, float _bigRing, float _smallRing, Vector2 _point) 
    {
        float result;

        if ((_point.y >= 0) &&( _point.y <= 1)) 
        {
            if (_distance >= _point.x) 
            {
                result = Mathf.Clamp01(((_distance - _bigRing) / (_point.x - _bigRing))*(_point.y - 1)  + 1);
            }
            else 
            {
                result = Mathf.Clamp01(((_distance - _point.x) / (_smallRing - _point.x)) * (-_point.y) + _point.y);
            }
            
            return result;
        }
        else 
        {
            return -1;
        }

        
    }

    //Points should be between rings for X and between 0 and 1 for Y else returns -1
    public float DistanceNormilizeWithTwoPoints(float _distance, float _bigRing, float _smallRing, float _smallRingValue01, Vector2 _point1, Vector2 _point2)
    {
        float result;

        if ((_point2.y >= 0) && (_point2.y < _smallRingValue01) && (_point1.y >= _smallRingValue01) && (_point1.y <= 1))
        {
            if (_distance >= _smallRing) 
            {
                if (_distance >= _point1.x) 
                {
                    result = Mathf.Clamp01(((_distance - _bigRing) / (_point1.x - _bigRing)) * (_point1.y - 1) + 1);
                }
                else 
                {
                    result = Mathf.Clamp01(((_distance - _point1.x) / (_smallRing - _point1.x)) * (_smallRingValue01 - _point1.y) + _point1.y);
                }
            }
            else 
            {
                if (_distance >= _point2.x) 
                {
                    result = Mathf.Clamp01(((_distance - _smallRing) / (_point2.x - _smallRing)) * (_point2.y - _smallRingValue01) + _smallRingValue01);
                }
                else 
                {
                    result = Mathf.Clamp01(((_distance - _point2.x) / (0 - _point2.x)) * (0 - _point2.y) + _point2.y);
                }
            }

            return result;
        }
        else
        {
            return -1;
        }


    }

    public float DistanceNormilizeBetweenTwoRingsLinear(float _distance, float _bigRing, float _smallRing)
    {
        float result;

        result = Mathf.Clamp01(-((_distance - _bigRing) / (_smallRing - _bigRing)) + 1);

        return result;
    }

    public float IdleShape(Vector2 _center, Vector2 _position, float _radius, Vector2 _dir) 
    {
        float result;
        Vector2 _currentDir = (_position - _center);
        float _dist = _currentDir.magnitude;
        _currentDir = _currentDir.normalized;
        float dot = Vector2.Dot(_dir, _currentDir);

        if (dot >= 0) 
        {
            result = Mathf.Clamp01((_dist - _radius) / (-_radius));
        }
        else 
        {
            result = Mathf.Clamp01(-(_dist - _radius) / (-_radius) + 1);
        }
        return result;
    }

   

}
