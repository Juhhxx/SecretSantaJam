using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TurnQueue_UIPanel : MonoBehaviour
    {
        [SerializeField] private GameVariableSettings _gameEvents;
        [SerializeField] private RectTransform portraitContainer;
        [SerializeField] private RectTransform portraitPrefab;

        class Portrait
        {
            public Actor actor;
            public RectTransform portrait;
        }

        private List<Portrait> _portraits = new List<Portrait>();

        private void Awake()
        {
            if (portraitPrefab.gameObject.activeInHierarchy)
            {
                portraitPrefab.gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            _gameEvents.gameStart += GameStart;
            _gameEvents.portraitGenerated += AddPortrait;
        }

        private void OnDisable()
        {
            _gameEvents.portraitGenerated -= AddPortrait;
            _gameEvents.gameStart -= GameStart;
        }

        private Coroutine _setup;

        public void GameStart()
        {
            IEnumerator ShowPanel()
            {
                foreach (var p in _portraits)
                {
                    LTSeq seq = LeanTween.sequence();
                    CanvasGroup pCanvasGroup = p.portrait.GetComponent<CanvasGroup>();
                    seq.append(() => { pCanvasGroup.alpha = 0; });

                    seq.append(() =>
                    {
                        pCanvasGroup.LeanAlpha(1f, 0.3f);
                        // p.portrait.LeanMoveY(p.portrait.position.y + 1f, 0.7f).setEaseInCubic();
                    });

                    seq.append(p.portrait.LeanScale(Vector3.one * 1.1f, 0.4f).setEasePunch());
                    yield return new WaitForSeconds(0.2f);
                }

                yield return new WaitUntil(() =>
                    !LeanTween.isTweening(_portraits[^1].portrait.gameObject));

                _setup = null;
            }

            _setup = StartCoroutine(ShowPanel());
        }

        private RectTransform lastPortraitUsed;

        public void ChangeTurn(Actor actor)
        {
            LTSeq seq = LeanTween.sequence();
            if (lastPortraitUsed != null)
            {
                LeanTween.cancel(lastPortraitUsed);
                seq.append(lastPortraitUsed.LeanScale(Vector3.one, 0.2f).setEaseOutCubic());
            }

            Portrait portrait = null;
            foreach (var p in _portraits)
            {
                if (p.actor == actor)
                {
                    portrait = p;
                    break;
                }
            }

            if (portrait != null)
            {
                RectTransform p = portrait.portrait;
                CanvasGroup pCanvasGroup = p.GetComponent<CanvasGroup>();
                pCanvasGroup.alpha = 1;
                LeanTween.cancel(p.gameObject);

                seq.append(p.LeanScale(Vector3.one * 1.2f, 0.4f).setEaseInCubic());
                seq.append(p.LeanScale(Vector3.one * 1.3f, 0.2f).setEasePunch());
            
                lastPortraitUsed = p;
            }
            
        }

        public void AddPortrait(Actor a, Texture2D p)
        {
            RectTransform portrait = Instantiate(portraitPrefab, portraitContainer);
            Portrait_UI portraitImage = portrait.GetComponent<Portrait_UI>();
            CanvasGroup pCanvasGroup = portraitImage.GetComponent<CanvasGroup>();
            pCanvasGroup.alpha = 0;
            portraitImage.gameObject.SetActive(true);
            portraitImage.SetPortrait(a, p);
            _portraits.Add(new Portrait() { actor = a, portrait = portrait });
        }
    }
}