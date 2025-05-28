using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    private CinemachineCamera cineCam;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        SetPlayerCameraFollow();
    }

    public void SetPlayerCameraFollow()
    {
        StartCoroutine(WaitAndAssignFollow());
    }

    private IEnumerator WaitAndAssignFollow()
    {
        Debug.Log(" Waiting for PlayerController.Instance...");

        while (PlayerController.Instance == null || !PlayerController.Instance.gameObject.activeInHierarchy)
            yield return null;

        Debug.Log(" PlayerController.Instance found: " + PlayerController.Instance.name);

        yield return null; // allow frame to settle

        cineCam = FindFirstObjectByType<CinemachineCamera>();

        if (cineCam != null)
        {
            Debug.Log(" Found CinemachineCamera: " + cineCam.name);
            cineCam.Follow = PlayerController.Instance.transform;
            Debug.Log(" Camera now following player.");
        }
        else
        {
            Debug.LogWarning(" CinemachineCamera not found.");
        }
    }

}
