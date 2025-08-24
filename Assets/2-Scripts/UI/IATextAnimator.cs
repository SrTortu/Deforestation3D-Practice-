using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IATextAnimator : MonoBehaviour
{
    [SerializeField] private float _timeAppear;
    [SerializeField] private float _timeFadeOut;

    public IEnumerator ControlTextAppear(TextMeshProUGUI screenText, TextMeshProUGUI titleText,
        GameText gameText, GameObject panel)
    {
        screenText.text = "";
        panel.SetActive(true);
        panel.GetComponent<Image>().DOFade(0.4f, 0);
        screenText.DOFade(1, 0);
        titleText.DOFade(1, 0);
        foreach (char character in gameText.Text)
        {
            screenText.text += character;
            yield return new WaitForSeconds(_timeAppear);
        }

        yield return new WaitForSeconds(3f);
        StartCoroutine(ControlPanelDissapear(panel));
        ControlTextDissappear(titleText);
        ControlTextDissappear(screenText);
    }

    private IEnumerator ControlPanelDissapear(GameObject panel)
    {
        Image panelImage = panel.GetComponent<Image>();
        panelImage.DOFade(0, _timeFadeOut);
        yield return new WaitForSeconds(_timeFadeOut);
        panel.SetActive(false);
    }

    private void ControlTextDissappear(TextMeshProUGUI text)
    {
        text.DOFade(0, _timeFadeOut);
    }
}