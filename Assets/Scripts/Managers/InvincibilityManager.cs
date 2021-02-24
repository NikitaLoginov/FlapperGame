using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityManager : MonoBehaviour
{
    private bool _isInvincible;
    private bool _isInsideObstacle;
    private float _invincibilityDuration = 5f;
    public float InvincibilityDuration { get { return _invincibilityDuration; } }
    public bool IsInvincible { get { return _isInvincible; } set { _isInvincible = value; } }

}
