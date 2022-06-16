using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FreeViewCamera : MonoBehaviour
{
    [SerializeField] private float speed = 15;
    public BoundsInt CameraBound { set; get; }

    private void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        float verticalInput = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;

        if (CameraBound != null)
        {
            Vector3 originalPos = gameObject.transform.position;
            float newX = Mathf.Clamp(originalPos.x + horizontalInput, CameraBound.xMin, CameraBound.xMax);
            float newY = Mathf.Clamp(originalPos.y + verticalInput, CameraBound.yMin, CameraBound.yMax);

            //Debug.Log("xMax: " + CameraBound.xMax);
            //Debug.Log("xMin: " + CameraBound.xMin);
            //Debug.Log("yMax: " + CameraBound.yMax);
            //Debug.Log("yMin: " + CameraBound.yMin);

            gameObject.transform.position = new Vector3(newX, newY, originalPos.z);
        }
        else
        {
            gameObject.transform.Translate(new Vector3(horizontalInput, verticalInput, 0));
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.Player.GetComponent<PlayerInteraction>().DisablePlayer();

        LevelInfo lvobj = DataBuffer.Instance.Get<LevelInfo>();
        Tilemap tilemap = lvobj.background.GetComponent<Tilemap>();
        tilemap.CompressBounds();
        CameraBound = tilemap.cellBounds;
    }

    private void OnDisable()
    {
        GameManager.Instance.Player.GetComponent<PlayerInteraction>().EnablePlayer();
    }

}
