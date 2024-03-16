using UnityEngine;
using UnityEngine.UI;

public class Hands : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField] private float smooth;
    [SerializeField] private float swayAmount; // This replaces multiplier

    public Transform anchor;
    public Image leftHand;
    public Image rightHand;

    private float xMousePos;
    private float yMousePos;

    void Start()
    {

    }

    void Update()
    {
        xMousePos = Input.GetAxisRaw("Mouse X");
        yMousePos = Input.GetAxisRaw("Mouse Y");


        // Calculate offset based on input
        float offsetX = xMousePos * swayAmount;
        float offsetY = yMousePos * swayAmount;

        // Apply offset with smoothing
        Vector3 targetPosition = new Vector3(offsetX, offsetY, 0f);
        anchor.localPosition = Vector3.Lerp(anchor.localPosition, targetPosition, smooth * Time.deltaTime);

    }
}
