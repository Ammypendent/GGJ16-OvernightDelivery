using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
    public MusicManager Music;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

	
	void Start () 
    {
        //Music.ChangeSong(1, 1f);
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (Input.GetKeyDown("1"))
        {
            Music.ChangeSong(0, 1f);
        }
        if (Input.GetKeyDown("2"))
        {
            Music.ChangeSong(1, 1f);
        }
	}
}
