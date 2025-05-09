using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideLine : MonoBehaviour
{
    [SerializeField] private GameObject guide;
    [SerializeField] TMPro.TextMeshProUGUI text;

    private void Start()
    {
        guide.SetActive(true);
        text.text = "Close Guideline";
    }

    public void ToggleGuide()
    {
        if (guide.activeSelf)
        {
            guide.SetActive(false);
            text.text = "Open Guideline";
        }
        else
        {
            guide.SetActive(true);
            text.text = "Close Guideline";
        }

    }

}
