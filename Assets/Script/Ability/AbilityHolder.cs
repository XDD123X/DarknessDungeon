using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    public Ability skill;

    public State state;

    public float cooldownTime;
    public float activeTime;

    public KeyCode key;

    public enum State
    {
        Ready,
        Active,
        OnCooldown
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void FixedUpdate()
    {
        switch (state)
        {
            case State.Active:
                {
                    if (activeTime > 0)
                    {
                        // Active the code each second
                        if (Mathf.Floor(activeTime) != Mathf.Floor(activeTime - Time.deltaTime))
                        {
                            skill.DuringSkill();
                        }
                        activeTime -= Time.deltaTime;
                    } else
                    {
                        cooldownTime = skill.CDTime;
                        skill.EndSkill();
                        state = State.OnCooldown;
                    }
                    break;
                }
            case State.Ready:
                {
                    if (Input.GetKey(key) && (state == State.Ready))
                    {
                        skill.Activate(this.gameObject);
                        state = State.Active;
                        activeTime = skill.activeTime;
                    }
                    break;
                }
            case State.OnCooldown:
                {
                    if (cooldownTime > 0)
                    {
                        cooldownTime -= Time.deltaTime;
                    } else
                    {
                        state = State.Ready;
                    }
                    break;
                }
        }
    }
    public void ActiveTheSkill()
    {
        if (state != AbilityHolder.State.Ready)
        {
            return;
        } else
        {
            skill.Activate(this.gameObject);
            state = State.Active;
            activeTime = skill.activeTime;
        }
    }
    public float radius;
    void OnDrawGizmosSelected()
    { 
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, radius);
    }

}
