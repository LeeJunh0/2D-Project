using System.Collections;
using UnityEngine;

public class Preview : MonoBehaviour
{
    [SerializeField] private bool isCheck;
    [SerializeField] private Color OnHighlightColor;
    [SerializeField] private Color OffHighlightColor;
    [SerializeField] private GameObject tipText;

    private void OnEnable()
    {
        tipText.SetActive(true);
        StartCoroutine(BuilingPreview());
    }

    public bool IsCheck 
    {
        get => isCheck;
        set
        {
            isCheck = value;
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = isCheck == true ? OnHighlightColor : OffHighlightColor;
        } 
    }

    public void SetPos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.transform.position.z * -1;
        Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = new Vector3(screenPos.x, transform.position.y, 0);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Building")
        {
            IsCheck = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Building")
            IsCheck = true;
    }

    private IEnumerator BuilingPreview()
    {
        while(true)
        {
            SetPos();
            yield return null;

            if(Input.GetMouseButtonDown(0))
            {
                
            }

            if(Input.GetMouseButtonDown(1))
            {
                gameObject.SetActive(false);
                yield break;
            }
        }
    }

    private void OnDisable()
    {
        tipText.SetActive(false);
    }
}
