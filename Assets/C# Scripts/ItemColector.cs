using UnityEngine;
using UnityEngine.UI;

public class ItemColector : MonoBehaviour
{
    #region Private Variables

    private int _diamonds = 0;
    [SerializeField] private Text diamondsText;
    [SerializeField] private AudioSource collectionSoundEffect;

    #endregion
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("colectable"))
        {
            collectionSoundEffect.Play();
            Destroy(collision.gameObject);
            _diamonds++;
            diamondsText.text = "diamante : " + _diamonds;
        }
    }
}