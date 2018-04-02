using UnityEngine;
using System.Collections;

public class FoodScript : MonoBehaviour {

    public int in_food = 2;
    private int in_respawnAmount = 0;
    public int in_respawnTime = 2;
    private GameManager gm;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        in_respawnAmount = in_food;
    }
	void OnMouseDown()
    {
        if (in_food > 0)
        {
            gm.GetFood(1);
            in_food -= 1;
        }
        else
        {
            gm.GetFood(0);
            Invoke("Respawn", in_respawnTime);
        }
    }

    private void Respawn()
    {
        in_food = in_respawnAmount;
    }
}
