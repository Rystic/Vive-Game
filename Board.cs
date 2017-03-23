using UnityEngine;
using System.Collections;

public class Board
{
    public Board(int boardSize_)
    {
        _size = boardSize_;
        _cells = new Cell[boardSize_, boardSize_];
        iterate(createCells);
        iterate(linkCells);
    }

    private void iterate(Iterateable func_)
    {
        int boardSize = _cells.GetLength(0);
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                func_(i, j, boardSize);
            }
        }
    }

    private void createCells(int i_, int j_, int boardSize_)
    {    
        float xpos = i_ * DEFAULT_CELL_SIZE * 0.645f;
        float ypos = j_ * DEFAULT_CELL_SIZE * 0.7525f;
        float yOffset = i_ % 2 == 0 ? DEFAULT_CELL_SIZE * 0.38f : 0;
        _cells[i_, j_] = new Cell(DEFAULT_CELL_SIZE);
        _cells[i_, j_].setPosition(xpos, DIST_TO_GROUND, ypos + yOffset);
    }

    private void linkCells(int i_, int j_, int boardSize_)
    {
        Cell currCell = _cells[i_, j_];

        if (j_ - 1 >= 0)
        {
            currCell.addNeighbor(_cells[i_, j_ - 1]);
        }

        if (j_ + 1 < _size)
        {
            currCell.addNeighbor(_cells[i_ , j_ + 1]);
        }

        if (i_ % 2 == 0)
        {
            if (j_ + 1 < _size)
            {
                if (i_ - 1 >= 0)
                {
                    currCell.addNeighbor(_cells[i_ - 1, j_ + 1]);
                }

                if (i_ + 1 < _size)
                {
                    currCell.addNeighbor(_cells[i_ + 1, j_ + 1]);
                }
            }
        }
        else
        {
            if (j_ - 1 >= 0)
            {
                if (i_ - 1 >= 0)
                {
                    currCell.addNeighbor(_cells[i_ - 1, j_ - 1]);
                }

                if (i_ + 1 < _size)
                {
                    currCell.addNeighbor(_cells[i_ + 1, j_ - 1]);
                }
            }
        }

        if (i_ - 1 >= 0)
        {
            currCell.addNeighbor(_cells[i_ - 1, j_]);
        }

        if (i_ + 1 < _size)
        {
            currCell.addNeighbor(_cells[i_ + 1, j_]);
        }
    }

    public int getBoardSize()
    {
        return _size;
    }

    public Cell getCell(int x_, int y_)
    {
        return _cells[x_,y_];
    }

    public static readonly float DEFAULT_CELL_SIZE = 12.0f;

    public static readonly float DIST_TO_GROUND = .25f;

    private delegate void Iterateable(int i_, int j_, int boardSize_);

    private Cell[,] _cells;
    private int _size;
}
