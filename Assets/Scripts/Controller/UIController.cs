using UnityEngine;
using Text = TMPro.TextMeshProUGUI;
public class UIController : MonoBehaviour
{

    [SerializeField] Text You, Deploy, Enemy, Begin, Broadcast;


    void Awake()
    {

        You.enabled = true;
        Deploy.enabled = false;
        Enemy.enabled = false;
        Begin.enabled = false;
    }

    public void DisplayCreateNewGameUI()
    {
        Deploy.enabled = true;
    }

    public void ChangeObjectColor(GameObject gobj, Color color)
    {
        Renderer renderer = gobj.GetComponent<Renderer>();
        renderer.material.SetColor("_Color", color);
    }

    public void DisplayStageAdditionUI()
    {
        Deploy.text = "Deploy your bomber and core \nin your AC!\n(type 1~2 to choose)";
    }

    public void DisplayStageInGameUI()
    {
        Enemy.enabled = true;
        Deploy.enabled = false;
    }

    public void SetBrocastMessage(string str)
    {
        Broadcast.enabled = true;
        Broadcast.text = str;
    }
}
