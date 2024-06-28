using UnityEngine;

namespace Game.Scripts.Location
{
    using DG.Tweening;
    using Game.Scripts.Manager;
    using TMPro;
    using Camera = UnityEngine.Camera;

    public class BuildingLocation : MonoBehaviour
    {
        [SerializeField] private Transform labelTrans;
        
        [SerializeField] private Transform groundTrans;
        
        [SerializeField] private int cashRequired;

        [SerializeField] private GameObject location;

        [SerializeField] private TextMeshPro info;
        
        private Transform cameraTrans;

        private bool playerIn;

        private float timeCounter = 0.15f;

        private Sequence labelAnim;
        
        private void Start()
        {
            cameraTrans = Camera.main.transform;
            
            info.text = cashRequired.ToString();

            labelAnim = DOTween.Sequence();

            labelAnim.SetAutoKill(false);

            labelAnim.Pause();

            labelAnim.Append(labelTrans.DOScale(new Vector3(-0.5f, 0.5f, 0.5f), 0.3f)).SetEase(Ease.OutBack);
            
            labelAnim.Append(labelTrans.DOScale(new Vector3(-0.3f, 0.3f, 0.3f), 0.2f)).SetEase(Ease.Linear);
        }

        private void Update()
        {
            labelTrans.LookAt(cameraTrans);   
        }

        private void OnTriggerExit(Collider other)
        {
            if(!playerIn) return;
            
            if (other.gameObject.name != "Player") return;

            if (playerIn)
            {
                groundTrans.DOKill();
                
                groundTrans.DOScale(new Vector3(2f, 0.1f, 2f), 0.3f).SetEase(Ease.OutBack);
                
                playerIn = false;
            }   
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.name != "Player") return;

            if (!playerIn)
            {
                groundTrans.DOKill();
                
                groundTrans.DOScale(new Vector3(2.5f, 0.1f, 2.5f), 0.3f).SetEase(Ease.OutBack);
                
                playerIn = true;
            }

            if (timeCounter > 0)
            {
                timeCounter -= Time.deltaTime;
            }
            else
            {
                if (CurrencyManager.Instance.TryUseCash(1))
                {
                    cashRequired -= 1;

                    labelAnim.Restart();
                    
                    OnUseCash();
                }

                timeCounter = 0.15f;
            }
        }

        private void OnUseCash()
        {
            info.text = cashRequired.ToString(); 
            
            if (cashRequired <= 0)
            {
                gameObject.SetActive(false);
                
                location.SetActive(true);
            }
        }
    }
}
