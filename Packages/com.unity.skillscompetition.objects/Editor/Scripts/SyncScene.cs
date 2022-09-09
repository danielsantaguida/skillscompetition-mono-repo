using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor.Scripts
{
    public class SyncScene : UnityEditor.Editor
    {

        private const string ScenePath = "Packages/com.unity.skillscompetition.objects/Runtime/BaseScene/";
        
        [MenuItem("Sync/MenuScene")]
        private static void SyncMenuScene()
        {
            Sync("Menu");
        }
        
        [MenuItem("Sync/PassingChallengeScene")]
        private static void SyncPassingScene()
        {
            Sync("PassingChallenge");
        }

        private static void Sync(string sceneName)
        {
            EditorSceneManager.OpenScene(Path.Combine(ScenePath, sceneName + "Sync.unity"), OpenSceneMode.Additive);
            
            var syncObjects = EditorSceneManager.GetSceneByName(sceneName + "Sync").GetRootGameObjects().ToList();
            var sceneObjects = EditorSceneManager.GetSceneByName(sceneName).GetRootGameObjects().ToList();
            
            var idx = 0;
            
            foreach (var syncGameObject in syncObjects)
            {
                while (sceneObjects[idx].GetComponent<Camera>() != null || sceneObjects[idx].GetComponent<Light>() != null)
                {
                    idx++;
                }

                if (syncGameObject.name != sceneObjects[idx].name)
                {
                    var instGameObject = Instantiate(syncGameObject);
                    instGameObject.transform.SetSiblingIndex(idx);
                }
            
                idx++;
            }

            EditorSceneManager.CloseScene(EditorSceneManager.GetSceneByName(sceneName + "Sync"), true);
        }
    }
}
