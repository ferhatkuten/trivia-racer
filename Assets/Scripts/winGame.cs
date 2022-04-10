using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fyber;
using static AdsInitializer;

public class winGame : MonoBehaviour , AdsRewardedListener
{
    [SerializeField] GameObject backroundMovement, bottomsideGameobject, x2Button;
    private CarsAndRoadMoves carsAndRoadMovesScript;
    private bool oneTimeTrigger, oneTimeX2;
    private AdsInitializer adsController;
    private ValuesController valuesControllerScript;
    // Start is called before the first frame update
    void Start()
    {
        valuesControllerScript = bottomsideGameobject.GetComponent<ValuesController>();
        carsAndRoadMovesScript = backroundMovement.GetComponent<CarsAndRoadMoves>();
        oneTimeTrigger = true;
        oneTimeX2 = true;
        adsController = GameObject.FindGameObjectWithTag("FairBidAds").GetComponent<AdsInitializer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (oneTimeX2 == false)
        {
            x2Button.SetActive(false);
        }
    }
    IEnumerator winPanelActivate()
    {
        
        yield return new WaitForSeconds(2f);
        carsAndRoadMovesScript.sameRoadCarSpeed = 0f;
        carsAndRoadMovesScript.oppositeRoadCarSpeed = 0f;
        carsAndRoadMovesScript.winPanel.SetActive(true);
        carsAndRoadMovesScript.soundControllerAudioSource.enabled = false;
        Time.timeScale = 0;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.tag == "redCar" && oneTimeTrigger)
        {
            oneTimeTrigger = false;
            
            // konfeti animasyonunu çalıştır
            StartCoroutine("winPanelActivate");
        }
    }

    public void coinX2()
    {       
        adsController.ShowRewarded("CoinX2", this);
        oneTimeX2 = false;
    }

    public void OnCompletion(string jokerName)
    {
        valuesControllerScript.coinValue *= 2;
    }
}
