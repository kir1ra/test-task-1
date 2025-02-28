using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _healthBar;

    [SerializeField] private float delay;
    [SerializeField] private RectTransform _delayedFill;

    private ReactiveProperty<float> _healthPercent = new(1);

    private void Start()
    {
        _healthPercent.Throttle(TimeSpan.FromSeconds(delay))
                      .Subscribe(UpdateDelayedBar)
                      .AddTo(this);
    }

    public void UpdateHealthPercent(float currentHealthPercent)
    {
        _healthPercent.Value = currentHealthPercent;
        _healthBar.DOValue(currentHealthPercent, .2f).SetEase(Ease.OutCubic);
    }

    void UpdateDelayedBar(float percent) 
    {
        _delayedFill.DOAnchorMax(new Vector2(percent, 1f), 1f)
                         .SetSpeedBased()
                         .SetEase(Ease.OutCubic);
    }
}