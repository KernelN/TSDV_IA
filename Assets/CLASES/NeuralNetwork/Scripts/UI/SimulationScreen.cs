using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulationScreen : MonoBehaviour
{
    public Text bestTeamTxt;
    public Text matchesCountTxt;
    public Text goodGensCountTxt;
    public Text badGensCountTxt;
    public Text bestFitnessTxt;
    public Text avgFitnessTxt;
    public Text worstFitnessTxt;
    public Text timerTxt;
    public Slider timerSlider;
    public Button pauseBtn;
    public Button stopBtn;
    public GameObject startConfigurationScreen;

    string matchesCountText;
    string goodGensCountText;
    string badGensCountText;
    string bestFitnessText;
    string avgFitnessText;
    string worstFitnessText;
    string timerText;
    int lastGeneration = 0;

    // Start is called before the first frame update
    void Start()
    {
        timerSlider.onValueChanged.AddListener(OnTimerChange);
        timerText = timerTxt.text;

        timerTxt.text = string.Format(timerText, PopulationManager.Instance.IterationCount);

        SetAllTexts();

        pauseBtn.onClick.AddListener(OnPauseButtonClick);
        stopBtn.onClick.AddListener(OnStopButtonClick);
    }

    void OnEnable()
    {
        SetAllTexts(); 

        matchesCountTxt.text = string.Format(matchesCountText, 0);
        goodGensCountTxt.text = string.Format(goodGensCountText, 0);
        badGensCountTxt.text = string.Format(badGensCountText, 0);
        bestFitnessTxt.text = string.Format(bestFitnessText, 0);
        avgFitnessTxt.text = string.Format(avgFitnessText, 0);
        worstFitnessTxt.text = string.Format(worstFitnessText, 0);
    }

    void OnTimerChange(float value)
    {
        PopulationManager.Instance.IterationCount = (int)value;
        timerTxt.text = string.Format(timerText, PopulationManager.Instance.IterationCount);
    }

    void OnPauseButtonClick()
    {
        PopulationManager.Instance.PauseSimulation();
    }

    void OnStopButtonClick()
    {
        PopulationManager.Instance.StopSimulation();
        this.gameObject.SetActive(false);
        startConfigurationScreen.SetActive(true);
        lastGeneration = 0;
    }

    void LateUpdate()
    {
        PopulationManager popManager = PopulationManager.Instance;
        
        if (lastGeneration != popManager.loops)
        {
            lastGeneration = popManager.loops;
            matchesCountTxt.text = string.Format(matchesCountText, popManager.loops);
            goodGensCountTxt.text = string.Format(goodGensCountText, popManager.goodGens);
            badGensCountTxt.text = string.Format(badGensCountText, popManager.badGens);
            bestFitnessTxt.text = string.Format(bestFitnessText, popManager.bestFitness);
            avgFitnessTxt.text = string.Format(avgFitnessText, popManager.avgFitness);
            worstFitnessTxt.text = string.Format(worstFitnessText, popManager.worstFitness);

            bestTeamTxt.text = popManager.isGoodTeamWinning ? "Good Team" : "Bad Team";
        }
    }

    void SetAllTexts()
    {
        if (string.IsNullOrEmpty(matchesCountText))
            matchesCountText = matchesCountTxt.text;   
        if (string.IsNullOrEmpty(goodGensCountText))
            goodGensCountText = goodGensCountTxt.text;   
        if (string.IsNullOrEmpty(badGensCountText))
            badGensCountText = badGensCountTxt.text;   
        if (string.IsNullOrEmpty(bestFitnessText))
            bestFitnessText = bestFitnessTxt.text;   
        if (string.IsNullOrEmpty(avgFitnessText))
            avgFitnessText = avgFitnessTxt.text;   
        if (string.IsNullOrEmpty(worstFitnessText))
            worstFitnessText = worstFitnessTxt.text;  
    }
}
