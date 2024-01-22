using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class StageManager : MonoBehaviour
{
    public GameObject goalFlag; // Set in inspector
    public CanvasGroup whiteFader;

    private CarAnimation carAnimation;
    private CarCustomizer carCustomizer;
    private GameObject touchControlHints;
    private List<ParticleSystem> fireworks;
    private ScoreTracker scoreTracker;
    private Slider currentProblemSlider;
    private Slider totalStageTimeSlider;
    private Speedometer speedometer;
    private TextDisplayManager uiDisplay;
    private TouchControlPanel touchControlPanel;
    private Transform playerCar;

    private int currentLane = -1;
    private float drivingSpeed = 1f;
    private readonly float driveSpeedAdjust = 1f; // Value to adjust by for right/wrong answer
    private float currentEquationTimeLeft = 0f; // Current time left to solve an equation
    private float currentEquationTotalTime = 0f; // Total time for the current problem
    private float stageTimeLeft = 0f; // The total time left until the stage is over
    private float stageTimeTotal = 0f; // Total play time for the stage
    private float countDownTimerLeft = 3f;
    private readonly float countDownTimerTotal = 3f;
    private float whiteFaderAlpha = 0f;

    [SerializeField] private float STAGE_TIME_TOTAL = 15f;
    [SerializeField] private float BASE_DRIVE_SPEED = 1f; // Reset drive speed to this value after a round

    private int highScore = 0;
    private int playerScore = 0;
    private int speedChangeThreshold;

    private Equation currentEquation;
    private State playState;

    private AudioSource audioSource;
    public AudioClip wrongAnswer;
    public AudioClip rightAnswer;

    public UnityEvent stageEnd = new();
    public UnityEvent stageStart = new();
    public UnityEvent zoomOut = new();

    private readonly MathManager mathManager = new();

    private void Awake()
    {
        // References
        playerCar = GameObject.Find("PlayerCar").GetComponent<Transform>();
        carCustomizer = playerCar.GetComponentInChildren<CarCustomizer>();
        audioSource = GetComponent<AudioSource>();
        carAnimation = playerCar.gameObject.GetComponentInChildren<CarAnimation>();
        currentProblemSlider = GameObject.Find("CurrentProblemSlider").GetComponent<Slider>();
        fireworks = GameObject.Find("Fireworks").GetComponentsInChildren<ParticleSystem>().ToList();
        scoreTracker = GetComponent<ScoreTracker>();
        speedometer = GameObject.Find("Speedoneedle").GetComponent<Speedometer>();
        totalStageTimeSlider = GameObject.Find("StageTimeSlider").GetComponent<Slider>();
        touchControlHints = GameObject.Find("TouchControlHints");
        touchControlPanel = GameObject.Find("TouchControlPanel").GetComponent<TouchControlPanel>();
        uiDisplay = GameObject.Find("TextDisplayManager").GetComponent<TextDisplayManager>();

        playState = State.countdown;
        highScore = PlayerPrefs.GetInt("Highscore", 0);

        FadeProgressBars(fadeTime: 0, onOrOff: false);
    }

    private void Start()
    {
        // Set up selected material on player car
        Material currentMat = GameManager.GetInstance().carMaterial;
        fireworks.ForEach((fw) => fw.Stop());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMenu();
        }

        switch (playState)
        {
            case State.driving:
                HandleDrivingState();
                break;
            case State.countdown:
                HandleCountDownState();
                break;
            case State.results:
                HandleResultsState();
                break;
            default:
                Debug.LogError("No valid game state!");
                break;
        }
    }

    void HandleDrivingState()
    {
        // for testing purposes: End round early by pressing E+R
        if (Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.R))
            EndRound();

        HandleDrivingInput();

        uiDisplay.SetStatusText("Time left: "
            + currentEquationTimeLeft.ToString().Substring(0, Mathf.Min(3, currentEquationTimeLeft.ToString().Length))
            + "\nScore: " + playerScore + "\nHighscore: " + highScore + "\nCoins: " + PlayerPrefs.GetInt("player_money"));

        // Track total stage time;
        if (stageTimeLeft < 0)
            goalFlag.SetActive(true);

        // Timer before time is up
        if (currentEquationTimeLeft > 0f)
        {
            currentEquationTimeLeft -= Time.deltaTime;
            stageTimeLeft -= Time.deltaTime;

            // Update timer sliders
            currentProblemSlider.value = currentEquationTimeLeft / currentEquationTotalTime;
            totalStageTimeSlider.value = stageTimeLeft / stageTimeTotal;
            return;
        }

        // When time is up
        HandleAnswer((currentEquation.solutionPositions.Contains(currentLane)));

        SetNextEquation();
    }

    public void EndRound()
    {
        stageEnd.Invoke();
        goalFlag.SetActive(false);
        playState = State.results;
        countDownTimerLeft = 3f;

        scoreTracker.ShowScores(playerScore);
        GameManager.GetInstance().AddPlayerMoney(playerScore);

        FadeProgressBars(fadeTime: 1, onOrOff: false);
    }

    private void HandleAnswer(bool solvedCorrectly)
    {
        audioSource.PlayOneShot(solvedCorrectly ? rightAnswer : wrongAnswer);
        AddScore(scoreTracker.ProblemDone(solvedCorrectly, GameManager.GetInstance().GetGameMode()));

        if (solvedCorrectly)
        {
            carAnimation.smokeBurst();

            // Adjust speed ticker
            speedChangeThreshold++;
            if (speedChangeThreshold > 2)
            {
                TweenDriveSpeedTo(drivingSpeed + driveSpeedAdjust);
                speedChangeThreshold = 0;
            }
            return;
        }

        // Not solved correctly...

        // Get a pass when too slow or in the middle lane...
        if (drivingSpeed <= 1 || currentLane == 2)
            return;

        // Adjust speed ticker
        speedChangeThreshold--;
        if (speedChangeThreshold < -1)
        {
            TweenDriveSpeedTo(drivingSpeed - driveSpeedAdjust);
            speedChangeThreshold = 0;
        }
    }

    private void AddScore(int score)
    {
        playerScore += score;
        if (playerScore > highScore)
        {
            highScore = playerScore;
            PlayerPrefs.SetInt("Highscore", playerScore);
        }
    }

    void HandleCountDownState()
    {
        // Countdown to next play section
        string[] countdownTexts = { "Ready?", "Set!", "GO!" };
        float progression = (countDownTimerTotal - countDownTimerLeft) / countDownTimerTotal * 2;
        string currentDisplay = countdownTexts[Mathf.RoundToInt(progression)];
        uiDisplay.SetStatusText(currentDisplay);


        if (uiDisplay.equationCenter.GetText() != currentDisplay) // Don't refresh the string constantly
        {
            touchControlHints.SetActive(true);
            uiDisplay.ClearDisplay();
            uiDisplay.equationCenter.SetText(currentDisplay);
        }

        if (countDownTimerLeft > 0f)
        {
            countDownTimerLeft -= Time.deltaTime;
            return;
        }

        // Setup next driving state
        touchControlHints.SetActive(false);
        SetNextEquation();
        SetDriveSpeed(BASE_DRIVE_SPEED);
        scoreTracker.HideScores();
        playState = State.driving;
        stageTimeLeft = stageTimeTotal = STAGE_TIME_TOTAL;
        FadeProgressBars(fadeTime: 1, onOrOff: true);
    }

    void HandleResultsState()
    {
        //scoreTracker.ShowScores();
        fireworks.ForEach((fw) => { if (fw.isStopped) fw.Play(); });

        if (Input.GetKeyDown(KeyCode.Space) || touchControlPanel.GetInput() != -1)
            RepeatStage();

        if (Input.GetKeyDown(KeyCode.N) && scoreTracker.GetRatio() > .7f)
            StartNextStage();
    }

    public void FadeToMenu()
    {
        zoomOut.Invoke();
        float fadeTime = 1f;
        LeanTween.value(gameObject, UpdateFloat, 0f, 1f, fadeTime).setEaseInCubic().setOnComplete(BackToMenu);

        void UpdateFloat(float alpha)
        {
            whiteFader.alpha = alpha;
        }
    }

    public void StartNextStage()
    {
        int currentProgress = Mathf.Min(PlayerPrefs.GetInt("stageProgression") + 1, GameManager.GetInstance().stageProgression.Count - 1);
        PlayerPrefs.SetInt("stageProgression", currentProgress);
        GameMode nextGameMode = GameManager.GetInstance().stageProgression[currentProgress];
        GameManager.GetInstance().SetGameMode(nextGameMode);
        SceneManager.LoadScene("EasyModeScene");
    }

    public void RepeatStage()
    {
        fireworks.ForEach((fw) => fw.Stop());
        scoreTracker.ResetAll();
        scoreTracker.HideScores();
        playState = State.countdown;
        TweenDriveSpeedTo(BASE_DRIVE_SPEED);
        stageStart.Invoke();
    }

    public float getDriveSpeed()
    {
        return drivingSpeed;
    }


    public void TweenDriveSpeedTo(float newSpeed)
    {
        LeanTween.value(drivingSpeed, newSpeed, 1f)
            .setEase(LeanTweenType.easeInElastic)
            .setOnUpdate(SetDriveSpeed);
    }

    public void SetDriveSpeed(float newSpeed)
    {
        drivingSpeed = newSpeed;
        speedometer.rotateTo(120 - newSpeed * 12);
    }

    private void SetNextEquation()
    {
        currentEquation = mathManager.GetNext();
        uiDisplay.ClearDisplay();
        uiDisplay.equationCenter.SetText(currentEquation.displayCenter);
        uiDisplay.equationLeft.SetText(currentEquation.displayLeft);
        uiDisplay.equationRight.SetText(currentEquation.displayRight);

        currentEquationTimeLeft = 5 - drivingSpeed;
        currentEquationTimeLeft = Mathf.Clamp(currentEquationTimeLeft, 1, 3);
        currentEquationTotalTime = currentEquationTimeLeft;

    }

    private void FadeProgressBars(float fadeTime, bool onOrOff)
    {
        float targetAlpha = onOrOff ? 1f : 0f;
        float startAlpha = onOrOff ? 0f : 1f;
        LeanTween.value(gameObject, SetBarsAlpha, startAlpha, targetAlpha, fadeTime).setEaseOutCirc();
        LeanTween.value(gameObject, SetBarsAlpha, startAlpha, targetAlpha, fadeTime).setEaseOutCirc();

        void SetBarsAlpha(float alpha)
        {
            currentProblemSlider.gameObject.GetComponent<CanvasGroup>().alpha = alpha;
            totalStageTimeSlider.gameObject.GetComponent<CanvasGroup>().alpha = alpha;
        }
    }

    private void HandleDrivingInput()
    {
        int keyboardInput = Input.GetKeyDown("1") ? 1 : Input.GetKeyDown("2") ? 2 : Input.GetKeyDown("3") ? 3 : -1;
        int touchInput = touchControlPanel.GetInput();
        int playerInput = (touchInput != -1) ? touchInput : keyboardInput;

        if (playerInput != -1)
        {
            currentLane = playerInput;
            float targetXPosition = (playerInput == 1 ? -2f : playerInput == 2 ? 0f : 2f);
            LeanTween.cancel(playerCar.gameObject);
            LeanTween.move(playerCar.gameObject, new Vector3(targetXPosition, 0, 0), 1)
                .setEaseInOutCubic();
        }
    }

    public void BackToMenu()
    {
        LeanTween.cancelAll();
        SceneManager.LoadScene("MainMenuScene");
    }

    public enum State
    {
        countdown, driving, results
    }

    public enum GameMode
    {
        addLv1, addLv2, addLv3, addLv4, addLv5,
        subLv1, subLv2, subLv3, subLv4,
        addSubLv1, addSubLv2, addSubLv3, addSubLv4,

        multLv1, multLv2, multLv3, multLv4,
        divLv1, divLv2, divLv3, divLv4,
        multDivLv1, multDivLv2, multDivLv3, multDivLv4,

        addSubMultDivLv1, addSubMultDivLv2, addSubMultDivLv3, addSubMultDivLv4,

        findBiggest, addMultLv3, addMultLv4,
    }

}
