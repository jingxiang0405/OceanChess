using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppEnum;
public class DeploySelectController : MonoBehaviour
{
    private GameObject currentSelectedGameObject;
    private int currentSelected { get; set; }

    private bool[] selectableArr;
    private int selectableArrSize, selectionCount, extra;

    [SerializeField] UIController ui;

    void Awake()
    {
        selectableArrSize =  App.SELECT_SHIP_COUNT+App.SELECT_ADDITION_COUNT;
        selectableArr = new bool[selectableArrSize];
        for(int i = 0; i < selectableArrSize; ++i) selectableArr[i] = false;

        //temporal usage
        InitStageShip();
    }

    void Update()
    {


        for(int i = 0; i < selectionCount; ++i)
        {
            int actuali = i + extra;
            if (Input.GetKey(KeyCode.Alpha1 + i))
            {
                if (selectableArr[actuali])
                {
                    currentSelected = actuali;
                    unselectCurr();
                    currentSelectedGameObject = this.gameObject.transform.GetChild(actuali).gameObject;
                    selectCurr();
                }
            }
        }
       
    }

    public void SetSelect(int state)
    {
        currentSelected = state;
        currentSelectedGameObject = this.gameObject.transform.GetChild(state).gameObject;
        selectCurr();
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

    public bool IsAllComplete()
    {
        int n=0;
        switch (App.STAGE)
        {
            case SelectStage.SELECT_SHIP:
                n = App.SELECT_SHIP_COUNT;
                break;
            case SelectStage.SELECT_ADDITION:
                n = App.SELECT_ADDITION_COUNT;
                break;
            case SelectStage.SELECT_INGAME:
                n = App.SELECT_ACTION_COUNT;
                break;
        }
        for (int i = extra; i < extra + n; ++i)
        {
            if (selectableArr[i])return false;
            
        }
        return true;
    }

    public void DeployComplete(int state)
    {
        GameObject obj = this.gameObject.transform.GetChild(state).gameObject;
        obj.SetActive(false);
        selectableArr[state] = false;

    }

    public void InitStageShip()
    {
        extra = 0;
        currentSelected = (int)CellState.Ship.AIRCRAFT_CARRIER;
        for (int i = App.SELECT_SHIP_COUNT-1; i >= extra ; --i)
        {
            selectableArr[i] = true;
            this.gameObject.transform.GetChild(i).gameObject.SetActive(true);

            currentSelectedGameObject = this.gameObject.transform.GetChild(i).gameObject;

            if (i == 0) selectCurr();

            else unselectCurr();
        }

        selectionCount = App.SELECT_SHIP_COUNT;


    }
    public void InitStageAddition()
    {
        currentSelected = (int)CellState.Addition.BOMBER;
        extra = App.SELECT_SHIP_COUNT;
        for (int i = App.SELECT_ADDITION_COUNT+extra - 1; i >= extra; --i)
        {
            selectableArr[i] = true;
            this.gameObject.transform.GetChild(i).gameObject.SetActive(true);
            currentSelectedGameObject = this.gameObject.transform.GetChild(i).gameObject;
            if (i == 0) selectCurr();
            else unselectCurr();
        }

        selectionCount = App.SELECT_ADDITION_COUNT;
    }

}                                                           
