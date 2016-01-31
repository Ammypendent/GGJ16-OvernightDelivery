using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour {
	public Rigidbody playerCenter;
	public GameObject babySpawn;
	private GameObject babyInHand;
	public GameObject babyPrefab;
	private Vector3 mouseStartPos;
	private Vector3 mouseEndPos;
	public Camera mainCamera;

    private float mouseHoldTime;
	private bool tossingTheBaby;
	private Rigidbody babyRigidBody;
	private float forwardVelocity;
	public float baseFowardVelocity;
	public float turnRate;
	private float rotation;
	private float sideVelocity;
    public float minBabyVelocity;
	public float maxBabyVelocity;
    public float timeToChargeUp;
    public GameObject playerHead;
    Vector3 toTilt;
	// Use this for initialization
	void Start () {
		tossingTheBaby = false;
        Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () {

		//movement
		forwardVelocity = baseFowardVelocity * Time.deltaTime;
		//rotation

		if (Input.GetKey(KeyCode.D))
		{//right turn
			//playerCenter.transform.Rotate(Vector3.up * turnRate * Time.deltaTime);
			sideVelocity = turnRate;
            toTilt =new Vector3(playerCenter.transform.localEulerAngles.x,
                playerCenter.transform.localEulerAngles.y,
                -5);
            
		}

		else if (Input.GetKey(KeyCode.A))
		{
			//playerCenter.transform.Rotate(-Vector3.up * turnRate * Time.deltaTime);
			sideVelocity = -turnRate;
            toTilt =new Vector3(playerCenter.transform.localEulerAngles.x,
                playerCenter.transform.localEulerAngles.y,
                5);
		}
		else
		{
			sideVelocity = 0;
            toTilt =new Vector3(playerCenter.transform.localEulerAngles.x,
                playerCenter.transform.localEulerAngles.y,
                0);
		}
		playerCenter.velocity = playerCenter.transform.TransformDirection(new Vector3(sideVelocity * Time.deltaTime, 0, forwardVelocity));
        playerCenter.transform.rotation = Quaternion.Slerp(playerCenter.transform.localRotation, Quaternion.Euler(toTilt), 0.2f);

		//do tilt


		//mouse velocity
		if (Input.GetMouseButtonDown(0) && !tossingTheBaby) 
		{
			babyInHand = Instantiate(babyPrefab,babySpawn.transform.position,babySpawn.transform.rotation) as GameObject;
			babyRigidBody = babyInHand.GetComponent<Rigidbody> ();
			//babyRigidBody.useGravity = false;
			//mouseStartPos = Input.mousePosition;
			mouseStartPos = mainCamera.ScreenToViewportPoint (Input.mousePosition);
			mouseHoldTime = 0;

			tossingTheBaby = true;
		}
		if (tossingTheBaby) 
		{
            if (mouseHoldTime <timeToChargeUp)
            {
                mouseHoldTime += Time.deltaTime;
            }
            babyRigidBody.transform.position = babySpawn.transform.position;
            babyRigidBody.transform.rotation = babySpawn.transform.rotation;

			//print (mouseDragTime);
		}

		if (Input.GetMouseButtonUp(0) && tossingTheBaby) 
		{
			if (babyInHand != null) {
				//babyRigidBody.useGravity = true;

				//babyRigidBody.AddForce(babyInHand.transform.TransformDirection (TossTheBaby ()));
                babyRigidBody.velocity = babyInHand.transform.TransformDirection (TossTheBaby ());
			}
			tossingTheBaby = false;
		}
	}


	public Vector3 TossTheBaby()
	{
        mouseHoldTime = mouseHoldTime > timeToChargeUp ? timeToChargeUp : mouseHoldTime;
        float percent = mouseHoldTime / timeToChargeUp;
        float forwardVel = ((maxBabyVelocity - minBabyVelocity) * percent)+minBabyVelocity;
        print("percent = "+ percent + " fwd vel "+ forwardVel);

        //Vector3 forwardForce = playerCenter.velocity * babyRigidBody.mass;
        //newVel.z += forwardVel * 10;
        return new Vector3( 0, 5,forwardVel*10);
	}

	private bool IsMouseMoving()
	{
		bool isMoving = false;
		float difInPos = mainCamera.ScreenToViewportPoint (Input.mousePosition).y-mouseStartPos.y;
		if (difInPos != 0) {
			isMoving = true;

		}
		return isMoving;
	}
}
