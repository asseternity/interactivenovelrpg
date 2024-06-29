using UnityEngine;
using UnityEngine.UI;

public class EnergyUpdater : MonoBehaviour
{
    public so_playerstats player;

    void Update()
    {
        GameObject stressSlider = transform.Find("StressSlider").gameObject;
        GameObject exhaustionSlider = transform.Find("ExhaustionSlider").gameObject;

        Slider stressSliderComponent = stressSlider.GetComponent<Slider>();
        Slider exhaustionSliderComponent = exhaustionSlider.GetComponent<Slider>();

        stressSliderComponent.value = player.Stress;
        exhaustionSliderComponent.value = player.Exhaustion;
    }
}
