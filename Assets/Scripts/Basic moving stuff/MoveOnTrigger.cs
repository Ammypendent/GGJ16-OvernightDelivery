using UnityEngine;
using System.Collections;

public class MoveOnTrigger : MonoBehaviour, ITriggerable 
{
    //Direction and speed
	public float xDirection;
	public float yDirection;
	public float zDirection;
	public float lerpSpeed; // in seconds!
    private float lerpParameter; //range [0,1], +=Time.deltaTime and then /lerpSpeed

	private Vector3 originalLocation;
	private Vector3 moveLocation;
    private Vector3 lastLocation;

    //Behavior
    public enum Moving
    {
        SingleTrigger,
        ReusableTrigger,
        OnOffSwitch,
    }
    public Moving MoveBehavior;

    [Tooltip("Only used when Move Behavior is OnOffSwitch")]
    public bool switchOn;

	private int moveDirection;	//1 - towards moveLocation, 0 - towards originalLocation
	private bool moving;


	// Use this for initialization
	void Start () 
	{
        if (MoveBehavior == Moving.OnOffSwitch && switchOn) //starting in on position
        {
            moving = true;
            moveDirection = 1;
        }
        else
        {
            moving = false;
            moveDirection = 0;
        }

		if (lerpSpeed < 0)
			lerpSpeed = 0;
        lerpParameter = 0;


		originalLocation = new Vector3 (transform.position.x, 
		                                transform.position.y, 
		                                transform.position.z);
        lastLocation = originalLocation;
		moveLocation = new Vector3 (transform.position.x + xDirection,
		                            transform.position.y + yDirection, 
		                            transform.position.z + zDirection);
	}

	void Update () 
	{
		if (moving)
		{
			MoveToPosition();
		}

	}

	public void Trigger()
	{
		if (MoveBehavior == Moving.SingleTrigger)
        {
		    print ("Move Single Trigger Activated");
            moving = true;
            moveDirection = 1;	//One way trigger
        }
		else if (MoveBehavior == Moving.ReusableTrigger)
        {
            print("Move Reusable Trigger Activated");
            moving = true;
            moveDirection = moveDirection == 0 ? 1 : 0; //Switch directions each time this is triggered 
            lastLocation = moveDirection == 1 ? originalLocation : moveLocation;

            //Calculating lerpParameter to keep same movement speed
            Vector3 placeToMoveTo = moveDirection == 0 ? originalLocation : moveLocation;
            float newparam = 1.0f - (Vector3.Distance(transform.position, placeToMoveTo)) / (Vector3.Distance(originalLocation, moveLocation));
            lerpParameter = newparam * lerpSpeed;
        }
        else if (MoveBehavior == Moving.OnOffSwitch)
        {
            print("Move OnOff Switch Activated");
            moving = !moving;
        }
		else
        {
            Debug.LogWarning("Unsupported Moving enum state has been detected!");
        }
	}

	void MoveToPosition()
	{
		Vector3 placeToMoveTo = moveDirection == 0 ? originalLocation : moveLocation;
        if (transform.position == placeToMoveTo)
        {
            if (MoveBehavior == Moving.OnOffSwitch)
            {
                //Switch directions
                moveDirection = moveDirection == 0 ? 1 : 0;
                placeToMoveTo = moveDirection == 0 ? originalLocation : moveLocation;
                lastLocation = moveDirection == 1 ? originalLocation : moveLocation;
                lerpParameter = 0;
            }
            else
            {
                moving = false;
                lerpParameter = 0;
            }
        }
        else
        {
            lerpParameter += Time.deltaTime;
            float t = lerpParameter / lerpSpeed;
            if (t > 1) { t = 1; }
            transform.position = Vector3.Lerp(lastLocation, placeToMoveTo, t);
        }
	}

}
