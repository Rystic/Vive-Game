using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class BuildingTypeFactory
{
    private static Dictionary<string, int> BUILDING_NAME_TO_ID = new Dictionary<string, int>();
    private static Dictionary<int, BuildingType> BUILDING_ID_TO_OBJECT = new Dictionary<int, BuildingType>();

    private static Dictionary<string, Iterateable> BUILDING_COMMANDS = new Dictionary<string, Iterateable>();
    private static Dictionary<string, PrimitiveType> BUILDING_MODELS = new Dictionary<string, PrimitiveType>();

    private delegate void Iterateable(string[] params_, BuildingType buildingType_);

    private static int nextAvailableId = 0;

    static BuildingTypeFactory()
    {
        BUILDING_COMMANDS.Add("BUILDING", createNewBuildingId);
        BUILDING_COMMANDS.Add("VALUE", addResourceValue);
        BUILDING_COMMANDS.Add("SCALEX", setXScale);
        BUILDING_COMMANDS.Add("SCALEY", setYScale);
        BUILDING_COMMANDS.Add("SCALEZ", setZScale);
        BUILDING_COMMANDS.Add("HEIGHT", setHeight);
        BUILDING_COMMANDS.Add("MODEL", setModel);

        BUILDING_MODELS.Add("CUBE", PrimitiveType.Cube);
        BUILDING_MODELS.Add("CYLINDER", PrimitiveType.Cylinder);
        BUILDING_MODELS.Add("SPHERE", PrimitiveType.Sphere);
        BUILDING_MODELS.Add("CAPSULE", PrimitiveType.Capsule);
    }

    public static void parseBuildingData(string[] lines_)
    {
        BuildingType buildingType = new BuildingType();
        foreach (string line in lines_)
        {
            if (line.Length == 0)
            {
                buildingType = new BuildingType();
                continue;
            }
            string[] lineInfo = line.Split('#');
            BUILDING_COMMANDS[lineInfo[0]](lineInfo[1].Split(','), buildingType);
        }
        Debug.Log("x: " + buildingType._x);
        Debug.Log("y: " + buildingType._y);
        Debug.Log("z: " + buildingType._z);
        Debug.Log("height: " + buildingType._height);
    }

    private static void createNewBuildingId(string[] lineInfo_, BuildingType buildingType_)
    {
        BUILDING_NAME_TO_ID.Add(lineInfo_[0], nextAvailableId);
        BUILDING_ID_TO_OBJECT.Add(nextAvailableId, buildingType_);
        nextAvailableId++;
    }

    private static void addResourceValue(string[] lineInfo_, BuildingType buildingType_)
    {
        buildingType_.setResourceValue(lineInfo_[0], int.Parse(lineInfo_[1]));
    }

    private static void setModel(string[] lineInfo_, BuildingType buildingType_)
    {
        buildingType_._model = BUILDING_MODELS[lineInfo_[0]];
    }

    private static void setXScale(string[] lineInfo_, BuildingType buildingType_)
    {
        buildingType_._x = getScaleValue(lineInfo_);
    }

    private static void setYScale(string[] lineInfo_, BuildingType buildingType_)
    {
        buildingType_._y = getScaleValue(lineInfo_);
    }

    private static void setZScale(string[] lineInfo_, BuildingType buildingType_)
    {
        buildingType_._z = getScaleValue(lineInfo_);
    }

    private static void setHeight(string[] lineInfo_, BuildingType buildingType_)
    {
        buildingType_._height = getScaleValue(lineInfo_);
    }

    private static float getScaleValue(string[] lineInfo_)
    {
        if (lineInfo_[0].Equals("SET"))
        {
            return float.Parse(lineInfo_[1]);
        }
        else if (lineInfo_[0].Equals("RANDOM"))
        {
            return UnityEngine.Random.Range(float.Parse(lineInfo_[1]), float.Parse(lineInfo_[2]));
        }
        return 0.0f;
    }
}