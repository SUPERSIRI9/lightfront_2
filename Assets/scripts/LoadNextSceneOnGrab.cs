using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using System.Collections;

public class LoadNextSceneOnGrab : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;

    [Header("Scene Settings")]
    public bool makeScenePublic = true;
    public float fadeDuration = 1f;
    public float delayAfterFade = 0.3f;

    [Header("Sound")]
    public AudioClip grabSound;
    private AudioSource audioSource;

    [Header("Fade UI")]
    public CanvasGroup fadeCanvasGroup; // Assign your FadePanel's CanvasGroup here

    private bool isFading = false;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        audioSource = GetComponent<AudioSource>();

        if (grabInteractable == null)
        {
            Debug.LogError("XRGrabInteractable not found!");
            return;
        }

        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (!isFading)
        {
            PlayGrabSound();
            StartCoroutine(FadeAndLoad());
        }
    }

    private void PlayGrabSound()
    {
        if (grabSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(grabSound);
        }
    }

    private IEnumerator FadeAndLoad()
    {
        isFading = true;

        if (fadeCanvasGroup != null)
        {
            // Fade to black
            float timer = 0f;
            while (timer < fadeDuration)
            {
                fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
                timer += Time.deltaTime;
                yield return null;
            }
            fadeCanvasGroup.alpha = 1f;
        }

        yield return new WaitForSeconds(delayAfterFade);

        LoadNextScene();
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        int nextSceneIndex = (currentSceneIndex + 1) % totalScenes;

        if (makeScenePublic)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    private void OnDestroy()
    {
        if (grabInteractable != null)
            grabInteractable.selectEntered.RemoveListener(OnGrab);
    }
}