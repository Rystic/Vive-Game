using UnityEngine;
using System.Collections;

public class Cell {

    public Cell(float size_)
    {
        _cube = new GameObject[3];
        _cube[0] = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _cube[1] = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _cube[2] = GameObject.CreatePrimitive(PrimitiveType.Cube);

        _cube[0].transform.rotation = Quaternion.Euler(90, 0, 0);
        _cube[1].transform.rotation = Quaternion.Euler(90, 0, 60);
        _cube[2].transform.rotation = Quaternion.Euler(90, 0, 120);

        _neighbors = new ArrayList();
        setSize(size_);
    }

        public bool addNeighbor(Cell cell_)
        {
            if (cell_ == null)
                return false;
            if (!_neighbors.Contains(cell_))
            {
                _neighbors.Add(cell_);
                return true;
            }
            return false;
        }

        public void setSize(float size_)
        {
            _cube[0].transform.localScale = new Vector3(size_ * 0.43f, size_ * 0.7525f, _height);
            _cube[1].transform.localScale = new Vector3(size_ * 0.43f, size_ * 0.7525f, _height);
            _cube[2].transform.localScale = new Vector3(size_ * 0.43f, size_ * 0.7525f, _height);
        }

        public void setPosition(float x_, float y_, float z_)
        {
            _x = x_;
            _z = z_;
            _y = y_;
            _cube[0].transform.position = new Vector3(x_, y_ + _height / 2, z_);
            _cube[1].transform.position = new Vector3(x_, y_ + _height / 2, z_);
            _cube[2].transform.position = new Vector3(x_, y_ + _height / 2, z_);
        }

        public void setHeight(float height_)
        {
            _height = height_;
            _cube[0].transform.localScale = new Vector3(_cube[0].transform.localScale.x, _cube[0].transform.localScale.y, _height);
            _cube[1].transform.localScale = new Vector3(_cube[1].transform.localScale.x, _cube[1].transform.localScale.y, _height);
            _cube[2].transform.localScale = new Vector3(_cube[2].transform.localScale.x, _cube[2].transform.localScale.y, _height);

            _y = Board.DIST_TO_GROUND + _height / 2;
            _cube[0].transform.position = new Vector3(_x, _y, _z);
            _cube[1].transform.position = new Vector3(_x, _y, _z);
            _cube[2].transform.position = new Vector3(_x, _y, _z);
    }


        public void setTerrain(GameTerrain terrain_)
        {
            _terrain = terrain_;
            _cube[0].GetComponent<Renderer>().material.SetColor("_Color", terrain_.getColor());
            _cube[1].GetComponent<Renderer>().material.SetColor("_Color", terrain_.getColor());
            _cube[2].GetComponent<Renderer>().material.SetColor("_Color", terrain_.getColor());
        }

        public int getNeighborCount()
        {
            return _neighbors.Count;
        }

        public ArrayList getNeighbors()
        {
            return _neighbors;
        }


        public void addBuilding(Building building_)
        {

        building_.setCell(this);
        _building = building_;
        _buildingModel = building_.createBuilding();
        _buildingModel.transform.position = new Vector3(_x, _height + (_building.getHeight()/2), _z);

        Color dirtColor = new Color((building_.getColor().r / 1.5f + _terrain.getColor().r ) / 2, ((building_.getColor().g/ 1.5f + _terrain.getColor().g) / 2), ((building_.getColor().b / 1.5f + _terrain.getColor().b) / 2));
        _cube[0].GetComponent<Renderer>().material.SetColor("_Color", dirtColor);
        _cube[1].GetComponent<Renderer>().material.SetColor("_Color", dirtColor);
        _cube[2].GetComponent<Renderer>().material.SetColor("_Color", dirtColor);
    }

        public bool hasBuilding()
        {
            return _building != null;
        }

        public Building getBuilding()
        {
            return _building;
        }

        public Civilization getCivilization()
        {
            return _building == null ? null : _building.getCivilization();
        }

        public bool isBuildable()
        {
            return _terrain.isBuildable() && !hasBuilding(); 
        }

        public int getTerrainType()
        {
            return _terrain.getTerrainType();
        }

        public float getHeight()
        {
            return _height;
        }

        private float _height = 1;

        private float _x;
        private float _z;
        private float _y;

        private GameObject[] _cube;
        private GameObject _buildingModel;
        private Building _building;
        private ArrayList _neighbors;

        private GameTerrain _terrain;
}
