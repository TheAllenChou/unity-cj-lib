/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

using UnityEngine;
using UnityEngine.Networking;

using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CjLib
{
  [ExecuteInEditMode]
  public class LatexFormula : MonoBehaviour
  {
    public static readonly string BaseUrl = "http://tex.s2cms.ru/svg/f(x) ";

    private int m_hash = BaseUrl.GetHashCode();

    [SerializeField]
    private string m_formula = "";

    private Texture m_texture;

  #if UNITY_EDITOR
    private double m_nextRefreshTime;
    private bool m_refreshing;

    public void OnEnable()
    {
      m_nextRefreshTime = Time.time;
      m_refreshing = false;
      EditorApplication.update += OnEditorUpdate;
    }

    public void OnDisable()
    {
      EditorApplication.update -= OnEditorUpdate;
    }

    private void OnEditorUpdate()
    {
      if (m_refreshing)
        return;

      if (EditorApplication.timeSinceStartup < m_nextRefreshTime)
        return;

      if (m_formula.GetHashCode() != m_hash)
      {
        m_refreshing = true;
        StartCoroutine(RefreshTexture());
      }
    }

    private IEnumerator RefreshTexture()
    {
      UnityWebRequest req = UnityWebRequest.Get(BaseUrl + m_formula);
      yield return req.SendWebRequest();

      m_hash = m_formula.GetHashCode();

      if (req.error != null)
      {
        Debug.LogWarning("Error loading from \"" + req.url + "\":" + req.error);
      }
      else
      {
        Debug.Log("Finished loading from \"" + req.url);

        string filePath = Application.dataPath + "/LaTex Textures/" + m_hash + ".svg";
        try
        {
          using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
          {
            fs.Write(req.downloadHandler.data, 0, req.downloadHandler.data.Length);
          }

          Debug.Log("Finished writing file from \"" + filePath);
        }
        catch (Exception ex)
        {
          Debug.LogWarning("Error writing file \"" + filePath + "\":" + ex.Message);
        }
      }

      m_refreshing = false;
      m_nextRefreshTime = EditorApplication.timeSinceStartup + 2.0;
    }

    public void OnValidate()
    {
      int newHash = m_formula.GetHashCode();
      if (newHash != m_hash)
      {
        m_nextRefreshTime = EditorApplication.timeSinceStartup + 2.0;
      }
    }
  #endif
  }
}
