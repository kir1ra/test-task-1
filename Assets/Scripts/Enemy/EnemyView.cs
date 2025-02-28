using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Test.Enemy
{
    public class EnemyView : MonoBehaviour
    {
        [Inject(Id = "EnemyId")] public readonly int EnemyId;

        [SerializeField] private Transform _rendererRoot;
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private ParticleSystem _deathPS;

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
            transform.position = oldPosition + (Vector3)direction * _speed * Time.deltaTime;
        }
        public void Teleport(Vector2 position)
        {
            transform.position = position;
        }

        public void Rotate(Vector2 direction)
        {
            _rendererRoot.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        }

        public void UpdateHealthPercent(float currentHealthPercent)
        {
            _healthBar.gameObject.SetActive(currentHealthPercent > 0 && currentHealthPercent < 1);
            _healthBar.UpdateHealthPercent(currentHealthPercent);
        }

        public async UniTaskVoid AnimateHit(float duration = .1f)
        {
            _hitTween?.Kill(true);
            _hitTween = _renderer.DOColor(Color.red, duration / 2).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Flash);
            await _hitTween.Play();
        }

        public async UniTask AnimateDeath()
        {
            _rendererRoot.gameObject.SetActive(false);
            _deathPS.Play();
            await UniTask.WaitForSeconds(_deathPS.main.duration);
        }

        public void Reset()
        {
            _rendererRoot.gameObject.SetActive(true);
        }
    }
}