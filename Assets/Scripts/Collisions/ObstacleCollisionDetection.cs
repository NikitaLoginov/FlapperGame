using UnityEngine;

public class ObstacleCollisionDetection : MonoBehaviour
{
    //to push off hats
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hat"))
        {
            EventBroker.CallKnockDownHat();
        }
        else if (other.CompareTag("Player"))
        {
            EventBroker.CallGameOver();
        }
    }
}
