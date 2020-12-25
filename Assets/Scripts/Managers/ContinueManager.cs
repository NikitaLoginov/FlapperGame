using System;
using UnityEngine;

public class ContinueManager : MonoBehaviour
{
    public static event Action ContinueHandler;

    public static void CallContinue()
    {
        if (ContinueHandler != null)
            ContinueHandler();
    }
}
