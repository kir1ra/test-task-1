using System;
using Test.Enemy;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Test.Player.Weapon.Projectile
{
    public class BoltView : MonoBehaviour
    {
        [Inject] DebugSettings _debugSettings;

        [SerializeField] Transform _rendererRoot;
        [Min(0)]
        [SerializeField] float _angularSpeed = 720f;

        private float _speed = 3f;
        private readonly Subject<int> _onEnemyHit = new();


        public Vector2 Position => transform.position;
        public IObservable<int> OnEnemyHit => _onEnemyHit;

        private void Start()
        {
            this.OnTriggerEnter2DAsObservable()
                .Select(other => other.GetComponent<EnemyView>())
                .Where(enemyView => enemyView != null)
                .Subscribe(enemyView => _onEnemyHit.OnNext(enemyView.EnemyId))
                .AddTo(this);

            if (_angularSpeed > 0)
                this.UpdateAsObservable()
                    .TakeUntilDisable(gameObject)
                    .RepeatUntilDestroy(gameObject)
                    .Subscribe(_ => transform.Rotate(Vector3.forward, _angularSpeed * Time.deltaTime))
                    .AddTo(this);
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        public void Move(Vector2 direction)
        {
            var oldPosition = transform.position;
            transform.position = Vector3.Lerp(oldPosition, oldPosition + (Vector3)direction * _speed, Time.deltaTime);
            if (_angularSpeed == 0)
                LookToward(direction);

            if (_debugSettings.EditorDrawLine)
                Debug.DrawLine(oldPosition + Vector3.back, transform.position, Color.red, 2f);
        }
        public void LookToward(Vector2 direction)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        }
        public void Teleport(Vector2 position)
        {
            transform.position = position;
        }
    }
}