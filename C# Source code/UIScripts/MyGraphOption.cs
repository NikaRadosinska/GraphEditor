using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MyGraphOption : MonoBehaviour
{
    private Settings settings;
    private UIManager uImanager;
    private int index;
    private SubGraphInfo subGraphInfo;
    public Button DeleteButton;
    public Button CopyToTextButton;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
        DeleteButton.onClick.AddListener(ToDelete);
        CopyToTextButton.onClick.AddListener(ToCopyToText);
    }

    public void Init(string Name, Settings settings, SubGraphInfo subGraphInfo, int index, UIManager uImanager)
    {
        transform.GetChild(0).GetComponent<TMP_Text>().text = Name;
        this.subGraphInfo = subGraphInfo;
        this.settings = settings;
        this.index = index;
        this.uImanager = uImanager;
    }

    public void OnClick()
    {
        settings.ChangeStateOnHold(subGraphInfo);
        transform.parent.parent.parent.parent.parent.gameObject.SetActive(false);
    }

    public void ToDelete()
    {
        settings.DeleteMyGraph(index);
    }

    public void ToCopyToText()
    {
        uImanager.InitOutputPanel(subGraphInfo.subGraphForOutput);
    }
}
