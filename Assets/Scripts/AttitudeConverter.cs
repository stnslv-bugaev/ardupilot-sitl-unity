using UnityEngine;

public static class AttitudeConverter
{
    public static Quaternion ConvertMavlinkAttitudeToUnity(float roll, float pitch, float yaw)
    {
        float pitchDeg = pitch * Mathf.Rad2Deg;
        float rollDeg = roll * Mathf.Rad2Deg;
        float yawDeg = yaw * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler((float)-pitchDeg, (float)yawDeg, (float)-rollDeg);
        return rot;
    }
}