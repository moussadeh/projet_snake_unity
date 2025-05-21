using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public TextMeshProUGUI TextScore;
    void Start()
    {
        int score = GameData.score;
        TextScore.text = "Score : " + score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
