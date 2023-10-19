using UnityEngine;
using AppEnum;
public class ActionSelectController : MonoBehaviour
{
    // Start is called before the first frame update
    private int selectionCount;
    private bool[] selectableArr;
    private int currentSelected { get; set; }
    private GameObject currentSelectedGameObject;
    private int[,] cursorResizeArr;
    [SerializeField] UIController ui;
    [SerializeField] CursorController cursorController;
    public Alignment battleshipAlignment;

    void Awake()
    {
        selectionCount = App.SELECT_ACTION_COUNT;
        selectableArr = new bool[selectionCount];
        for (int i = 0; i < selectionCount; ++i)
        {
            selectableArr[i] = true;
        }
        battleshipAlignment = Alignment.VERTICAL;
        cursorResizeArr = new int[,] { {1, 1}, {2, 2 }, { 1, 3 }, { 3, 3 } };
        //fixme specification should not be here
        for (int i = selectionCount - 1; i >= 0; --i)
        {
            currentSelectedGameObject = this.transform.GetChild(i).gameObject;
            if (i == 0) selectCurr();
            else unselectCurr();
        }
        currentSelected = -1;

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < selectionCount; ++i)
        {
            if (Input.GetKey(KeyCode.Alpha1 + i))
            {
                if (selectableArr[i])
                {
                    if(currentSelected!= i)
                    {
                        cursorController.ResetCursorScale();
                        cursorController.SetBorder(10- cursorResizeArr[i, 0], 10-cursorResizeArr[i, 1]);
                        cursorController.ScaleCursor(cursorResizeArr[i, 0], cursorResizeArr[i, 1]);
                        currentSelected = i;
                        unselectCurr();
                        currentSelectedGameObject = this.gameObject.transform.GetChild(i).gameObject;
                        selectCurr();
                    }

                }

            }
        }

     
            if (Input.GetKeyDown(KeyCode.Tab) && currentSelected == (int)Action.BATTLESHIP_ATTACK)
            {
                if(battleshipAlignment == Alignment.VERTICAL)
                {
                    battleshipAlignment = Alignment.HORIZONTAL;
                    cursorController.ResetCursorScale();
                    cursorController.ScaleCursor(3, 1);
                    cursorController.SetBorder(7, 9);

            }
            else
                {
                    battleshipAlignment = Alignment.VERTICAL;
                    cursorController.ResetCursorScale();
                    cursorController.ScaleCursor(1, 3);
                    cursorController.SetBorder(9, 7);

            }
        }
        
    }

    public void SetActionEnabled(int action, bool enabled)
    {
        selectableArr[action] = enabled;
    }

    private void unselectCurr()
    {
        ui.ChangeObjectColor(currentSelectedGameObject, new Color(0.5f, 0.5f, 0.5f, 1f));
    }

    private void selectCurr()
    {
        ui.ChangeObjectColor(currentSelectedGameObject, new Color(1f, 1f, 1f, 1f));

    }

    public int GetCurrentSelected()
    {
        return currentSelected;
    }
}
