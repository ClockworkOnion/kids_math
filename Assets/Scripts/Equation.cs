
public struct Equation
{
    public string displayLeft;
    public string displayCenter;
    public string displayRight;
    public int solution;
    public int[] solutionPositions;

    public Equation(string leftString, string rightString, string centerString, int _solution, int[] _solutionPositions)
    {
        displayLeft = leftString;
        displayCenter = centerString;
        displayRight = rightString;
        solution = _solution;
        solutionPositions = _solutionPositions;
    }
}
