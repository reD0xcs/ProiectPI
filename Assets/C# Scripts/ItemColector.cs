using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemColector : MonoBehaviour
{
    private int diamonds = 0;

    [SerializeField] private Text diamondsText;

    [SerializeField] private AudioSource collectionSoundEffect;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("colectable"))
        {
            collectionSoundEffect.Play();
            Destroy(collision.gameObject);
            diamonds++;
            diamondsText.text = "diamante : " + diamonds;
        }
    }
}
