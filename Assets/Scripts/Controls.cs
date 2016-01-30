using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour {
	public GameObject playerCenter;
	public GameObject babySpawn;
	private GameObject babyInHand;
	public GameObject babyPrefab;
	private Vector3 mouseStartPos;
	private Vector3 mouseEndPos;

	private float mouseDragTime;
	private bool tossingTheBaby;

	// Use this for initialization
	void Start () {
		tossingTheBaby = false;
	}
	
	// Update is called once per frame
	void Update () {

		//movement

		//mouse velocity
		if (Input.GetMouseButtonDown(0) && !tossingTheBaby) 
		{
			babyInHand = Instantiate(babyPrefab,babySpawn.transform.position,transform.rotation) as GameObject;
			mouseStartPos = Input.mousePosition;
			mouseDragTime = 0;
			tossingTheBaby = true;
		}
		if (tossingTheBaby) 
		{
			mouseDragTime += Time.deltaTime;
		}

		if (Input.GetMouseButtonUp(0) && tossingTheBaby) 
		{
			if (babyInHand != null) {
				print ("baby in hand");
				mouseEndPos = Input.mousePosition;
				babyInHand.GetComponent<Rigidbody> ().velocity = TossTheBaby (mouseStartPos,
					mouseEndPos, mouseDragTime);
			} 
			else 
			{
				print ("didn't get the baby");
			}
			tossingTheBaby = false;
		}
	}


	public Vector3 TossTheBaby(Vector3 startPos, Vector3 endPos, float time)
	{
		float velX = ((endPos.x - startPos.x) / time)*0.005f;
		float velY = ((endPos.y - startPos.y) / time)*0.005f;
		float velZ = 1 / time;//the forward velocity of the player plus a seed.
		print (velX);
		return new Vector3( velX, velY,velZ);
	}
}
