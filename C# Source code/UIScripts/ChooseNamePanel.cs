using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChooseNamePanel : MonoBehaviour
{
    public TMP_InputField inputField;
    public Button submitButton;
    private Settings settings;

    private void Start()
    {
        submitButton.onClick.AddListener(SubmitButton);
    }

    public void Init(Settings settings)
    {
        inputField.text = "";
        this.settings = settings;
    }

    public void SubmitButton()
    {
        if(inputField.text != "" && !settings.PlayerSettings.MyGraphNames.Contains(inputField.text))
        {
            settings.SaveName(inputField.text);
            gameObject.SetActive(false);
        }
    }
}
