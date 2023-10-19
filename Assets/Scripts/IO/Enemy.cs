using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppEnum;
public struct StateParameter
{
    public int state, x, y;
    public StateParameter(int state, int x, int y)
    {
        this.state = state;
        this.x = x;
        this.y = y;
    }
}

public struct Pos
{
    public int x;
    public int y;

    public Pos(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public override bool Equals(object obj)
    {
        return ((Pos)obj).x == x && ((Pos)obj).y == y;
    }
}
public class Enemy : MonoBehaviour
{
    [SerializeField] GameController gameController;
    [SerializeField] EnemyTableController enemyTableController;
    public Alignment battleshipAlignment;
    private List<Pos> attackedList;
    void Awake()
    {
        attackedList = new List<Pos>();
    }

    public void DeployBoard()
    {
        int count, alignment;
        List<int> usedList = new List<int>();
        alignment = Random.Range((int)Alignment.VERTICAL, (int)Alignment.HORIZONTAL+1);
        for(int i = 0; i < App.SELECT_SHIP_COUNT; ++i)
        {
            int p1, p2;
            count = App.CAPACITY_ARR[i];
            p1 = Random.Range(0, 10);
            usedList.Add(p1);
            p2 = Random.Range(0, 10 - count);

            for (int c = 0; c < count; ++c)
            {
                if (alignment == (int)Alignment.VERTICAL)
                {
                    enemyTableController.DeployCell(0, p1, p2 + c);
                }
                else {
                    enemyTableController.DeployCell(0, p2 + c, p1); }



            }

        }
    }
    private Pos getPos()
    {
        Pos pos = new Pos(0, 0);
   
        pos.x = Random.Range(0, 10);
        pos.y = Random.Range(0, 10);

        attackedList.Add(pos);
        return pos;
    } 
    private int getAction()
    {

        return (int)Action.BASIC_ATTACK;
    }
    private StateParameter getActionParameter()
    {
        StateParameter parameter;
        parameter.state = getAction();
        Pos pos = getPos();
        parameter.x = pos.x;
        parameter.y = pos.y;

        return parameter;
    }

    //called by GameController to notify that it is your turn.
    public void NotifyTurnStart()
    {
        gameController.EnemyAction(getActionParameter());
        gameController.EnemyAction(getActionParameter());
        gameController.EnemyAction(getActionParameter());
        gameController.MyTurn();
    }
}
