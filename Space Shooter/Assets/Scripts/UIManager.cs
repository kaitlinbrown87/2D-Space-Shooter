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
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private TextMeshProUGUI _gameOverText;
    [SerializeField]
    private TextMeshProUGUI _restartText;
    [SerializeField]
    private GameManager _gameManager;
    [SerializeField]
    private TextMeshProUGUI _ammoText;
    [SerializeField]
    private bool _noAmmo = false;
    // Variable for the slider
    [SerializeField]
    private Slider _thrusterSlider;
    [SerializeField]
    private Image _thrusterSliderFill;
    [SerializeField]
    private TextMeshProUGUI _waveDisplay;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score:" + 0;
        _ammoText.text = "Ammo:";
        _gameOverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("gameManager is NULL.");
        }
    }

    // Update is called once per frame
    public void UpdateScore (int playerScore)
    {
        _scoreText.text = "Score:" + playerScore.ToString();
        _gameOverText.gameObject.SetActive(false);
       
      
    }
    public void ChangeAmmoCount(int _ammoCount)
    {
        _ammoText.text = "Ammo:" + _ammoCount;
        if (_ammoCount == 0)
        {
            _noAmmo = true;
            StartCoroutine(NoAmmoFlickerRoutine());
        }
       
    }
    public void Updatelives (int currentLives)
    {
        _livesImg.sprite = _liveSprites[currentLives];
        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }
    void GameOverSequence ()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }
    IEnumerator GameOverFlickerRoutine ()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
                yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
       
    }
    IEnumerator NoAmmoFlickerRoutine()
    {
        while (_noAmmo == true)
        {
            yield return new WaitForSeconds(0.5f);
            _ammoText.enabled = false;
            yield return new WaitForSeconds(0.5f);
            _ammoText.enabled = true;
        }
    }
    public void UpdateThrusterSlider (float _thrustValue)
    {
        if (_thrustValue >= 0 && _thrustValue <=10)
        {
            _thrusterSlider.value = _thrustValue;
        }
    }
    public void ThrusterSliderUsableCoolor (bool _usableThrusters)
    {
        if (_usableThrusters)
        {
            _thrusterSliderFill.color = Color.green;
        }
        else if (_usableThrusters)
        {
            _thrusterSliderFill.color = Color.blue;
        }
    }
    public void DisplayWaveNumber (int waveNumber)
    {
        _waveDisplay.text = "Wave:" + waveNumber;
        _waveDisplay.gameObject.SetActive(true);
        StartCoroutine(WaveDisplayroutine());
    }
    IEnumerator WaveDisplayroutine()
    {
        while (_waveDisplay == true)
        {
            yield return new WaitForSeconds(3.0f);
            _waveDisplay.gameObject.SetActive(false);
        }
    }
}
