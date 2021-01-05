using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsTapController : MonoBehaviour
{

    List<Transform> coins = new List<Transform>();
    int _pointsForCoin = 1;

    private void Start()
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            coins.Add(transform.GetChild(i));
        }
    }


    void Update()
    {
        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
        {
            //Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;

            //something hit
            if (Physics.Raycast(raycast, out raycastHit))
            {
                for (int i = 0; i < coins.Count; i++)
                {
                    if (raycastHit.collider.transform.position == coins[i].position)
                    {
                        raycastHit.collider.gameObject.SetActive(false);
                        EventBroker.CallUpdateScore(_pointsForCoin);
                    }

                }
            }

        }
    }
}
