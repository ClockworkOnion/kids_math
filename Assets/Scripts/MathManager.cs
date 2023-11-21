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
            // Add 
            case StageManager.GameMode.addLv1:
                return AddEquation(individualCeiling: 4, solutionCeiling: 5, proximity: 3);
            case StageManager.GameMode.addLv2:
                return AddEquation(individualCeiling: 10, solutionCeiling: 11, proximity: 5);
            case StageManager.GameMode.addLv3:
                return AddEquation(individualCeiling: 10, solutionCeiling: 20, proximity: 5);
            case StageManager.GameMode.addLv4:
                return AddEquation(individualCeiling: 20, solutionCeiling: 40, proximity: 5);
            case StageManager.GameMode.addLv5:
                return AddEquation(individualCeiling: 99, solutionCeiling: 100, proximity: 8);

	        // Multiply
            case StageManager.GameMode.multLv1:
                return MultEquation(individualCeiling: 3, solutionCeiling: -1, proximity: 2);
            case StageManager.GameMode.multLv2:
                return MultEquation(individualCeiling: 5, solutionCeiling: -1, proximity: 4);
            case StageManager.GameMode.multLv3:
                return MultEquation(individualCeiling: 9, solutionCeiling: -1, proximity: 8);

	        // Subtract
            case StageManager.GameMode.subLv1:
                return SubtractEquation(individualCeiling: 5, proximity: 2);
            case StageManager.GameMode.subLv2:
                return SubtractEquation(individualCeiling: 10, proximity: 3);
            case StageManager.GameMode.subLv3:
                return SubtractEquation(individualCeiling: 19, proximity: 5);
            case StageManager.GameMode.subLv4:
                return SubtractEquation(individualCeiling: 30, proximity: 5);

	        // Divide -> 9 / 3 = ? -> 3 x ? = 9
            case StageManager.GameMode.divLv1:
                return DivideEquation(individualCeiling: 3, proximity: 2);
            case StageManager.GameMode.divLv2:
                return DivideEquation(individualCeiling: 9, proximity: 5);

	        // Add or Multiply
            case StageManager.GameMode.addMultLv3:
                return RandBool() ? MultEquation(individualCeiling: 9, solutionCeiling: -1, proximity: 8) : 
		            AddEquation(individualCeiling: 10, solutionCeiling: 20, proximity: 5);
	
	        // Add or Subtract
            case StageManager.GameMode.addSubLv1:
                return RandBool() ? SubtractEquation(individualCeiling: 5, proximity: 2) : 
		            AddEquation(individualCeiling: 4, solutionCeiling: 5, proximity: 3);
            case StageManager.GameMode.addSubLv2:
                return RandBool() ? SubtractEquation(individualCeiling: 10, proximity: 3) : 
		            AddEquation(individualCeiling: 10, solutionCeiling: 11, proximity: 5);
	
	        // others
            case StageManager.GameMode.findBiggest:
                return FindLargest();

            default:
                Debug.LogError("MathManager GetNext was asked to return invalid type. Returning LevelOneEquation instead");
                return AddEquation(individualCeiling: 4, solutionCeiling: 5, proximity: 5);
        }
    }

    public Equation AddEquation(int individualCeiling, int solutionCeiling, int proximity)
    {
        int solution = Random.Range(1, solutionCeiling);
        int addend1 = Random.Range(1, Mathf.Min((individualCeiling + 1), solution - 2));
        int addend2 = solution - addend1;

        int notASolution = solution;
        while (notASolution == solution || notASolution <= 0)
        {
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

    public Equation MultEquation(int individualCeiling, int solutionCeiling, int proximity) {
        int multiplicand1 = Random.Range(1, individualCeiling+1);
        int multiplicand2 = Random.Range(1, individualCeiling+1);

        // Multiplicand of 1 is too easy, so increase it half the time.
        if (multiplicand1 == 1 && RandBool()) multiplicand1++;
        if (multiplicand2 == 1 && RandBool()) multiplicand2++;

        int solution = multiplicand1 * multiplicand2;
        int notASolution = AltChoice(solution, proximity);

        // Place right and wrong solutions
        string leftDisplay, rightDisplay;
        bool correctAnswerLeft = (Random.Range(0, 2) == 1);
        leftDisplay = (correctAnswerLeft ? solution.ToString() : notASolution.ToString());
        rightDisplay = (correctAnswerLeft ? notASolution.ToString() : solution.ToString());

        string centerDisplay = multiplicand1.ToString() + " * " + multiplicand2.ToString();

        int[] solutionPosition = new int[1];
        solutionPosition[0] = correctAnswerLeft ? 1 : 3;

        return new Equation(leftDisplay, rightDisplay, centerDisplay, solution, solutionPosition);
    }

    public Equation SubtractEquation(int individualCeiling, int proximity)
    {
        int subtrahend1 = Random.Range(1, individualCeiling);
        int subtrahend2 = Random.Range(1, individualCeiling);
        int solution = Mathf.Max(subtrahend1, subtrahend2) - Mathf.Min(subtrahend1, subtrahend2);
        int notASolution = AltChoice(solution, proximity);

        // Place right and wrong solutions
        string leftDisplay, rightDisplay;
        bool correctAnswerLeft = (Random.Range(0, 2) == 1);
        leftDisplay = (correctAnswerLeft ? solution.ToString() : notASolution.ToString());
        rightDisplay = (correctAnswerLeft ? notASolution.ToString() : solution.ToString());

        string centerDisplay = Mathf.Max(subtrahend1, subtrahend2).ToString() +
	         " - " + Mathf.Min(subtrahend1, subtrahend2).ToString();

        int[] solutionPosition = new int[1];
        solutionPosition[0] = correctAnswerLeft ? 1 : 3;

        return new Equation(leftDisplay, rightDisplay, centerDisplay, solution, solutionPosition);
    }

    public Equation DivideEquation(int individualCeiling, int proximity) {
        int dividend1 = Random.Range(1, individualCeiling + 1);
        int dividend2 = Random.Range(1, individualCeiling + 1);
        int solution = dividend1 * dividend2;
        int notASolution = AltChoice(dividend1, proximity);

        // Place right and wrong solutions
        string leftDisplay, rightDisplay;
        bool correctAnswerLeft = (Random.Range(0, 2) == 1);
        leftDisplay = (correctAnswerLeft ? dividend1.ToString() : notASolution.ToString());
        rightDisplay = (correctAnswerLeft ? notASolution.ToString() : dividend1.ToString());

        string centerDisplay = solution.ToString() + " / " + dividend2.ToString();

        int[] solutionPosition = new int[1];
        solutionPosition[0] = correctAnswerLeft ? 1 : 3;

        return new Equation(leftDisplay, rightDisplay, centerDisplay, solution, solutionPosition);
    }

    /// <summary>
    /// Creates an alternative (i.e. wrong answer) to given solution, while making sure
    /// the solution is within the given proximity, greater than 0 and not equal
    /// to the actual solution. 
    /// </summary>
    /// <param name="solution">The correct solution</param>
    /// <param name="proximity">Maximum difference to given solution. Lower numbers make for more difficult problems</param>
    /// <returns>An integer, different from the given solution and greater than 0, within the specified proximity</returns>
    private int AltChoice(int solution, int proximity) {
        int difference = Random.Range(1, proximity);
        int alternative = Mathf.Max(1, Mathf.Abs(solution + difference * RandSign()));
        return (alternative == solution) ? ++alternative : alternative;
    }

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
