using UnityEngine;
using System.Collections;
using Fyber;
//using UnityEngine.Advertisements;
using Facebook.Unity;
using GoogleMobileAds.Api;
using UnityEngine.UI;
using Firebase.Analytics;
using System;

public class AdsInitializer : MonoBehaviour, RewardedListener
{
    private string jokerName;
    private string IOS_APP_ID = "119224";
    private string ANDROID_APP_ID = "119226";
    private string IOS_BANNER_ID = "325335";
    private string ANDROID_BANNER_ID = "325358";
    private string IOS_INTERSTITIAL_ID = "325336";
    private string ANDROID_INTERSTITIAL_ID = "325359";
    private string IOS_REWARDED_ID = "325337";
    private string ANDROID_REWARDED_ID = "325362";
    private string current_app_id;
    private string current_unity_id;
    private string current_banner_id;
    private string current_interstitial_id;
    private string current_rewarded_id;
    private string androidUnityAdsGameId = "4025111";
    private string iosUnityAdsGameId = "4025110";
    AdsRewardedListener _rewardedListener;
    private bool adsInitialized = false;
    private bool fbInitialized = false; 
    private static AdsInitializer obj = null;
    public bool rewardedAdsOnAvailable = false;


    public interface AdsRewardedListener
    {
        void OnCompletion(string jokerName);
    }

    // Use this for initialization

    private void Awake()
    {
        if (obj == null)
        {
            obj = this;
            DontDestroyOnLoad(this);
        }
        else if (obj != this)
        {
            Destroy(gameObject);
        }
    }


    public IEnumerator Start()
    {
#if UNITY_IPHONE
            current_app_id = IOS_APP_ID;
            current_banner_id = IOS_BANNER_ID;
            current_interstitial_id = IOS_INTERSTITIAL_ID;
            current_rewarded_id = IOS_REWARDED_ID;
            current_unity_id = iosUnityAdsGameId;
#elif UNITY_ANDROID
        current_app_id = ANDROID_APP_ID;
        current_banner_id = ANDROID_BANNER_ID;
        current_interstitial_id = ANDROID_INTERSTITIAL_ID;
        current_rewarded_id = ANDROID_REWARDED_ID;
        current_unity_id = androidUnityAdsGameId;
#endif
        bool testMode = false;
        DontDestroyOnLoad(this.gameObject);

        //Advertisement.Initialize(current_unity_id, testMode);
        MobileAds.Initialize(initStatus => {


#if UNITY_IPHONE || UNITY_ANDROID
            FairBid.Start(current_app_id);
#endif

            Rewarded.SetRewardedListener(this);
            Rewarded.Request(current_rewarded_id);
            Rewarded.EnableAutoRequesting(current_rewarded_id);
           

            Interstitial.Request(current_interstitial_id);
            Interstitial.EnableAutoRequesting(current_interstitial_id);

            //adsController.Init();
            StartCoroutine(ShowDeleyadBanner());

            //FairBid.ShowTestSuite();
            adsInitialized = true;

        });

        //Interstitial.Show(current_interstitial_id);
        //FairBid.ShowTestSuite();
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
        while (!adsInitialized || !fbInitialized)
        {
            yield return null;
        }



        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                FirebaseAnalytics.LogEvent("app_start");
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                //app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });




    }

    internal void ShowRewarded(string v, CarsAndRoadMoves carsAndRoadMoves)
    {
        throw new NotImplementedException();
    }

    IEnumerator ShowDeleyadBanner()
    {
        yield return new WaitForSeconds(3);
        ShowBanner();
    }

    private void InitCallback()
    {
        fbInitialized = true;
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    // Update is called once per frame
    public void ShowBanner()
    {
        Banner.Show(current_banner_id);
    }

    public void DestroyBanner()
    {
        Banner.Destroy(current_banner_id);
    }


    public void ShowInterstitial()
    {
        if (Interstitial.IsAvailable(current_interstitial_id))
        {
            Interstitial.Show(current_interstitial_id);
        }
    }

    public void ShowRewarded(string JokerName, AdsRewardedListener adsRewardedListener)
    {
        _rewardedListener = adsRewardedListener;
        jokerName = JokerName;
        if (Rewarded.IsAvailable(current_rewarded_id))
        {
            Rewarded.Show(current_rewarded_id);
        }
    }
    public void CompletionAd()
    {
        Debug.Log("Fyber CompletionAd");

    }

    public void OnShow(string placementId, ImpressionData impressionData)
    {
        Debug.Log("Fyber OnShow");

        //throw new System.NotImplementedException();
    }

    public void OnClick(string placementId)
    {
        Debug.Log("Fyber OnClick");

        //throw new System.NotImplementedException();
    }

    public void OnHide(string placementId)
    {
        Debug.Log("Fyber OnHide");

        //throw new System.NotImplementedException();
    }

    public void OnShowFailure(string placementId, ImpressionData impressionData)
    {
        Debug.Log("Fyber OnShowFailure");

        //throw new System.NotImplementedException();
    }

    public void OnAvailable(string placementId)
    {
        Debug.Log("Fyber OnAvailable");
        rewardedAdsOnAvailable = true;
        //throw new System.NotImplementedException();
    }

    public void OnUnavailable(string placementId)
    {
        Debug.Log("Fyber OnUnavailable");
        rewardedAdsOnAvailable = false;
        //throw new System.NotImplementedException();
    }

    public void OnCompletion(string placementId, bool userRewarded)
    {
        Debug.Log("Fyber OnCompletion");
        if (userRewarded)
        {
            _rewardedListener.OnCompletion(jokerName);

        }
    }

    public void OnRequestStart(string placementId)
    {
        Debug.Log("Fyber OnRequestStart");
        //throw new System.NotImplementedException();
    }
}
