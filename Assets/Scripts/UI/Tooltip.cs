using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using crass;
using TMPro;

public class Tooltip : Singleton<Tooltip>
{
    public Color DamageColor;
    public int TitleSize, PartOfSpeechBurnSize, DefinitionSize;
    public TextMeshProUGUI ContentMirror;

    TextMeshProUGUI content;
    RectTransform rectTransform;
    string alignment;

    TagPair damageTag;
    
    void Awake ()
    {
        SingletonSetInstance(this, true);
        content = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();
        damageTag = new TagPair{ Start = $"<#{ColorUtility.ToHtmlStringRGB(DamageColor)}>", End = "</color>" };
    }

    void Update ()
    {
        transform.position = Input.mousePosition;
        rectTransform.pivot = new Vector2
        (
            rectTransform.anchoredPosition.x > 0 ? 1 : 0,
            rectTransform.anchoredPosition.y > 0 ? 1 : 0            
        );

        alignment = rectTransform.anchoredPosition.x > 0 ? "left" : "right";
    }

    public void SetCard (Card card)
    {
        if (card == null)
        {
            content.enabled = false;
            ContentMirror.text = "";
            return;
        }
        content.enabled = true;
        content.text =
$@"<align=""{alignment}""><b><size={TitleSize}>{card.Name}</size></b>
<i><size={PartOfSpeechBurnSize}>verb; burns for {damageTag.Wrap(card.Burn.ToString())}</size></i>
<size={DefinitionSize}>do {damageTag.Wrap(card.Damage.ToString())} damage to one's opponent.</size></align>";

        ContentMirror.text = content.text;
    }
}
