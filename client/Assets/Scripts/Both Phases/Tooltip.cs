using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CTShared;
using TMPro;
using crass;

public class Tooltip : Singleton<Tooltip>
{
    public RectTransform Canvas;
    public float Delay;
    public TextMeshProUGUI ContentMirror;

    public int TitleSize, PartOfSpeechBurnSize, DefinitionSize;

    TextMeshProUGUI content;
    RectTransform rectTransform;

    object currentTool; // what we're hovering over
    
    void Awake ()
    {
        SingletonSetInstance(this, true);
        content = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();

        currentTool = 137; // so it's not equal to null and we clear out the display in the next call
        setTool(null);
    }

    void Update ()
    {
        Vector3 pos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(Canvas, Input.mousePosition, CameraCache.Main, out pos);
        transform.position = pos;

        rectTransform.pivot = new Vector2
        (
            rectTransform.anchoredPosition.x > 0 ? 1 : 0,
            rectTransform.anchoredPosition.y > 0 ? 1 : 0            
        );
    }

    public void ClearTool ()
    {
        setTool(null);
    }

    public void SetTool (Card card)
    {
        setTool(card);
    }

    public void SetTool (KeyState keyState)
    {
        setTool(keyState);
    }

    // the tooltip shows info on a tool
    void setTool (object tool)
    {
        // Debug.Log(tool.GetType().Name);
        if (tool == currentTool) return;

        currentTool = tool;

        content.enabled = false;
        ContentMirror.text = "";

        StopAllCoroutines();

        if (tool == null) return;

        StartCoroutine(setRoutine(tool));
    }

    IEnumerator setRoutine (object tool)
    {
        yield return new WaitForSeconds(Delay);

        string contentText = "";

        switch (tool)
        {
            case Card c:
                contentText = getText(c);
                break;

            case KeyState k:
                contentText = getText(k);
                break;

            default:
                throw new NotImplementedException($"the type {tool.GetType().Name} is not currently supported for tooltips");
        }

        content.enabled = true;

        content.text = contentText;
        ContentMirror.text = contentText;
    }

    string getText (Card card)
    {
        return
            $"<align=\"center\"><b><size={TitleSize}>{card.Word}</size></b>\n" +
            $"<i><size={PartOfSpeechBurnSize}>{card.PartOfSpeech} · <b>{card.Burn.ToString()}</b></size></i></align>\n" +
            $"<align=\"left\"><size={DefinitionSize}><b>meaning</b> {card.Definition} " +
            $"<b>usage</b> {card.EffectText}</size></align>";
    }

    string getText (KeyState key)
    {
        return $"<size={TitleSize}>this is a key</size>";
    }
}
