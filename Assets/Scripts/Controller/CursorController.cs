using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{

    private int x, y;
    private int borderX, borderY;
    private bool enable = false;
    private Vector3 defaultScale;

    //for input delay
    private readonly float delay = 0.25f;
    private float t;
    [SerializeField] GameController gameController;

    void Awake()
    {
        x = 0;
        y = 0;
        enable = true;
        t = 0;
        borderX = 9;
        borderY = 9;
        defaultScale = new Vector3(0.19f, 0.19f, 1);
    }


    void Update()
    {
        if (t <= 0)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {

                if (y < borderY)
                {
                    ++y;
                    this.transform.position += new Vector3(0f, 1f, 0f);
                    t = delay;
                }

            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                if (x > 0)
                {
                    --x;
                    this.transform.position += new Vector3(-1f, 0f, 0f);
                    t = delay;
                }
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                if (y > 0)
                {
                    --y;
                    this.transform.position += new Vector3(0f, -1f, 0f);
                    t = delay;
                }
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                if (x < borderX)
                {
                    ++x;
                    this.transform.position += new Vector3(1f, 0f, 0f);
                    t = delay;
                }
            }
        }

        if (enable && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space)))
        {
            enable = false;
            gameController.CursorSelect(x, y);
        }

        t -= Time.deltaTime;
    }


    public void SetEnable(bool b)
    {
        this.enable = b;
    }

    public void SetBorder(int x, int y)
    {
        borderX = x;
        borderY = y;
    }
    public void MoveCursor(int coef)
    {
        this.transform.position += new Vector3(coef * App.TABLE_GAP, 0f, 0f);
    }

    public void ScaleCursor(int x, int y)
    {
        this.transform.localScale = new Vector3(transform.localScale.x*x, transform.localScale.y*y, 1);
    }

    public void ResetCursorScale()
    {
        this.transform.localScale = defaultScale; 
    }
}
