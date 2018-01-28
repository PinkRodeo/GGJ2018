using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

using Wundee;
using Wundee.Stories;

using DG.Tweening;

namespace Kingdom
{
    public class ConversationUI : MonoBehaviour
    {
        [Header("Self")]
        public Text conversationText;

        [Header("Choices")]
        public RectTransform buttonLayout;

        public GameObject choiceButtonPrefab;

        [SerializeField]
        private CanvasGroup m_CanvasGroup;

        [SerializeField]
        private AudioMixer m_AudioMixer;

        private AudioMixerSnapshot mainSnapshot;
        private AudioMixerSnapshot eventSnapshot;

        void Awake()
        {
            mainSnapshot = m_AudioMixer.FindSnapshot("Overworld");
            eventSnapshot = m_AudioMixer.FindSnapshot("InEvent");

            m_CanvasGroup.alpha = 0;
            m_CanvasGroup.blocksRaycasts = false;
            m_CanvasGroup.interactable = false;
        }

        // Use this for initialization
        void Start()
        {
            var game = KingdomGameEntry.gameInstance;

            
            
            var spyMessage = game.world.locations["LOC_SMUGGLERS_DEN"].storyHolder.AddStoryNode("SPY_TEST_1");
            
            SetToStoryNode(spyMessage);
            
        }

        public void SetToStoryNode(StoryNode storyNode)
        {
            conversationText.text = storyNode.definition.nodeText;

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
            m_CanvasGroup.blocksRaycasts = p_IsVisible;
            m_CanvasGroup.interactable = p_IsVisible;

            if (m_VisibilityTweener != null)
            {
                m_VisibilityTweener.Kill();
            }

            var targetAlpha = p_IsVisible ? 1.0f : 0.0f;
            var currentAlpha = m_CanvasGroup.alpha;
            var AlphaDif = Mathf.Abs(targetAlpha - currentAlpha);

            m_VisibilityTweener = DOTween.To(() => m_CanvasGroup.alpha, x =>
                  m_CanvasGroup.alpha = x, targetAlpha, AlphaDif * 0.3f).SetEase(Ease.InCirc);

            if (p_IsVisible)
            {
                eventSnapshot.TransitionTo(AlphaDif);
            }
            else
            {
                mainSnapshot.TransitionTo(AlphaDif);
            }
        }
    }
}