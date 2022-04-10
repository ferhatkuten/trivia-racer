using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLoseCoin : MonoBehaviour
{
    [SerializeField] GameObject coinBackround, questionBar, petrolButton, bottomsideGameobjects, petrolGameobjects;
    private Rigidbody2D myBody;
    // Start is called before the first frame update
    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.tag == "winCoin")
        {
            transform.localPosition = 
                new Vector3(Mathf.Clamp(transform.localPosition.x, coinBackround.transform.localPosition.x - 178, questionBar.transform.localPosition.x), 
                Mathf.Clamp(transform.localPosition.y, questionBar.transform.localPosition.y, coinBackround.transform.localPosition.y));
            myBody.velocity = new Vector2(-1000f, +300f);

            if (transform.localPosition.x == coinBackround.transform.localPosition.x - 178 && transform.localPosition.y == coinBackround.transform.localPosition.y)
            {
                Destroy(this.gameObject);
            }
        }
        
        if (this.gameObject.tag == "petrolLoseCoin")
        {
            transform.localPosition =
                new Vector3(Mathf.Clamp(transform.localPosition.x, petrolGameobjects.transform.localPosition.x+petrolButton.transform.localPosition.x, bottomsideGameobjects.transform.localPosition.x + coinBackround.transform.localPosition.x),
                Mathf.Clamp(transform.localPosition.y, bottomsideGameobjects.transform.localPosition.y + coinBackround.transform.localPosition.y, petrolGameobjects.transform.localPosition.y + petrolButton.transform.localPosition.y));

            
            
                myBody.velocity = new Vector2(-500f, 500f);
            

            if (transform.localPosition.x == petrolGameobjects.transform.localPosition.x + petrolButton.transform.localPosition.x && transform.localPosition.y == petrolGameobjects.transform.localPosition.y + petrolButton.transform.localPosition.y)
            {
                Destroy(this.gameObject);
            }
        }
        
    }
}
