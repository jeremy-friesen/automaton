using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkButton : MonoBehaviour{
    public void openTwitter(){
        Application.OpenURL("https://twitter.com/automatonGame");
    }

    public void openDiscord(){
        Application.OpenURL("https://discord.gg/2tJsBQJ");
    }

    public void openReddit(){
        Application.OpenURL("https://www.reddit.com/r/automaton");
    }

    public void openTwitch(){
        Application.OpenURL("https://www.twitch.tv/jeremy_friesen");
    }

    public void giveFeedback(){
        Application.OpenURL("mailto:jeremyfriesendeveloper@gmail.com");
    }

    public void openCustom(string link){
        Application.OpenURL(link);
    }
}
