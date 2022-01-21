using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MainBehaviour
{
    public Button exitButton;

    private void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        exitButton.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        Application.Quit();
    }
}
