using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityManager : MonoBehaviour
{
    bool _isInvincible;
    float _invincibilityDuration = 5f;
    float _invincibilityGravityModifier = 0f;
    string _name = "Invincibility";
    Vector3 _invincibilityPosition = new Vector3(2.5f, 3f, 0);
    public float InvincibilityDuration { get { return _invincibilityDuration; } }
    public bool IsInvincible { get { return _isInvincible; } set { _isInvincible = value; } }

    public float InvincibilityGravityModifier { get { return _invincibilityGravityModifier; } }
    public string Name { get { return _name; } }

    public Vector3 InvincibilityPosition { get { return _invincibilityPosition; } }

}
