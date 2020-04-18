using UnityEngine;
public class Timer
{
    float cooldown;
    float timeStarted;
    bool autoRestart;

    public Timer(float cooldownTime, bool autoRestart = true)
    {
        cooldown = cooldownTime;
        timeStarted = -cooldown - 1f;
        this.autoRestart = autoRestart;
    }

    public bool Ready()
    {
        return Time.time - timeStarted >= cooldown;
    }

    public void Start()
    {
        timeStarted = Time.time;
    }

    public void Reset()
    {
        timeStarted = -cooldown - 1f;
    }

    public void SetCooldown(float cooldownTime)
    {
        cooldown = cooldownTime;
    }

    public bool Check()
    {
        if (Ready())
        {
            if (autoRestart) Start();
            return true;
        }
        return false;
    }
}