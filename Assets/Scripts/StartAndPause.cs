using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAndPause : MonoBehaviour
{
    [SerializeField] GameObject startPanel, gamePlayPanel, gamePlayPetrol, summary, bottomsideGameobject;
    public GameObject soundController;
    private ValuesController valuesControllerScript;
    // Start is called before the first frame update
    private void Awake()
    {
        Time.timeScale = 0;
        soundController.GetComponent<AudioSource>().enabled = false;
        startPanel.SetActive(true);

    }
    void Start()
    {
        valuesControllerScript = bottomsideGameobject.GetComponent<ValuesController>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StartButton()
    {
        
        if (PlayerPrefs.GetInt("gamePlayIntro") == 0)
        {
            PlayerPrefs.SetInt("gamePlayIntro", 1);
            startPanel.SetActive(false);
            gamePlayPanel.SetActive(true);
            gamePlayPetrol.SetActive(true);

        }
        else
        {
            Time.timeScale = 1;
            startPanel.SetActive(false);
            soundController.GetComponent<AudioSource>().enabled = true;
            
        }
        
    }

    public void gamePLayIntroPetrolbutton() //
    {
        if (valuesControllerScript.petrolValue == 0 && valuesControllerScript.coinValue >= 10)
        {
            valuesControllerScript.coinValue -= 10;
            valuesControllerScript.removeCoinNumber = 10;
        }
        valuesControllerScript.petrolValue = 100;
        gamePlayPetrol.SetActive(false);
        summary.SetActive(true);
    }

    

    public void gamePlayStartButton()
    {
        summary.SetActive(false);
        gamePlayPanel.SetActive(false);
        Time.timeScale = 1;
        soundController.GetComponent<AudioSource>().enabled = true;
    }

    public void StopButton()
    {
        Time.timeScale = 0;
        soundController.GetComponent<AudioSource>().enabled = false;
        startPanel.SetActive(true);
    }
}
