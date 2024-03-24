using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Hands : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField] private float smooth;
    [SerializeField] private float swayAmount;

    public Transform anchor;

    private float xMousePos;
    private float yMousePos;

    public Animator gun;

    void Start()
    {
    }

    void Update()
    {
        xMousePos = Input.GetAxisRaw("Mouse X");
        yMousePos = Input.GetAxisRaw("Mouse Y");
        float offsetX = xMousePos * swayAmount;
        float offsetY = yMousePos * swayAmount;
        Vector3 targetPosition = new Vector3(-offsetX, -offsetY, 0f);

        if (Input.GetMouseButtonDown(0))
        {
            gun.Play("gunShoot");
            targetPosition += new Vector3(1000f,1000f, 10f);
        }

        anchor.localPosition = Vector3.Lerp(anchor.localPosition, targetPosition, smooth * Time.deltaTime);
    }
}
