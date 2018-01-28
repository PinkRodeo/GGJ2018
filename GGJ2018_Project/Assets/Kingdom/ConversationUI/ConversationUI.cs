﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

using Wundee;
using Wundee.Stories;

using DG.Tweening;

using TMPro;
using System;

namespace Kingdom
{
    public class ConversationUI : MonoBehaviour
    {
        [Header("Self")]
        public TextMeshProUGUI HeaderText;
        public TextMeshProUGUI MainText;
        public RectTransform MainPanel;

        public Button SealButton;

        [Header("Choices")]
        public RectTransform buttonLayout;

        public GameObject choiceButtonPrefab;

        [SerializeField]
        private CanvasGroup[] m_CanvasGroups;
        [SerializeField]
        private CanvasGroup m_MainCanvasGroup;
        [SerializeField]
        private CanvasGroup m_FadeCanvasGroup;
        
        [SerializeField]
        private AudioMixer m_AudioMixer;

        private AudioMixerSnapshot mainSnapshot;
        private AudioMixerSnapshot eventSnapshot;

        void Awake()
        {
            mainSnapshot = m_AudioMixer.FindSnapshot("Overworld");
            eventSnapshot = m_AudioMixer.FindSnapshot("InEvent");

            foreach(var canvasGroup in m_CanvasGroups)
            {
                canvasGroup.alpha = 0;
                canvasGroup.blocksRaycasts = false;
                canvasGroup.interactable = false;
            }

            SealButton.onClick.AddListener(OnSealButtonClicked);
        }

        // Use this for initialization
        void Start()
        {
            var game = KingdomGameEntry.gameInstance;

            game.SetConversationUI(this);
            
           var spyMessage = game.world.locations["LOC_SPYMASTERS_HOUSE"].storyHolder.AddStoryNode("SPY_TEST_1");

            

           SetToStoryNode(spyMessage);
            
        }

        public void SetToStoryNode(StoryNode storyNode)
        {
            HeaderText.text = "SPY REPORT - " + storyNode.parentLocation.definition.name;

            MainText.text = storyNode.definition.nodeText;

            // choices
            
            // TODO: clean up these children
            buttonLayout.DetachChildren();
            foreach (var choice in storyNode.storyChoices)
            {
                var choiceButtonGO = Instantiate<GameObject>(choiceButtonPrefab, buttonLayout);
                var choiceButtonUI = choiceButtonGO.GetComponent<ConversationButtonUI>();

                choiceButtonUI.SetToStoryChoice(this, choice);
            }

            SetVisible(true);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                var game = KingdomGameEntry.gameInstance;

                var spyMessageDefinition = game.definitions.storyNodeDefinitions["SPY_TEST_1"];
                var spyMessage = spyMessageDefinition.GetConcreteType(null) as StoryNode;
                
                SetToStoryNode(spyMessage);
            }
        }


        private Tweener m_VisibilityTweenerMain;

        public void SetVisible(bool p_IsVisible)
        {
            m_MainCanvasGroup.blocksRaycasts = p_IsVisible;
            m_MainCanvasGroup.interactable = p_IsVisible;

            
            if (m_VisibilityTweenerMain != null)
            {
                m_VisibilityTweenerMain.Kill();
                m_VisibilityTweenerMain = null;
            }

            var targetAlpha = p_IsVisible ? 1.0f : 0.0f;
            var currentAlpha = m_MainCanvasGroup.alpha;
            var AlphaDif = Mathf.Abs(targetAlpha - currentAlpha);

            m_VisibilityTweenerMain = DOTween.To(() => m_MainCanvasGroup.alpha, x =>
            {
                m_MainCanvasGroup.alpha = x;
            }, targetAlpha, AlphaDif * 0.4f).SetEase(Ease.InCirc);

            if (p_IsVisible)
            {
                eventSnapshot.TransitionTo(AlphaDif);
            }
            else
            {
                mainSnapshot.TransitionTo(AlphaDif);
            }
            
            MainPanel.localPosition = new Vector3(0,0,0);
            SealButton.gameObject.SetActive(false);
            SetVisibleFade(p_IsVisible);
        }

        private Tweener m_VisibilityTweenerBackground;

        public void SetVisibleFade(bool p_IsVisible)
        {
            m_FadeCanvasGroup.blocksRaycasts = p_IsVisible;
            m_FadeCanvasGroup.interactable = p_IsVisible;


            if (m_VisibilityTweenerBackground != null)
            {
                m_VisibilityTweenerBackground.Kill();
                m_VisibilityTweenerBackground = null;
            }

            var targetAlpha = p_IsVisible ? 1.0f : 0.0f;
            var currentAlpha = m_FadeCanvasGroup.alpha;
            var AlphaDif = Mathf.Abs(targetAlpha - currentAlpha);

            m_VisibilityTweenerBackground = DOTween.To(() => m_FadeCanvasGroup.alpha, x =>
            {
                m_FadeCanvasGroup.alpha = x;
            }, targetAlpha, AlphaDif * 0.4f).SetEase(Ease.InCirc);

            if (p_IsVisible)
            {
                eventSnapshot.TransitionTo(AlphaDif);
            }
            else
            {
                mainSnapshot.TransitionTo(AlphaDif);
            }
        }

        public void SetStartPosition()
        {
            SetVisibleFade(false);

            MainPanel.localPosition = new Vector3(0, -800, 0);

            SealButton.enabled = true;
            SealButton.gameObject.SetActive(true);
        }

        private Tweener m_ComeIntoViewTweener;


        private void OnSealButtonClicked()
        {
            if (m_ComeIntoViewTweener != null)
            {
                m_ComeIntoViewTweener.Kill();
                m_ComeIntoViewTweener = null;
            }

            SetVisibleFade(true);


            SealButton.enabled = false;

            m_ComeIntoViewTweener = DOTween.To(() => MainPanel.localPosition, x =>
            {
                MainPanel.localPosition = x;
            }, new Vector3(0, 0, 0), 1.1f).SetEase(Ease.OutBounce);

            m_ComeIntoViewTweener.OnComplete(
                () => SealButton.gameObject.active = false 
            );

        }

        public void SetSpyMessageMode()
        {
            

        }

        public void SetRegularMessageMode()
        {

        }
    }
}