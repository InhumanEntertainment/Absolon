using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

//=====================================================================================================================================//
//=====================================================================================================================================//
//=====================================================================================================================================//
[System.Serializable]
public class Build
{   
    public string Name = "Product Name";
    public string Version = "1.0";
    public string BundleId = "com.inhuman.default";

    public string Path = "Build/Folder/";
    public BuildTarget Target = BuildTarget.StandaloneWindows;
    public BuildOptions Options = BuildOptions.AutoRunPlayer | BuildOptions.ShowBuiltPlayer;
    public string Script;
    public string ScriptCommand;

    public AspectRatios Aspect = AspectRatios.Free;

    public string Icon;
    public string Splash;

    public static Dictionary<BuildTarget, string> TargetExtension = new Dictionary<BuildTarget, string>() 
    { 
        { BuildTarget.StandaloneWindows, ".exe" }, 
        { BuildTarget.StandaloneWindows64, ".exe" },
        { BuildTarget.Android, ".apk" },
        { BuildTarget.iPhone, ".xcode" },
        { BuildTarget.MetroPlayer, ".zip" },
        { BuildTarget.StandaloneOSXIntel, ".app" },
        { BuildTarget.WebPlayer, "" },
        { BuildTarget.FlashPlayer, "" },
    };

    public enum AspectRatios { None, iPhone, iPad, iPhone5, Nexus7, KindleFire, PC, Free }
    public static Dictionary<AspectRatios, Rect> TargetAspect = new Dictionary<AspectRatios, Rect>() 
    { 
        { AspectRatios.iPhone, new Rect(0, 0, 640, 960) }, 
        { AspectRatios.iPhone5, new Rect(0, 0, 640, 960) }, 
        { AspectRatios.iPad, new Rect(0, 0, 768, 1024) },       
        { AspectRatios.Free, new Rect(0, 0, 1000, 1000) },
        { AspectRatios.KindleFire, new Rect(0, 0, 600, 1004) },
        { AspectRatios.PC, new Rect(0, 0, 640, 480) },
    };

    //=====================================================================================================================================//
    public void Activate()
    {
        Debug.Log("BUILD MANAGER: " + Name);

        PlayerSettings.productName = Name;
        PlayerSettings.iOS.applicationDisplayName = Name;
        PlayerSettings.bundleVersion = Version;
        PlayerSettings.bundleIdentifier = BundleId;
        PlayerSettings.iPhoneBundleIdentifier = BundleId;

        EditorUserBuildSettings.SetBuildLocation(Target, BuildPath());

        if (TargetAspect.ContainsKey(Aspect))
        {
            PlayerSettings.defaultScreenWidth = (int)TargetAspect[Aspect].width;
            PlayerSettings.defaultScreenHeight = (int)TargetAspect[Aspect].height;
        }


        // Set Icons //
        if (Icon != "")
	    {
            string path = AssetDatabase.GUIDToAssetPath(Icon);
            Texture2D texture = (Texture2D)AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));
            Texture2D[] icons = new Texture2D[] {texture};
            PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.iPhone, icons);
	    }

        // Set Splash //
        if (Splash != "")
	    {
            string splashPath = AssetDatabase.GUIDToAssetPath(Splash);
            Texture2D splashTexture = (Texture2D)AssetDatabase.LoadAssetAtPath(splashPath, typeof(Texture2D));
            PlayerSettings.resolutionDialogBanner = splashTexture;

            // Copy Te
            File.Copy(splashPath, "Assets/Textures/Splash.tga", true);
            AssetDatabase.Refresh();
	    }
   
        // Call Script //
        GameObject scriptObject = GameObject.Find(Script);
        if (scriptObject)
        {
            scriptObject.SendMessage(ScriptCommand, SendMessageOptions.DontRequireReceiver);
        }
    }

    //=====================================================================================================================================//
    public void Run()
    {        
        string buildPath = BuildPath();
        Debug.Log("BUILD MANAGER: Building - " + buildPath);

        string[] levels = new string[EditorBuildSettings.scenes.Length];       
        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
		{
            levels[i] = EditorBuildSettings.scenes[i].path;
		}

        CreateFolder(System.IO.Path.GetDirectoryName(buildPath));
        BuildPipeline.BuildPlayer(levels, buildPath, Target, Options);
    }

    //=====================================================================================================================================//
    public string BuildPath()
    {
        string result = Path + Name;
        if(TargetExtension.ContainsKey(Target))
            result += TargetExtension[Target];

        return result;
    }

    //=====================================================================================================================================//
    static void CreateFolder(string path)
    {
        if (!Directory.GetParent(path).Exists)
        {
            CreateFolder(Directory.GetParent(path).FullName);           
        }
        if (!System.IO.Directory.Exists(path))
        {
            System.IO.Directory.CreateDirectory(path);
            Debug.Log("Created Folder: " + path);
        }
    }
}

