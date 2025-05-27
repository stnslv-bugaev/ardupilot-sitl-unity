using UnityEngine;

public class PropellerController : MonoBehaviour
{
    [Header("Параметры вращения")]
    public float minPWM = 1070f;    // Минимум, с которого мотор начинает крутиться
    public float maxPWM = 2000f;

    public float minRPM = 1000f;    // RPM при минимальном вращении
    public float maxRPM = 6000f;    // RPM при полном газе

    [Header("Настройки")]
    public float currentPWM = 1000f;
    public bool clockwise = true;

    private float currentAngle = 0f;

    void Update()
    {
        float rpm = 0f;

        if (currentPWM >= minPWM)
        {
            float t = (currentPWM - minPWM) / (maxPWM - minPWM);
            rpm = Mathf.Lerp(minRPM, maxRPM, t);
        }

        float degreesPerSecond = rpm * 6f;
        float delta = degreesPerSecond * Time.deltaTime;
        currentAngle += clockwise ? -delta : delta;

        transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
    }

    public void SetPWM(float pwm)
    {
        currentPWM = Mathf.Clamp(pwm, 1000f, 2000f);
    }
}