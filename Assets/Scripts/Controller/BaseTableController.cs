using AppEnum;
using UnityEngine;

public class BaseTableController : MonoBehaviour
{
    protected ChessBoard board;
    protected int[] shipHPs;
    protected void Initialize()
    {
        board = new ChessBoard();
        shipHPs = App.CAPACITY_ARR;

    }

    public bool IsShipOrAdditionDestroyed(int state) {
        return shipHPs[state] == 0;
    }
    public virtual bool IsAllDestroyed()
    {
        foreach(int i in shipHPs)
        {
            if (i != 0) return false;
        }
        return true;
  
    }
}
