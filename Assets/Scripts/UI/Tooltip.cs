using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using crass;
using TMPro;

public class Tooltip : Singleton<Tooltip>
{
    public RectTransform Canvas;
    public float Delay;
    public Color DamageColor;
    public int TitleSize, PartOfSpeechBurnSize, DefinitionSize;
    public TextMeshProUGUI ContentMirror, LeftBurn, RightBurn;

    TextMeshProUGUI content;
    RectTransform rectTransform;

    TagPair damageTag;

    Card currentCard = null;
    IEnumerator setEnum;
    bool hide = true, onLeftOfCursor;
    
    void Awake ()
    {
        SingletonSetInstance(this, true);
        content = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();
        damageTag = new TagPair{ Start = $"<#{ColorUtility.ToHtmlStringRGB(DamageColor)}>", End = "</color>" };

        SetCard(null);
    }

    void Update ()
    {
        onLeftOfCursor = rectTransform.anchoredPosition.x > 0;

        Vector3 pos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(Canvas, Input.mousePosition, CameraCache.Main, out pos);
        transform.position = pos;

        rectTransform.pivot = new Vector2
        (
            onLeftOfCursor ? 1 : 0,
            rectTransform.anchoredPosition.y > 0 ? 1 : 0            
        );

        if (hide) return;

        setBurn();
    }

    public void SetCard (Card card)
    {
        if (card == currentCard) return;

        currentCard = card;

        hide = true;
        content.enabled = false;
        ContentMirror.text = "";
        LeftBurn.text = "";
        RightBurn.text = "";

        if (setEnum != null)
        {
            StopCoroutine(setEnum);
            setEnum = null;
        }

        if (card == null) return;

        setEnum = setRoutine(card);
        StartCoroutine(setEnum);
    }

    IEnumerator setRoutine (Card card)
    {
        yield return new WaitForSeconds(Delay);
        
        hide = false;

        content.enabled = true;
        content.text =
            $"<align=\"center\"><b><size={TitleSize}>{card.Word}</size></b>\n" +
            $"<i><size={PartOfSpeechBurnSize}>{card.PartOfSpeech}</size></i></align>\n" +
            $"<align=\"left\"><size={DefinitionSize}><b>meaning</b> {card.Definition} " +
            $"<b>usage</b> {card.EffectText}</size></align>";

        ContentMirror.text = content.text;

        setBurn();
    }

    void setBurn ()
    {
        string burn = damageTag.Wrap(currentCard.Burn.ToString());
        LeftBurn.text = onLeftOfCursor ? burn : "";
        RightBurn.text = onLeftOfCursor ? "" : burn;
    }
}
