using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class GameMode : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private EndlessPipeGenerator _pipeGenerator;
    [SerializeField] private ScreenController _screenController;
    [SerializeField] private GameSaver _gameSaver;
    [SerializeField] private AudioHandler _audioHandler;

    [Header("Fade")]
    [SerializeField] private FadeScreen _fadeScreen;
    [SerializeField] private float _fadeTime = 0.05f;
    [SerializeField] private Color _fadeColor = Color.black;

    [Header("Data")]
    [SerializeField] private PlayerParameters _gameWaitingParameters;
    [SerializeField] private PlayerParameters _gameRunningParameters;
    [SerializeField] private PlayerParameters _gameOverParameters;

    [field: SerializeField] public int Score { get; private set; }


    private void Awake() 
    {
        _playerController.MovementParameters = _gameWaitingParameters;
        _playerController.enabled = false;

        AudioUtility.AudioHandler = _audioHandler;    

        StartCoroutine(_fadeScreen.FadeOut(_fadeTime, _fadeColor));
        _screenController.ShowStartHud();
    }

    public void StartWaiting()
    {
        _playerController.MovementParameters = _gameWaitingParameters;
        _screenController.ShowWaitingHud();
    }

    public void StartWithDelay(float delay)
    {
        StartCoroutine(StartGameRoutine(delay));
    }

    private IEnumerator StartGameRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartGame();
    }

    public void StartGame()
    {
        _playerController.enabled = true;
        _playerController.MovementParameters = _gameRunningParameters;
        _playerController.Flap();
        _pipeGenerator.StartSpawn();
        _screenController.ShowInGameHud();
    }

    public void GameOver()
    {
        _playerController.MovementParameters = _gameOverParameters;
        _screenController.ShowGameOverHud();
        HandleNewScore();
    }

    public void RestartGame()
    {
        StartCoroutine(RestartGameCoroutine());
    }

    private IEnumerator RestartGameCoroutine()
    {
        yield return StartCoroutine(_fadeScreen.FadeIn(_fadeTime, _fadeColor));
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void HandleNewScore()
    {
        int highScore = _gameSaver.CurrentSave.HighestScore;

        if(Score > highScore)
        {
            _gameSaver.SaveGame(new SaveGameData(){ HighestScore = Score } );
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        _screenController.ShowPauseHud();
    }
    
    public void ResumeGame() 
    {
        Time.timeScale = 1;
        _screenController.ShowInGameHud();
    }

    public void IncrementScore()
    {
        Score++;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
