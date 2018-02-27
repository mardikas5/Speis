using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using ProtoBuf.Meta;
using ProtoBuf;
using System.Collections.Generic;
using UnityEngine.Assertions;

public static class Persistence
{
    public static readonly string SavegameRoot = "./savegames/";
    public static readonly string WorldFileName = "World";
    public static readonly string GameFileName = "gameData";
    public static readonly string MetaFileName = "meta";

    public static readonly string ResourceObjectPath = "ResourceObjects/";


    public static bool InitializedModel = false;


    public static void InitializeModel()
    {
        if ( !InitializedModel )
        {
            RuntimeTypeModel model = RuntimeTypeModel.Default;

            model.Add( typeof( Vector2 ), false ).SetSurrogate( typeof( Vector2Surrogate ) );
            model.Add( typeof( Vector3 ), false ).SetSurrogate( typeof( Vector3Surrogate ) );
            model.Add( typeof( Quaternion ), false ).SetSurrogate( typeof( QuaternionSurrogate ) );

            model.Add( typeof( ProtoBase ), true );

            model.Add( typeof( Storable.Surrogate ), true );

            //structure behaviours
            model.Add( typeof( ResourceProducer.Surrogate ), true );
            model.Add( typeof( StructureBehaviour.Surrogate ), true );
            model.Add( typeof( Storage.Surrogate ), true );

            //entity
            model.Add( typeof( Entity.Surrogate ), true );
            model.Add( typeof( Structure.Surrogate ), true );
            model.Add( typeof( Mineable.Surrogate ), true );
            

            InitializedModel = true;
        }
    }

    private static void AssureDirectoryExists( string filename )
    {
        string savegamePath = SavegameRoot + filename;

        if ( !Directory.Exists( savegamePath ) )
        {
            Directory.CreateDirectory( savegamePath );
        }
    }

    public static void SaveGame( string filename, GameData gameData )
    {
        Assert.IsNotNull( gameData );
        Assert.IsNotNull( filename );

        InitializeModel();

        AssureDirectoryExists( filename );

        string path = SavegameRoot + filename + "/" + GameFileName;
        string tmpPath = path + ".tmp";

        FileStream filestream = new FileStream( tmpPath, FileMode.Create, FileAccess.Write );
        Serializer.Serialize( filestream, gameData );
        filestream.Close();

        if ( File.Exists( path ) )
        {
            File.Delete( path );
        }

        Debug.Log( "Saved" );

        File.Move( tmpPath, path );
    }

    public static GameData LoadGame( string filename )
    {
        Assert.IsNotNull( filename );

        InitializeModel();

        string directory = SavegameRoot + filename + "/";
        Debug.Log( filename + ", " + directory );
        Assert.IsTrue( File.Exists( directory + GameFileName ) );

        string path = directory + GameFileName;

        FileStream filestream = new FileStream( path, FileMode.Open, FileAccess.Read );
        GameData gameData = ProtoBuf.Serializer.Deserialize<GameData>( filestream );

        filestream.Close();

        return gameData;
    }

    //public static VersionData LoadMeta( string filename )
    //{
    //    string directory = SavegameRoot + filename + "/";

    //    if ( filename == null || !File.Exists( directory + MetaFileName ) )
    //    {
    //        Debug.Log( "Version data for save is null. Generate a new game." );
    //        return null;
    //    }

    //    string path = directory + MetaFileName;

    //    FileStream filestream = new FileStream( path, FileMode.Open, FileAccess.Read );
    //    VersionData metaData = ProtoBuf.Serializer.Deserialize<VersionData>( filestream );

    //    filestream.Close();

    //    return metaData;
    //}

    //public static void SaveMeta( string filename, VersionData versionData )
    //{
    //    Assert.IsNotNull( versionData );
    //    Assert.IsNotNull( filename );

    //    InitializeModel();

    //    Persistence.AssureDirectoryExists( filename );

    //    string metaPath = SavegameRoot + filename + "/" + MetaFileName;
    //    string tmpPath = metaPath + ".tmp";

    //    FileStream filestream = new FileStream( tmpPath, FileMode.Create, FileAccess.Write );
    //    Serializer.Serialize( filestream, versionData );
    //    filestream.Close();

