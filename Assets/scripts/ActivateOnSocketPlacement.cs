using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

public class ActivateOnSocketPlacement : MonoBehaviour
{
    [Header("Visual Effects")]
    public GameObject lightRay;
    public GameObject rainbow;

    [Header("UI Panel")]
    public GameObject infoPanel; // Panel with the info content
    public Button closeButton;   // Optional: assign a button to hide the panel

    private XRSocketInteractor socket;
    private bool hasActivated = false;

    void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();

        if (socket == null)
        {
            Debug.LogError("XRSocketInteractor component missing!");
        }

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(HideInfoPanel);
        }
    }

    void Update()
    {
        if (!hasActivated && socket.hasSelection)
        {
            ActivateAll();
            hasActivated = true;
        }

        // Optional: deactivate everything when object removed
        if (hasActivated && !socket.hasSelection)
        {
            hasActivated = false;
            // Uncomment if you want to hide when object is removed
            // lightRay.SetActive(false);
            // rainbow.SetActive(false);
            // infoPanel.SetActive(false);
        }
    }

    private void ActivateAll()
    {
        if (lightRay != null)
            lightRay.SetActive(true);

        if (rainbow != null)
            rainbow.SetActive(true);

        if (infoPanel != null)
            infoPanel.SetActive(true);

        Debug.Log("Object placed in socket. Activated visuals and info panel.");
    }

    public void HideInfoPanel()
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(false);
            Debug.Log("Info panel hidden.");
        }
    }
}