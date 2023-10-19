using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppEnum;
//data in a game
public class App
{
    public static readonly int[] CAPACITY_ARR = new int[7] { 5, 4, 3, 3, 2, 1, 1 };
    public static readonly int SELECT_SHIP_COUNT = 5;
    public static readonly int SELECT_ADDITION_COUNT = 2;
    public static readonly int SELECT_ACTION_COUNT = 4;
    public static readonly int TABLE_GAP = 16;
    public static readonly float CELL_LEFTBOTTOM_X = -12.5f;
    public static readonly float CELL_LEFTBOTTOM_Y = -5.5f;

    public static readonly float CURSOR_POSITION_X = -13f;
    public static readonly float CURSOR_POSITION_Y = -6f;
    public static readonly float CURSOR_ENEMY_POSITION_X = 3.5f;
    public static readonly float CURSOR_ENEMY_POSITION_Y = CURSOR_Y;
    public static readonly int CURSOR_X = 0;
    public static readonly int CURSOR_Y = 0;

    public static readonly int[] ACTION_CD = { 0, 2, 2, 3 };
    public static SelectStage STAGE;
   

}

