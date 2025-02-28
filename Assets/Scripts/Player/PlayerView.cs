using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Test.Player
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private Transform _rendererRoot;
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private HealthBar _healthBar;

        private float _speed;
        private Tweener _hitTween;

        public Vector2 Position => transform.position;

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        public void Move(Vector2 direction)
        {
            var oldPosition = transform.position;
            transform.position = Vector3.Lerp(oldPosition, oldPosition + (Vector3)direction * _speed, Time.deltaTime);
            Rotate(direction);
        }
        public void Rotate(Vector2 direction)
        {
            _rendererRoot.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        }

        public void UpdateHealthPercent(float currentHealthPercent)
        {
            _healthBar.UpdateHealthPercent(currentHealthPercent);
        }

        public async UniTaskVoid AnimateHit(float duration = .1f)
        {
            _hitTween?.Kill(true);
            _hitTween = _renderer.DOColor(Color.red, duration / 2).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Flash);
            await _hitTween.Play();
        }

        public async UniTask AnimateRevive(float duration = 2f)
        {
            var collider = gameObject.AddComponent<CircleCollider2D>();
            await DOTween.To(() => collider.radius, r => collider.radius = r, 5, duration);
            Destroy(collider);
        }
    }
}