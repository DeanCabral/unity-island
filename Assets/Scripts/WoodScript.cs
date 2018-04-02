using UnityEngine;
using System.Collections;

public class WoodScript : MonoBehaviour {

    public int in_wood = 1;
    private GameManager gm;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void OnMouseDown()
    {
        if (in_wood > 0)
        {
            gm.GetWood(1);
            in_wood -= 1;
        }
        else
        {
            gm.GetWood(0);
        }
    }
}
