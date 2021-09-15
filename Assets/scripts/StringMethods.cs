using System;
using System.Text;

public static class StringMethods{

  //make debug class for this
  /*
  public static void readScriptFile(String sceneName){
    Debug.Log("Start file read:");
    string curLine;
    System.IO.StreamReader file;
    file = new System.IO.StreamReader(Application.persistentDataPath + "/" + sceneName + "Script.txt");
    while ((curLine = file.ReadLine()) != null){
      Debug.Log("curLine: " + curLine);
    }
    file.Close();
  }
  */
  
  // Used to find function name from code line
  public static string funcName(string line){
    StringBuilder sb = new StringBuilder();
    for(int i = 0; i < line.Length; i++){
      if(line[i] != '(' && line[i] != ')' && line[i] != '{'
          && line[i] != '}' && line[i] != ';' && line[i] != ' '){
        sb.Append(line[i]);
      }
    }
    return sb.ToString();
  }

  // returns a code line without any spaces
  public static string killWhiteSpace(string str){
    StringBuilder sb = new StringBuilder();
    for(int i = 0; i < str.Length; i++){
      if(str[i] != ' '){
        sb.Append(str[i]);
      }
    }
    return sb.ToString();
  }

  // returns a substring
  public static string cutString(string str, int start, int length){
    if(str.Length < start + length){
      length = str.Length;
    }
    StringBuilder sb = new StringBuilder();
    for(int i = start; i < start + length; i++){
      sb.Append(str[i]);
    }
    return sb.ToString();
  }

  // returns the position of the first occurance of the char
  public static int findFirstOccurance(string str, char charToFind){
    for(int i = 0; i < str.Length; i++){
      if(str[i] == charToFind){
        return i;
      }
    }
    return -1;
  }

  public static int findFirstOccuranceStr(String str, String strToFind){
    int subStrLen = strToFind.Length;
    for(int i = 0; i < str.Length - strToFind.Length; i++){
      if(str.Substring(i, subStrLen) == strToFind){
        return i;
      }
    }
    return -1;
  }
}