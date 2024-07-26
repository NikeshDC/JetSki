using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using Animation;

[CustomEditor(typeof(AnimatorAnimationPlayer))]
public class AnimatorAnimationPlayerEditor : Editor
{
    private AnimatorAnimationPlayer animationPlayer;

    private string assetpath = "Assets";
    private string controllerName = "NewController";

    private void OnEnable()
    {
        animationPlayer = (AnimatorAnimationPlayer)target;  
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        assetpath = EditorGUILayout.TextField("Save Path", assetpath);
        controllerName = EditorGUILayout.TextField("Controller name", controllerName);

        if (GUILayout.Button("Setup Animator Controller"))
        {
            string savePath = assetpath + "/" + controllerName + ".controller";

            Animator animator = animationPlayer.animator;
            if (animator == null)
            {
                Debug.LogError("Target animator has not been set");
            }
            else
            {
                AnimatorController animatorController = animator.runtimeAnimatorController as AnimatorController;
                SetupNewAnimatorControllerFrom(animatorController, savePath, animator);
            }
        }
    }

    private void SetupNewAnimatorControllerFrom(AnimatorController baseController, string savePath, Animator animator)
    {
        //copy base animator controller
        AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(baseController), savePath);
        AnimatorController newAnimatorController = AssetDatabase.LoadAssetAtPath<AnimatorController>(savePath);
        animator.runtimeAnimatorController = newAnimatorController;

        //add speed parameters for each state and assign it to the state's speed parameter
        foreach (AnimatorControllerLayer layer in newAnimatorController.layers)
        {
            ChildAnimatorState[] cstates = layer.stateMachine.states;
            foreach (ChildAnimatorState cstate in cstates)
            {
                AnimatorState state = cstate.state;

                //make speed parameter to control speed of the state
                string speedParameter = AnimatorAnimationPlayer.GetStateSpeedParameter(layer.name, state.name);
                newAnimatorController.AddParameter(speedParameter, AnimatorControllerParameterType.Float);
                state.speedParameter = speedParameter;
                state.speedParameterActive = true;
                animator.SetFloat(speedParameter, 1.0f);  //default speed parameter to 1.0f
                state.speed = 1.0f;
            }
        }
    }
}
