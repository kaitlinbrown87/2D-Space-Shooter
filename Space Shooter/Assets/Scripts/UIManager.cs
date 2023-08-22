using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Handle to text
    [SerializeField]
    private TextMeshProUGUI _scoreText;
    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score:" + 0;
    }

    // Update is called once per frame
    public void UpdateScore (int playerScore)
    {
        _scoreText.text = "Score:" + playerScore.ToString();
    }
}
