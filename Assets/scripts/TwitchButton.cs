using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Text;
using System;

public class TwitchButton : MonoBehaviour {
	
  void Start(){
    //isLive("ludwig");
    string html = getHTML("ludwig");
    saveHTML(html);
    Debug.Log(Application.persistentDataPath);
  }

  private bool isLive(string channel){
    string html = getHTML(channel);
    //string html = "<div class=\"tw-inline-block\"> <div class=\"channel-status-info channel-status-info--hosting tw-border-radius-medium tw-inline-block\">    <p class=\"tw-strong tw-upcase\">      Hosting</p></div></div>";
    if(html.IndexOf("channel-status-info",0,html.Length) == -1){
      Debug.Log("channel is live!");
      return true;
    }
    Debug.Log("channel is offline");
    return false;
  }

  private string getHTML(string channel){
    string urlAddress = "https://www.twitch.tv/" + channel;

    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
  
    if(response.StatusCode == HttpStatusCode.OK){
      Stream receiveStream = response.GetResponseStream();
      StreamReader readStream = null;

      if(String.IsNullOrWhiteSpace(response.CharacterSet)){
        readStream = new StreamReader(receiveStream);
      }else {
        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
      }

      string data = readStream.ReadToEnd();

      response.Close();
      readStream.Close();

      return data;
    }

    return "";
  }

  public void saveHTML(string html){
    StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/html.txt", false);
    writer.Write(html);
    writer.Close();
	}


}