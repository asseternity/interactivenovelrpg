using UnityEngine;

public class CanvasSwitcher : MonoBehaviour
{
    public Canvas canvas1;
    public Canvas canvas2;
    public Canvas canvas3;
    public Canvas canvas4;

    public void Start()
    {
        canvas1.gameObject.SetActive(true);
        canvas2.gameObject.SetActive(false);
        canvas4.gameObject.SetActive(false);
        canvas4.gameObject.SetActive(false);
    }

    public void SetCanvas1()
    {
        canvas1.gameObject.SetActive(true);
        canvas2.gameObject.SetActive(false);
        canvas4.gameObject.SetActive(false);
        canvas4.gameObject.SetActive(false);
    }

    public void SetCanvas2()
    {
        canvas1.gameObject.SetActive(false);
        canvas2.gameObject.SetActive(true);
        canvas4.gameObject.SetActive(false);
        canvas4.gameObject.SetActive(false);
    }

    public void SetCanvas3()
    {
        canvas1.gameObject.SetActive(false);
        canvas2.gameObject.SetActive(false);
        canvas4.gameObject.SetActive(true);
        canvas4.gameObject.SetActive(false);
    }

    public void SetCanvas4()
    {
        canvas1.gameObject.SetActive(false);
        canvas2.gameObject.SetActive(false);
        canvas4.gameObject.SetActive(false);
        canvas4.gameObject.SetActive(true);
    }
}
