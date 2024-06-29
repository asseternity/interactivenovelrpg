using UnityEngine;
using UnityEngine.UI;

public class AbilityCheckAnimation : MonoBehaviour
{
    private float rotationSpeed = 10000f;
    private float dampingFactor = 0.95f;
    private float taxiSpeed = 1000f;
    private bool areWeDampingYet = false;
    private bool areWeFadingInYet = false;

    [Range(0f, 1f)]
    private float opacity = 0f;

    void Start()
    {
        Invoke("StartDamping", 1f);
        GameObject textObject = transform.Find("RollResult").gameObject;
        Text text = textObject.GetComponent<Text>();
        text.color = new Color(22f / 255f, 38f / 255f, 76f / 255f, 0f);
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
            areWeFadingInYet = true;
        }
        if (areWeFadingInYet)
        {
            opacity += Time.deltaTime * 3f; // Increase opacity over time
            opacity = Mathf.Clamp(opacity, 0f, 1f); // Ensure opacity stays within range
            // Set the color with normalized values (0 to 1 range)
            text.color = new Color(22f / 255f, 38f / 255f, 76f / 255f, opacity);
        }
    }

    void StartDamping()
    {
        areWeDampingYet = true;
    }
}
