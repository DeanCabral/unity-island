using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    /* Initilalises game object for the playable character
    GameObject PC;
    // Boolean variable for allowing the camera to follow the player
    bool bl_followPlayer = true;
	// Use this for initialization
	void Start () {

        // Finds the player object with the specified tag
        PC = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {

        // Checks if camera can follow the player
        if (bl_followPlayer)
        {
            // Calls cameraFollow method
            cameraFollow();
        }
	
	}

    void cameraFollow ()
    {
        // Creates vector position to the x and y position of the player character
        Vector3 newCamPos = new Vector3(PC.transform.position.x, PC.transform.position.y, this.transform.position.z);
        // Sets the vector position of the camera to the player objects position
        this.transform.position = newCamPos;
    }
    */

    public Transform target;
    public float damping = 1;
    public float lookAheadFactor = 3;
    public float lookAheadReturnSpeed = 0.5f;
    public float lookAheadMoveThreshold = 0.1f;

    private float m_OffsetZ;
    private Vector3 m_LastTargetPosition;
    private Vector3 m_CurrentVelocity;
    private Vector3 m_LookAheadPos;

    // Use this for initialization
    private void Start()
    {
        m_LastTargetPosition = target.position;
        m_OffsetZ = (transform.position - target.position).z;
        transform.parent = null;
    }


    // Update is called once per frame
    private void Update()
    {
        // only update lookahead pos if accelerating or changed direction
        float xMoveDelta = (target.position - m_LastTargetPosition).x;

        bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

        if (updateLookAheadTarget)
        {
            m_LookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
        }
        else
        {
            m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
        }

        Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward * m_OffsetZ;
        Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

        transform.position = newPos;

        m_LastTargetPosition = target.position;
    }
}
