using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building {

    public Building(int buildingType_, Color color_)
    {
        _buildingType = buildingType_;
        _color = color_;
    }

    public GameObject createBuilding()
    {
        GameObject model = null;
        if (_buildingType == BUILDING_HOUSE)
        {
            model = GameObject.CreatePrimitive(PrimitiveType.Cube);
            model.GetComponent<Renderer>().material.SetColor("_Color", _color);
            model.transform.localScale = new Vector3(UnityEngine.Random.Range(4.5f, 6f), 3f, UnityEngine.Random.Range(2f, 4f));
            model.transform.rotation = Quaternion.Euler(90, 0, 120);
            _height = 3f;
        }
        else if (_buildingType == BUILDING_FARM)
        {
            model = GameObject.CreatePrimitive(PrimitiveType.Cube);
            model.GetComponent<Renderer>().material.SetColor("_Color", _color);
            float size = UnityEngine.Random.Range(4f, 6f);
            model.transform.localScale = new Vector3(size, .5f, size);
            _height = .7f;
        }
        else if (_buildingType == BUILDING_MINE)
        {
            model = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            model.GetComponent<Renderer>().material.SetColor("_Color", _color);
            model.transform.localScale = new Vector3(3f, 6f, 3f);
            _height = 6f;
        }
        else if (_buildingType == BUILDING_MARKET)
        {
            model = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            model.GetComponent<Renderer>().material.SetColor("_Color", _color);
            model.transform.localScale = new Vector3(8f, 8f, 8f);
            _height = 3f;
        }
        else if (_buildingType == BUILDING_MANOR)
        {
            model = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            model.GetComponent<Renderer>().material.SetColor("_Color", _color);
            model.transform.localScale = new Vector3(7f, 7f, 7f);
            _height = 8f;
        }
        return model;
    }

    public float getHeight()
    {
        return _height;
    }

    public int getBuildingType()
    {
        return _buildingType;
    }

    public Color getColor()
    {
        return _color;
    }

    public void setColor(Color color_)
    {
        _color = color_;
    }

    public void setCivilization(Civilization civilization_)
    {
        _civilization = civilization_;
    }

    public Civilization getCivilization()
    {
        return _civilization;
    }

    public void setCell(Cell cell_)
    {
        _cell = cell_;
    }

    public Cell getCell()
    {
        return _cell;
    }

    public const int BUILDING_HOUSE = 0;
    public const int BUILDING_FARM = 1;
    public const int BUILDING_MINE = 2;
    public const int BUILDING_MARKET = 3;
    public const int BUILDING_MANOR = 4;

    private Civilization _civilization;
    private Cell _cell;
    private Color _color;

    private float _height;
    private int _buildingType;
}
