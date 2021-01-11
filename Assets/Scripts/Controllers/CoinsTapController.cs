using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsTapController : MonoBehaviour
{

    List<Transform> _coins = new List<Transform>();
    int _pointsForCoin = 1;

    private void Start()
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            _coins.Add(transform.GetChild(i));
        }
    }


    void Update()
    {
        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0)) // and bool isSlowMotion
        {
            //Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;

            //something hit
            if (Physics.Raycast(raycast, out raycastHit))
            {
                for (int i = 0; i < _coins.Count; i++)
                {
                    if (raycastHit.collider.transform.position == _coins[i].position)
                    {
                        raycastHit.collider.gameObject.SetActive(false);
                        EventBroker.CallUpdateScore(_pointsForCoin);
                    }

                }
            }

        }
    }
}
