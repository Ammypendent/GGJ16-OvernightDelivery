using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class BabyCollider : MonoBehaviour 
{
    public bool DoesBabyDisappear;
    public string ScorePhrase;
    public int Score;
    private AudioSource soundSource;

    void Start()
    {
        soundSource = GetComponent<AudioSource>();
    }

	void OnTriggerEnter(Collider baby)
	{
        if (ScorePhrase != "")
            print(ScorePhrase);
        else
            print("Dude, write down a silly phrase for this!");
        //Play Sound


        if (DoesBabyDisappear)
            Destroy(baby.gameObject);
        else
            Destroy(baby.gameObject, 5f);
	}

}
