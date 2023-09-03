using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;
    private void Start()
    {
    }
    private void Update()
    {
        
        // If R key is pressed
        // Restart the current scene
        if (Input.GetKeyDown(KeyCode.R)&& _isGameOver == true)
        { 
            SceneManager.LoadScene(1); //current Game Scene;
        }
    }

    public void GameOver ()
    {
        Debug.Log("GameManager:: GameOver() called");
        _isGameOver = true;
    }
}
