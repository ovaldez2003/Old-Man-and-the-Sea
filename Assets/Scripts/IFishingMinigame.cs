using System;

public interface IFishingMinigame
{
    void StartGame(Action<bool> onComplete);
}