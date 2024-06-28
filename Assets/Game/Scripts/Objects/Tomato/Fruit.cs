using UnityEngine;

namespace Game.Scripts.Objects.Tomato
{
    using System;
    using DG.Tweening;

    public class Fruit : MonoBehaviour
    {
        [SerializeField] private Transform trans;

        private bool initialized;

        private Sequence reachTargetAnim;
        
        private bool isReady;
        public  bool IsReady => isReady;

        private void Start()
        {
            if (!initialized)
            {
                reachTargetAnim = DOTween.Sequence();

                reachTargetAnim.SetAutoKill(false);

                reachTargetAnim.Pause();
                
                reachTargetAnim.Append(trans.DOScale(new Vector3(4f, 4f, 4f), 0.2f).SetEase(Ease.OutBack));
                
                reachTargetAnim.Append(trans.DOScale(new Vector3(3f, 3f, 3f), 0.15f).SetEase(Ease.Linear));
                
                initialized = true;
            }   
        }

        public void Regenerate(Transform parent)
        {
            trans.DOKill();
            
            isReady = false;
            
            trans.SetParent(parent);

            trans.localScale = Vector3.zero;
            
            trans.localPosition = Vector3.zero;
            
            trans.DOScale(new Vector3(3, 3, 3), 0.2f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                isReady = true;
            });
        }

        public void MoveToTarget(Transform parent)
        {
            isReady = false;

            trans.DOKill();
            
            trans.SetParent(parent);

            trans.DOLocalMove(Vector3.zero, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
            {
                reachTargetAnim.Restart();
                
                isReady = true;
            });
        }

        public void Recycle()
        {
            trans.parent = null;
            
            isReady = false;

            gameObject.SetActive(false);
        }
    }
}
