using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AfterCrashNewRedCar : MonoBehaviour
{
    /// <summary> özet
    /// *** karşıdan gelen arabayla çarpışmas sonucu kontroller
    /// *** aynı yoldaki arabayla çarpışma sonrası kontroller
    /// *** yeni araba spritena geçiş için kontroller
    /// </summary>
    [SerializeField] GameObject oppositeRoadCar, sameRoadCar;
    private Rigidbody2D oppositeRoadCarBody , sameRoadCarBody;
    public Sprite[] redCars = new Sprite[5];
    public int redCarNumber;
    private PolygonCollider2D oppositeRoadCarCollider , sameRoadCarCollider;
    public GameObject soundController;
    public AudioClip PassCarSound;
    public AudioClip crashSound;
    public AudioClip redCaExplosionSound;



    // Start is called before the first frame update
    void Start()
    {
        oppositeRoadCarBody = oppositeRoadCar.GetComponent<Rigidbody2D>();
        sameRoadCarBody = sameRoadCar.GetComponent<Rigidbody2D>();
        oppositeRoadCarCollider = oppositeRoadCar.GetComponent<PolygonCollider2D>();
        sameRoadCarCollider = sameRoadCar.GetComponent<PolygonCollider2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // karşıdan gelen arabayla çarpışmas sonucu kontroller
        if(collision.tag == "oppositeRoadCar")
        {
            oppositeRoadCarCollider.enabled = false;
            oppositeRoadCar.transform.Rotate(0, 0, -15);
            oppositeRoadCarBody.velocity = new Vector2(-0.5f, oppositeRoadCarBody.velocity.y);
            afterCrash();

        }

        // aynı yoldaki arabayla çarpışma sonrası kontroller
        if(collision.tag == "sameRoadCar")
        {
            sameRoadCarCollider.enabled = false;      
            sameRoadCar.transform.Rotate(0, 0, -15f);
            sameRoadCarBody.velocity = new Vector2(3f, 0.6f);
            afterCrash();
        }
    }
    // ilk çarpışmadan sonra triggerin yeniden tetiklenmesini engeller 1 saniye içerisinde yeniden aktif hale getirir 
    private void reActive()
    {
        sameRoadCarCollider.enabled = true;
        oppositeRoadCarCollider.enabled = true;
    }

    //yeni araba spritena geçiş için kontroller
    private void otherCar()
    {
        if (redCarNumber < 5)
        {
            redCarNumber++;
            if(redCarNumber < 4)
            {
                GetComponent<SpriteRenderer>().sprite = redCars[redCarNumber];
            }
                    
        }
        
    }
    private void afterCrash()
    {
        otherCar();
        Invoke("reActive", 1f);
        if (redCarNumber <= 4)
        {
            crashSoundController();
            Invoke("passSoundController", 0.6f);
        }
        else
        {
            explosionSoundController();
            Invoke("soundOff", 1f);
        }
    }

    private void crashSoundController()
    {      
        soundController.GetComponent<AudioSource>().clip = crashSound;
        soundController.GetComponent<AudioSource>().enabled = false;
        soundController.GetComponent<AudioSource>().enabled = true;
        soundController.GetComponent<AudioSource>().pitch = 1;
    }

    private void passSoundController()
    {
        soundController.GetComponent<AudioSource>().pitch = 0;
        soundController.GetComponent<AudioSource>().clip = PassCarSound;
        soundController.GetComponent<AudioSource>().pitch = 1;
        soundController.GetComponent<AudioSource>().enabled = false;
        soundController.GetComponent<AudioSource>().enabled = true;
    }

    private void explosionSoundController()
    {
        soundController.GetComponent<AudioSource>().clip = redCaExplosionSound;
        soundController.GetComponent<AudioSource>().enabled = false;
        soundController.GetComponent<AudioSource>().enabled = true;
    }
    private void soundOff()
    {
        soundController.GetComponent<AudioSource>().enabled = false;
    }

    



}
