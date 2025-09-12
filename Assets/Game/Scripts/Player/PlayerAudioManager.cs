using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _footstepSfx;
    [SerializeField] private AudioSource _punchSfx;
    [SerializeField] private AudioSource _landingSfx;

    [SerializeField] private AudioSource _glideSfx;


    private void PlayFootstepSfx()
    {
        _footstepSfx.volume = Random.Range(0.7f, 1f);
        _footstepSfx.pitch = Random.Range(0.5f, 2.5f);
        _footstepSfx.Play();
    }

    private void PlayCrouchwalkSfx()
    {
        _footstepSfx.volume = Random.Range(0.25f, 0.5f);
        _footstepSfx.pitch = Random.Range(0.5f, 0.75f);
        _footstepSfx.Play();
    }

    private void PlayPunchSfx()
    {
        _punchSfx.volume = 1f;
        _punchSfx.pitch = 0.5f;
        _punchSfx.Play();
    }

    private void PlayHookSfx()
    {
        _punchSfx.volume = Random.Range(0.5f, 1.5f);
        _punchSfx.pitch = 1f;
        _punchSfx.Play();
    }

    private void PlayUppercutSfx()
    {
        _punchSfx.volume = Random.Range(1.5f, 2f);
        _punchSfx.pitch = 2f;
        _punchSfx.Play();
    }

    private void PlayLandingSfx()
    {
        _landingSfx.volume = Random.Range(0.7f, 1f);
        _landingSfx.pitch = Random.Range(0.5f, 2.5f);
        _landingSfx.Play();
    }
    public void PlayGlideSfx()
    {
        _glideSfx.Play();
    }
    public void StopGlideSfx()
    {
        _glideSfx.Stop();
    }
}