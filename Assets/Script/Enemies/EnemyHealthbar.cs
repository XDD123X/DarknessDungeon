using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthbar : MonoBehaviour
{
    public Transform target;
    public Vector2 offset;

    public Image healthbar_hp;

    public Image healthbar_bg;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Reset()
    {
        SetHealthBar(100f, 100f);
        Hide();
    }

    public void Display()
    {
        healthbar_hp.enabled = true;
        healthbar_bg.enabled = true;
    }

    public void Hide()
    {
        healthbar_hp.enabled = false;
        healthbar_bg.enabled = false;
    }

    public void SetHealthBar(float currHP, float maxHP)
    {
        float percent = currHP / maxHP;
        healthbar_hp.fillAmount = percent;
    }

    void Update()
    {
        if (target != null)
        {
            this.transform.position = new Vector3(target.position.x + offset.x, target.position.y + offset.y, target.position.z);
        }
    }
}
