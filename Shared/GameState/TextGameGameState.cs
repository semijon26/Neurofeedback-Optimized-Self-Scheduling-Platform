namespace Shared.GameState;
[Serializable]
public class TextGameGameState : AbstractGameState
{
    public int ErrorCount;
    public string TargetText;
    public int TimeLeft;
    public int FullWordsWritten;
    public TextGameGameState(int errorCount, string targetText, int timeLeft, int fullWordsWritten) 
    {
        ErrorCount = errorCount;
        TargetText = targetText;
        TimeLeft = timeLeft;
        FullWordsWritten = fullWordsWritten;
    }
}