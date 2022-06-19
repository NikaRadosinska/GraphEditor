using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class OutputPanel : MonoBehaviour
{
    public Toggle indexesToggle;
    public Toggle freeEdgesToggle;

    public Button toCopyButton;
    public Button exitButton;

    public TMP_Text text;

    private List<List<int>> indexes;

    void Start()
    {
        indexesToggle.onValueChanged.AddListener(IndexToggleChanged);
        freeEdgesToggle.onValueChanged.AddListener(FreeEdgesToggleChanged);
        toCopyButton.onClick.AddListener(OnToCopyButton);
        exitButton.onClick.AddListener(OnExitButtonPressed);
    }

    public void Init(List<Vector2> indexes)
    {
        this.indexes = new List<List<int>>();

        int c = 0;
        while (indexes.Count > c)
        {
            if (this.indexes.Count <= indexes[c].x)
            {
                this.indexes.Add(new List<int>());
            } 
            else
            {
                this.indexes[(int)indexes[c].x].Add((int)indexes[c].y);
                c++;
            }
        }

        text.text = "";
        for (int i = 0; i < this.indexes.Count; i++)
        {
            text.text += i;
            text.text += ":";

            for (int j = 0; j < this.indexes[i].Count; j++)
            {
                text.text += " " + this.indexes[i][j];
            }

            text.text += "\n";
        }
    }

    private void OnExitButtonPressed()
    {
        gameObject.SetActive(false);
    }

    private void IndexToggleChanged(bool val)
    {
        ChangeText(val, freeEdgesToggle.isOn);
    }

    private void FreeEdgesToggleChanged(bool val)
    {
        ChangeText(indexesToggle.isOn, val);
    }

    private void OnToCopyButton()
    {
        GUIUtility.systemCopyBuffer = text.text;
        Debug.Log(GUIUtility.systemCopyBuffer);
        gameObject.SetActive(false);
    }

    private void ChangeText(bool haveIndexes, bool haveFreeEdges)
    {
        text.text = "";
        for (int i = 0; i < indexes.Count; i++)
        {
            if (haveIndexes)
            {
                text.text += i;
                text.text += ":";
            }

            bool added = false;

            for (int j = 0; j < indexes[i].Count; j++)
            {
                if(!haveFreeEdges && indexes[i][j] == -1)
                {
                    continue;
                }

                if ((j == 0 || !added) && !haveIndexes)
                {
                    added = true;
                    text.text += indexes[i][j];
                } 
                else
                {
                    added = true;
                    text.text += " " + indexes[i][j];
                }
            }

            if (added) text.text += "\n";
        }
    }
}
