using System;
using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RoundTransitionUI : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private TMP_Text roundText;
    [SerializeField]
    private TMP_Text titleText;
    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private TMP_Text typesText;
    [SerializeField]
    private TMP_Text speedText;
    [SerializeField]
    private Button continueButtonl;
    [SerializeField]
    private Button stopButton;
    [SerializeField]
    private Button conButton;
    private bool _responded;
    public bool ContinueChosen { get; private set; }

    private void Awake()
    {
        panel.SetActive(false);

        continueButtonl.onClick.AddListener(() =>
        {
            ContinueChosen = true;
            _responded = true;
            panel.SetActive(false);
        });

        stopButton.onClick.AddListener(() =>
        {
            ContinueChosen = false;
            _responded = true;
            panel.SetActive(false);
        });
    }

    public IEnumerator Show(int roundIndex, int totalScore, string nextTypes, float maxSpeedKmh, bool isGameOver, bool isGameClear = false)
    {
        _responded = false;
        ContinueChosen = false;
        if (roundIndex == 0 && !isGameOver)
            titleText.text = "START!";
        else
        {
            if (isGameOver)
            {
                titleText.text = "GAME OVER";
                conButton.gameObject.SetActive(false);
            }
            else
            {
                titleText.text = "CLEAR!!";
                if (isGameClear)
                    conButton.gameObject.SetActive(false);

            }
        }


        roundText.text = $"ROUND {roundIndex + 1}";
        scoreText.text = $"Score: {totalScore:N0}";
        typesText.text = nextTypes;
        speedText.text = $"Max Speed: {maxSpeedKmh:F0} km/h";

        panel.SetActive(true);

        yield return new WaitUntil(() => _responded);
    }

}
