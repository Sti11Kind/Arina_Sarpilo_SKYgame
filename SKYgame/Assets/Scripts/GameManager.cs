using UnityEngine;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    public DateTime raceStart;
    private TimeSpan raceTime;
    private TimeSpan penaltyTime;
    private bool racing=false;
    public delegate void TimerEvent();
    private TimeSpan bestTime;
   // [SerializeField] private int penaltyTimeVal = 3; //on
    [SerializeField] private TMP_Text raceTimeText, bestTimeText;
    [SerializeField] private string bestTimeKey = "LVLBestTime";

    [SerializeField] private int flagPenaltyTime = 1; 
    [SerializeField] private int hitPenaltyTime = 3; 

    public static event Action<int> PenaltyAdded; 

    private void OnEnable()
    {
        StarpGate.StartRace += OnRaceStart;
        FinishGate.FinishRace += OnRaceFinish;
        SlalomFlag.RacePenalty += AddFlagPenalty; 
        Obstacles.OnPlayerHit += AddHitPenalty;
    }

    private void OnDisable()
    {
        StarpGate.StartRace -= OnRaceStart;
        FinishGate.FinishRace -= OnRaceFinish;
        SlalomFlag.RacePenalty -= AddFlagPenalty;  
        Obstacles.OnPlayerHit -= AddHitPenalty; 
    }

    private void Start()
    {
        if(PlayerPrefs.HasKey(bestTimeKey))
        {
            int bestTimeTicks = PlayerPrefs.GetInt(bestTimeKey);
            bestTime = new TimeSpan(bestTimeTicks);
            bestTimeText.text = "BEST TIME: " + bestTime.ToString("ss\\:ff");
        }
        else
        {
            bestTime = new TimeSpan(long.MaxValue);
            bestTimeText.text = "BEST TIME: --:--";
        }
    }

    

    void AddFlagPenalty() 
    {
        penaltyTime += new TimeSpan(0, 0, flagPenaltyTime);
        PenaltyAdded?.Invoke(flagPenaltyTime);
    }

    void AddHitPenalty() 
    {
        penaltyTime += new TimeSpan(0, 0, hitPenaltyTime);
        PenaltyAdded?.Invoke(hitPenaltyTime);
    }

    void OnRaceStart()
    {
        racing = true;
        raceStart = DateTime.Now;
        Debug.Log("Race started");
    }

    void OnRaceFinish()
    {
        racing = false;
        float raceTimeF = (float)raceTime.TotalMilliseconds / 1000f;
        if (GameData.Instance != null)
        {
            GameData.Instance.AddTime(raceTimeF);
        }
        else
        {
            Debug.Log("GameData.Instance is NULL");
        }
        if (raceTime < bestTime)
        {
            bestTime = raceTime;
            bestTimeText.text = "BEST TIME: " + bestTime.ToString("ss\\:ff");
            bestTimeText.color = Color.gold;
            PlayerPrefs.SetInt(bestTimeKey, (int)bestTime.Ticks);
            PlayerPrefs.Save();
        }
       // Debug.Log("Race FINISHED");
    }

   private void Update()
    {
        if (racing) 
            raceTime = DateTime.Now - raceStart + penaltyTime;
        //Debug.Log("Race time" + raceTime);
        raceTimeText.text = "Time: " + raceTime.ToString("ss\\:ff");
    }
}
