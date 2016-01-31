using UnityEngine;
using System.Collections;

public class TheBaby : MonoBehaviour 
{
    Vector3 torque;
    public Rigidbody rb;
    public float hangtime;

    void Start () 
    {
        torque = new Vector3 (Random.Range(-30f,30f),Random.Range(-90f,90f),Random.Range(-30f,30f));
        rb.AddTorque(torque);
	}
	
	// Update is called once per frame
	void Update () 
    {
        hangtime += Time.deltaTime;
	}
}
