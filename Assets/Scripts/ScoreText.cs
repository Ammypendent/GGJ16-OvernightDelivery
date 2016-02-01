using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour 
{
    public string displayText;
    private bool displaying;
    private float displayTime;
    public Text message;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (displayTime > 0f)
        {
            displayTime -= Time.deltaTime;
            if (displayTime <= 0f)
                gameObject.SetActive(false);
        }
	}

    public void DisplayMessage(string text, Transform location)
    {
        gameObject.SetActive(true);
        Vector3 displayLocation = new Vector3(location.position.x, location.position.y + 4f, location.position.z);
        transform.position = displayLocation;
        displayTime = 3f;
        displayText = text;
        message.text = displayText;
        print("shit happened: " + text);
    }
}
