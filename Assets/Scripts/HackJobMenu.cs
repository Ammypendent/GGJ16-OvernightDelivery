using UnityEngine;
using System.Collections;

public class HackJobMenu : MonoBehaviour {

    public GameObject StartButton;
    public GameObject CreditsButton;
    public GameObject ExitButton;
    public GameObject BackButton;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if ( Physics.Raycast (ray,out hit,100.0f))
            {
                if (hit.transform.gameObject.tag == "StartButton")
                {
                    print("Time to start Game");
                    Application.LoadLevel(1);
                }
                else if (hit.transform.gameObject.tag == "CreditsButton")
                {
                    print("Credits Button Hit!");
                    ISwitchable actionScript = hit.transform.gameObject.GetComponent<ISwitchable>();
                    actionScript.ActivateSwitch();
                }
                else if (hit.transform.gameObject.tag == "ExitButton")
                {
                    print("Time to flee the game!");
                    Application.Quit();
                }
                else if (hit.transform.gameObject.tag == "BackButton")
                {
                    print("Go back to main menu!");
                    ISwitchable actionScript = hit.transform.gameObject.GetComponent<ISwitchable>();
                    actionScript.ActivateSwitch();
                }
            }
        }
	}
}
