using UnityEngine;
using UnityEngine.UI;

public class AbilityCheckAnimation : MonoBehaviour
{
    private float rotationSpeed = 10000f;
    private float dampingFactor = 0.95f;
    private float taxiSpeed = 1000f;
    private bool areWeDampingYet = false;

    void Start()
    {
        Invoke("StartDamping", 1f);
    }

    void Update()
    {
        GameObject imageObject = transform.Find("DiceSprite").gameObject;
        GameObject textObject = transform.Find("RollResult").gameObject;

        Image image = imageObject.GetComponent<Image>();
        Text text = textObject.GetComponent<Text>();

        image.rectTransform.Rotate(Vector3.forward * Time.deltaTime * rotationSpeed);

        if (areWeDampingYet && rotationSpeed > taxiSpeed)
        {
            rotationSpeed *= dampingFactor;
        }

        if (
            rotationSpeed < taxiSpeed
            && Quaternion.Angle(image.rectTransform.rotation, Quaternion.identity) < 1f
        )
        {
            rotationSpeed = 0f;
            image.rectTransform.rotation = Quaternion.identity;
        }
    }

    void StartDamping()
    {
        areWeDampingYet = true;
    }
}
