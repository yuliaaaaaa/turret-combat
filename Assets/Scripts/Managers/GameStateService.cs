using System;
public enum GameState
{
    WaitingForStart = 0,
    Playing = 1,
    Paused = 2,
    Win = 3,
    Lose = 4
}
public class GameStateService
{
    public GameState CurrentState { get; private set; } = GameState.WaitingForStart;

    public event Action<GameState> StateChanged;

    public void ResetToWaiting(bool notify = true)
    {
        SetState(GameState.WaitingForStart, notify);
    }

    public void StartGame()
    {
        if (CurrentState != GameState.WaitingForStart)
            return;

        SetState(GameState.Playing);
    }

    public void PauseGame()
    {
        if (CurrentState != GameState.Playing)
            return;

        SetState(GameState.Paused);
    }

    public void ResumeGame()
    {
        if (CurrentState != GameState.Paused)
            return;

        SetState(GameState.Playing);
    }

    public void SetWin()
    {
        if (IsTerminal(CurrentState))
            return;

        SetState(GameState.Win);
    }

    public void SetLose()
    {
        if (IsTerminal(CurrentState))
            return;

        SetState(GameState.Lose);
    }

    private void SetState(GameState newState, bool notify = true)
    {
        CurrentState = newState;

        if (notify)
            StateChanged?.Invoke(CurrentState);
    }

    private static bool IsTerminal(GameState state)
    {
        return state == GameState.Win || state == GameState.Lose;
    }
}