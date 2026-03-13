using UnityEngine;
using System;

namespace MyGame.Presentation
{
    public class WoodView : MonoBehaviour
    {
        [SerializeField] private GameObject fireEffectPrefab;
        public event Action OnFireTouched; // Logic層へ知らせるための通知

        public void PlayFireEffect()
        {
            if (fireEffectPrefab != null)
            {
                var effect = Instantiate(fireEffectPrefab, transform.position, Quaternion.identity);
                effect.transform.SetParent(this.transform);
            }
        }

        public void Remove() => Destroy(gameObject);

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("FireSource"))
            {
                OnFireTouched?.Invoke(); // 「火が触れた！」と叫ぶだけ
            }
        }
    }
}