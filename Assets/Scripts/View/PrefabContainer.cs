using UnityEngine;
using AppEnum;
using System.Collections.Generic;

public class PrefabContainer : MonoBehaviour
{


    [SerializeField] GameObject[] ShipPrefabArray;
    List<GameObject> UIInstances;
    void Start()
    {
        UIInstances = new List<GameObject>();
    }


    /*
     * Factory method
     * return the index in array.
    */
    public int NewUIInstance(int state)
    {
        int i = UIInstances.Count;
        GameObject prefab = ShipPrefabArray[state];
        GameObject obj = GameObject.Instantiate<GameObject>(prefab, null);
        UIInstances.Add(obj);
        return i;
    }
    public void SetUIPosition(int index, Vector3 v3)
    {
        UIInstances[index].transform.position = v3;
    }
}
