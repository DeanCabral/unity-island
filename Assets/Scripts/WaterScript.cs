using UnityEngine;
using System.Collections;

public class WaterScript : MonoBehaviour {

    public int in_water = 2;
    private int in_respawnAmount = 0;
    public int in_respawnTime = 2;
    private GameManager gm;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        in_respawnAmount = in_water;
    }
    void OnMouseDown()
    {
        if (in_water > 0)
        {
            gm.GetWater(1);
            in_water -= 1;
        }
        else
        {
            gm.GetWater(0);
            Invoke("Respawn", in_respawnTime);
        }
    }

    private void Respawn()
    {
        in_water = in_respawnAmount;
    }
}
