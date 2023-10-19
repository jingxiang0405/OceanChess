using AppEnum;
using UnityEngine;

public class EnemyTableController : BaseTableController
{

    private int reconRangeMin, reconRangeMax;

    [SerializeField] GameController gameController;
    [SerializeField] PrefabContainer prefabContainer;
    [SerializeField] UIController ui;
    [SerializeField] ActionSelectController actionSelectController;
    // Start is called before the first frame update
    void Awake()
    {
        base.Initialize();
        reconRangeMin = (int)CellState.Ship.AIRCRAFT_CARRIER;
        reconRangeMax = (int)CellState.Addition.AC_CORE;
    }


    // Update is called once per frame
    void Update()
    {
        
    }



    public bool Targetable(int action, int x, int y)
    {

        if (action == (int)Action.RECON) return true;
        int cellState = board.GetCell(x, y);
        switch (cellState)
        {
            case (int)CellState.Condition.RUIN:
            case (int)CellState.Condition.FAIL:
            case (int)CellState.Condition.SINK:
                return false;
        }
        return true;
        
    }

    public void Target(int action, int x, int y)
    {
        
        
        if (action == (int)Action.BASIC_ATTACK)
        {
            destroyCell(x, y);
        }

        else if (action == (int)Action.BOMBER)
        {
            destroyCell(x, y);
            destroyCell(x + 1, y);
            destroyCell(x, y + 1);
            destroyCell(x + 1, y + 1);
        }
        else if (action == (int)Action.BATTLESHIP_ATTACK)
        {
            if (actionSelectController.battleshipAlignment == Alignment.HORIZONTAL)
            {
                destroyCell(x, y);
                destroyCell(x + 1, y);
                destroyCell(x + 2, y);
            }
            else
            {
                destroyCell(x, y);
                destroyCell(x, y + 1);
                destroyCell(x, y + 2);
            }
        }

        if (IsAllDestroyed()) gameController.NotifyWinner("You");

    }

    public void Recon(int x, int y)
    {
        int count = 0;
        int startX = x - 1;
        int startY = y - 1;
        int endX = x + 1;
        int endY = y + 1;

        if (x == 0) ++startX;
        else if (x == 9) --endX;

        if (y == 0) ++startY;
        else if (y == 9) --endY;

        int state;
        for (int i = startX; i < endX; ++i)
        {
            for (int j = startY; j < endY; ++j)
            {
                state = board.GetCell(x, y);
                
                if (state!= (int)CellState.Ship.SUBMARINE &&state >= reconRangeMin && state <= reconRangeMax)
                {
                    ++count;
                }
            }
        }

        ui.SetBrocastMessage($"[RECON]\n{count} Found");
    }

    private void destroyCell(int x, int y) 
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
                break;
            case (int)CellState.Condition.FAIL:
            case (int)CellState.Condition.RUIN:
            case (int)CellState.Condition.SINK:
                deploy = false;
                newUI = false;
                break;
            default:
                deployState = (int)CellState.Condition.RUIN;
                renderState = deployState;
                shipHPs[currState]--;
                break;


        }

        if (newUI)prefabContainer.SetUIPosition(prefabContainer.NewUIInstance(renderState), new Vector3(App.CELL_LEFTBOTTOM_X +App.TABLE_GAP+ x, App.CELL_LEFTBOTTOM_Y + y, 1f));
        if (deploy) board.DeployState(deployState, x, y);

    }

    public void DeployCell(int state, int x, int y){

        this.board.DeployState(state, x, y);
    }

    override public bool IsAllDestroyed()
    {
        int[,] arr = board.GetArr();
        for (int x = 0; x < 10; ++x)
        {
            for (int y = 0; y < 10; ++y)
            {
                int state = arr[x, y];
                if (!(state == (int)CellState.Condition.BLANK || state == (int)CellState.Condition.RUIN || state == (int)CellState.Condition.FAIL))
                {
                    return false;
                }
            }
        }

        return true;
    }
}
