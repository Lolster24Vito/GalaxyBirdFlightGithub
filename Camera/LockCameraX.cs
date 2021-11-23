using UnityEngine;
using Cinemachine;

/// <summary>
/// An add-on module for Cinemachine Virtual Camera that locks the camera's Z co-ordinate
/// </summary>
[ExecuteInEditMode]
[SaveDuringPlay]
[AddComponentMenu("")] // Hide in menu
public class LockCameraX : CinemachineExtension
{
    [Tooltip("Lock the camera's X position to this value")]
    public float m_XPosition = 10;
    public float yLimiterPos = 70f;


    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (enabled && stage == CinemachineCore.Stage.Body)
        {


            var pos = state.RawPosition;
            pos.x= m_XPosition;
            //Limit the playerScreen
            if (PlayerScoreManager.score > 0.0f)
            {
                if (pos.y < (PlayerScoreManager.score * 96f - yLimiterPos))
                {
                    pos.y = (PlayerScoreManager.score * 96f - yLimiterPos);
                }
            }
            
            state.RawPosition = pos;
        }
    }
    private void Update()
    {

    }
}