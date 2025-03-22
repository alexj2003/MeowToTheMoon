using System;
using System.Collections.Generic;
using UnityEngine; 


public class ChargeIndicator : MonoBehaviour
{

    public float jumpCharge;
    public string Image;
    public int ImageNum;

    public GameObject PlayerControllerGO;

    public List<string> indicatorImages = new List<string>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerControllerGO == null) {
            PlayerControllerGO = GameObject.Find("PlayerPrefab");
        }

        // Add filepaths to the list
        indicatorImages.Add("charge indicator 1.png");
        indicatorImages.Add("charge indicator 2.png");
        indicatorImages.Add("charge indicator 3.png");
        indicatorImages.Add("charge indicator 4.png");
        indicatorImages.Add("charge indicator 5.png");
        indicatorImages.Add("charge indicator 6.png");
        indicatorImages.Add("charge indicator 7.png");
        indicatorImages.Add("charge indicator 8.png");
        indicatorImages.Add("charge indicator 9.png");

        Image = indicatorImages[0];
    }

    // Update is called once per frame
    void Update()
    {
        ImageNum = 0;
        Image = indicatorImages[0];

        // i dont get this GetComponent shit
        jumpCharge = PlayerControllerGO.GetComponent<PlayerController>().jumpCharge;
        
        // between 0 and 8
        if (jumpCharge != 0){
            ImageNum = (int)(Mathf.Abs(jumpCharge) * 8 / PlayerController.maxJumpForce);
            if (jumpCharge == 0){
                ImageNum = 0;
            }
        }
    }
}
