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

                openSequence.Append(Transform.DOScale(new Vector3(3, 3, 3), 0.4f).SetEase(Ease.OutBack)).OnComplete(() =>
                {
                    isReady = true;
                });
                
                initialized = true;
            }

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

        public void Collect(Transform parent)
        {
            IsReady = false;

            Transform.SetParent(parent);
            
            Transform.DOKill();
            
            Transform.DOLocalMove(Vector3.zero, 0.3f).SetEase(Ease.InOutSine);
        }
    }
}
