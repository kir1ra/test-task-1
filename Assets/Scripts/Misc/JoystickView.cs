using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Test
{
    public class JoystickView : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Image _inputHandler;
        [SerializeField] private RectTransform _stickParent;
        [SerializeField] private Image _stick;

        private readonly Subject<Vector2> _onInput = new();

        public IObservable<Vector2> OnInput => _onInput;

        private void Start()
        {
            _inputHandler.OnMouseDownOnUIAsObservable()
                         .Do(_ => PositionJoystick())
                         .SelectMany(_ => _inputHandler.UpdateAsObservable())
                         .TakeUntil(_inputHandler.OnMouseUpOnUIAsObservable())
                         .DoOnCompleted(() => ResetStick())
                         .RepeatUntilDestroy(gameObject)
                         .Subscribe(mp => Drag())
                         .AddTo(this);
            
            ResetStick();
        }

        private void Drag()
        {
            var stickPosition = _stickParent.InverseTransformPoint(Input.mousePosition);
            var stickParentRect = _stickParent.rect;

            var radius = stickParentRect.width / 2;
            var distance = stickPosition.magnitude;
            if (distance > radius)
            {
                stickPosition = stickPosition.normalized * radius;
            }
                
            _stick.rectTransform.localPosition = stickPosition;
            _onInput.OnNext(_stick.rectTransform.localPosition / radius);            
        }

        private void PositionJoystick()
        {
            _stickParent.gameObject.SetActive(true);
            _stickParent.localPosition = _canvas.transform.InverseTransformPoint(Input.mousePosition);
        }

        private void ResetStick()
        {
            _stick.rectTransform.localPosition = Vector2.zero;
            _stickParent.gameObject.SetActive(false);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            ResetStick();
        }
        

    }
}