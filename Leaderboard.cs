using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SceneManagement;

public class Leaderboard : MonoBehaviour
{
    void Start()
    {
        Social.localUser.Authenticate(success => {
            if (success)
            {
                Debug.Log("Authentication successful.");
                string userInfo = "Username: " + Social.localUser.userName +
                    "\nUser ID: " + Social.localUser.id +
                    "\nIsUnderage: " + Social.localUser.underage;
                Debug.Log(userInfo);
            }
            else
                Debug.Log("Authentication failed");
        });
    }

    public void ShowLeaderboard()
    {
        Social.ShowLeaderboardUI();
    }
    public void PostScore()
    {
        Social.ReportScore((long)Manager.instance.depth, "high_scores", null);
    }
}
