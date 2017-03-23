using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingType
{
    private Dictionary<string, int> _resourceToValue = new Dictionary<string, int>();
    public PrimitiveType _model;
    public float _x;
    public float _y;
    public float _z;
    public float _height;

    public void setResourceValue(string tag_, int value_)
    {
        _resourceToValue.Add(tag_, value_);
    }    
}
