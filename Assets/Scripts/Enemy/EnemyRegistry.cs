using System;
using System.Collections.Generic;
using UniRx;

namespace Test.Enemy
{
    public class EnemyRegistry
    {
        readonly ReactiveDictionary<int, EnemyPresenter> _enemies = new ReactiveDictionary<int, EnemyPresenter>();
        public IReadOnlyReactiveProperty<int> EnemyCount { get; private set; }

        public EnemyRegistry()
        {
            _enemies = new ReactiveDictionary<int, EnemyPresenter>();
            EnemyCount = _enemies.ObserveCountChanged().ToReadOnlyReactiveProperty(); ;
        }

        public EnemyPresenter this[int index]
        {
            get
            {
                _enemies.TryGetValue(index, out EnemyPresenter result);
                return result;
            }
        }

        public bool ContainsIndex(int index) => _enemies.ContainsKey(index);

        public IEnumerable<EnemyPresenter> Enemies
        {
            get { return _enemies.Values; }
        }

        public void AddEnemy(int id, EnemyPresenter enemy)
        {
            _enemies.Add(id, enemy);
        }

        public void RemoveEnemy(int id)
        {
            _enemies.Remove(id);
        }


    }
}
