using UnityEngine;
using UnityEngine.UI;

public class ButtonColor : MonoBehaviour
{
    public Color hoverColor;
    public Color activeColor;
    private Color originalColor;
    private Text buttonText;
    private Image buttonImage;
    public AudioSource click;

    void Start()
    {
        buttonText = GetComponentInChildren<Text>();
        buttonImage = GetComponent<Image>();
        originalColor = buttonText.color;
    }

    public void OnHoverEnter()
    {
        buttonText.color = hoverColor;
        if (buttonImage != null)
        {
            buttonImage.color = hoverColor;
        }
    }

    public void OnHoverExit()
    {
        buttonText.color = originalColor;
        if (buttonImage != null)
        {
            buttonImage.color = originalColor;
        }
    }

    public void OnActivate()
    {
        buttonText.color = activeColor;
        if (buttonImage != null)
        {
            buttonImage.color = activeColor;
        }
        click.Play();
    }
}
