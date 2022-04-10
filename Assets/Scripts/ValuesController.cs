using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static AdsInitializer;

public class ValuesController : MonoBehaviour , AdsRewardedListener
{
    /// <summary> özet
    /// *** süre 5 saniyenin altına inerse süre barı kırmızıya dönmesi kontrolleri
    /// *** petrol ve geçiş hakkı satın alma kontrolleri
    /// *** petrol ve geçiş hakkı ses kontrolleri
    /// *** petrol ve geçiş hakkı warnin uyarısı kontrolü
    /// *** hangi soruda olduğunu tutan int değeri question number burda ama question controllerda kontrol ediliyor
    /// </summary>
    public Text timeValueText, PetrolValueText, coinValueText;
    public float timeValue, removeCoinNumber;
    public int petrolValue, questionsNumber, answerPositionController;
    [SerializeField] GameObject GreenClockBar, petrolLoseCoin, coinBackround, backroundMovement, petrolAndBottomside ;
    public Sprite RedClockBarSprite, GreenClockBarSprite;
    public GameObject soundController;
    public AudioClip PassCarSound;
    public AudioClip addPetrolSound;
    public int coinValue;
    public GameObject petrolGreenBar;
    [SerializeField] GameObject petrolWarning; 
    public float petrolFillAmountControl;
    public float yellowFinishBarFillAmountControl;
    private bool oneTimeFinsihBar2;
    private AdsInitializer adsController;
    // Start is called before the first frame update
    void Start()
    {
        adsController = GameObject.FindGameObjectWithTag("FairBidAds").GetComponent<AdsInitializer>();
        oneTimeFinsihBar2 = true;
        questionsNumber = 0;
        if(PlayerPrefs.GetInt("firstGameCoin")== 0)
        {
            PlayerPrefs.SetInt("firstGameCoin", 1);
            coinValue = 10;
            petrolValue = 0;
            petrolFillAmountControl = 1;
            backroundMovement.GetComponent<CarsAndRoadMoves>().redCar.transform.position = new Vector3(1f, -0.3f);
            backroundMovement.GetComponent<CarsAndRoadMoves>().sameRoadCar.transform.position = new Vector3(1f, 6.18f);
            backroundMovement.GetComponent<CarsAndRoadMoves>().oppositeRoadCar.transform.position = new Vector3(-1f, 17.14f);
        }
        else
        {
            coinValue = PlayerPrefs.GetInt("CoinValue");
            petrolValue = 100;
            petrolFillAmountControl = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        backroundMovement.GetComponent<CarsAndRoadMoves>().yellowFinishBarImage.fillAmount = yellowFinishBarFillAmountControl;
        petrolGreenBar.GetComponent<Image>().fillAmount = petrolFillAmountControl;
        PlayerPrefs.SetInt("CoinValue", coinValue);
        timeValue -= Time.deltaTime;
        timeValueText.text = ((int)timeValue).ToString();
        //PetrolValueText.text = petrolValue.ToString();
        coinValueText.text = coinValue.ToString();

        if (backroundMovement.GetComponent<CarsAndRoadMoves>().finishBarNumber == 10 && oneTimeFinsihBar2)
        {
            Time.timeScale = 0;
            backroundMovement.GetComponent<CarsAndRoadMoves>().startPanel.SetActive(true);
            backroundMovement.GetComponent<CarsAndRoadMoves>().soundControllerAudioSource.enabled = false;
            oneTimeFinsihBar2 = false;
            adsController.ShowRewarded("midGame", this);
        }

        // süre 5 saniyenin altına inerse süre barı kırmızıya dönmesi kontrolleri
        if ((int)timeValue <= 5)
        {
            GreenClockBar.GetComponent<Image>().sprite = RedClockBarSprite;
        }
        else
        {
            GreenClockBar.GetComponent<Image>().sprite = GreenClockBarSprite;
        }
        
        // petrol 0 a indiğinde yeterli coin varsa otomatik satın alma kontrolleri
        if(petrolValue == 0 && coinValue > 9 && Time.timeScale == 1)
        {
            removeCoinNumber = 10;
            StartCoroutine("addPetrolLoseCoin");
            petrolFillAmountControl = 1;
            petrolValue = 100;
            coinValue -= 10;            
        }

       

        WarningsController();

    }

    // petrol buttonu petrol arttırma kontrolleri 
    public void PetrolButton()
    {
        if (coinValue >0 && petrolValue < 100)
        {
            if(petrolValue == 90 && coinValue >=1)
            {
                coinValue -= 1;
                removeCoinNumber = 1;
                afterTakePetrol();
            }
            else if(petrolValue == 80 && coinValue >= 2)
            {
                coinValue -= 2;
                removeCoinNumber = 2;
                afterTakePetrol();
            }
            else if (petrolValue == 70 && coinValue >= 3)
            {
                coinValue -= 3;
                removeCoinNumber = 3;
                afterTakePetrol();
            }
            else if (petrolValue == 60 && coinValue >= 4)
            {
                coinValue -= 4;
                removeCoinNumber = 4;
                afterTakePetrol();
            }
            else if (petrolValue == 50 && coinValue >= 5)
            {
                coinValue -= 5;
                removeCoinNumber = 5;
                afterTakePetrol();
            }
            else if (petrolValue == 40 && coinValue >= 6)
            {
                coinValue -= 6;
                removeCoinNumber = 6;
                afterTakePetrol();
            }
            else if (petrolValue == 30 && coinValue >= 7)
            {
                coinValue -= 7;
                removeCoinNumber = 7;
                afterTakePetrol();
            }
            else if (petrolValue == 20 && coinValue >= 8)
            {
                coinValue -= 8;
                removeCoinNumber = 8;
                afterTakePetrol();
            }
            else if (petrolValue == 10 && coinValue >= 9)
            {
                coinValue -= 9;
                removeCoinNumber = 9;
                afterTakePetrol();
            }
            else if (petrolValue == 0 && coinValue >= 10)
            {
                coinValue -= 10;
                removeCoinNumber = 10;
                afterTakePetrol();
            }
            
           
        }
    }
    private void afterTakePetrol()
    {
        petrolValue = 100;
        petrolFillAmountControl = 1;
        petrolGreenBar.GetComponent<Image>().fillAmount = petrolFillAmountControl;

        StartCoroutine("addPetrolLoseCoin");
        AddPetrolSoundController();
        Invoke("PassSoundController", 0.5f);
        //gaza basma efekti için
        if (soundController.GetComponent<AudioSource>().clip == PassCarSound)
        {
            soundController.GetComponent<AudioSource>().pitch = 1.5f;
        }
    }


    IEnumerator addPetrolLoseCoin()
    {
        for (int i = 0; i < removeCoinNumber; i++)
        {
            Instantiate(petrolLoseCoin, new Vector3(this.gameObject.transform.localPosition.x + coinBackround.transform.localPosition.x, this.gameObject.transform.localPosition.y + coinBackround.transform.localPosition.y), Quaternion.identity, petrolAndBottomside.transform);
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    private void WarningsController()
    {       
        if(petrolValue <= 50 && Time.timeScale == 1)
        {
            petrolWarning.SetActive(true);
        }
        else
        {
            petrolWarning.SetActive(false);
        }
    }

    private void AddPetrolSoundController()
    {
        soundController.GetComponent<AudioSource>().clip = addPetrolSound;
        soundController.GetComponent<AudioSource>().pitch = 2;
        soundController.GetComponent<AudioSource>().enabled = false;
        soundController.GetComponent<AudioSource>().enabled = true;
    }
    

    private void PassSoundController()
    {
        soundController.GetComponent<AudioSource>().pitch = 0;
        soundController.GetComponent<AudioSource>().clip = PassCarSound;
        soundController.GetComponent<AudioSource>().pitch = 2f;
        soundController.GetComponent<AudioSource>().enabled = false;
        soundController.GetComponent<AudioSource>().enabled = true;
    }

    public void OnCompletion(string jokerName)
    {
        throw new System.NotImplementedException();
    }
}
