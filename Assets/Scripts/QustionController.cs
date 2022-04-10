using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QustionController : MonoBehaviour
{
    /// <summary> özet
    /// *** bidaki soruya geçme kontrolleri
    /// *** süre bitince yeni soruya geçme kontrolleri
    /// *** soruların ve cevapların textlerinin kontrolleri
    /// *** sağdaki ve soldaki cevap butonu kontrolleri doğruysa 2 altın ekler yeşil yanar yanlışsa kırmızı yanar
    /// </summary>
    public Text leftAnswerText, rightAnswerText, questionText;
    public GameObject leftAnswerBar, rightAnswerBar;
    private string[] questions = new string[10];
    private string[] answers = new string[10];
    private string[] wrongAnswers = new string[10];
    public GameObject bottomSideGameObjects;
    private float time;
    private bool onetime;
    private ValuesController valuesControllerScript;
    public AudioClip coinSound;
    [SerializeField] GameObject winCoin;
    // Start is called before the first frame update
    void Start()
    {

        questions[0] = "How many times has Katie Price been married?";
        questions[1] = "What is the middle name of Angela Merkel?";
        questions[2] = "Alberta is a province of which country?";
        questions[3] = "Where was Prince Philip born?";
        questions[4] = "What is the capital of Switzerland?";
        questions[5] = "What is Japanese sake made from?";
        questions[6] = "Which animal name means river horse?";
        questions[7] = "How many legs does a lobster have?";
        questions[8] = "What planet is known as the red planet";
        questions[9] = "Which is the world's most populous country?";
        answers[0] = "three";
        answers[1] = "Dorothea";
        answers[2] = "Canada";
        answers[3] = "Greece";
        answers[4] = "Bern";
        answers[5] = "Rice";
        answers[6] = "Hippopotamus";
        answers[7] = "Eight";
        answers[8] = "Mars";
        answers[9] = "China";
        wrongAnswers[0] = "Four";
        wrongAnswers[1] = "Emma";
        wrongAnswers[2] = "Australia";
        wrongAnswers[3] = "Denmark";
        wrongAnswers[4] = "Brussels";
        wrongAnswers[5] = "Fig";
        wrongAnswers[6] = "Parrot";
        wrongAnswers[7] = "Eleven";
        wrongAnswers[8] = "Uranus";
        wrongAnswers[9] = "America";
        bottomSideGameObjects.GetComponent<ValuesController>().questionsNumber = 0;
        bottomSideGameObjects.GetComponent<ValuesController>().answerPositionController = 0;
        onetime = true;
        valuesControllerScript = bottomSideGameObjects.GetComponent<ValuesController>();
    }

    // Update is called once per frame
    void Update()
    {
        questionsWriter();
        timeOver();

    }

    // bidaki soruya geçme kontrolleri
    private void nextQuestion()
    {
        valuesControllerScript.answerPositionController++;
        valuesControllerScript.questionsNumber++;

        rightAnswerBar.GetComponent<Image>().color = Color.white;
        leftAnswerBar.GetComponent<Image>().color = Color.white;

        rightAnswerBar.GetComponent<Button>().enabled = true;
        leftAnswerBar.GetComponent<Button>().enabled = true;

        valuesControllerScript.timeValue = 21;
        onetime = true;
    }

    //süre bitince yeni soruya geçme kontrolleri
    private void timeOver()
    {
        time = valuesControllerScript.timeValue;
        if ((int)time == 0 && onetime)
        {
            
            onetime = false;
            leftAnswerBar.GetComponent<Button>().enabled = false;
            rightAnswerBar.GetComponent<Button>().enabled = false;


            #region  // #region: süre bitince doğru ve yanlış şıkların kırmızı ve yeşil yanması kontrolleri bu regionun içinde
            if (leftAnswerText.text == answers[valuesControllerScript.questionsNumber])
            {
                leftAnswerBar.GetComponent<Image>().color = Color.green;
            }
            if (leftAnswerText.text == wrongAnswers[valuesControllerScript.questionsNumber])
            {
                leftAnswerBar.GetComponent<Image>().color = Color.red;
            }

            if (rightAnswerText.text == answers[valuesControllerScript.questionsNumber])
            {
                rightAnswerBar.GetComponent<Image>().color = Color.green;
            }
            if (rightAnswerText.text == wrongAnswers[valuesControllerScript.questionsNumber])
            {
                rightAnswerBar.GetComponent<Image>().color = Color.red;
            }
            #endregion 

            Invoke("nextQuestion", 0.5f);
        }
    }

    // soruların ve cevapların textlerinin kontrolleri
    private void questionsWriter()
    {
        if(valuesControllerScript.questionsNumber >= 10)
        {
            valuesControllerScript.questionsNumber = 0;
        }
        questionText.text = questions[valuesControllerScript.questionsNumber];

        if(valuesControllerScript.answerPositionController %2 == 0)
        {
            leftAnswerText.text = answers[valuesControllerScript.questionsNumber];
            rightAnswerText.text = wrongAnswers[valuesControllerScript.questionsNumber];
        }
        
        if(valuesControllerScript.answerPositionController %2 == 1)
        {
            leftAnswerText.text = wrongAnswers[valuesControllerScript.questionsNumber];
            rightAnswerText.text = answers[valuesControllerScript.questionsNumber];
        }
    }

    // sağdaki cevap butonu kontrolleri doğruysa 2 altın ekler yeşil yanar yanlışsa kırmızı yanar
    public void RightAnswerButton()
    {
        rightAnswerBar.GetComponent<Button>().enabled = false;
        leftAnswerBar.GetComponent<Button>().enabled = false;
        if (rightAnswerText.text == answers[valuesControllerScript.questionsNumber])
        {
            valuesControllerScript.coinValue += 2;
            StartCoroutine("addWinCoin");
            GetComponent<AudioSource>().enabled = true;
            Invoke("CoinSoundController", 0.3f);
            rightAnswerBar.GetComponent<Image>().color = Color.green;
        }

        if (rightAnswerText.text == wrongAnswers[valuesControllerScript.questionsNumber])
        {
            rightAnswerBar.GetComponent<Image>().color = Color.red;
        }
        Invoke("nextQuestion", 0.5f);
    }

    // sağdaki cevap butonu kontrolleri doğruysa 2 altın ekler yeşil yanar yanlışsa kırmızı yanar
    public void LeftAnswerButton()
    {
        leftAnswerBar.GetComponent<Button>().enabled = false;
        rightAnswerBar.GetComponent<Button>().enabled = false;
        if (leftAnswerText.text == answers[valuesControllerScript.questionsNumber])
        {
            valuesControllerScript.coinValue += 2;
            StartCoroutine("addWinCoin");
            GetComponent<AudioSource>().enabled = true;
            Invoke("CoinSoundController", 0.3f);
            leftAnswerBar.GetComponent<Image>().color = Color.green;
        }

        if (leftAnswerText.text == wrongAnswers[valuesControllerScript.questionsNumber])
        {
            leftAnswerBar.GetComponent<Image>().color = Color.red;
        }
        Invoke("nextQuestion", 0.5f);
    }

    private void CoinSoundController()
    {
        GetComponent<AudioSource>().enabled = false;
    }

    IEnumerator addWinCoin()
    {
        for (int i = 0; i < 2; i++)
        {
            Instantiate(winCoin, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y), Quaternion.identity, bottomSideGameObjects.transform);
            yield return new WaitForSeconds(0.1f);
        }

    }

}
