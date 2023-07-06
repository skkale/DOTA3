using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class select_character : MonoBehaviour
{

    public static int i;
    public static int currentCharacter;

    public GameObject[] AllCharacters;
    public GameObject ArrowRight;
    public GameObject ArrowLeft;
    public GameObject ButtonSelectCharatcer;
    public GameObject TextSelectCharatcer;

    private void Start()
    {
        if (PlayerPrefs.HasKey("CurrentlyCharacter"))
        {
            i = PlayerPrefs.GetInt("CurrentlyCharacter");
            currentCharacter = PlayerPrefs.GetInt("CurrentlyCharacter");

            ButtonSelectCharatcer.SetActive(false);
            TextSelectCharatcer.SetActive(true);
        }
        else PlayerPrefs.SetInt("CurrentlyCharacter", i);

        AllCharacters[i].SetActive(true);
        
        if(i > 0)
        {
            ArrowLeft.SetActive(true);
        }
        if(i == AllCharacters.Length) 
        {
            ArrowRight.SetActive(false);
        }
    }

    public void Arrow_Right()
    {
        if(i<AllCharacters.Length)
        {
            if (i == 0)
            {
                ArrowLeft.SetActive(true);
            }
            AllCharacters[i].SetActive(false);
            i++;
            Debug.Log(i);
            AllCharacters[i].SetActive(true);

            if(currentCharacter == i)
            {
                ButtonSelectCharatcer.SetActive(false);
                TextSelectCharatcer.SetActive(true);
            }
            else
            {
                ButtonSelectCharatcer.SetActive(true);
                TextSelectCharatcer.SetActive(false);
            }
            if(i+1 == AllCharacters.Length)
            {
                ArrowRight.SetActive(false);
            }
        }
    }

    public void Arrow_Left()
    {
        if (i < AllCharacters.Length)
        {
            AllCharacters[i].SetActive(false);
            i--;
            Debug.Log(i);
            AllCharacters[i].SetActive(true);
            ArrowRight.SetActive(true);


            if (currentCharacter == i)
            {
                ButtonSelectCharatcer.SetActive(false);
                TextSelectCharatcer.SetActive(true);
            }
            else
            {
                ButtonSelectCharatcer.SetActive(true);
                TextSelectCharatcer.SetActive(false);
            }

            if (i <= 0)
            {
                ArrowLeft.SetActive(false);
            }
        }
    }

    public void SelectCharacter()
    {
        PlayerPrefs.SetInt("CurrentCharacter", i);
        currentCharacter = i;
        ButtonSelectCharatcer.SetActive(false);
        TextSelectCharatcer.SetActive(true);
    }

    public void ChangeScene()
    {
            SceneManager.LoadScene("Game");   
    }

}
