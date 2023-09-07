using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchControlPanel : MonoBehaviour
{
    private int inputNumber = -1;

    public void SetInputByTouch(int targetLane)
    {
        inputNumber = targetLane;
    }

    public void ResetInput()
    {
        inputNumber = -1;
    }

    public int GetInput()
    {
        int temp = inputNumber;
        ResetInput();
        return temp;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

}
