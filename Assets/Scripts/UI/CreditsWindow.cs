using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsWindow : MonoBehaviour
{
    public void OnOtherGames()
    {
        Application.OpenURL("https://play.google.com/store/apps/dev?id=9062759065419948123");
    }

    public void OnPrivacyPolicy()
    {
        Application.OpenURL("https://sites.google.com/view/politykalavadunegon/strona-g%C5%82%C3%B3wna");
    }

    public void OnZapSplat()
    {
        Application.OpenURL("https://www.zapsplat.com/");
    }

    public void On0x72()
    {
        Application.OpenURL("https://0x72.itch.io/");
    }

    public void OnSerpentSoundStudios()
    {
        Application.OpenURL("https://www.serpentsoundstudios.com/");
    }
}
