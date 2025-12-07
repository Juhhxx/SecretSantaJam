using System;
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
        struct Portrait
        {
            public Actor actor;
            public Texture2D portrait;
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
            _gameEvents.portraitGenerated += AddPortrait;
            
        }

        private void OnDisable()
        {
            _gameEvents.portraitGenerated -= AddPortrait;
        }

        public void AddPortrait(Actor a, Texture2D p)
        {
            _portraits.Add(new Portrait(){actor = a, portrait = p});
            RectTransform portrait = Instantiate(portraitPrefab, portraitContainer);
            Portrait_UI portraitImage = portrait.GetComponent<Portrait_UI>();
            
            portraitImage.SetPortrait(a, p);
            
            portrait.gameObject.SetActive(true);
        }
    }
}