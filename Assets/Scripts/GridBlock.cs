﻿// Author(s): Paul Calande, Yixiang Xu

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBlock
{
    int x;

    int y;

    Block block;

    Grid grid;

    public GridBlock(int xStart, int yStart, Block myBlock)
    {
        x = xStart;
        y = xStart;
        block = myBlock;
    }

    //Return the x index of the gridblock
    public int GetX()
    {
        return x;
    }

    //Return the y index of the gridblock
    public int GetY()
    {
        return y;
    }

    /*Repeatedly moves the GridBlock along the Grid in
     *the given direction, one Tile at a time, only stopping
     *at occupied Tiles (preventing Tile overlap) or the
     *border.*/
    public void Move(Enums.Direction direction)
    {
        Tile[,] gridTiles = grid.GetTiles();
        Tile[,] blockTiles = block.GetTiles();
        List<Vector2> exTilesIndex = new List<Vector2>();
        bool movable = true;

        switch (direction)
        {
            case Enums.Direction.Right:
                if (x + block.GetWidth() + 1 > grid.GetWidth())
                    movable = false;

                if (movable)
                {
                    for (int i = 0; i < block.GetHeight(); i++)
                    {
                        int maxX = -1;
                        for (int j = 0; j < block.GetWidth(); j++)
                        {
                            if (blockTiles[j, i].GetIsOccupied())
                                maxX = j;
                        }

                        if (blockTiles[maxX + 1, i].GetIsOccupied())
                        {
                            movable = false;
                            break;
                        }

                        if (maxX != -1)
                            exTilesIndex.Add(new Vector2(maxX, i));
                    }
                }

                if (movable)
                {
                    for (int i = 0; i < exTilesIndex.Count; i++)
                    {
                        int exTileX = (int)exTilesIndex[i].x;
                        int exTileY = (int)exTilesIndex[i].y;

                        for (int j = exTileX; j >= 0; j--)
                        {
                            gridTiles[j + x + 1, y + exTileY].Duplicate(blockTiles[j, exTileY]);
                            gridTiles[j + x, y + exTileY].Clear();
                        }
                    }

                    x += 1;
                }

                break;
            case Enums.Direction.Down:
                if (y + block.GetHeight() + 1 > grid.GetHeight())
                    movable = false;

                if (movable)
                {
                    for (int i = 0; i < block.GetWidth(); i++)
                    {
                        int maxY = -1;
                        for (int j = 0; j < block.GetHeight(); j++)
                        {
                            if (blockTiles[i, j].GetIsOccupied())
                                maxY = j;
                        }

                        if (blockTiles[i, maxY + 1].GetIsOccupied())
                        {
                            movable = false;
                            break;
                        }

                        if (maxY != -1)
                            exTilesIndex.Add(new Vector2(i, maxY));
                    }
                }

                if (movable)
                {
                    for (int i = 0; i < exTilesIndex.Count; i++)
                    {
                        int exTileX = (int)exTilesIndex[i].x;
                        int exTileY = (int)exTilesIndex[i].y;

                        for (int j = exTileY; j >= 0; j--)
                        {
                            gridTiles[x + exTileX, y + j + 1].Duplicate(blockTiles[exTileX, j]);
                            gridTiles[x + exTileX, y + j].Clear();
                        }
                    }

                    y += 1;
                }

                break;
            case Enums.Direction.Left:
                if (x - 1 < 0)
                    movable = false;

                if (movable)
                {
                    for (int i = 0; i < block.GetHeight(); i++)
                    {
                        int minX = -1;
                        for (int j = block.GetWidth() - 1; j >= 0; j--)
                        {
                            if (blockTiles[j, i].GetIsOccupied())
                                minX = j;
                        }

                        if (blockTiles[minX - 1, i].GetIsOccupied())
                        {
                            movable = false;
                            break;
                        }

                        if (minX != -1)
                            exTilesIndex.Add(new Vector2(minX, i));
                    }
                }

                if (movable)
                {
                    for (int i = 0; i < exTilesIndex.Count; i++)
                    {
                        int exTileX = (int)exTilesIndex[i].x;
                        int exTileY = (int)exTilesIndex[i].y;

                        for (int j = 0; j < exTileX; j++)
                        {
                            gridTiles[j + x - 1, y + exTileY].Duplicate(blockTiles[j, exTileY]);
                            gridTiles[j + x, y + exTileY].Clear();
                        }
                    }

                    x -= 1;
                }

                break;

            case Enums.Direction.Up:
                if (y - 1 < 0)
                    movable = false;

                if (movable)
                {
                    for (int i = 0; i < block.GetWidth(); i++)
                    {
                        int minY = -1;
                        for (int j = block.GetHeight() - 1; j >= 0; j--)
                        {
                            if (blockTiles[i, j].GetIsOccupied())
                                minY = j;
                        }

                        if (blockTiles[i, minY - 1].GetIsOccupied())
                        {
                            movable = false;
                            break;
                        }

                        if (minY != -1)
                            exTilesIndex.Add(new Vector2(i, minY));
                    }
                }

                if (movable)
                {
                    for (int i = 0; i < exTilesIndex.Count; i++)
                    {
                        int exTileX = (int)exTilesIndex[i].x;
                        int exTileY = (int)exTilesIndex[i].y;

                        for (int j = 0; j < exTileY; j++)
                        {
                            gridTiles[x + exTileX, y + j - 1].Duplicate(blockTiles[exTileX, j]);
                            gridTiles[x + exTileX, y + j].Clear();
                        }
                    }

                    y -= 1;
                }

                break;
            
        }
    }
}