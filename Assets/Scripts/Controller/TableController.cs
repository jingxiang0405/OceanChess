using UnityEngine;
using AppEnum;
using System.Collections.Generic;

public class TableController : BaseTableController
{

    [SerializeField]  PrefabContainer prefabContainer;
    [SerializeField]  GameController gameController;
    [SerializeField] Enemy enemy;
    [SerializeField] ActionSelectController selectController;
    private int[] itemCountArr;
    private int[] itemRequiredLengthArr;
    private Alignment[] shipAlignmentArr;

    void Awake()
    {
        base.Initialize();
        itemCountArr = new int[7];
        itemRequiredLengthArr = App.CAPACITY_ARR;
        shipAlignmentArr = new Alignment[5];
    }


    // return whether nearby(4 dir) cell is equal to given state.
    private Direction checkNearby(int state, int x, int y)
    {

        if (x != 0) if (board.GetCell(x - 1, y) == state) return Direction.LEFT;
        if (x != 9) if (board.GetCell(x + 1, y) == state) return Direction.RIGHT;
        if (y != 0) if (board.GetCell(x, y - 1) == state) return Direction.DOWN;
        if (y != 9) if (board.GetCell(x, y + 1) == state) return Direction.TOP;
        return Direction.NA;
    }

    // fixme There may be better ways.
    public bool ShipDeployable(int state, int x, int y)
    {
        if (board.GetCell(x, y) == (int)CellState.Condition.BLANK){  
            int count = itemCountArr[state];
            
            if (count == 0) // The type of ship hasn't be deployed yet. 
            {
                return true;
            }
            else if (count < itemRequiredLengthArr[state])
            {
                Direction dir = checkNearby(state, x, y);

                // Horizontal
                if (dir == Direction.LEFT || dir == Direction.RIGHT)
                {

                    if(count==1)shipAlignmentArr[state] = Alignment.HORIZONTAL;
                    else
                    {
                        if (shipAlignmentArr[state] != Alignment.HORIZONTAL) return false;// Can not turn around
                    }
                    return true;
                }

                // Vertical
                else if (dir == Direction.TOP || dir == Direction.DOWN)
                {
                    if(count==1)shipAlignmentArr[state] = Alignment.VERTICAL;
                    else
                    {
                        if (shipAlignmentArr[state] != Alignment.VERTICAL) return false;// Can not turn around
                    }
                    return true;
                }

                // Not found
                else return false;
            }
 
         }
        return false;
    }

    private int countNearState(int state, int x, int y)
    {
        int count = 0;

            if (y != 0) if (board.GetCell(x, y - 1) == state) ++count;
            if (y != 9) if (board.GetCell(x, y + 1) == state) ++count;
       
        
            if (x != 0) if (board.GetCell(x - 1, y) == state) ++count;
            if (x != 9) if (board.GetCell(x + 1, y) == state) ++count;
        

        return count;

      
    }
    public bool AdditionDeployable(int state, int x, int y) {

        if (board.GetCell(x, y) == (int)CellState.Ship.AIRCRAFT_CARRIER)
        {

            if(itemCountArr[state] < itemRequiredLengthArr[state])
            if(state == (int)CellState.Addition.AC_CORE)return true;
            else if (state == (int)CellState.Addition.BOMBER)
            {
                int count = 0;
                count += countNearState((int)CellState.Ship.AIRCRAFT_CARRIER, x, y);
                count += countNearState((int)CellState.Addition.AC_CORE, x, y);
                return count == 1;
            }
        }
        return false;
    }

    public void Deploy(int state, int x, int y)
    {
        board.DeployState(state, x, y);
        prefabContainer.SetUIPosition(prefabContainer.NewUIInstance(state), new Vector3(App.CELL_LEFTBOTTOM_X+x, App.CELL_LEFTBOTTOM_Y+ y, 1f));
        ++itemCountArr[state];
        if (itemCountArr[state] == itemRequiredLengthArr[state])
        {
            gameController.DeployComplete(state);
        }
    }

    //fixme
    public List<StateParameter> Target(int action, int x, int y)
    {
        Debug.Log($"Enemy target {x}, {y}");
        List<StateParameter> list = new List<StateParameter>();
        switch (action)
        {

            case (int)Action.BASIC_ATTACK:
                destroyCell(ref list, x, y);
                break;
            case (int)Action.BOMBER:
                destroyCell(ref list, x, y);
                destroyCell(ref list, x + 1, y);
                destroyCell(ref list, x, y + 1);
                destroyCell(ref list, x + 1, y + 1);
                break;
            case (int)Action.BATTLESHIP_ATTACK:
                if (enemy.battleshipAlignment == Alignment.HORIZONTAL)
                {
                    destroyCell(ref list, x, y);
                    destroyCell(ref list, x + 1, y);
                    destroyCell(ref list, x + 2, y);
                }
                else
                {
                    destroyCell(ref list, x, y);
                    destroyCell(ref list, x, y + 1);
                    destroyCell(ref list, x, y + 2);
                }
                break;


        }
        if (IsAllDestroyed()) gameController.NotifyWinner("Enemy");
        return list;
    }


 

    private void destroyCell(ref List<StateParameter> list, int x, int y)
    {

        int currState = board.GetCell(x, y);
        int deployState = currState;
        int renderState = currState;
        bool deploy = true;
        bool newUI = true;
        switch (currState)
        {
            case (int)CellState.Condition.BLANK:
                deployState = (int)CellState.Condition.FAIL;
                renderState = deployState;
                break;
            case (int)CellState.Addition.AC_CORE:
                deployState = (int)CellState.Ship.AIRCRAFT_CARRIER;
                renderState = (int)CellState.Condition.RUIN;
                shipHPs[currState]--;
                deploy = false;
                newUI = false;
                break;
            case (int)CellState.Addition.BOMBER:
                deployState = (int)CellState.Condition.RUIN;
                renderState = deployState;
                shipHPs[currState]--;
                shipHPs[(int)CellState.Ship.AIRCRAFT_CARRIER]--;
                //if (IsShipOrAdditionDestroyed(currState)) selectController.SetActionEnabled((int)Action.BOMBER, false);
                break;
            case (int)CellState.Condition.FAIL:
            case (int)CellState.Condition.RUIN:
            case (int)CellState.Condition.SINK:
                deploy = false;
                newUI = false;
                break;
            case (int)CellState.Ship.RECONSHIP:
                //if (IsShipOrAdditionDestroyed(currState)) selectController.SetActionEnabled((int)Action.RECON, false);               
                deployState = (int)CellState.Condition.RUIN;
                renderState = deployState;
                shipHPs[currState]--;
                break;
            case (int)CellState.Ship.BATTLESHIP:
                //if (IsShipOrAdditionDestroyed(currState)) selectController.SetActionEnabled((int)Action.BATTLESHIP_ATTACK, false);
                deployState = (int)CellState.Condition.RUIN;
                renderState = deployState;
                shipHPs[currState]--;
                break;
            default:
                deployState = (int)CellState.Condition.RUIN;
                renderState = deployState;
                shipHPs[currState]--;
                break;
        }
        list.Add(new StateParameter(deployState, x, y));
        if (deploy) board.DeployState(deployState, x, y);
        if (newUI) prefabContainer.SetUIPosition(prefabContainer.NewUIInstance(renderState), new Vector3(App.CELL_LEFTBOTTOM_X  + x, App.CELL_LEFTBOTTOM_Y + y, 1f));


    }
}
