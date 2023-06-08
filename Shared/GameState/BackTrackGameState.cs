namespace Shared.GameState;
[Serializable]
public class BackTrackGameState: AbstractGameState
{
    public int CorrectlyRecognizedNumbers;
    public int Lives;
    public int RequiredNumbersToWin;
    public int ShowNextFieldCounter;
    public int TimeLeft;
    public List<int> PredictableNumbers;

    public BackTrackGameState(int correctlyRecognizedNumbers, int lives, int requiredNumbersToWin, int showNextFieldCounter, int timeLeft, List<int> predictableNumbers)
    {
        CorrectlyRecognizedNumbers = correctlyRecognizedNumbers;
        Lives = lives;
        RequiredNumbersToWin = requiredNumbersToWin;
        ShowNextFieldCounter = showNextFieldCounter;
        TimeLeft = timeLeft;
        PredictableNumbers = predictableNumbers;
    }
}