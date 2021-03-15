using System;
using System.Collections;
using UnityEngine;

public class HatController : MonoBehaviour
{
    private GameObject _hat;
    [SerializeField] private Rigidbody _hatRb;
    private Vector3 _hatPos;
    private float _force = 8f;

    private void Awake()
    {
        _hat = this.gameObject;
        _hatRb = _hat.GetComponent<Rigidbody>();
        _hatRb.useGravity = false;
        _hatRb.isKinematic = true;
        
        EventBroker.KnockDownHatHandler += KnockDownHat;
    }

    private void KnockDownHat()
    {
        ApplyForce(_force);
        StartCoroutine(DisableHat());
    }

    private void ApplyForce(float force)
    {
        _hat.transform.SetParent(null, true);
        _hatPos = _hat.transform.position;
        _hatRb.isKinematic = false;
        _hatRb.useGravity = true;
        _hatRb.AddForce(Vector3.left * force, ForceMode.Impulse);
    }

    private IEnumerator DisableHat()
    {
        yield return new WaitForSeconds(2f);
        _hat.gameObject.SetActive(false); 
    }

    private void OnDisable()
    {
        StopCoroutine(DisableHat());
        EventBroker.KnockDownHatHandler -= KnockDownHat;
    }

}