    //    if ( File.Exists( metaPath ) )
    //    {
    //        File.Delete( metaPath );
    //    }
    //    File.Move( tmpPath, metaPath );
    //}


    //public static void SaveOverworld( string filename, OverworldData overworldData )
    //{
    //    Assert.IsNotNull( filename );
    //    Assert.IsNotNull( overworldData );

    //    InitializeModel();

    //    if ( overworldData == null )
    //        return;

    //    Persistence.AssureDirectoryExists( filename );

    //    string savegamePath = SavegameRoot + filename;

    //    // Save overworld		
    //    string overworldPath = SavegameRoot + filename + "/" + WorldFileName;
    //    string overworldTmpPath = overworldPath + ".tmp";

    //    FileStream filestream = new FileStream( overworldTmpPath, FileMode.Create, FileAccess.Write );
    //    Serializer.Serialize( filestream, overworldData );
    //    filestream.Close();

    //    if ( File.Exists( overworldPath ) )
    //    {
    //        File.Delete( overworldPath );
    //    }
    //    File.Move( overworldTmpPath, overworldPath );
    //}

    //public static OverworldData LoadOverworld( string filename )
    //{
    //    InitializeModel();

    //    string directory = SavegameRoot + filename + "/";

    //    if ( !File.Exists( directory + WorldFileName ) )
    //    {
    //        Debug.LogError( "missing overworld" );
    //        return null;
    //    }

    //    // Load overworld data
    //    string overworldPath = directory + WorldFileName;

    //    FileStream filestream = new FileStream( overworldPath, FileMode.Open, FileAccess.Read );

    //    OverworldData overworldData = ProtoBuf.Serializer.Deserialize<OverworldData>( filestream );

    //    filestream.Close();

    //    return overworldData;
    //}

    public static string[] GetSavegames()
    {
        string[] savegamePaths = Directory.GetDirectories( Persistence.SavegameRoot );

        string[] savegames = new string[savegamePaths.Length];

        for ( int i = 0; i < savegamePaths.Length; ++i )
        {
            savegames[i] = savegamePaths[i].Substring( SavegameRoot.Length );
        }

        return savegames;
    }

    //public static string SaveLocation( LocationData data, string savegameName, string name )
    //{
    //    InitializeModel();

    //    string directory = SavegameRoot + savegameName + "/";

    //    string[] files = Directory.GetFiles( directory ).Select( o => o.Substring( directory.Length ) ).ToArray();

    //    if ( name.Length == 0 )
    //    {
    //        for ( int i = 0; i < 128; ++i )
    //        {
    //            string check = string.Format( "location_{0}", i );
    //            if ( Array.IndexOf( files, check ) == -1 )
    //            {
    //                name = check;
    //                break;
    //            }
    //        }
    //    }

    //    if ( name.Length > 0 )
    //    {
    //        string locationPath = directory + name;
    //        string locationTmpPath = directory + name + ".tmp";

    //        FileStream file = new FileStream( locationTmpPath, FileMode.Create, FileAccess.Write );
    //        ProtoBuf.Serializer.Serialize( file, data );
    //        file.Close();

    //        if ( File.Exists( locationPath ) )
    //        {
    //            File.Delete( locationPath );
    //        }
    //        File.Move( locationTmpPath, locationPath );
    //    }

    //    return name;
    //}

    //public static LocationData LoadLocation( string filename, OverworldLocationData overworldLocation )
    //{
    //    InitializeModel();

    //    string name = overworldLocation.InternalName;
    //    string directory = SavegameRoot + filename + "/";

    //    LocationData data = null;

    //    if ( !File.Exists( directory + name ) )
    //    {
    //        data = LocationFactory.GenerateLocation( new Int3( 128, LocationConstants.HeightLevels, 128 ), overworldLocation );
    //        if ( !overworldLocation.IsTemporary )
    //        {
    //            name = Persistence.SaveLocation( data, GameController.Instance.ActiveGameName, name );
    //        }
    //    }
    //    else
    //    {
    //        string overworldPath = directory + name;

    //        FileStream file = new FileStream( overworldPath, FileMode.Open, FileAccess.Read );
    //        data = ProtoBuf.Serializer.Deserialize<LocationData>( file );
    //        file.Close();
    //    }

    //    GameController gameController = GameController.Instance;

    //    return data;
    //}
}
