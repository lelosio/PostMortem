using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI ammo;
    public Image[] slot;
    public Sprite[] slotSprite;
    public Transform inventory;

    public void Start()
    {
        ammo.richText = true;
    }

    public void Update()
    {
    }

    public void Button()
    {
        SceneManager.LoadScene("gra");
    }
}
