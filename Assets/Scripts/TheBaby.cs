using UnityEngine;
using System.Collections;

public class TheBaby : MonoBehaviour 
{
    Vector3 torque;
    public Rigidbody rb;
    public float hangtime;
    public Material blue;
    public Material pink;
    public Renderer changeMe;
    public AudioSource babyAudio;

    void Start () 
    {
        torque = new Vector3 (Random.Range(-30f,30f),Random.Range(-90f,90f),Random.Range(-30f,30f));
        rb.AddTorque(torque);
        int rnd = Random.Range(1,3);
        //print("random color " + rnd);

        //Material mat =  GetComponent<MeshRenderer>().materials[0];
        changeMe.material = rnd == 1 ? blue : pink;
	}
	
	// Update is called once per frame
	void Update () 
    {
        hangtime += Time.deltaTime;
	}
}
