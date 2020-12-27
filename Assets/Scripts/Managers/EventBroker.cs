using System;
using UnityEngine;

public class EventBroker : MonoBehaviour
{
    public static event Action PlayerControllerContinueHandler;

    public static void CallContinue()
    {
        if (PlayerControllerContinueHandler != null)
            PlayerControllerContinueHandler();
    }
}
