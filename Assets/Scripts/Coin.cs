using UnityEngine;

public class Coin : MonoBehaviour
{
    public enum CoinType
    {
        Normal,
        Red,
        Blue
    }

    [SerializeField] CoinType coinType = CoinType.Normal;
    [SerializeField] int value = 1;
    [SerializeField] float spinSpeed = 90f;

    void Update() => transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.World);

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Coin touched by {other.name}");
        if (other.CompareTag("Player"))
        {
            if (coinType == CoinType.Normal)
            {
                GameManager.Instance.AddScore(value);
            }
            else if ( coinType == CoinType.Red)
            {
                GameManager.Instance.CollectRedCoin(other.transform);
            }
            else if ( coinType == CoinType.Blue)
            {
                GameManager.Instance.AddBonusCoin();
            }

            Destroy(gameObject);
        }
    }
}
