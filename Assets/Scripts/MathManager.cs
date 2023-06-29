using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathManager
{
    public Equation LevelOneEquation()
    {
        int addend1 = Random.Range(1, 5);
        int addend2 = Random.Range(1, 5);
        int solution = addend1 + addend2;

        int notASolution = Random.Range(1, 11); // Create alternative (wrong) solution
        while (solution == notASolution) { notASolution = Random.Range(1, 11); } // Make sure the presented solutions are not the same

        string leftDisplay, rightDisplay, centerDisplay;

        // Place right and wrong solutions
        bool correctAnswerLeft = (Random.Range(0, 2) == 1);
        leftDisplay = (correctAnswerLeft ? solution.ToString() : notASolution.ToString());
        rightDisplay = (correctAnswerLeft ? notASolution.ToString() : solution.ToString());

        centerDisplay = addend1.ToString() + " + " + addend2.ToString();

        int[] solutionPosition = new int[1];
        solutionPosition[0] = correctAnswerLeft ? 1 : 3;

        return new Equation(leftDisplay, rightDisplay, centerDisplay, solution, solutionPosition);
    }

    public Equation FindLargest()
    {
        int[] values = new int[3];
        string[] strings = new string[3];

        int maxValue = 0;
        for (int i = 0; i < 3; i++)
        {
            int n1 = Random.Range(1, 5);
            int n2 = Random.Range(1, 5);
            values[i] = n1 + n2;
            if (values[i] > maxValue) maxValue = values[i]; 
            strings[i] = n1.ToString() + " + " + n2.ToString();
        }

        List<int> solutionList = new List<int>();
        int index = 1;
        foreach (var v in values)
        {
            if (v >= maxValue) solutionList.Add(index);
            index++;
        }
        Debug.Log("values List:");
        foreach (var v in values )
        {
            Debug.Log(v);
        }
        Debug.Log("Max:" + maxValue.ToString());

        return new Equation(leftString: strings[0], centerString: strings[1], rightString: strings[2], _solution: maxValue, _solutionPositions: solutionList.ToArray());
    }

    public Equation GetNext() {
        StageManager.GameMode currentGameMode = GameManager.GetInstance().GetGameMode();
        switch (currentGameMode) {

            case StageManager.GameMode.equationLvl1:
                return LevelOneEquation();
            case StageManager.GameMode.findBiggest:
                return FindLargest();
            default:
                Debug.LogError("MathManager GetNext was asked to return invalid type. Returning LevelOneEquation instead");
                return LevelOneEquation();
	}
    }
}
