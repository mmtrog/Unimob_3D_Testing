namespace Game.Scripts.Objects
{
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using Game.Scripts.Manager;
    using UnityEngine;

    public class Cash : MonoBehaviour
    {
        public bool IsReady { get; set; }

        public async void OnSpawn(Vector3 spawnPos, Vector3 targetLocalPos)
        {
            IsReady = false;
            
            transform.position = spawnPos;

            transform.DOKill();

            transform.DOLocalMove(targetLocalPos, 0.35f).SetEase(Ease.InOutSine);

            await UniTask.Delay(1000);
            
            IsReady = true;
        }
        
        public void MoveToTarget(Transform parent)
        {
            IsReady = false;

            transform.SetParent(parent);
            
            transform.DOKill();
            
            transform.DOLocalMove(new Vector3(0,0.5f, 0), 0.4f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                gameObject.SetActive(false);
                
                transform.SetParent(null);
                
                CurrencyManager.Instance.AddCash(1);
            });
        }
    }
}