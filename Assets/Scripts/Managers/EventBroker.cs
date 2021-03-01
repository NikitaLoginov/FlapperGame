using System;
using UnityEngine;

public class EventBroker : MonoBehaviour
{
    public static event Action<int> UpdateScoreHandler;
    public static event Action DifficultyHandler;
    public static event Action CanStartGameHandler;
    public static event Action KnockDownHatHandler;

    //Buttons
    public static event Action ForwardDashHandler;
    public static event Action SlowMotionHandler;
    public static event Action InvincibilityHandler;

    public static event Action GameOverHandler;
    public static event Action RestartGameHandler;
    public static event Action ShopButtonHandler;
    public static event Action BackButtonHandler;
    public static event Action StartButtonHandler;
    public static event Action<int> ChangeHatHandler;
    public static event Action PauseGameHandler;
    public static event Action ResumeGameHandler;
    public static event Action MainMenuButtonHandler;


    public static void CallCanStartGame()
    {
        CanStartGameHandler?.Invoke();
    }
    public static void CallUpdateScore(int scoreValue)
    {
        if (UpdateScoreHandler != null)
            UpdateScoreHandler(scoreValue);
    }
    public static void CallDifficultyModifier()
    {
        DifficultyHandler?.Invoke();
    }

    public static void CallKnockDownHat()
    {
        KnockDownHatHandler?.Invoke();
    }

    public static void CallGameOver()
    {
        GameOverHandler?.Invoke();
    }

    //Buttons
    public static void CallForwardDash()
    {
        ForwardDashHandler?.Invoke();
    }

    public static void CallSlowMotion()
    {
        SlowMotionHandler?.Invoke();
    }

    public static void CallInvincibility()
    {
        InvincibilityHandler?.Invoke();
    }


    public static void CallRestartGame()
    {
        RestartGameHandler?.Invoke();
    }

    public static void CallOnShopButtonPressed()
    {
        ShopButtonHandler?.Invoke();
    }

    public static void CallOnBackButtonPressed()
    {
        BackButtonHandler?.Invoke();
    }

    public static void CallOnStartButtonPressed()
    {
        StartButtonHandler?.Invoke();
    }

    public static void CallChangeHat(int ID)
    {
        ChangeHatHandler?.Invoke(ID);
    }

    public static void CallPauseGame()
    {
        PauseGameHandler?.Invoke();
    }

    public static void CallResumeGame()
    {
        ResumeGameHandler?.Invoke();
    }

    public static void CallMainMenu()
    {
        MainMenuButtonHandler?.Invoke();
    }
}
