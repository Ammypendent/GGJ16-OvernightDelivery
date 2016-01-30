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

	private float mouseDragTime;
	private bool tossingTheBaby;
	private Rigidbody babyRigidBody;
	private float forwardVelocity;
	public float baseFowardVelocity;
	public float turnRate;
	private float rotation;
	private float sideVelocity;
	public float maxBabyVelocity;
	// Use this for initialization
	void Start () {
		tossingTheBaby = false;

	}
	
	// Update is called once per frame
	void Update () {

		//movement
		forwardVelocity = baseFowardVelocity * Time.deltaTime;//replace this
		//playerCenter.velocity = //playerCenter.transform.TransformDirection ( Vector3.forward * forwardVelocity * Time.deltaTime);
		//rotation

		if (Input.GetKey(KeyCode.D))
		{//right turn
			//playerCenter.transform.Rotate(Vector3.up * turnRate * Time.deltaTime);
			sideVelocity = turnRate;
		}

		else if (Input.GetKey(KeyCode.A))
		{
			//playerCenter.transform.Rotate(-Vector3.up * turnRate * Time.deltaTime);
			sideVelocity = -turnRate;
		}
		else
		{
			sideVelocity = 0;
		}
		playerCenter.velocity = playerCenter.transform.TransformDirection(new Vector3(sideVelocity * Time.deltaTime, 0, forwardVelocity));

		//do tilt


		//mouse velocity
		if (Input.GetMouseButtonDown(0) && !tossingTheBaby) 
		{
			babyInHand = Instantiate(babyPrefab,babySpawn.transform.position,babySpawn.transform.rotation) as GameObject;
			babyRigidBody = babyInHand.GetComponent<Rigidbody> ();
			babyRigidBody.useGravity = false;
			//mouseStartPos = Input.mousePosition;
			mouseStartPos = mainCamera.ScreenToViewportPoint (Input.mousePosition);
			mouseDragTime = 0;
			tossingTheBaby = true;
		}
		if (tossingTheBaby) 
		{
			if (IsMouseMoving ()) {
				mouseDragTime += Time.deltaTime;
			} else {
				mouseDragTime = 0;
			}
			//babyRigidBody.transform.position = babySpawn.transform.position;
			//babyRigidBody.transform.rotation = babySpawn.transform.rotation;
			babyRigidBody.velocity = playerCenter.velocity;
			//print (mouseDragTime);
		}

		if (Input.GetMouseButtonUp(0) && tossingTheBaby) 
		{
			if (babyInHand != null) {
				//print ("baby in hand");
				babyRigidBody.useGravity = true;
				mouseEndPos = mainCamera.ScreenToViewportPoint (Input.mousePosition);//Input.mousePosition;
				/*babyRigidBody.velocity = babyInHand.transform.TransformDirection (
					TossTheBaby (mouseStartPos,
						mouseEndPos, mouseDragTime,forwardVelocity));*/

				babyRigidBody.AddForce(babyInHand.transform.TransformDirection (
					TossTheBaby (mouseStartPos,
						mouseEndPos, mouseDragTime,forwardVelocity)));
			}
			tossingTheBaby = false;
		}
	}


	public Vector3 TossTheBaby(Vector3 startPos, Vector3 endPos, float time, float forwardVelocity)
	{
		/*
		float velX = ((endPos.x - startPos.x) / time) * 1;//0.005f;
		float velY = ((endPos.y - startPos.y) / time)*0.5f;//0.005f;
		float modVelY = velY<0 ? 0:velY;
		float velZ = (forwardVelocity)+modVelY / time;//the forward velocity of the player plus a seed.
		print ("forward velocity is "+ velZ) ;
		if (velZ > maxBabyVelocity)
		{
			velZ = maxBabyVelocity;
		}
		//+forward
		print (velX+" "+velY);*/
		float velX = ((endPos.x - startPos.x) / time) * 100;//0.005f;
		float velY = ((endPos.y - startPos.y) / time)*20;//0.005f;
		float modVelY = velY<0 ? 0:velY;
		float velZ = (forwardVelocity)+modVelY / time;//the forward velocity of the player plus a seed.
		print ("forward velocity is "+ velZ) ;
		if (velZ > maxBabyVelocity)
		{
			velZ = maxBabyVelocity;
		}
		//+forward
		print (velX+" "+velY);
		return new Vector3( velX, velY,velZ);
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
