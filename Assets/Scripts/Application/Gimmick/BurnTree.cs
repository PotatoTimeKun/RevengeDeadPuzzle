using UnityEngine;

public class BurnTree : MonoBehaviour
{
    // 木に「火（FireSource）」が触れた時に実行される
    private void OnTriggerEnter(Collider other)
    {
        // 触れてきた相手のタグが "FireSource" かどうかをチェック
        if (other.CompareTag("FireSource"))
        {
            StartBurning();
        }
    }

    void StartBurning()
    {
        Debug.Log("燃え");
        
        // 【演出】もし炎のエフェクトがあればここで再生（後述）
        
        // 2秒後に木を完全に消去する
        Destroy(gameObject, 2.0f);
    }
}