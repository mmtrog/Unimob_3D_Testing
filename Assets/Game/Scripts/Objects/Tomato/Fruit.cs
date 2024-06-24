using UnityEngine;

namespace Game.Scripts.Objects.Tomato
{
    using System;
    using DG.Tweening;

    public class Fruit : MonoBehaviour
    {
        [SerializeField] private Transform trans;
        
        private bool initialized;

        private Sequence openSequence;
        
        private void OnEnable()
        {
            if (!initialized)
            {
                openSequence = DOTween.Sequence();

                openSequence.SetAutoKill(false);

                openSequence.Pause();

                openSequence.Append(trans.DOScale(new Vector3(3, 3, 3), 0.4f).SetEase(Ease.OutBack));
                
                initialized = true;
            }

            trans.localScale = Vector3.zero;
            
            openSequence.Restart();
        }
    }
}