//=====================================================================================================================================//
//=====================================================================================================================================//
//=====================================================================================================================================//
[XmlRoot("BuildManager"), System.Serializable]
public class BuildManagerData
{
    static string DataPath = Application.dataPath + "/BuildManager.xml";

    public int Current = -1;
    public List<Build> Builds = new List<Build>();

    //=====================================================================================================================================//
    public void Save()
    {
        Debug.Log("Saved to File: " + DataPath);

        XmlSerializer serializer = new XmlSerializer(typeof(BuildManagerData));
        using (var stream = new FileStream(DataPath, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    //=====================================================================================================================================//
    public static BuildManagerData Load()
    {
        if (File.Exists(DataPath))
        {
            Debug.Log("Loaded from File: " + DataPath);
            XmlSerializer serializer = new XmlSerializer(typeof(BuildManagerData));

            using (var stream = new FileStream(DataPath, FileMode.Open))
            {
                return serializer.Deserialize(stream) as BuildManagerData;
            }
        }
        else
        {
            return new BuildManagerData();
        }
    }
}

//=====================================================================================================================================//
//=====================================================================================================================================//
//=====================================================================================================================================//
public class BuildManager : EditorWindow
{
    public BuildManagerData Data;
    public Texture2D[] Icons = new Texture2D[16];
    public Texture2D[] Splashes = new Texture2D[16];
    public GameObject ScriptObject;

    Vector2 ScrollPos = Vector2.zero;
    bool[] Foldouts = new bool[1000];

    //============================================================================================================================================//
    [MenuItem("Inhuman/Build Manager")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(BuildManager));
    }

    //============================================================================================================================================//
    public BuildManager()
    {
        Data = BuildManagerData.Load();
    }

    //============================================================================================================================================//
    void Load()
    {
        Data = BuildManagerData.Load();
    }

    //============================================================================================================================================//
    void Save()
    {
        if (Data != null)
        {
            Data.Save();
        }
    }

    //============================================================================================================================================//
    void OnGUI()
    {
        if (Data.Current > Data.Builds.Count)
            Data.Current = -1;

        EditorGUILayout.BeginVertical();
        ScrollPos = GUILayout.BeginScrollView(ScrollPos);
        EditorGUILayout.BeginVertical();

        GUI.backgroundColor = Color.cyan;
        
        // File //====================================================================================//
        GUI.backgroundColor = Color.grey;
        if (GUILayout.Button("Player Settings", EditorStyles.toolbarButton))
        {
            EditorApplication.ExecuteMenuItem("Edit/Project Settings/Player");
        } 
        
        GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Load", EditorStyles.toolbarButton))
            {
                Load();
            }

            if (GUILayout.Button("Save", EditorStyles.toolbarButton))
            {
                Save();
            }
        GUILayout.EndHorizontal();

        

        GUI.backgroundColor = Color.red;
        GUILayout.BeginHorizontal();
            if (GUILayout.Button("Switch", EditorStyles.toolbarButton))
            {
                EditorUserBuildSettings.SwitchActiveBuildTarget(Data.Builds[Data.Current].Target);       
            }

            if (GUILayout.Button("Build", EditorStyles.toolbarButton))
            {
                Data.Builds[Data.Current].Run();
            }
        GUILayout.EndHorizontal();

        GUI.backgroundColor = Color.green;
        GUILayout.BeginHorizontal();
            if (GUILayout.Button("Play", EditorStyles.toolbarButton))
            {
                string playPath = Application.dataPath.Replace("/Assets", "") + "/" + Data.Builds[Data.Current].BuildPath();
                if (File.Exists(playPath))
                {
                    System.Diagnostics.Process proc = new System.Diagnostics.Process();
                    proc.StartInfo.FileName = playPath;
                    proc.Start();
                }
                else
                    Debug.LogWarning("Build does not exist.");
            }

            if (GUILayout.Button("Explore", EditorStyles.toolbarButton))
            {
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = Path.GetDirectoryName(Application.dataPath.Replace("/Assets", "") + "/" + Data.Builds[Data.Current].BuildPath());
                proc.Start();
            }

            //if (EditorGUILayout.(Data.Current, "Test Script"))
        GUILayout.EndHorizontal();        
        
        GUI.backgroundColor = Color.white;       

        if (Data == null)
        {
            //Load();
        }
        else
        {
            // Builds //====================================================================================//
            GUILayout.Label("Builds", EditorStyles.boldLabel);

            for (int i = 0; i < Data.Builds.Count; i++)
            {
                GUI.backgroundColor = Color.black;
                GUILayout.BeginHorizontal();
                 
                //Foldouts[i] = EditorGUILayout.Foldout(Foldouts[i], Data.Builds[i].Name + " : " + Data.Builds[i].Target.ToString());
                if (Data.Current == i)
                {
                    GUI.contentColor = Color.cyan;
                    GUI.backgroundColor = Color.black;
                }
                else
                {
                    GUI.contentColor = Color.white;
                    GUI.backgroundColor = Color.grey;
                }

                if (GUILayout.Button(Data.Builds[i].Name + " : " + Data.Builds[i].Target.ToString(), EditorStyles.toolbarButton))
                {
                    Data.Current = i;
                    Data.Builds[i].Activate();
                    ScriptObject = null;
                }

                //GUILayout.FlexibleSpace();
                if (GUILayout.Button("X", EditorStyles.toolbarButton, GUILayout.Width(30)))
                {
                    Data.Current = -1;
                    Data.Builds.RemoveAt(i);
                }
                if (GUILayout.Button("+", EditorStyles.toolbarButton, GUILayout.Width(30)))
                {
                    Build build = new Build();
                    Data.Builds.Insert(i, build);
                    Data.Current++;
                }
                if (GUILayout.Button("^", EditorStyles.toolbarButton, GUILayout.Width(30)))
                {
                    if (i > 0)
                    {
                        Build temp = Data.Builds[i];
                        Data.Builds[i] = Data.Builds[i - 1];
                        Data.Builds[i - 1] = temp;
                        //Foldouts[i] = false;
                        //Foldouts[i - 1] = true;
                    }
                }
                if (GUILayout.Button("v", EditorStyles.toolbarButton, GUILayout.Width(30)))
                {
                    if (i < Data.Builds.Count - 1)
                    {
                        Build temp = Data.Builds[i];
                        Data.Builds[i] = Data.Builds[i + 1];
                        Data.Builds[i + 1] = temp;
                        //Foldouts[i] = false;
                        //Foldouts[i + 1] = true;
                    }
                }

                GUILayout.EndHorizontal();

                /*if (Foldouts[i])
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(32);
                    GUILayout.BeginVertical();

                    GUILayout.BeginHorizontal();
                    
                    // Icon //
                    if (Data.Builds[i].Icon != "")
                    {
                        string path = AssetDatabase.GUIDToAssetPath(Data.Builds[i].Icon);
                        Icons[i] = (Texture2D)AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));                        
                    }
                    else
                    {
                        Texture2D[] icons = PlayerSettings.GetIconsForTargetGroup(BuildTargetGroup.iPhone);
                        if (icons.Length > 0)
                        {
                            string path = AssetDatabase.GetAssetPath(icons[0]);
                            Data.Builds[i].Icon = AssetDatabase.AssetPathToGUID(path);
                        }
                    }
                    GUILayout.Box(Icons[i], GUILayout.Width(54), GUILayout.Height(54));
                    Rect iconRect = GUILayoutUtility.GetLastRect();

                    // Splash //
                    if (Data.Builds[i].Splash != "")
                    {
                        string path = AssetDatabase.GUIDToAssetPath(Data.Builds[i].Splash);
                        Splashes[i] = (Texture2D)AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));
                    }
                    else
                    {
                        if (PlayerSettings.resolutionDialogBanner)
                        {
                            string path = AssetDatabase.GetAssetPath(PlayerSettings.resolutionDialogBanner);
                            Data.Builds[i].Splash = AssetDatabase.AssetPathToGUID(path);
                        }
                    }
                    GUILayout.Box(Splashes[i], GUILayout.Width(54), GUILayout.Height(54));
                    Rect splashRect = GUILayoutUtility.GetLastRect();

                    GUILayout.EndHorizontal();

                    // Drag + Drop //
                    if (Event.current.type == EventType.DragUpdated)
                    {
                        if (iconRect.Contains(Event.current.mousePosition) || splashRect.Contains(Event.current.mousePosition))
                        {
                            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                        }
                    }
                    if (Event.current.type == EventType.DragPerform)
                    {
                        if (iconRect.Contains(Event.current.mousePosition))
                        {
                            for (int x = DragAndDrop.objectReferences.Length - 1; x >= 0; x--)
                            {
                                string assetPath = AssetDatabase.GetAssetPath(DragAndDrop.objectReferences[x]);
                                string guid = AssetDatabase.AssetPathToGUID(assetPath);
                                Data.Builds[i].Icon = guid;
                            }
                        }
                        else if (splashRect.Contains(Event.current.mousePosition))
                        {
                            for (int x = DragAndDrop.objectReferences.Length - 1; x >= 0; x--)
                            {
                                string assetPath = AssetDatabase.GetAssetPath(DragAndDrop.objectReferences[x]);
                                string guid = AssetDatabase.AssetPathToGUID(assetPath);
                                Data.Builds[i].Splash = guid;
                            }
                        }
                    }

                    Data.Builds[i].Name = EditorGUILayout.TextField("Build Name:", Data.Builds[i].Name);
                    Data.Builds[i].ProductName = EditorGUILayout.TextField("Product Name:", Data.Builds[i].ProductName);
                    Data.Builds[i].Version = EditorGUILayout.TextField("Version:", Data.Builds[i].Version);
                    Data.Builds[i].BundleId = EditorGUILayout.TextField("Bundle Id:", Data.Builds[i].BundleId);
                    Data.Builds[i].Path = EditorGUILayout.TextField("Path:", Data.Builds[i].Path);
                    Data.Builds[i].Target = (BuildTarget)EditorGUILayout.EnumPopup("Target:", Data.Builds[i].Target);
                    Data.Builds[i].Aspect = (Build.AspectRatios)EditorGUILayout.EnumPopup("Aspect:", Data.Builds[i].Aspect);

                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                }*/
            }

            GUI.contentColor = Color.white; 
            GUI.backgroundColor = Color.white;

            if (GUILayout.Button("New Build", EditorStyles.toolbarButton))
            {
                Build build = new Build();
                Foldouts[Data.Builds.Count] = true;
                Data.Builds.Add(build);
            }

            // Property View ======================================================================//
            if (Data.Current >= 0 && Data.Current < Data.Builds.Count)
            {
                GUILayout.Label(Data.Builds[Data.Current].Name, EditorStyles.boldLabel);
            
                GUILayout.BeginHorizontal();
                // Icon //
                if (Data.Builds[Data.Current].Icon != "")
                {
                    string path = AssetDatabase.GUIDToAssetPath(Data.Builds[Data.Current].Icon);
                    Icons[Data.Current] = (Texture2D)AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));
                }
                else
                {
                    Texture2D[] icons = PlayerSettings.GetIconsForTargetGroup(BuildTargetGroup.iPhone);
                    if (icons.Length > 0)
                    {
                        string path = AssetDatabase.GetAssetPath(icons[0]);
                        Data.Builds[Data.Current].Icon = AssetDatabase.AssetPathToGUID(path);
                    }
                }
                GUILayout.Box(Icons[Data.Current], GUIStyle.none, GUILayout.Width(128), GUILayout.Height(128));
                Rect iconRect = GUILayoutUtility.GetLastRect();

                // Splash //
                if (Data.Builds[Data.Current].Splash != "")
                {
                    string path = AssetDatabase.GUIDToAssetPath(Data.Builds[Data.Current].Splash);
                    Splashes[Data.Current] = (Texture2D)AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));
                }
                else
                {
                    if (PlayerSettings.resolutionDialogBanner)
                    {
                        string path = AssetDatabase.GetAssetPath(PlayerSettings.resolutionDialogBanner);
                        Data.Builds[Data.Current].Splash = AssetDatabase.AssetPathToGUID(path);
                    }
                }
                GUILayout.Box(Splashes[Data.Current], GUIStyle.none, GUILayout.Width(128), GUILayout.Height(128));
                Rect splashRect = GUILayoutUtility.GetLastRect();
                GUILayout.EndHorizontal();

                // Drag + Drop //
                if (Event.current.type == EventType.DragUpdated)
                {
                    if (iconRect.Contains(Event.current.mousePosition) || splashRect.Contains(Event.current.mousePosition))
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    }
                }
                if (Event.current.type == EventType.DragPerform)
                {
                    if (iconRect.Contains(Event.current.mousePosition))
                    {
                        for (int x = DragAndDrop.objectReferences.Length - 1; x >= 0; x--)
                        {
                            string assetPath = AssetDatabase.GetAssetPath(DragAndDrop.objectReferences[x]);
                            string guid = AssetDatabase.AssetPathToGUID(assetPath);
                            Data.Builds[Data.Current].Icon = guid;
                        }
                    }
                    else if (splashRect.Contains(Event.current.mousePosition))
                    {
                        for (int x = DragAndDrop.objectReferences.Length - 1; x >= 0; x--)
                        {
                            string assetPath = AssetDatabase.GetAssetPath(DragAndDrop.objectReferences[x]);
                            string guid = AssetDatabase.AssetPathToGUID(assetPath);
                            Data.Builds[Data.Current].Splash = guid;
                        }
                    }
                }

