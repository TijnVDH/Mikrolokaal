using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    TextMeshProUGUI mText;
    public int teamScore {  get; private set; }

    // score values
    [SerializeField] int enemyDefeat;
    [SerializeField] int correctItemDrop;
    [SerializeField] int upgradePickUp;

    public GameObject EnableOnWin;
    public int pointsToWin;

    void Awake()
    {
        teamScore = 0;
        mText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        mText.text = teamScore.ToString();
        if(teamScore >= pointsToWin)
        {
            EnableOnWin.SetActive(true);
        }
    }

    public void EnemyDefeatPoints()
    {
        teamScore += enemyDefeat;
        Debug.Log("+10 points");
    }

    public void ItemDropPoints()
    {
        teamScore += correctItemDrop;
        Debug.Log("+20 points");
    }

    public void UpgradePickupPoints()
    {
        teamScore += upgradePickUp;
        Debug.Log("+5 points");
    }

    public int GetScore()
    {
        return teamScore;
    }
}
