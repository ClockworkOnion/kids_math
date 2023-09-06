using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathManager
{
    public Equation GetNext()
    {
        StageManager.GameMode currentGameMode = GameManager.GetInstance().GetGameMode();
        switch (currentGameMode)
        {
            case StageManager.GameMode.addLv1:
                return AddEquation(individualCeiling: 4, solutionCeiling: 5, proximity: 3);
            case StageManager.GameMode.addLv2:
                return AddEquation(individualCeiling: 10, solutionCeiling: 11, proximity: 5);
            case StageManager.GameMode.addLv3:
                return AddEquation(individualCeiling: 10, solutionCeiling: 20, proximity: 5);
            case StageManager.GameMode.findBiggest:
                return FindLargest();
            default:
                Debug.LogError("MathManager GetNext was asked to return invalid type. Returning LevelOneEquation instead");
                return AddEquation(individualCeiling: 4, solutionCeiling: 5, proximity: 5);
        }
    }

    public Equation Level2Equation()
    {
        return AddEquation(individualCeiling: 10, solutionCeiling: 10, proximity: 10);
    }

    public Equation AddEquation(int individualCeiling, int solutionCeiling, int proximity)
    {
        int solution = Random.Range(1, solutionCeiling);
        int addend1 = Random.Range(1, Mathf.Min((individualCeiling + 1), solution - 2));
        int addend2 = solution - addend1;

        int notASolution = solution;
        while (notASolution == solution || notASolution <= 0)
        {
            int alternative =
        notASolution = Mathf.Min(solutionCeiling, solution + Random.Range(1, proximity) * RandSign());
        }

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

    public Equation SubtractEquation(int individualCeiling, int solutionCeiling)
    {
        int solution = Random.Range(1, solutionCeiling + 1);
        int addend1 = Random.Range(1, individualCeiling + 1);
        int addend2 = addend1 - solution;

        int notASolution = Random.Range(1, solutionCeiling); // Create alternative (wrong) solution
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

    //public Equation MultEquation(int individualCeiling, int solutionCeiling) {
    //    int addend1 = Random.Range(1, individualCeiling);
    //    int otherMax = 0;
    //    while (addend1 * otherMax < solutionCeiling) { otherMax++; }
    //    int addend2 = Random.Range(1, otherMax);
    //    int solution = addend1 * addend2;

    //}

    public bool RandBool()
    {
        return Random.Range(0, 2) == 1;
    }

    public int RandSign()
    {
        return (RandBool() ? 1 : -1);
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
        foreach (var v in values)
        {
            Debug.Log(v);
        }
        Debug.Log("Max:" + maxValue.ToString());

        return new Equation(leftString: strings[0], centerString: strings[1], rightString: strings[2], _solution: maxValue, _solutionPositions: solutionList.ToArray());
    }

}
