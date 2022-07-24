using UnityEngine;
using UnityEngine.UI;

//This scripts is for all texts that appears for a short time. Used for score pop-ups and bonus alerts.
public class TextAnimator : MonoBehaviour
{
    [SerializeField] private Transform pivot;
    [SerializeField] private bool rotate;
    [SerializeField] private float speed;
    [SerializeField] private float fadeSpeed;
    [SerializeField] private Color color;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private float timer;
    [SerializeField] private float fadeDelay = 0.4f;

    private void Awake()
    {
        startPos = transform.position;
    }


    private void OnEnable()
    {
        color = GetComponent<Text>().color;
        if (pivot != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(pivot.position);
        }
        else
        {
            transform.position = startPos;
        }

        if (rotate)
            transform.eulerAngles = new Vector3(0, 0, -179);
    }

    private void OnDisable()
    {
        color.a = 1;
        GetComponent<Text>().color = color;
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > fadeDelay)
        {
            if (color.a > 0.01f)
            {
                color.a -= 0.01f * fadeSpeed;
                GetComponent<Text>().color = color;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        transform.position += new Vector3(0, Time.deltaTime * 100 * speed, 0);
        if (rotate)
            this.transform.rotation =
                Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 10);
    }
}