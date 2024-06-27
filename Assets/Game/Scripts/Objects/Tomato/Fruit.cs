using UnityEngine;

namespace Game.Scripts.Objects.Tomato
{
    using DG.Tweening;

    public class Fruit : MonoBehaviour
    {
        [SerializeField] private Transform trans;
        
        private bool initialized;

        private bool isReady;
        
        private Sequence openSequence;

        public Transform Transform => trans;

        public bool IsReady
        {
            get => isReady;
            set => isReady = value;
        }

        private void OnEnable()
        {
            if (!initialized)
            {
                openSequence = DOTween.Sequence();

                openSequence.SetAutoKill(false);

                openSequence.Pause();

                openSequence.Append(Transform.DOScale(new Vector3(2, 2, 2), 1f).SetEase(Ease.OutBack)).OnComplete(() =>
                {
                    isReady = true;
                });
                
                initialized = true;
            }

            isReady = false;
            
            Transform.localScale = Vector3.zero;
            
            openSequence.Restart();
        }

        public void Regenerate(Transform parent)
        {
            isReady = false;
            
            Transform.SetParent(parent);
            
            Transform.localPosition = Vector3.zero;
            
            gameObject.SetActive(true);
        }

        public void MoveToTarget(Transform parent)
        {
            IsReady = false;

            Transform.SetParent(parent);
            
            Transform.DOKill();
            
            Transform.DOLocalMove(Vector3.zero, 0.25f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                IsReady = true;
            });
        }

        public void Recycle()
        {
            trans.parent = null;
            
            gameObject.SetActive(false);
        }
    }
}
