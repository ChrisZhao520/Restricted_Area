using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActiveSystem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Highlighted;
    public GameObject highlightedAudio;

    private AudioSource HighlightedAudioSource;

    void Start()
    {
        HighlightedAudioSource = highlightedAudio.GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log(gameObject.GetComponent<Text>().color);
        /*GameObject.Find("Text").GetComponent<Text>().color = new Color(150, 150, 150);
        gameObject.GetComponent<Text>().color = new Color(150, 150, 150);*/

        HighlightedAudioSource.Play();

        Highlighted.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Highlighted.SetActive(false);
    }

}
