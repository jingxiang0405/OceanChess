using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppEnum;
public class ChessBoard
{
    private int[,] arr;

    public ChessBoard()
    {
        arr = new int[10, 10];

        for(int x = 0; x < 10; ++x)
        {
            for(int y = 0; y < 10; ++y)
            {
                arr[x, y] = (int)CellState.Condition.BLANK;
            }
        }
    }

    public void DeployState(int state, int x, int y)
    {
        arr[x, y] = state;
    }

    public int[,] GetArr()
    {
        return arr;
    }
    public int GetCell(int x, int y)
    {
        return arr[x,y];
    }


}
