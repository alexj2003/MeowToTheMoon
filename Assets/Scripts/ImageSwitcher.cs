using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ImageSwitcher : MonoBehaviour
{
    public int imageNumber;
    public GameObject ChargeIndicatorGO;
    public Image Charge0;
    public Sprite Charge1;
    public Sprite Charge2;
    public Sprite Charge3;
    public Sprite Charge4;
    public Sprite Charge5;
    public Sprite Charge6;
    public Sprite Charge7;
    public Sprite Charge8;
    public Sprite Charge9;

    public List<Sprite> indicatorSprites = new List<Sprite>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        indicatorSprites.Add(Charge1);
        indicatorSprites.Add(Charge2);
        indicatorSprites.Add(Charge3);
        indicatorSprites.Add(Charge4);
        indicatorSprites.Add(Charge5);
        indicatorSprites.Add(Charge6);
        indicatorSprites.Add(Charge7);
        indicatorSprites.Add(Charge8);
        indicatorSprites.Add(Charge9);
    }

    // Update is called once per frame
    void Update()
    {
        imageNumber = ChargeIndicatorGO.GetComponent<ChargeIndicator>().ImageNum;
        Charge0.sprite = indicatorSprites[imageNumber];
    }
}
