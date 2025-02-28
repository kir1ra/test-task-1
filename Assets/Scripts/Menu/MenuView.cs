using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Test.Menu
{
    public class MenuView : MonoBehaviour
    {
        [SerializeField] Image Background;
        [SerializeField] RectTransform GameOverPanel;
        [SerializeField] Button PlayBtn, NewGameBtn, ReviveBtn;

        private readonly Subject<Unit> _onNewGameStarted = new();
        private readonly Subject<Unit> _onReviveAndContinueRequested = new();
        public IObservable<Unit> OnNewGameRequested => _onNewGameStarted;
        public IObservable<Unit> OnReviveAndContinueRequested => _onReviveAndContinueRequested;

        private void Start()
        {
            GameOverPanel.transform.localScale = Vector3.zero;
            NewGameBtn.transform.localScale = Vector3.zero;
            ReviveBtn.transform.localScale = Vector3.zero;

            PlayBtn.OnClickAsObservable().Subscribe(_onNewGameStarted.OnNext).AddTo(this);
            NewGameBtn.OnClickAsObservable().Subscribe(_onNewGameStarted).AddTo(this);
            ReviveBtn.OnClickAsObservable().Subscribe(_onReviveAndContinueRequested.OnNext).AddTo(this);
        }

        internal async UniTask HideStartButton()
        {
            await PlayBtn.transform.DOScale(0, .4f).SetEase(Ease.InBack);
            await FadeOutBkg();
            gameObject.SetActive(false);
        }

        internal async UniTask ShowGameOverPanel()
        {
            gameObject.SetActive(true);

            await FadeInBkg(1f);
            await GameOverPanel.DOScale(1f, .4f).SetEase(Ease.OutBack);
            await ReviveBtn.transform.DOScale(1f, .4f).SetEase(Ease.OutBack);
            await NewGameBtn.transform.DOScale(1f, .4f).SetEase(Ease.OutBack);
        }

        internal async UniTask HideGameOverPanel()
        {
            await GameOverPanel.transform.DOScale(0, .4f).SetEase(Ease.InBack);
            await FadeOutBkg();
            gameObject.SetActive(false);

            NewGameBtn.transform.localScale = Vector3.zero;
            ReviveBtn.transform.localScale = Vector3.zero;
        }

        private async UniTask FadeInBkg(float duration = 0.2f) => await Background.DOFade(.8f, duration);
        private async UniTask FadeOutBkg(float duration = 0.2f) => await Background.DOFade(0f, duration);

    }
}
    