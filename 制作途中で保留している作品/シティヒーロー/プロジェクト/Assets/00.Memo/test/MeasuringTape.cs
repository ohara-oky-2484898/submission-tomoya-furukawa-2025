using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;


public class MeasuringTape : MonoBehaviour
{
    public Transform tapeBody;
    public float currentLength = 1f;
    public float speed = 2f;

    public GameObject tickPrefab;
    public GameObject numberPrefab;
    public float tickSpacing = 0.1f;

    private List<GameObject> tickPool = new List<GameObject>();

    public void OnAdjustLength(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        currentLength = Mathf.Max(0f, currentLength + value * speed * Time.deltaTime);
        tapeBody.localScale = new Vector3(0.1f, currentLength, 0.1f);
        //UpdateTicks();
    }

    void UpdateTicks()
    {
        int tickCount = Mathf.FloorToInt(currentLength / tickSpacing);

        while (tickPool.Count <= tickCount)
        {
            GameObject tick = Instantiate(tickPrefab, tapeBody);
            tickPool.Add(tick);

            if (tickPool.Count % 5 == 0)
            {
                GameObject number = Instantiate(numberPrefab, tick.transform);
                number.transform.localPosition = new Vector3(0.1f, 0f, 0f);
            }
        }

        for (int i = 0; i < tickPool.Count; i++)
        {
            bool active = i <= tickCount;
            tickPool[i].SetActive(active);
            if (active)
            {
                float z = i * tickSpacing;
                tickPool[i].transform.localPosition = new Vector3(0, 0, z);

                TextMesh text = tickPool[i].GetComponentInChildren<TextMesh>();
                if (text != null)
                {
                    text.text = (z * 100).ToString("F0"); // cm
                }
            }
        }
    }
}
