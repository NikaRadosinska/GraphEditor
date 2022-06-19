using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MyGraphsPanel : MonoBehaviour
{
    public Button exitButton;

    public Transform contentHolder;
    [SerializeField]
    private MyGraphOption myGraphOptionPrefab; 

    public void Init(Settings settings, UIManager uIManager)
    {
        exitButton.onClick.AddListener(OnExitButtonClicked);

        for (int i = 0; i < contentHolder.childCount; i++)
        {
            //Debug.Log("Destroy");
            Destroy(contentHolder.GetChild(i).gameObject);
        }

        for (int i = 0; i < settings.PlayerSettings.MyGraphs.Count; i++)
        {
            //Debug.Log("Create");
            MyGraphOption option = Instantiate(myGraphOptionPrefab, contentHolder).GetComponent<MyGraphOption>();
            option.Init(settings.PlayerSettings.MyGraphNames[i], settings, settings.PlayerSettings.MyGraphs[i], i, uIManager);
        }
    }

    private void OnExitButtonClicked()
    {
        gameObject.SetActive(false);
    }
}
