using UnityEngine;
using System.Collections;

public class SlowTerrainScript : MonoBehaviour {

    private PlayerMovement pm;

    void Start()
    {
        pm = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    void OnTriggerEnter2D()
    {
        pm.bl_canSprint = false;
        pm.fl_speed = 2.0f;
    }

    void OnTriggerExit2D()
    {
        pm.bl_canSprint = true;
        if (pm.bl_sprinting)
        {
            pm.fl_speed = 8.0f;
        }
        else
        {
            pm.fl_speed = 5.0f;
        }
            
    }
}
