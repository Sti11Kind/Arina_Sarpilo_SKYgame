using UnityEngine;
using static GameManager;

public class SlalomFlag : MonoBehaviour
{
    public static event TimerEvent OnCorrectPass;
    public static event TimerEvent OnWrongPass;
    private enum Direction {  Left, Right };
    [SerializeField] private Direction direction;
    [SerializeField] private Material goodMat, badMat;
    private bool flagPassed = false;
    public static event GameManager.TimerEvent RacePenalty;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerControl.player != null &&
            PlayerControl.player.position.z < transform.position.z &&
            flagPassed == false)
        {
            Direction passingDirection = Direction.Right;
            if (PlayerControl.player.position.x < transform.position.x)
                passingDirection = Direction.Left;

            flagPassed = true;
            Debug.LogError("Player passed on: " + passingDirection);
            MeshRenderer renderer = GetComponent<MeshRenderer>();
            if (passingDirection == direction)
            {
                renderer.material = goodMat;

                if (OnCorrectPass != null)
                    OnCorrectPass.Invoke();
            }
            else
            {
                renderer.material = badMat;

                if (RacePenalty != null)
                    RacePenalty.Invoke();

                if (OnWrongPass != null)
                    OnWrongPass.Invoke();
            }

            if (passingDirection == direction)
            {
                Debug.LogError("passed on correct side");
            }
            else
            {
                Debug.LogError("passed on other side");
            }
        }
    }
}
