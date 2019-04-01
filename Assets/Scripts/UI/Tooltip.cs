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
$@"<align=""center""><b><size={TitleSize}>{card.Word}</size></b>
<i><size={PartOfSpeechBurnSize}>{card.PartOfSpeech}; burns for {damageTag.Wrap(card.Burn.ToString())}</size></i></align>
<align=""left""><size={DefinitionSize}>{card.Definition}</size></align>";

        ContentMirror.text = content.text;
    }
}
