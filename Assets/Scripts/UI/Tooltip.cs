using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using crass;
using TMPro;

public class Tooltip : Singleton<Tooltip>
{
    public RectTransform Canvas;
    public Color DamageColor;
    public int TitleSize, PartOfSpeechBurnSize, DefinitionSize;
    public TextMeshProUGUI ContentMirror, LeftBurn, RightBurn;

    TextMeshProUGUI content;
    RectTransform rectTransform;

    TagPair damageTag;

    Card currentCard;
    
    void Awake ()
    {
        SingletonSetInstance(this, true);
        content = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();
        damageTag = new TagPair{ Start = $"<#{ColorUtility.ToHtmlStringRGB(DamageColor)}>", End = "</color>" };
    }

    void Update ()
    {
        bool trueIfLeft = rectTransform.anchoredPosition.x > 0;

        Vector3 pos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(Canvas, Input.mousePosition, CameraCache.Main, out pos);
        transform.position = pos;
        
        rectTransform.pivot = new Vector2
        (
            trueIfLeft ? 1 : 0,
            rectTransform.anchoredPosition.y > 0 ? 1 : 0            
        );

        if (currentCard == null) return;

        string burn = damageTag.Wrap(currentCard.Burn.ToString());
        LeftBurn.text = trueIfLeft ? burn : "";
        RightBurn.text = trueIfLeft ? "" : burn;
    }

    public void SetCard (Card card)
    {
        currentCard = card;

        if (card == null)
        {
            content.enabled = false;
            ContentMirror.text = "";
            LeftBurn.text = "";
            RightBurn.text = "";
            return;
        }
        content.enabled = true;
        content.text =
$@"<align=""center""><b><size={TitleSize}>{card.Word}</size></b>
<i><size={PartOfSpeechBurnSize}>{card.PartOfSpeech}</size></i></align>
<align=""left""><size={DefinitionSize}><b>meaning</b> {card.Definition} <b>usage</b> {card.EffectText}</size></align>";

        ContentMirror.text = content.text;
    }
}
