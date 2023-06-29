using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{

    public static StageManager instance;

    private Transform playerCar;
    private Speedometer speedometer;
    private int targetLane = -1;
    private float drivingSpeed = 10f;
    private float driveSpeedAdjust = 5f; // Value to adjust by for right/wrong answer
    private float timeToSolve = 0f; // Time left to solve an equation
    private float totalTimeLeft = 0f; // The total time left until the stage is over
    private float countDownTimer = 5f;
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
        uiDisplay = GameObject.Find("TextDisplayManager").GetComponent<TextDisplayManager>();
        speedometer = GameObject.Find("Speedoneedle").GetComponent<Speedometer>();
        audioSource = GetComponent<AudioSource>();

        playState = State.countdown;
        highScore = PlayerPrefs.GetInt("Highscore", 0);
    }

    private void Start()
    {
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
			+ timeToSolve.ToString().Substring(0, Mathf.Min(3, timeToSolve.ToString().Length))
			+ "\nScore: " + playerScore + "\nHighscore: " + highScore);

        if (timeToSolve > 0f)
        {
            timeToSolve -= Time.deltaTime;
            return;
        }

        if ( currentEquation.solutionPositions.Contains(targetLane) )
        {
            playerScore += 10;
            audioSource.PlayOneShot(rightAnswer);
            if (playerScore > highScore)
            {
                highScore = playerScore;
                PlayerPrefs.SetInt("Highscore", playerScore);
            }
            LeanTween.value(drivingSpeed, drivingSpeed + driveSpeedAdjust, 1f).setEase(LeanTweenType.easeInBounce).setOnUpdate(setDriveSpeed);
        }
        else if (!(drivingSpeed <= 5))
        {
            LeanTween.value(drivingSpeed, drivingSpeed - driveSpeedAdjust, 1f).setEase(LeanTweenType.easeInElastic).setOnUpdate(setDriveSpeed);
            audioSource.PlayOneShot(wrongAnswer);
        }

        SetNextEquation();
    }

    void HandleCountDownState()
    {
        string[] countdownTexts = { "Ready?", "Set!", "GO!" };
        float progression = (5 - countDownTimer) / 5 * 2;
        uiDisplay.SetStatusText(countdownTexts[Mathf.RoundToInt(progression)]);
        if (countDownTimer > 0f) { countDownTimer -= Time.deltaTime; }
        else
        {
            SetNextEquation();
            playState = State.driving;
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

        timeToSolve = Mathf.Max(1, 8f - drivingSpeed / 7f);
    }

    private void HandleInput()
    {
        int playerInput = (Input.GetKeyDown("1") ? 1 : Input.GetKeyDown("2") ? 2 : Input.GetKeyDown("3") ? 3 : -1);
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
        countdown, driving
    }

    public enum GameMode
    {
        equationLvl1, findBiggest
    }

}

