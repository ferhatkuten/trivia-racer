using System.Collections;
using System.Collections.Generic;
using Fyber;
using UnityEngine;
using UnityEngine.SceneManagement;
using static AdsInitializer;

public class gameOver : MonoBehaviour , AdsRewardedListener
{
    public GameObject redCar;
    private AfterCrashNewRedCar AfterCrashNewRedCarScript;
    private Animator explosionAnimator;
    public GameObject explosion;
    private bool oneTime;
    [SerializeField] GameObject gameOverPanel;
    private AdsInitializer adsController;
    // Start is called before the first frame update
    void Start()
    {
        adsController = GameObject.FindGameObjectWithTag("FairBidAds").GetComponent<AdsInitializer>();
        AfterCrashNewRedCarScript = redCar.GetComponent<AfterCrashNewRedCar>();
        explosionAnimator = explosion.GetComponent<Animator>();
        oneTime = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(AfterCrashNewRedCarScript.redCarNumber == 5 && oneTime)
        {
            explosion.SetActive(true);
            explosion.transform.position = redCar.transform.position;
            explosionAnimator.SetTrigger("explosion");
            Destroy(explosion, 1);
            Invoke("stopGame", 1);
            redCar.SetActive(false);
            oneTime = false;

        }
    }

    private void stopGame()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void PlayAgainButton()
    {
        adsController.ShowRewarded("gameOver", this);
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1;
    }

    public void OnCompletion(string jokerName)
    {
        
    }
}
