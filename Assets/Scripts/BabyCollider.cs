using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class BabyCollider : MonoBehaviour 
{
    public bool DoesBabyDisappear;
    public string ScorePhrase;
    public int Score;
    private AudioSource soundSource;
    private ScoreTracker GameMaster;

    void Start()
    {
        soundSource = GetComponent<AudioSource>();
        GameMaster = GameObject.Find("GameMaster").GetComponent<ScoreTracker>();
        if (GameMaster == null)
            Debug.Log("Hey! GameMaster object that's supposed to have ScoreTracker.cs is missing!");
    }

	void OnTriggerEnter(Collider baby)
	{
        if (ScorePhrase != "")
            ScorePhrase = "Dude set some goofy score text man!";
        TheBaby babystats = baby.GetComponent<TheBaby>();
        float TotalBabyScore = 0f;
        TotalBabyScore += babystats.hangtime * 10f;
        TotalBabyScore += babystats.rb.velocity.magnitude*5f;
        TotalBabyScore += Score;
        GameMaster.AddScore((int)TotalBabyScore, ScorePhrase, gameObject.transform);
        soundSource.Play(); //We will probably use mixer if we have time
        if (DoesBabyDisappear)
            Destroy(baby.gameObject);
        else
            Destroy(baby.gameObject, 5f);
	}

}
