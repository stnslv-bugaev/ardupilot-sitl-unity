using System;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    [SerializeField] private PropellerController[] propellers;
    [SerializeField] private DroneSoundManager soundManager;

    private Vector3 velocity = Vector3.zero;
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private float rotationSmoothFactor = 2f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        // Плавное движение с инерцией
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        // Плавный поворот
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothFactor * Time.fixedDeltaTime);
    }

    public void SetPosition(Vector3 newPosition)
    {
        targetPosition = newPosition;
    }

    public void SetRotation(Quaternion rotation)
    {
        targetRotation = rotation;
    }
    
    public void SetPropellersPWM(float[] pwms)
    {
        if (pwms.Length != propellers.Length)
        {
            Debug.LogError("Количество скоростей не соответствует количеству пропеллеров.");
            return;
        }

        for (int i = 0; i < propellers.Length; i++)
        {
            propellers[i].SetPWM(pwms[i]);
        }
        soundManager?.SetPWM(pwms);
    }
}