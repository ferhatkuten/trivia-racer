using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarsAndRoadMoves : MonoBehaviour
{
    /// <summary> özet
    /// *** backround ve arabaların hareket kontrolleri
    /// *** diğer arabaların spritelerının kontrolü
    /// *** petrol ve geçiş hakkı kontrolleri ayrıca sarı barı arttırma
    /// *** petrol azaldıkça karşıdan gelen arabanın hızını arttırır %50 altına düşerse çarpacak hızda olur kontrolleri
    /// *** vites kontrolleri
    /// *** gyroscope ile kırmızı araba hareketi
    /// </summary>
    public float roadSpeed, redCarHorizontalSpeed;
    public float oppositeRoadCarSpeed, sameRoadCarSpeed;
    [SerializeField] Camera cam;
    public GameObject yellowFinishBar, soundController, winPanel, finishLine, startPanel;
    [SerializeField] GameObject[] gears;
    public GameObject redCar, oppositeRoadCar, sameRoadCar;
    private Rigidbody2D myBody;
    private Rigidbody2D oppositeRoadCarBody, sameRoadCarBody, redCarBody;
    public GameObject questionBar;
    private bool oneTimeControl, oneTimeFinsihBar;
    private ValuesController valuesControllerScript;
    public Image yellowFinishBarImage;
    public AudioClip PassCarSound;
    public Sprite[] spriteNumber = new Sprite[141];
    public AudioSource soundControllerAudioSource;
    private int PitchControl;
    public float pitchNumber, pitchNumber2;
    private float gyroscopeDirectionX;
    private float roadEndDirection;
    public int finishBarNumber;
    public Vector3 edges;
    public GameObject petrolGreenBar;
    public Sprite redBar, greenBar;
    private AdsInitializer adsController;

    // Start is called before the first frame update
    void Start()
    {
        oneTimeControl = true;
        oneTimeFinsihBar = true;
        myBody = GetComponent<Rigidbody2D>();
        oppositeRoadCarBody = oppositeRoadCar.GetComponent<Rigidbody2D>();
        sameRoadCarBody = sameRoadCar.GetComponent<Rigidbody2D>();
        sameRoadCarBody.velocity = new Vector2(sameRoadCarBody.velocity.x, -sameRoadCarSpeed);
        valuesControllerScript = questionBar.GetComponent<ValuesController>();
        yellowFinishBarImage = yellowFinishBar.GetComponent<Image>();
        redCarBody = redCar.GetComponent<Rigidbody2D>();
        otherCarsNewSprites();
        soundControllerAudioSource = soundController.GetComponent<AudioSource>();
        PitchControl = 0;
        roadEndDirection = 35.06f;
        sameRoadCarSpeed = 3;
        finishBarNumber = 0;

    }

    // Update is called once per frame
    void Update()
    {      

        if (finishBarNumber == 20 && oneTimeFinsihBar)
        {
            oneTimeFinsihBar = false;
            finishLine.SetActive(true);
            finishLine.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -roadSpeed);

        }
             
        // benzin fullendiğinde motor sesinin artması için
        if (valuesControllerScript.petrolValue == 100 && soundControllerAudioSource.pitch <2.5f && soundControllerAudioSource.clip == PassCarSound)
        {
            soundControllerAudioSource.pitch += 0.002f;
        }

        PassCarSoundEffect(); // araba geçişlerinde gaza basma efekti için

        myBody.velocity = new Vector2(myBody.velocity.x, -roadSpeed);
        oppositeRoadCarBody.velocity = new Vector2(oppositeRoadCarBody.velocity.x, -oppositeRoadCarSpeed);

        speedDownBecauseOfPetrol();

        // kameranın yatay ve dikey pozisyonlarını alır
        edges = cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, cam.nearClipPlane));

        // roadın hareket kontrolleri road en aşağı geldiğinde her şeyi eski pozisyonuna getirir
        if (transform.position.y + roadEndDirection <= -edges.y) 
        {
            oppositeRoadCarBody.velocity = new Vector2(0, oppositeRoadCarBody.velocity.y);
            transform.position = new Vector3(transform.position.x, 5.67f, 0);
            oppositeRoadCar.transform.position = new Vector3(oppositeRoadCar.transform.position.x, 17.14f, 0);
            sameRoadCar.transform.position = new Vector3(1, 6.18f,0);
            oppositeRoadCar.transform.position = new Vector3(-1, 17.14f);
            oppositeRoadCar.transform.rotation = new Quaternion(0, 0, 0, 0);
            sameRoadCar.transform.rotation = new Quaternion(0, 0, 0, 0);
            sameRoadCarBody.velocity = new Vector2(0, -sameRoadCarSpeed);
            otherCarsNewSprites();
        }

        // arabanın -1 ile 1 dışına çıkmaması için
        redCar.transform.position = new Vector3(Mathf.Clamp(redCar.transform.position.x, -1f, 1f), redCar.transform.position.y);
        // gyroscope kontrolleri
        gyroscopeDirectionX = Input.acceleration.x * redCarHorizontalSpeed;

        // kırmızı araba aynı yoldaki arabaya yaklaştığında diğer şeride geçer
        if (Mathf.Abs(redCar.transform.position.y - sameRoadCar.transform.position.y) <= 3.2f && oneTimeControl)
        {
            oneTimeControl = false;
            if (valuesControllerScript.petrolValue != 100 && soundControllerAudioSource.clip == PassCarSound)
            {
                soundControllerAudioSource.pitch = pitchNumber;
            }

            
            Invoke("petrolController",2.5f);

        }
        //kırmızı araba karşı şeride geçtiğinde kendi şeridine döner
        
        // kırmızı araba kendi şeridine dönünce yatay hızı durur
        if (redCar.transform.position.x > 0f)
        {                      
            PitchControl = 0;
        }
        else if (redCar.transform.position.x < 0f)//karşı şeride geçmesini kontrol eden geçiş hakkı
        {
            PitchControl = 1;
        }
    }

    private void FixedUpdate()
    {
        redCarBody.velocity = new Vector2(gyroscopeDirectionX, 0f); // gyroscope ile kırmızı araba hızı kontrolü
    }

    //petrol ve geçiş hakkı kontrolleri ayrıca sarı barı arttırma ve 
    //içinde bulunduğu if'in belli bür süre boyunca 1 kere çalışması için oneTimeControl var
    private void petrolController()
    {
        if (valuesControllerScript.petrolValue > 0)
        {
            valuesControllerScript.petrolValue -= 10;
        }
       
        if (yellowFinishBarImage.fillAmount < 1)
        {
            valuesControllerScript.yellowFinishBarFillAmountControl += 0.05f;
            yellowFinishBarImage.fillAmount = valuesControllerScript.yellowFinishBarFillAmountControl ;
            finishBarNumber += 1;
            valuesControllerScript.petrolFillAmountControl -= 0.1f;
            petrolGreenBar.GetComponent<Image>().fillAmount = valuesControllerScript.petrolFillAmountControl;
        }

        oneTimeControl = true;

    }
    // petrol azaldıkça karşıdan gelen arabanın hızını arttırır %50 altına düşerse çarpacak hızda olur kontrolleri
    private void speedDownBecauseOfPetrol()
    {
        if(finishBarNumber != 20)
        {
            if (valuesControllerScript.petrolValue >= 90)
            {
                oppositeRoadCarSpeed = 4.5f;
                pitchNumber = 2;
                pitchNumber2 = 4;
                for (int i = 0; i < 5; i++)
                {
                    gears[i].GetComponent<Image>().color = Color.white;
                }
                gears[4].GetComponent<Image>().color = Color.green;
                roadEndDirection = 39.42f;
                roadSpeed = 8;
                petrolGreenBar.GetComponent<Image>().sprite = greenBar;
            }
            else if (valuesControllerScript.petrolValue >= 80)
            {
                for (int i = 0; i < 5; i++)
                {
                    gears[i].GetComponent<Image>().color = Color.white;
                }
                gears[3].GetComponent<Image>().color = Color.green;
                oppositeRoadCarSpeed = 4.8f;
                pitchNumber = 1.6f;
                pitchNumber2 = 3;
                roadEndDirection = 35.06f;
                roadSpeed = 7;
                petrolGreenBar.GetComponent<Image>().sprite = greenBar;
            }
            else if (valuesControllerScript.petrolValue >= 60)
            {
                for (int i = 0; i < 5; i++)
                {
                    gears[i].GetComponent<Image>().color = Color.white;
                }
                gears[2].GetComponent<Image>().color = Color.green;
                oppositeRoadCarSpeed = 5f;
                pitchNumber = 1.3f;
                pitchNumber2 = 2;
                roadEndDirection = 26.36f;
                roadSpeed = 6;
                petrolGreenBar.GetComponent<Image>().sprite = greenBar;
            }
            else if (valuesControllerScript.petrolValue >= 50)
            {
                for (int i = 0; i < 5; i++)
                {
                    gears[i].GetComponent<Image>().color = Color.white;
                }
                gears[1].GetComponent<Image>().color = Color.green;
                oppositeRoadCarSpeed = 5.2f;
                pitchNumber = 1f;
                pitchNumber2 = 1.5f;
                roadEndDirection = 19.82f;
                roadSpeed = 5;
                petrolGreenBar.GetComponent<Image>().sprite = greenBar;
            }
            else if(valuesControllerScript.petrolValue < 50)
            {
                for (int i = 0; i < 5; i++)
                {
                    gears[i].GetComponent<Image>().color = Color.white;
                }
                gears[0].GetComponent<Image>().color = Color.green;
                oppositeRoadCarSpeed = 6.2f;
                pitchNumber = 0.5f;
                pitchNumber2 = 1f;
                roadEndDirection = 11.2f;
                roadSpeed = 4;
                if(Time.timeScale == 1)
                {
                    petrolGreenBar.GetComponent<Image>().sprite = redBar;
                }
                
            }
        }
        

    }
    // diğer arabalara yeni sprite belirler
    private void otherCarsNewSprites()
    {
        sameRoadCar.GetComponent<SpriteRenderer>().sprite = spriteNumber[Random.Range(0, 141)];
        oppositeRoadCar.GetComponent<SpriteRenderer>().sprite = spriteNumber[Random.Range(0, 141)];
        oppositeRoadCar.transform.Rotate(0, 0, 180);
    }
    // gaza basma efekti için pitchnumber değeri speedDownBecauseOfPetrol metodunda petrol seviyesine göre kontrol ediliyor
    private void PassCarSoundEffect()
    {
        if(soundControllerAudioSource.clip == PassCarSound)
        {
            if (PitchControl == 0 && soundControllerAudioSource.pitch > pitchNumber)
            {
                soundControllerAudioSource.pitch -= 0.003f;
            }
            else if (PitchControl == 1 && soundControllerAudioSource.pitch < pitchNumber2)
            {
                soundControllerAudioSource.pitch += 0.003f;
            }
        }
        
        
    }

    
}