using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Civilization {

    public Civilization(Board mainBoard_, Color color_, string name_)
    {
        _mainBoard = mainBoard_;
        _color = color_;
        _scale = 3f;
        _complete = false;
        _wealth = 0;
        _name = name_;

        _buildingList[Building.BUILDING_FARM] = new ArrayList();
        _buildingList[Building.BUILDING_HOUSE] = new ArrayList();
        _buildingList[Building.BUILDING_MINE] = new ArrayList();
        _buildingList[Building.BUILDING_MARKET] = new ArrayList();
        _buildingList[Building.BUILDING_MANOR] = new ArrayList();

        int boardSize = mainBoard_.getBoardSize();
        for (int i = 0; i < 5; i++)
        {
            while (true)
            {
                int x = UnityEngine.Random.Range(0, boardSize);
                int y = UnityEngine.Random.Range(0, boardSize);
                if (_mainBoard.getCell(x, y).isBuildable())
                {
                    Building house = new Building(Building.BUILDING_HOUSE, _color);
                    _buildingList[house.getBuildingType()].Add(house);
                    _mainBoard.getCell(x, y).addBuilding(house);
                    break;
                }
            }
        }

        _consumed = 1;
        _produced = 0;
    }

    public void calculateAction()
    {
        if (_complete)
            return;

        Cell selectedCell = null;

        selectedCell = searchAdjacentTilesByDifferentCiv(Building.BUILDING_HOUSE);
        if (selectedCell != null)
        {
            Building oldBuilding = selectedCell.getBuilding();
            Color nc = oldBuilding.getColor();
            _buildingList[oldBuilding.getBuildingType()].Remove(oldBuilding);
            addBuilding(selectedCell, Building.BUILDING_MARKET, nc);
            _consumed--;
            _produced+=12;
            _wealth+=2;
            return;
        }

        selectedCell = searchAdjacentTilesBySurrounded(Building.BUILDING_HOUSE, Building.BUILDING_HOUSE, 4);
        if (selectedCell != null)
        {
            Building oldBuilding = selectedCell.getBuilding();
            Color nc = oldBuilding.getColor();
            _buildingList[oldBuilding.getBuildingType()].Remove(oldBuilding);
            addBuilding(selectedCell, Building.BUILDING_MANOR, nc);
            _wealth += 8;
            return;
        }

        if (_consumed > _produced)
        {
            selectedCell = searchAdjacentTilesByType(Building.BUILDING_FARM, GameTerrain.MARSH);
            if (selectedCell == null)
                selectedCell = searchAdjacentTilesByType(Building.BUILDING_HOUSE, GameTerrain.MARSH);
            if (selectedCell == null)
                selectedCell = searchAdjacentTilesByHeight(Building.BUILDING_FARM, false);
            if (selectedCell == null)
                selectedCell = searchAdjacentTilesByHeight(Building.BUILDING_HOUSE, false);
            if (selectedCell != null)
            {
                addBuilding(selectedCell, Building.BUILDING_FARM);
                _produced += 2;
                if (selectedCell.getTerrainType() == GameTerrain.MARSH)
                {
                    _produced += 4;
                }
                return;
            }
        }
        else
        {
            selectedCell = searchAdjacentTilesByType(Building.BUILDING_MINE, GameTerrain.STONE);
            if (selectedCell == null)
                selectedCell = searchAdjacentTilesByType(Building.BUILDING_HOUSE, GameTerrain.STONE);
            if (selectedCell != null)
            {
                addBuilding(selectedCell, Building.BUILDING_MINE);
                _consumed += 3;
                _scale += .2f;
                return;
            }

            selectedCell = searchAdjacentTilesByHeight(Building.BUILDING_FARM, true);
            if (selectedCell == null)
                selectedCell = searchAdjacentTilesByHeight(Building.BUILDING_HOUSE, true);
            if (selectedCell != null)
            {
                addBuilding(selectedCell, Building.BUILDING_HOUSE);
                _consumed++;
                return;
            }        
        }

        selectedCell = searchAdjacentTilesByDiversity(Building.BUILDING_FARM);
        if (selectedCell != null)
        {
            Building oldBuilding = selectedCell.getBuilding();
            _buildingList[oldBuilding.getBuildingType()].Remove(oldBuilding);
            addBuilding(selectedCell, Building.BUILDING_MARKET);
            _wealth += 1;
            _scale += .1f;
            return;
        }

        Debug.Log(_name + " has completed with a wealth of " + _wealth + ", a population of " + _buildingList[Building.BUILDING_HOUSE].Count + ", and a climbing distance of " + _scale + ".");
        _complete = true;
    }

    private void addBuilding(Cell cell_, int buildingType_)
    {
        addBuilding(cell_, buildingType_, _color);
    }

    private void addBuilding(Cell cell_, int buildingType_, Color _nc)
    {
        Building building = new Building(buildingType_, _nc);
        building.setCivilization(this);
        _buildingList[buildingType_].Add(building);
        cell_.addBuilding(building);
    }

    private Cell searchAdjacentTilesByHeight(int buildingType_, bool higherTile_)
    {
        Cell selectedCell = null;
        ArrayList buildingList = _buildingList[buildingType_];
        foreach (Building building in buildingList)
        {
            Cell currCell = building.getCell();
            foreach (Cell adjCell in currCell.getNeighbors())
            {
                if (adjCell.isBuildable())
                {
                    if (Mathf.Abs(adjCell.getHeight() - currCell.getHeight()) <= _scale)
                    {
                        if (selectedCell == null)
                        {
                            selectedCell = adjCell;
                            continue;
                        }
                        if (higherTile_ ? selectedCell.getHeight() < adjCell.getHeight() : selectedCell.getHeight() > adjCell.getHeight())
                            selectedCell = adjCell;
                    }
                }
            }
        }
        return selectedCell;
    }

    private Cell searchAdjacentTilesByType(int buildingType_, int terrainType_)
    {
        ArrayList buildingList = _buildingList[buildingType_];
        foreach (Building building in buildingList)
        {
            Cell currCell = building.getCell();
            foreach (Cell adjCell in currCell.getNeighbors())
            {
                if (adjCell.isBuildable())
                {
                    if (Mathf.Abs(adjCell.getHeight() - currCell.getHeight()) <= _scale)
                    {
                        if (adjCell.getTerrainType() == terrainType_)
                            return adjCell;
                    }
                }
            }
        }
        return null;
    }

    private Cell searchAdjacentTilesByDifferentCiv(int buildingType_)
    {
        ArrayList buildingList = _buildingList[buildingType_];
        foreach (Building building in buildingList)
        {
            Cell currCell = building.getCell();
            foreach (Cell adjCell in currCell.getNeighbors())
            {
                if (adjCell.getCivilization() != null && adjCell.getCivilization() != this)
                {
                    float r = (adjCell.getBuilding().getColor().r + _color.r) / 2;
                    float g = (adjCell.getBuilding().getColor().g + _color.g) / 2;
                    float b = (adjCell.getBuilding().getColor().b + _color.b) / 2;
                    currCell.getBuilding().setColor(new Color(r, g, b));
                    return currCell;
                }
            }
        }
        return null;
    }

    private Cell searchAdjacentTilesByDiversity(int buildingType_)
    {
        ArrayList buildingList = _buildingList[buildingType_];
        foreach (Building building in buildingList)
        {
            Cell currCell = building.getCell();
            HashSet<int> div = new HashSet<int>();
            foreach (Cell adjCell in currCell.getNeighbors())
            {
                if (adjCell.hasBuilding())
                    div.Add(adjCell.getBuilding().getBuildingType());
            }
            if (div.Count >= 3)
                return currCell;
        }
        return null;
    }

    private Cell searchAdjacentTilesBySurrounded(int buildingType_, int surroundType_, int surroundTotal_)
    {
        ArrayList buildingList = _buildingList[buildingType_];
        foreach (Building building in buildingList)
        {
            Cell currCell = building.getCell();
            int surroundCount = 0;
            foreach (Cell adjCell in currCell.getNeighbors())
            {
                if (adjCell.hasBuilding())
                {
                    if (adjCell.getBuilding().getBuildingType() == surroundType_)
                        surroundCount++;
                }
            }
            if (surroundCount >= surroundTotal_)
                return currCell;
        }
        return null;
    }

    private Dictionary<int, ArrayList> _buildingList = new Dictionary<int, ArrayList>();

    private string _name;

    private int _consumed;
    private int _produced;
    private float _scale;
    private int _wealth;

    bool _complete;

    private Board _mainBoard;
    private Color _color;
}
