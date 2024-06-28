namespace Game.Scripts.Objects
{
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using Game.Scripts.Manager;
    using UnityEngine;

    public class Cash : MonoBehaviour
    {
        [SerializeField] private Transform trans;
        
        public bool IsReady { get; set; }
        
        public async void OnSpawn(Vector3 spawnPos, Vector3 targetLocalPos)
        {
            IsReady = false;
            
            trans.position = spawnPos;

            trans.localScale = new Vector3(190f, 190f, 190f);

            trans.rotation = Quaternion.Euler(new Vector3(-90,0,0));
            
            trans.DOKill();

            trans.DOLocalMove(targetLocalPos, 0.35f).SetEase(Ease.InOutSine);

            await UniTask.Delay(1000);
            
            IsReady = true;
        }
        
        public void MoveToTarget(Transform parent)
        {
            IsReady = false;

            transform.SetParent(parent);

            trans.DOKill();

            trans.DOScale(new Vector3(100f, 100f, 100f), 0.3f).SetEase(Ease.InOutSine);
            
            trans.DOLocalMove(new Vector3(0, 0.8f, 0), 0.4f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                CurrencyManager.Instance.AddCash(1);

                gameObject.SetActive(false);

                trans.SetParent(null);
            });
        }
    }
}