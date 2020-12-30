using System;
using UnityEngine;

public class EventBroker : MonoBehaviour
{
    public static event Action PlayerControllerContinueHandler;
    public static event Action ContinueGameHandler;
    public static event Action<int> UpdateScoreHandler;
    public static event Action DifficultyHandler;

    //Buttons
    public static event Action ForwardDashHandler;
    public static event Action SlowMotionHandler;
    public static event Action InvincibilityHandler;

    public static event Action GameOverHandler;
    public static event Action RestartGameHandler;

    public static void PlayerControllerCallContinue()
    {
        if (PlayerControllerContinueHandler != null)
            PlayerControllerContinueHandler();
    }

    public static void CallContinueGame()
    {
        if (ContinueGameHandler != null)
            ContinueGameHandler();
    }

    public static void CallUpdateScore(int scoreValue)
    {
        if (UpdateScoreHandler != null)
            UpdateScoreHandler(scoreValue);
    }
    public static void CallDifficultyModifier()
    {
        if (DifficultyHandler != null)
            DifficultyHandler();
    }

    //Buttons
    public static void CallForwardDash()
    {
        if (ForwardDashHandler != null)
            ForwardDashHandler();
    }

    public static void CallSlowMotion()
    {
        if (SlowMotionHandler != null)
            SlowMotionHandler();
    }

    public static void CallInvincibility()
    {
        if (InvincibilityHandler != null)
            InvincibilityHandler();
    }

    public static void CallGameOver()
    {
        if (GameOverHandler != null)
            GameOverHandler();
    }

    public static void CallRestartGame()
    {
        if (RestartGameHandler != null)
            RestartGameHandler();
    }
}
