using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class UICard : MonoBehaviour
{
    public Card Card;
    
    [Header("UI References")]
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Burn;
    public TextMeshProUGUI Damage;
    public TextMeshProUGUI SelectionOrder;

    public Button Button { get; private set; }
    
    void Awake ()
    {
        Button = GetComponent<Button>();
        SelectionOrder.text = "";
    }

    void Update ()
    {
        Title.text = Card.Name;
        Burn.text = Card.Burn.ToString();
        Damage.text = Card.Damage.ToString();
    }
}
