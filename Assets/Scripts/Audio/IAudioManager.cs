
public interface IAudioManager
{
    void PlaySound(TypeOfSound soundType);
    void StopSound(TypeOfSound soundType);
    
    void PlayMusic(TypeOfSound soundType);
    void StopMusic();
}
