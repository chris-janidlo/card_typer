using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int Max;
    public Image Bar;

    [SerializeField]
    int _value;
    public int Value
    {
        get => _value;
        set
        {
            _value = Mathf.Clamp(value, 0, Max);
            Bar.fillAmount = (float) _value / Max;
            if (_value == 0)
            {
                var temp = Death;
                if (temp != null) temp(this);
            }
        }
    }

    public event System.Action<Health> Death;

    void Awake ()
    {
        Value = Max;
    }
}
