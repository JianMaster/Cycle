using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameSelcetAward : MonoBehaviour
{
    public event Action<PropertyValue> selectAwardEvent;
    public Button btn1;
    public Text txt1;
    public Button btn2;
    public Text txt2;
    public Button btn3;
    public Text txt3;
    // Start is called before the first frame update
    public void Init(PropertyValue[] values)
    {
        btn1.onClick.AddListener(() => OnClick(values[0]));
        btn2.onClick.AddListener(() => OnClick(values[1]));
        btn3.onClick.AddListener(() => OnClick(values[2]));

        txt1.text = values[0].prop.ToString() + "+" + values[0].value;
        txt2.text = values[1].prop.ToString() + "+" + values[1].value;
        txt3.text = values[2].prop.ToString() + "+" + values[2].value;
    }

    void OnClick(PropertyValue value){
        selectAwardEvent(value);
        Destroy(gameObject);
    }

}
