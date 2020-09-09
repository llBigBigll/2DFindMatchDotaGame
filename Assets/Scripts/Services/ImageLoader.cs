using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ImageLoader
{ 
    public static List<Texture2D> getTextures(TextAsset textFile) 
    {
        List<Texture2D> answer = new List<Texture2D>();
        var fileNames = textFile.text.Split(new char[]{' ', '\n' });
        foreach (var fileName in fileNames)
        {
            var trimmed = fileName.Trim();
            answer.Add(Resources.Load<Texture2D>($"HeroesPics/{trimmed}"));
        }
        return answer;
    }
}
