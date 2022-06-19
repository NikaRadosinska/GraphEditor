using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : AccessBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnClickAction);
    }

    // Update is called once per frame
    void Update()
    {
        //VYRIESIT ABY ONCLICK NAJPRV VEDEL VYBRAT VERTEXY POTOM EDGEENDY
    }

    private void OnClickAction()
    {

    }
}
