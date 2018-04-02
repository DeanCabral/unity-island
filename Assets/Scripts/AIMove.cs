using UnityEngine;
using System.Collections;

public class AIMove : MonoBehaviour {

    public Vector3 pointA;
    public Vector3 pointB;    
    float speed = 0.3f;

    void Start () {

        StartCoroutine("Patrol");
    }

    IEnumerator Patrol()
    {
        pointA = transform.position;
        while (true)
        {
            float i = Mathf.PingPong(Time.time * speed, 1);
            transform.position = Vector3.Lerp(pointA, pointB, i);
            yield return null;
        }
    }
	
}
