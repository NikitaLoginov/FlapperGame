using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityManager : MonoBehaviour
{
    private bool _isInvincible;
    private bool _isInsideObstacle;
    private float _invincibilityDuration = 5f;
    private float _invincibilityGravityModifier = 0f;
    private Vector3 _invincibilityPosition = new Vector3(2.5f, 3f, 0);
    public float InvincibilityDuration { get { return _invincibilityDuration; } }
    public bool IsInvincible { get { return _isInvincible; } set { _isInvincible = value; } }
    public bool IsInsideObstacle { get { return _isInsideObstacle; } set { _isInsideObstacle = value; } }

    public float InvincibilityGravityModifier { get { return _invincibilityGravityModifier; } }

    public Vector3 InvincibilityPosition { get { return _invincibilityPosition; } }

}
