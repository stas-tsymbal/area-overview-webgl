using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 * Icon sample, use for show icon
 */
public class IconPrefabSample : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;

    public void SetImage(Sprite _sprite)
    {
        image.sprite = _sprite;
    }

    public void SetText(string _text)
    {
        text.text = _text;
    }
    
    // close this window
    public void Close()
    {
        IconViewController.Instance.Close();
    }
}
