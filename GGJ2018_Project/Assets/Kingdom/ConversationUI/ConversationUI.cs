using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

using Wundee;
using Wundee.Stories;

using DG.Tweening;

using TMPro;


namespace Kingdom
{
    public class ConversationUI : MonoBehaviour
    {
        [Header("Self")]
        public TextMeshProUGUI conversationText;

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
            conversationText.text = 
                "<style=\"Title\">" + storyNode.parentLocation.definition.name + 
                "</style>\n" 
                + storyNode.definition.nodeText;

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
        }
    }
}