                Data.Builds[Data.Current].Name = EditorGUILayout.TextField("Product Name:", Data.Builds[Data.Current].Name);
                Data.Builds[Data.Current].Version = EditorGUILayout.TextField("Version:", Data.Builds[Data.Current].Version);
                Data.Builds[Data.Current].BundleId = EditorGUILayout.TextField("Bundle Id:", Data.Builds[Data.Current].BundleId);
                Data.Builds[Data.Current].Path = EditorGUILayout.TextField("Path:", Data.Builds[Data.Current].Path);
                Data.Builds[Data.Current].Target = (BuildTarget)EditorGUILayout.EnumPopup("Target:", Data.Builds[Data.Current].Target);
                Data.Builds[Data.Current].Aspect = (Build.AspectRatios)EditorGUILayout.EnumPopup("Aspect:", Data.Builds[Data.Current].Aspect);
                
                if (Data.Builds[Data.Current].Script != "")
                {
                    GameObject obj = GameObject.Find(Data.Builds[Data.Current].Script);
                    if (obj)
	                {
                        ScriptObject = obj;                            
	                }
                }

                ScriptObject = (GameObject)EditorGUILayout.ObjectField(ScriptObject, typeof(GameObject));
                Data.Builds[Data.Current].ScriptCommand = EditorGUILayout.TextField("Script Command:", Data.Builds[Data.Current].ScriptCommand);
                
                if (ScriptObject && ScriptObject.name != Data.Builds[Data.Current].Script)
                {
                    Debug.Log("Before: " + Data.Builds[Data.Current].Script + " - After: " + ScriptObject.name);
                    Data.Builds[Data.Current].Script = ScriptObject.name;
                }
       
            }
        }
        EditorGUILayout.EndVertical();
        GUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    //============================================================================================================================================//
    void OnInspectorUpdate()
    {
        this.Repaint();
    }
}