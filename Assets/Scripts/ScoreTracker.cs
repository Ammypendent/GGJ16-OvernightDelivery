using UnityEngine;
using System.Collections;

public class ScoreTracker : MonoBehaviour 
{
    public int BabiesThrown;
    public float GameScore;
    public GameObject[] ScoreTexts;
    private float comboTimer;
    public static int ComboNumber;

	// Use this for initialization
	void Start () 
    {
        BabiesThrown = 0;
        GameScore = 0;
        ComboNumber = 1;
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (comboTimer > 0f)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0f)
            {
                comboTimer = 0f;
                ComboNumber = 1;
            }
        }
	}

    /// <summary>
    /// Adds to Game Score
    /// </summary>
    /// <param name="score"></param>
    public void AddScore(int score)
    {
        GameScore += score * ComboNumber;
        ShowScoreAdded(score * ComboNumber);
        comboTimer = 3f;
        ComboNumber += 1;
    }

    void ShowScoreAdded(int comboScore)
    {

    }
}
