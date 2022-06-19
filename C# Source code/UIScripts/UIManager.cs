using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MainBehaviour
{
    public Button ExitButton;

    public Button CtrlButton;
    public Button AddButton;

    public Button MyGraphButton;
    public Button SettingsButton;

    public Button UndoButon;
    public Button RedoButon;

    public MyGraphsPanel MyGraphsPanel;
    public ChooseNamePanel ChooseNamePanel;
    public GameObject HintsPanel;
    public OutputPanel OutputPanel;
    public SettingsPanel SettingsPanel;

    private void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        ExitButton.onClick.AddListener(OnExitButtonClicked);
        MyGraphButton.onClick.AddListener(OpenMyGraphsPanel);
        UndoButon.onClick.AddListener(OnUndoButton);
        RedoButon.onClick.AddListener(OnRedoButton);
        SettingsButton.onClick.AddListener(OnSettingsButton);

        FromCtrl();
        FromAdd();

        MyGraphsPanel.gameObject.SetActive(false);
        ChooseNamePanel.gameObject.SetActive(false);
        HintsPanel.SetActive(false);
        OutputPanel.gameObject.SetActive(false);
        SettingsPanel.gameObject.SetActive(false);
    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }

    public void ToCtrl()
    {
        CtrlButton.interactable = true;
    }

    public void FromCtrl()
    {
        CtrlButton.interactable = false;
    }

    public void ToAdd()
    {
        AddButton.interactable = true;
    }

    public void OpenMyGraphsPanel()
    {
        MyGraphsPanel.gameObject.SetActive(true);
        MyGraphsPanel.Init(Settings, this);
    }

    public void FromAdd()
    {
        AddButton.interactable = false;
    }

    public void SaveName()
    {
        ChooseNamePanel.gameObject.SetActive(true);
        ChooseNamePanel.Init(Settings);
    }

    public void ShowHints()
    {
        HintsPanel.SetActive(!HintsPanel.activeInHierarchy);
    }

    public void HideHints()
    {
        HintsPanel.SetActive(false);
    }

    public void InitOutputPanel(List<Vector2> indexes)
    {
        OutputPanel.gameObject.SetActive(true);
        OutputPanel.Init(indexes);
    }

    public void OnUndoButton()
    {
        GraphManager.Undo();
    }

    public void OnRedoButton()
    {
        //GraphManager.Redo();
    }

    public void OnSettingsButton()
    {
        SettingsPanel.Init();
    }
}
