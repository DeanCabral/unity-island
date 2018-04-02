using UnityEngine;
using System.Collections;

public class RotateToCursor : MonoBehaviour {

    // 3D Vector for mouse position
    Vector3 mousePos;
    // 3D Vector for position
    Vector3 direction;
    // Camera object
    Camera cam;
    // 2D Rigidbody object
    Rigidbody2D RD;

    public bool bl_canRotate = true;

	// Use this for initialization
	void Start () {

        // Assigns the rigidbody to the game object's current rigid body
        RD = this.GetComponent<Rigidbody2D>();
        // Assigns the cam variable to the main camera in the game world
        cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {

        if (bl_canRotate)
        {
            // Calculates the mouse position on screen from a ScreenToWorld point Vector3
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Sets the rotation of the object to look towards the direction of the mouse position
            transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);
        }
        
    }

}
