using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CTShared;
using TMPro;

public class UIKey : MonoBehaviour
{
    public KeyboardKey Key;

    Image border;
    TextMeshProUGUI label;

    UIKeyboard parent;

    public void Initialize (UIKeyboard parent)
    {
        this.parent = parent;
        
        border = GetComponent<Image>();

        label = GetComponentInChildren<TextMeshProUGUI>();
        label.text = Key.ToLabel();
        label.gameObject.AddComponent<UIKeyHover>().Initialize(parent.Agent, Key);
        
        SetActiveState(false);
    }

	public void SetActiveState (bool value)
    {
        border.color = value ? parent.BorderOnColor : parent.BorderOffColor;
        label.color = value ? parent.LabelOnColor : parent.LabelOffColor;
    }
}

public class UIKeyHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Agent agent;
    KeyboardKey key;

	public void OnPointerEnter (PointerEventData eventData)
	{
        Tooltip.Instance.SetTool(agent.Keyboard[key]);
	}

	public void OnPointerExit (PointerEventData eventData)
	{
        Tooltip.Instance.ClearTool();
	}

    public void Initialize (Agent agent, KeyboardKey key)
    {
        this.agent = agent;
        this.key = key;
    }
}
