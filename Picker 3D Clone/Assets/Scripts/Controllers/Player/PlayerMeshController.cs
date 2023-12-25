using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Controllers.Player
{
    public class PlayerMeshController : MonoBehaviour
    {
        [SerializeField] private new Renderer renderer;
        [SerializeField] private TextMeshPro scaleText;
        [SerializeField] private ParticleSystem confetti;

        private PlayerData.PlayerMeshData _data;

        internal void SetData(PlayerData.PlayerMeshData data)
        {
            _data = data;
        }

        internal void ScaleUpPlayer()
        {
            renderer.gameObject.transform.DOScaleX(_data.scaleCounter, 1).SetEase(Ease.Flash);
            
        }

        internal void ShowUpText()
        {
            scaleText.DOFade(1, 0).SetEase(Ease.Flash).OnComplete(() =>
            {
                scaleText.DOFade(0, .30f).SetDelay(.35f);
                scaleText.rectTransform.DOAnchorPosY(1f, 0.65f).SetEase(Ease.Linear);
            });

        }

        internal void PlayConfetti()
        {
            confetti.Play();
            
        }

        internal void OnReset()
        {
            renderer.gameObject.transform.DOScaleX(1, 1).SetEase(Ease.Linear);
        }
    }
}