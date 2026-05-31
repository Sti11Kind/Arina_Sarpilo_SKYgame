using UnityEngine;

public class SnowBallDetect : MonoBehaviour
{
    public string snowballTag;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(snowballTag))
        {
            Destroy(collision.gameObject);
        }
    }

}