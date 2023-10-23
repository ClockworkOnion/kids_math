using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{

    public static StageManager instance;

    private Transform playerCar;
    private CarAnimation carAnimation;
    private Speedometer speedometer;
    private ScoreTracker scoreTracker;
    private Slider currentProblemSlider;
    private Slider totalStageTimeSlider;
    private TouchControlPanel touchControlPanel;
    private GameObject touchControlHints;
    private int targetLane = -1;
    private float drivingSpeed = 15f;
    private float driveSpeedAdjust = 5f; // Value to adjust by for right/wrong answer
    private float timeToSolveLeft = 0f; // Current time left to solve an equation
    private float totalTimeToSolve = 0f; // Total time for the current problem
    private float totalStageTimeLeft = 0f; // The total time left until the stage is over
    private float totalStageTimeTotal = 0f; // Total play time for the stage

    [SerializeField] private float STAGE_TIME_TOTAL = 15f;
    [SerializeField] private float BASE_DRIVE_SPEED = 15f;

    private float countDownTimerLeft = 3f;
    private float countDownTimerTotal = 3f;
    private Equation currentEquation;
    private int rightSolutionPosition = 0;
    private int playerScore = 0;
    private int highScore = 0;
    private State playState;



    private AudioSource audioSource;
    public AudioClip wrongAnswer;
    public AudioClip rightAnswer;

    private readonly MathManager mathManager = new();
    public TextDisplayManager uiDisplay;

    private void Awake()
    {
        // References
        playerCar = GameObject.Find("PlayerCar").GetComponent<Transform>();
        carAnimation = playerCar.gameObject.GetComponentInChildren<CarAnimation>();
        uiDisplay = GameObject.Find("TextDisplayManager").GetComponent<TextDisplayManager>();
        speedometer = GameObject.Find("Speedoneedle").GetComponent<Speedometer>();
        audioSource = GetComponent<AudioSource>();
        currentProblemSlider = GameObject.Find("CurrentProblemSlider").GetComponent<Slider>();
        totalStageTimeSlider = GameObject.Find("StageTimeSlider").GetComponent<Slider>();
        scoreTracker = GetComponent<ScoreTracker>();

        playState = State.countdown;
        highScore = PlayerPrefs.GetInt("Highscore", 0);

        // Hide sliders initally
        currentProblemSlider.gameObject.GetComponent<CanvasGroup>().alpha = 0;
        totalStageTimeSlider.gameObject.GetComponent<CanvasGroup>().alpha = 0;
    }

    private void Start()
    {
        touchControlPanel = GameObject.Find("TouchControlPanel").GetComponent<TouchControlPanel>();
        touchControlHints = GameObject.Find("TouchControlHints");
    }

    void Update()
    {
        // State machine
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
        // Inputs & Menu
        if (Input.GetKeyDown(KeyCode.Escape)) { SceneManager.LoadScene("MainMenuScene"); }
        HandleInput();

        uiDisplay.SetStatusText("Time left: "
            + timeToSolveLeft.ToString().Substring(0, Mathf.Min(3, timeToSolveLeft.ToString().Length))
            + "\nScore: " + playerScore + "\nHighscore: " + highScore);

        // Timer before time is up
        if (timeToSolveLeft > 0f)
        {
            timeToSolveLeft -= Time.deltaTime;
            totalStageTimeLeft -= Time.deltaTime;

            // Update timer sliders
            currentProblemSlider.value = timeToSolveLeft / totalTimeToSolve;
            totalStageTimeSlider.value = totalStageTimeLeft / totalStageTimeTotal;

            return;
        }

        // Track total stage time;
        if (totalStageTimeLeft < 0) TotalTimeIsUp();

        // When time is up
        if (currentEquation.solutionPositions.Contains(targetLane))
        {
            HandleRightAnswer();
            scoreTracker.ProblemDone(true);
        }
        else if (!(drivingSpeed <= 5) && !(targetLane == 2))
        {
            HandleWrongAnswer();
            scoreTracker.ProblemDone(false);
        }

        SetNextEquation();
    }

    void TotalTimeIsUp()
    {
        playState = State.results;
        countDownTimerLeft = 3f;

        scoreTracker.ShowScores();

        // Fade out the progress bars
        float fadeTime = 1;
        float transpAlpha = 0;
        float opaqueAlpha = 1;
        LeanTween.value(gameObject, (float value) => { currentProblemSlider.gameObject.GetComponent<CanvasGroup>().alpha = value; }, opaqueAlpha, transpAlpha, fadeTime).setEaseOutCirc();
        LeanTween.value(gameObject, (float value) => { totalStageTimeSlider.gameObject.GetComponent<CanvasGroup>().alpha = value; }, opaqueAlpha, transpAlpha, fadeTime).setEaseOutCirc();
    }

    private void HandleRightAnswer()
    {
        playerScore += 10;
        audioSource.PlayOneShot(rightAnswer);
        if (playerScore > highScore)
        {
            highScore = playerScore;
            PlayerPrefs.SetInt("Highscore", playerScore);
        }
        LeanTween.value(drivingSpeed, drivingSpeed + driveSpeedAdjust, 1f).setEase(LeanTweenType.easeInBounce).setOnUpdate(setDriveSpeed);

        carAnimation.smokeBurst();
    }

    private void HandleWrongAnswer()
    {
        LeanTween.value(drivingSpeed, drivingSpeed - driveSpeedAdjust, 1f).setEase(LeanTweenType.easeInElastic).setOnUpdate(setDriveSpeed);
        audioSource.PlayOneShot(wrongAnswer);
    }

    void HandleCountDownState()
    {
        // Countdown to next play section
        string[] countdownTexts = { "Ready?", "Set!", "GO!" };
        float progression = (countDownTimerTotal - countDownTimerLeft) / countDownTimerTotal * 2;
        uiDisplay.SetStatusText(countdownTexts[Mathf.RoundToInt(progression)]);
        string nextString = countdownTexts[Mathf.RoundToInt(progression)];
        if (uiDisplay.equationCenter.GetText() != nextString)
        {
            touchControlHints.SetActive(true);
            uiDisplay.ClearDisplay();
            uiDisplay.equationCenter.SetText(countdownTexts[Mathf.RoundToInt(progression)]);
        }

        if (countDownTimerLeft > 0f) { countDownTimerLeft -= Time.deltaTime; }
        else
        {
            touchControlHints.SetActive(false);
            SetNextEquation();
            setDriveSpeed(BASE_DRIVE_SPEED);
            scoreTracker.HideScores();
            playState = State.driving;
            totalStageTimeLeft = STAGE_TIME_TOTAL;
            totalStageTimeTotal = totalStageTimeLeft;

            // Fade in the progress bars
            LeanTween.value(gameObject, (float value) => { currentProblemSlider.gameObject.GetComponent<CanvasGroup>().alpha = value; }, 0, 1, 1).setEaseOutCirc();
            LeanTween.value(gameObject, (float value) => { totalStageTimeSlider.gameObject.GetComponent<CanvasGroup>().alpha = value; }, 0, 1, 1).setEaseOutCirc();
        }
    }

    void HandleResultsState()
    {
        scoreTracker.ShowScores();
        if (Input.GetKeyDown(KeyCode.Space) || touchControlPanel.GetInput() != -1)
        {
            scoreTracker.ResetAll();
            scoreTracker.HideScores();
            playState = State.countdown;
            setDriveSpeed(15f);
        }
    }

    public float getDriveSpeed()
    {
        return drivingSpeed;
    }

    public void setDriveSpeed(float newSpeed)
    {
        drivingSpeed = newSpeed;
        speedometer.rotateTo(120 - newSpeed * 4);
    }

    private void SetNextEquation()
    {
        uiDisplay.ClearDisplay();
        currentEquation = mathManager.GetNext();
        uiDisplay.equationCenter.SetText(currentEquation.displayCenter);
        uiDisplay.equationLeft.SetText(currentEquation.displayLeft);
        uiDisplay.equationRight.SetText(currentEquation.displayRight);

        timeToSolveLeft = Mathf.Max(1, 8f - drivingSpeed / 4f);
        totalTimeToSolve = timeToSolveLeft;

    }

    private void HandleInput()
    {
        int keyboardInput = (Input.GetKeyDown("1") ? 1 : Input.GetKeyDown("2") ? 2 : Input.GetKeyDown("3") ? 3 : -1);
        int touchInput = touchControlPanel.GetInput();
        int playerInput = (touchInput != -1) ? touchInput: keyboardInput;

        if (playerInput != -1)
        {
            targetLane = playerInput;
            float targetXPosition = (playerInput == 1 ? -2f : playerInput == 2 ? 0f : 2f);
            LeanTween.cancel(playerCar.gameObject);
            LeanTween.move(playerCar.gameObject, new Vector3(targetXPosition, 0, 0), 1).setEaseInOutCubic().setOnComplete(() => { });
        }
    }

    public enum State
    {
        countdown, driving, results
    }

    public enum GameMode
    {
        addLv1, addLv2, addSubLv1, addSubLv2, addLv3, addSubLv3, findBiggest
    }

}

