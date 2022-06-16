using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private Camera playerFollowCamera;
    [SerializeField] private Camera freeViewCamera;

    private Dictionary<View, Camera> viewCam = new();

    public enum View
    {
        PLAYER_FOLLOWING,
        FULL_VIEW
    }

    private View currentView = View.PLAYER_FOLLOWING;

    private void Start()
    {
        viewCam.Add(View.PLAYER_FOLLOWING, playerFollowCamera);
        viewCam.Add(View.FULL_VIEW, freeViewCamera);
    }

    private void Update()
    {
        if (!GameManager.Instance.PlayerGaming) return;
        if (GameManager.Instance.IsGamePaused()) return;

        if (Input.GetKeyUp(KeyCode.L))
        {
            SwitchCamera(currentView.Next());
        }
    }

    public void SwitchCamera(View view)
    {
        Debug.Log(string.Format("Switch camera: {0}", view.ToString()));
        EnableCamera(view);
    }

    private void EnableCamera(View view)
    {
        Camera cam;
        if (!viewCam.TryGetValue(view, out cam))
        {
            Debug.LogWarning(string.Format("no camera exist for view: {0}", view.ToString()));
            return;
        }

        Vector3 prevPos = viewCam[currentView].gameObject.transform.position;
        viewCam[currentView].gameObject.SetActive(false);
        currentView = view;

        cam.gameObject.SetActive(true);
        cam.gameObject.transform.position = prevPos;
    }
}
