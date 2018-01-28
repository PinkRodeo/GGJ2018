using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using Wundee.Stories;

using DG.Tweening;

using Wundee;
using Kingdom;

namespace Kingdom
{
    public class BackgroundController : MonoBehaviour
    {
        public string locationKey;

        private CanvasGroup mainCanvasGroup;
        private Image[] childElements;
        private Vector3[] childElementsLocations;

        private bool m_IsBecomingVisible = false;

        void Awake()
        {
            mainCanvasGroup = GetComponentInChildren<CanvasGroup>();
            if (mainCanvasGroup == null)
            {
                mainCanvasGroup = GetComponent<CanvasGroup>();
            }

            if (mainCanvasGroup == null)
            {
                Debug.Log("NO canvas for " + locationKey); 
            }

            mainCanvasGroup.alpha = 0.0f;
            /*
            childElements = GetComponentsInChildren<Image>();
            childElementsLocations = new Vector3[childElements.Length];
            for (int i = 0; i < childElements.Length; i++)
            {
                childElementsLocations[i] = childElements[i].rectTransform.localPosition;

                childElements[i].rectTransform.localPosition = childElementsLocations[i] + new Vector3(0, -400, 0);
            }
            */
        }

        // Use this for initialization
        void Start()
        {

#if UNITY_EDITOR
            Game.instance.definitions.locationDefinitions.GetCopy().ContainsKey(locationKey);
#endif

            Invoke("Initialize", 0.5f);
        }

        void Initialize()
        {
            Game.instance.conversationUI.OnLocationOpened += OnLocationOpened;

        }

        public void OnLocationOpened (string p_LocationKey)
        {
            if (p_LocationKey == locationKey)
            {
                Debug.Log("hey there, it's happening");
                SetBackgroundVisible(true);
                Game.instance.conversationUI.OnLocationClosed += OnLocationClosed;

            }
        }


        public void OnLocationClosed (string p_LocationKey)
        {
            if (p_LocationKey == locationKey)
            {
                SetBackgroundVisible(false);
                Game.instance.conversationUI.OnLocationClosed -= OnLocationClosed;
            }
        }


        private Tweener m_VisibilityTweener;

        public void SetBackgroundVisible(bool p_IsVisible)
        {
            if (m_IsBecomingVisible == p_IsVisible)
                return;

            m_IsBecomingVisible = p_IsVisible;

            if (m_VisibilityTweener != null)
            {
                m_VisibilityTweener.Kill();
                m_VisibilityTweener = null;
            }

            var targetAlpha = p_IsVisible ? 1.0f : 0.0f;
            var currentAlpha = mainCanvasGroup.alpha;
            var alphaDif = Mathf.Abs(targetAlpha - currentAlpha);

            m_VisibilityTweener = DOTween.To(() => mainCanvasGroup.alpha, x =>
            {
                mainCanvasGroup.alpha = x;
            }, targetAlpha, alphaDif * 1.5f).SetEase(Ease.InBack);

            /*
            for (int i = 0; i < childElements.Length; i++)
            {
                childElements[i].DOKill();
                DOTween.To(
                    () => childElements[i].rectTransform.localPosition, 
                    x => { childElements[i].rectTransform.localPosition = x; }, 
                    childElementsLocations[i] + new Vector3(0, p_IsVisible ? 0 : 0, 0), 
                    alphaDif * 1.5f * Random.Range(0.4f, 0.8f))
                    
                    .SetDelay(Random.Range(0.0f, 0.1f));
            }
            */

          
        }


        
    }

}
