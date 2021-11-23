using UnityEngine;
using Cinemachine;

/// <summary>
/// An add-on module for Cinemachine Virtual Camera that locks the camera's Z co-ordinate
/// </summary>
[ExecuteInEditMode]
[SaveDuringPlay]
[AddComponentMenu("")] // Hide in menu
public class CameraYMinMax : CinemachineExtension
{
    [Tooltip("Lock the camera's Y position to this value")]
    public float min_YPosition = 10;
    public float max_YPosition = 10;



    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (enabled && stage == CinemachineCore.Stage.Body)
        {
            var pos = state.RawPosition;
            if (pos.y <min_YPosition)
            {

                pos.y = min_YPosition;
            }
            if (pos.y > max_YPosition)
            {
                pos.y = max_YPosition;
            }
            state.RawPosition = pos;
        }
    }
}