using UnityEngine;

public class Email : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        OpenURL();
    }

    private void OpenURL()
    {
        string mailtoUrl = $"mailto:{GlobalMetadata.Email}";
        Application.OpenURL(mailtoUrl);
    }
}
