using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check for touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                HandleTouch(touch.position);
            }
        }
    }

    void HandleTouch(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform != null)
            {
                // Here you can check the tag or name of the hit object to determine what action to take
                Debug.Log("Touched: " + hit.transform.name);

                if (hit.transform.CompareTag("Email"))
                {
                    OpenEmailClient();
                }
                else if (hit.transform.CompareTag("Phone"))
                {
                    OpenPhoneDialer();
                }
                else if (hit.transform.CompareTag("LinkedIn"))
                {
                    Application.OpenURL(GlobalMetadata.LinkedIn);
                }
                else if (hit.transform.CompareTag("Website"))
                {
                    Application.OpenURL(GlobalMetadata.Website);
                }
            }
        }
    }


    private void OpenEmailClient()
    {
        string mailtoUrl = $"mailto:{GlobalMetadata.Email}";
        Application.OpenURL(mailtoUrl);
    }

    private void OpenPhoneDialer()
    {
        string telUrl = $"tel:{GlobalMetadata.Number}";
        Application.OpenURL(telUrl);
    }
}
