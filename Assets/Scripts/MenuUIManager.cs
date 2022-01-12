using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

[DefaultExecutionOrder(1000)]
public class MenuUIManager : MonoBehaviour
{

    public TMP_InputField inputField;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Text warningTextYourName;
    [SerializeField] bool isPlayerNameTexted;
    [SerializeField] GameObject Objects4StartFX;
    [SerializeField] GameObject buttonUI;

    void Update()
    {
        if(inputField.text != string.Empty)
            isPlayerNameTexted = true;

        //Debug.Log(inputField.text);
    } 

    public void NewPlayerNameWritten()
    {
        if (isPlayerNameTexted)
        {
            string name = inputField.GetComponent<TMP_InputField>().text.ToString();
            GameManager.Instance.playerNameCurrent = name;
        }
            
    }

    public void StartNew()
    {
        if (!isPlayerNameTexted)
        {
            StartCoroutine(warningRoutine());
            return;
        }
        else if (isPlayerNameTexted)
        {
            NewPlayerNameWritten();

            StartCoroutine(startNewRoutine());
        }

    }

    IEnumerator warningRoutine()
    {
        warningTextYourName.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.75f);

        warningTextYourName.gameObject.SetActive(false);
    }


    IEnumerator startNewRoutine()
    {
        buttonUI.gameObject.SetActive(false);

        Rigidbody[] bricksRb = Objects4StartFX.GetComponentsInChildren<Rigidbody>();
        for (int index = 0; index < bricksRb.Length ; index ++)
        {
            bricksRb[index].useGravity = true;
        }

        yield return new WaitForSeconds(1.75f);

        SceneManager.LoadScene(1);
    }


    public void Exit()
    {
        GameManager.Instance.SaveBest();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void SaveBestEver()
    {
        GameManager.Instance.SaveBest();
    }

    public void LoadBestEver()
    {
        GameManager.Instance.LoadBest();

    }
}
