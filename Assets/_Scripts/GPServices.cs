using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class GPServices : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text ScoreText;
    public TMP_Text LogText;
    public GameObject BlockPanel;
    
    private int score;

    public int Score
    {
        get => score;
        set
        {
            score = value;
            ScoreText.text = score.ToString();
            ReportScore();
            PlayerPrefs.SetInt("Score",score);
        }
    }
    void Start()
    {
        if (PlayerPrefs.HasKey("Score"))
            Score = PlayerPrefs.GetInt("Score");
        else Score = 0;
        
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                ((GooglePlayGames.PlayGamesPlatform)Social.Active).SetGravityForPopups(Gravity.TOP);
            }
        });
        
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            // enables saving game progress.
            .EnableSavedGames()
            // registers a callback to handle game invitations received while the game is not running.
            .RequestEmail()
            // requests a server auth code be generated so it can be passed to an
            //  associated back end server application and exchanged for an OAuth token.
            .RequestServerAuthCode(false)
            // requests an ID token be generated.  This OAuth token can be used to
            //  identify the player to other services such as Firebase.
            .RequestIdToken()
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();

        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, SignInCallback);
    }

    public void SignIn()
    {
        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, SignInCallback);
    }

    private void SignInCallback(SignInStatus status)
    {
        switch (status)
        {
            case SignInStatus.Success:
                Log("Login success!");
                BlockPanel.SetActive(false);
                break;
            case SignInStatus.UiSignInRequired:
                Log("Ui Sign In Required!");
                BlockPanel.SetActive(true);
                break;
            case SignInStatus.DeveloperError:
                Log("Developer Error!");
                BlockPanel.SetActive(true);
                break;
            case SignInStatus.NetworkError:
                Log("Network Error!");
                BlockPanel.SetActive(true);
                break;
            case SignInStatus.InternalError:
                Log("Internal Error!");
                BlockPanel.SetActive(true);
                break;
            case SignInStatus.Canceled:
                Log("Canceled!");
                BlockPanel.SetActive(true);
                break;
            case SignInStatus.AlreadyInProgress:
                Log("Already In Progress!");
                BlockPanel.SetActive(true);
                break;
            case SignInStatus.Failed:
                Log("Failed!");
                BlockPanel.SetActive(true);
                break;
            case SignInStatus.NotAuthenticated:
                Log("Not Authenticated!");
                BlockPanel.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(status), status, null);
        }
    }

    public void SignOut()
    {
        PlayGamesPlatform.Instance.SignOut();
        BlockPanel.SetActive(true);
    }

    public void ShowLeaderboard()
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI();
    }
    
    public void ShowAchievements()
    {
        PlayGamesPlatform.Instance.ShowAchievementsUI();
    }

    public void Plus1000()
    {
        Score += 1000;
    }

    public void Plus100()
    {
        Score += 100;
    }

    private void ReportScore()
    {
        PlayGamesPlatform.Instance.ReportScore(Score, GPGSIds.leaderboard, (success) =>
        {
            if (success)
                Log("Success reported score: " + Score);
            else Log("Failed reported score: " + Score);
        });
    }

    public void UnlockFirstAchievement()
    {
        PlayGamesPlatform.Instance.UnlockAchievement(GPGSIds.achievement__1,(success) =>
        {
            if (success)
                Log("Success unlock achievement: " + GPGSIds.achievement__1);
            else Log("Failed unlock achievement: " + GPGSIds.achievement__1);
        });
    }
    
    public void UnlockSecondAchievement()
    {
        PlayGamesPlatform.Instance.UnlockAchievement(GPGSIds.achievement__2,(success) =>
        {
            if (success)
                Log("Success unlock achievement: " + GPGSIds.achievement__2);
            else Log("Failed unlock achievement: " + GPGSIds.achievement__2);
        });
    }

    public void Log(string massage)
    {
        LogText.text = massage + "\n" + LogText.text;
    }
    
}
