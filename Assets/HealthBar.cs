using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public float speed = 50;
    public Health target;
    Image i;
    void Start() {
        i = GetComponent<Image>();
    }
    void FixedUpdate()
    {
        float c = i.fillAmount;
        float t = 0;
        if (target) t = target.health / target.maxHealth;
        if (float.IsInfinity(c) || float.IsNaN(c)) c = 0;
        i.fillAmount = Mathf.Lerp(c, t, speed * Time.deltaTime);
    }
}
