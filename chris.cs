using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class chris : MonoBehaviour
{
    private static Civilization _civ1;
    private static Civilization _civ2;
    private static Civilization _civ3;
    private static Civilization _civ4;
    private static Civilization _civ5;
    private static Civilization _civ6;
    private static Civilization _civ7;
    private static Civilization _civ8;
    private  bool started;

    void Awake()
    {
        

        started = false;
        if (_mainBoard == null)
        {
            string[] lines = System.IO.File.ReadAllLines("assets/Vive-Game/buildings.txt");
            BuildingTypeFactory.parseBuildingData(lines);
            started = true;
            _mainBoard = new Board(64);
            TerrainGenerator.generateTerrain(_mainBoard);
            _civ1 = new Civilization(_mainBoard, Color.red, "Peoples' Red-public");
            _civ2 = new Civilization(_mainBoard, Color.blue, "Imperial Blue");
            _civ3 = new Civilization(_mainBoard, Color.yellow, "Banana Kingdom");
            _civ4 = new Civilization(_mainBoard, Color.green, "Green Barony");
            _civ5 = new Civilization(_mainBoard, new Color(.65f, .0f, .85f), "Royal Purple");
            _civ6 = new Civilization(_mainBoard, Color.black, "Black Hole");
            _civ7 = new Civilization(_mainBoard, Color.white, "White Wind");
            _civ8 = new Civilization(_mainBoard, Color.cyan, "Cyan Dynasty");
        }
    }

    int cycle = 0;

    void Update()
    {
        if (!started)
            return;
        started = true;
        cycle++;
        if (cycle >= 20)
        {
            _civ1.calculateAction();
            _civ2.calculateAction();
            _civ3.calculateAction();
            _civ4.calculateAction();
            _civ5.calculateAction();
            _civ6.calculateAction();
            _civ7.calculateAction();
            _civ8.calculateAction();
            cycle = 0;
        }
    }

    private static Board _mainBoard;
}
