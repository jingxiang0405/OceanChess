using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppEnum;
public class GameController : MonoBehaviour
{
    [SerializeField] UIController uiController;
    [SerializeField] TableController playerTableController;
    [SerializeField] EnemyTableController enemyTableController;
    
    [SerializeField] DeploySelectController deploySelectController;
    [SerializeField] ActionSelectController actionSelectController;
    [SerializeField] CursorController cursorController;
    [SerializeField] Enemy enemy;

    [SerializeField] BombController bombController;
    private bool myTurn;
    private bool end;
    void Awake()
    {
 
    

        //temporal usage
        createNewGame();
    }

    public void createNewGame()
    {
        end = false;
        App.STAGE = SelectStage.SELECT_SHIP;
        uiController.DisplayCreateNewGameUI();
    }

    //function callback
    public void CursorSelect(int x, int y)
    {
        int state;


        if (App.STAGE == SelectStage.SELECT_SHIP) {
            state = deploySelectController.GetCurrentSelected();
            if (playerTableController.ShipDeployable(state, x, y))
            {
                playerTableController.Deploy(state, x, y);
            }
            if (deploySelectController.IsAllComplete())// switch stage to "Addition"
            {
                App.STAGE = SelectStage.SELECT_ADDITION;
                uiController.DisplayStageAdditionUI();
                deploySelectController.InitStageAddition();
            }
        }
        else if (App.STAGE == SelectStage.SELECT_ADDITION) {
            state = deploySelectController.GetCurrentSelected();
            if (playerTableController.AdditionDeployable(state, x, y))
            {
                playerTableController.Deploy(state, x, y);
            }
            if (deploySelectController.IsAllComplete())// switch stage to "InGame"
            {
                App.STAGE = SelectStage.SELECT_INGAME;
                cursorController.MoveCursor(1);
                uiController.DisplayStageInGameUI();
                enemyTableController.gameObject.SetActive(true);
                actionSelectController.gameObject.SetActive(true);
                myTurn = true;
                enemy.DeployBoard();

            }
        }
        else { 
                state = actionSelectController.GetCurrentSelected();

                if (myTurn)
                {

                    if (state == (int)Action.RECON)
                    {
                        enemyTableController.Recon(x, y);
                    }

                    else
                    {

                            if (enemyTableController.Targetable(state, x, y))
                            {

                                enemyTableController.Target(state, x, y);
                                EnemyTurn();
                            }
                        
                    }
                }
           
                
        }

        cursorController.SetEnable(true);

    }

    public void DeployComplete(int state)
    {
        deploySelectController.DeployComplete(state);
    }
    
    public void MyTurn()
    {
        myTurn = true;
    }

    public void EnemyTurn()
    {

            myTurn = false;
            if (!end) enemy.NotifyTurnStart();
        
    }

    public void EnemyAction(StateParameter parameter)
    {
        playerTableController.Target(parameter.state, parameter.x, parameter.y);
    }

    public void NotifyWinner(string name)
    {
        uiController.SetBrocastMessage($"{name} Win!!");
        bombController.gameObject.SetActive(true);
    }



}
