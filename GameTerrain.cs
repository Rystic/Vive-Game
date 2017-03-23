using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameTerrain
{
    static GameTerrain()
    {
        _terrainColor[GRASS] = new Color(0f, .35f, .1f);
        _terrainColor[STONE] = new Color(.35f, .35f, .35f);
        _terrainColor[WATER] = new Color(.05f, .05f, .65f);
        _terrainColor[MARSH] = new Color(.25f, .25f, .10f);

        _buildable[GRASS] = true;
        _buildable[STONE] = true;
        _buildable[WATER] = false;
        _buildable[MARSH] = true;

    }

    public GameTerrain(int type_)
    {
        _type = type_;

        Color defaultColor = _terrainColor[_type];
        float variation = UnityEngine.Random.Range(.75f, 1.5f);
        _color = new Color(defaultColor.r * variation, defaultColor.g * variation, defaultColor.b * variation);
    }

    public Color getColor()
    {
        return _color;
    }

    public bool isBuildable()
    {
        return _buildable[_type];
    }

    public int getTerrainType()
    {
        return _type;
    }

    private static Dictionary<int, Color> _terrainColor = new Dictionary<int, Color>();
    private static Dictionary<int, bool> _buildable = new Dictionary<int, bool>();

    public const int GRASS = 1;
    public const int STONE = 2;
    public const int WATER = 3;
    public const int MARSH = 4;

    private int _type;
    private Color _color;

}
