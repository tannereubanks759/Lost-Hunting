using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FPSDisplay : MonoBehaviour
{
    public int avgFrameRate;
    public TextMeshProUGUI display_Text;

    public void Start()
    {
        
    }
    public void Update()
    {
        float current = 0;
        current = Time.frameCount / Time.time;
        avgFrameRate = (int)current;
        display_Text.text = avgFrameRate.ToString() + " FPS";
    }
}