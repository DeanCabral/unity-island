using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public bool bl_moving;
    public bool bl_sprinting;
    public bool bl_canMove = true;
    public bool bl_canSprint = true;
    public float fl_speed = 5.0f;

    private GameManager gm;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

	void Update () {

        if (bl_canMove)
        {
            movePlayer();
        }
        
        checkIfMoving();

	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Shelter")
        {
            gm.CallIncreaseShelter();
            print("Entered Shelter");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Shelter")
        {
            gm.CallDecreaseShelter();
            print("Exited Shelter");
        }    
    }

    void movePlayer()
    {
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (bl_canSprint)
            {
                bl_sprinting = true;
                fl_speed = 10.0f;
            }
            
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            bl_sprinting = false;
            fl_speed = 5.0f;
        }

        if (Input.GetKey(KeyCode.W))
        {     
            transform.Translate(Vector3.up * fl_speed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey(KeyCode.S))
        {    
            transform.Translate(Vector3.down * fl_speed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey(KeyCode.A))
        {    
            transform.Translate(Vector3.left * fl_speed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey(KeyCode.D))
        {    
            transform.Translate(Vector3.right * fl_speed * Time.deltaTime, Space.World);
        }        
        
    }

    void checkIfMoving()
    {
        if (Input.GetKey(KeyCode.W) != true && Input.GetKey(KeyCode.S) != true && Input.GetKey(KeyCode.A) != true && Input.GetKey(KeyCode.D) != true)
        {    
            bl_moving = false;
        }
        else
        {           
            bl_moving = true;
        }
    }
}
