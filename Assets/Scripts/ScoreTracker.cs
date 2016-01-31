using UnityEngine;
using System.Collections;

public class ScoreTracker : MonoBehaviour 
{
    public int BabiesThrown;
    public float GameScore;
    public ScoreText[] DisplayTexts;
    private int displayIndex;
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
    /// Adds to Game Score and handles the pop-up text
    /// </summary>
    /// <param name="score"></param>
    public void AddScore(int score, string scoretext, Transform location)
    {
        GameScore += score * ComboNumber;
        ShowScoreAdded(score * ComboNumber);
        comboTimer = 3f;
        ComboNumber += 1;
        if (DisplayTexts.Length > 1)
        {
            DisplayTexts[displayIndex].DisplayMessage(scoretext, location);
            displayIndex++;
            if (displayIndex >= DisplayTexts.Length)
                displayIndex = 0;
        }
    }

    void ShowScoreAdded(int comboScore)
    {

    }
}
