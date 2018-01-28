using System.Collections;
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


        private Tweener m_VisibilityTweener;

        public void SetVisible(bool p_IsVisible)
        {

            foreach (var canvasGroup in m_CanvasGroups)
            {
                canvasGroup.blocksRaycasts = p_IsVisible;
                canvasGroup.interactable = p_IsVisible;
            }
            
            if (m_VisibilityTweener != null)
            {
                m_VisibilityTweener.Kill();
            }

            var targetAlpha = p_IsVisible ? 1.0f : 0.0f;
            var currentAlpha = m_CanvasGroups[0].alpha;
            var AlphaDif = Mathf.Abs(targetAlpha - currentAlpha);

            m_VisibilityTweener = DOTween.To(() => m_CanvasGroups[0].alpha, x =>
            {
                foreach (var canvasGroup in m_CanvasGroups)
                {
                    canvasGroup.alpha = x;
                }
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

        }

        public void SetStartPosition()
        {
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