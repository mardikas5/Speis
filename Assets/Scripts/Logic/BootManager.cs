using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BootManager : Singleton<BootManager>
{
    [SerializeField]
    public Object LoadScene;

    public string ScenePathFolder = "Assets/Scenes/";

    public void NewGame()
    {

    }

    public void LoadGame( GameData data )
    {
        //Scene GameScene = SceneManager.GetSceneByPath( ScenePathFolder + LoadScene.name + ".unity");

        AsyncOperation op = SceneManager.LoadSceneAsync( LoadScene.name );

        op.completed += ( x ) => SceneLoadedHandler( x, data );
    }

    private void SceneLoadedHandler( AsyncOperation obj, GameData data )
    {
        GameSceneInitializer.Instance.LoadGamedataToScene( data );
    }
}
