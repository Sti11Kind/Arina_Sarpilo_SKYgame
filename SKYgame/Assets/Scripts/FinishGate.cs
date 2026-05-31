using UnityEngine;
using static GameManager;

public class FinishGate : MonoBehaviour
{
    public static TimerEvent FinishRace;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            FinishRace.Invoke();
        }
    }
}
