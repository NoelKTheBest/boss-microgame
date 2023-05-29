using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor
{   
    bool foldoutStep;
    bool foldoutDash;
    string tab = "    ";
    Rect dashTimeDisplayArea;
    
    // Serialized Properties
    SerializedProperty _speed;
    //SerializedProperty _directionForAttack;
    //SerializedProperty _attackDirectionOverride;
    //SerializedProperty _directionForStep;
    //SerializedProperty _stepDirectionOverride;
    SerializedProperty _isStepping;
    SerializedProperty _stepTime;
    SerializedProperty _stepCooldown;
    SerializedProperty _stepSpeed;
    //SerializedProperty _directionForDash;
    //SerializedProperty _dashDirectionOverride;
    SerializedProperty _isDashing;
    SerializedProperty _totalDashTime;
    SerializedProperty _dashTime;
    SerializedProperty _dashSpeed;
    SerializedProperty _dashCooldown;
    SerializedProperty _accelTime;
    SerializedProperty _accelAmount;
    SerializedProperty _decelTime;
    SerializedProperty _decelAmount;

    void OnEnable()
    {
        _speed = serializedObject.FindProperty("speed");
        //_directionForAttack = serializedObject.FindProperty("directionForAttack");
        //_attackDirectionOverride = serializedObject.FindProperty("attackDirectionOverride");
        //_directionForStep = serializedObject.FindProperty("directionForStep");
        //_stepDirectionOverride = serializedObject.FindProperty("stepDirectionOverride");
        _isStepping = serializedObject.FindProperty("isStepping");
        _stepTime = serializedObject.FindProperty("stepTime");
        _stepCooldown = serializedObject.FindProperty("stepCooldown");
        _stepSpeed = serializedObject.FindProperty("stepSpeed");
        //_directionForDash = serializedObject.FindProperty("directionForDash");
        //_dashDirectionOverride = serializedObject.FindProperty("dashDirectionOverride");
        _isDashing = serializedObject.FindProperty("isDashing");
        _totalDashTime = serializedObject.FindProperty("totalDashTime");
        _dashTime = serializedObject.FindProperty("dashTime");
        _dashSpeed = serializedObject.FindProperty("dashSpeed");
        _dashCooldown = serializedObject.FindProperty("dashCooldown");
        _accelTime = serializedObject.FindProperty("accelTime");
        _accelAmount = serializedObject.FindProperty("accelAmount");
        _decelTime = serializedObject.FindProperty("decelTime");
        _decelAmount = serializedObject.FindProperty("decelAmount");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update(); //not sure how this works

        PlayerController t = (PlayerController)target;

        #region Movement Speed and Attack Direction
        // ----HEADER----
        GUILayout.Label("Fields for movement", EditorStyles.boldLabel);

        // GUI Control for PlayerController.speed
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("" + tab + "Speed");
        _speed.floatValue = EditorGUILayout.FloatField(_speed.floatValue);
        EditorGUILayout.EndHorizontal();

        // ----HEADER----
        //GUILayout.Label("Attack Variables", EditorStyles.boldLabel);

        // GUI Control for PlayerController.directionForAttack
        //EditorGUILayout.BeginHorizontal();
        //GUILayout.Label("Direction For Attack");
        //_directionForAttack.stringValue = EditorGUILayout.TextField(_directionForAttack.stringValue);
        //EditorGUILayout.EndHorizontal();
        
        // GUI Control for PlayerController.attackDirectionOverride
        //EditorGUILayout.BeginHorizontal();
        //GUILayout.Label(new GUIContent("Attack Direction Override",
        //    "This value lets you override the input for the " +
        //    "direction of the attack for testing purposes"));
        //_attackDirectionOverride.stringValue = EditorGUILayout.TextField(_attackDirectionOverride.stringValue);
        //EditorGUILayout.EndHorizontal();
        #endregion

        #region Attack Movement And Step Controls
        foldoutStep = EditorGUILayout.BeginFoldoutHeaderGroup(foldoutStep,
            "Attack Movement and Stepping");
        if (foldoutStep)
        {
            // GUI Control for PlayerController.directionForStep
            //EditorGUILayout.BeginHorizontal();
            //GUILayout.Label("" + tab + "Direction For Step");
            //_directionForStep.stringValue = EditorGUILayout.TextField(_directionForStep.stringValue);
            //EditorGUILayout.EndHorizontal();

            // GUI Control for PlayerController.stepDirectionOverride
            //EditorGUILayout.BeginHorizontal();
            //GUILayout.Label(new GUIContent("" + tab + "Step Direction Override",
            //    "This value lets you override the input for the " +
            //    "direction of the step for testing purposes"));
            //_stepDirectionOverride.stringValue = EditorGUILayout.TextField(_stepDirectionOverride.stringValue);
            //EditorGUILayout.EndHorizontal();

            // GUI Control for PlayerController.isStepping
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("" + tab + "Is Stepping");
            _isStepping.boolValue = EditorGUILayout.Toggle(_isStepping.boolValue);
            EditorGUILayout.EndHorizontal();

            // GUI Control for PlayerController.stepTime
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("" + tab + "Step Time");
            _stepTime.floatValue = EditorGUILayout.FloatField(_stepTime.floatValue);
            EditorGUILayout.EndHorizontal();

            // GUI Control for PlayerController.stepCooldown
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("" + tab + "Step Cooldown");
            _stepCooldown.floatValue = EditorGUILayout.FloatField(_stepCooldown.floatValue);
            EditorGUILayout.EndHorizontal();

            // GUI Control for PlayerController.stepSpeed
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("" + tab + "Step Speed");
            _stepSpeed.floatValue = EditorGUILayout.FloatField(_stepSpeed.floatValue);
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        #endregion

        #region Dash Controls
        foldoutDash = EditorGUILayout.BeginFoldoutHeaderGroup(foldoutDash, "Dash Variables");
        if (foldoutDash)
        {
            Rect tempForAccel;
            Rect tempForDecel;

            // GUI Control for PlayerController.directionForDash
            //EditorGUILayout.BeginHorizontal();
            //GUILayout.Label("" + tab + "Direction For Dash");
            //_directionForDash.stringValue = EditorGUILayout.TextField(_directionForDash.stringValue);
            //EditorGUILayout.EndHorizontal();

            // GUI Control for PlayerController.dashDirectionOverride
            //EditorGUILayout.BeginHorizontal();
            //GUILayout.Label(new GUIContent("" + tab + "Dash Direction Override",
            //    "This value lets you override the input for the direction" +
            //    " of the dash for testing purposes"));
            //_dashDirectionOverride.stringValue = EditorGUILayout.TextField(_dashDirectionOverride.stringValue);
            //EditorGUILayout.EndHorizontal();

            // GUI Control for PlayerController.isDashing
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("" + tab + "Is Dashing");
            _isDashing.boolValue = EditorGUILayout.Toggle(_isDashing.boolValue);
            EditorGUILayout.EndHorizontal();

            // Create Space between GUI elements
            GUILayout.Label("");

            // GUI Control for Editing Dash Variables
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("" + tab + "Total Dash Time",
                "The total amount of time for the dash. Acceleration time, deceleration time," +
                " and main dash time need to add up to total dash time"));
            _totalDashTime.floatValue = EditorGUILayout.FloatField(_totalDashTime.floatValue);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("" + tab + "Dash Cooldown",
                "The amount of time it takes for the dash to be ready to use again."));
            _dashCooldown.floatValue = EditorGUILayout.FloatField(_dashCooldown.floatValue);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("" + tab + "Dash Speed",
                "The max speed of the dash."));
            _dashSpeed.floatValue = EditorGUILayout.FloatField(_dashSpeed.floatValue);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("" + tab + "Dash Time: " + _dashTime.floatValue,
                "The amount of time the dash spends at max speed."));
            EditorGUILayout.EndHorizontal();

            // Create Space between GUI elements
            GUILayout.Label("");

            // GUI Control for tweeking the the dash mechanic
            {
                EditorGUILayout.BeginHorizontal();

                // GUI Control for PlayerController.accelTime
                {
                    EditorGUILayout.BeginVertical();
                    GUILayout.Label(new GUIContent("Accel Time: " + t.accelTime,
                        "The time it takes for the dash to reach top speed"));

                    // Time for acceleration is input as the percentage of time the dash takes to build up speed
                    _accelTime.floatValue = GUILayout.HorizontalSlider(_accelTime.floatValue, 0f, 1f);
                    float tempFloatA = _accelTime.floatValue * 100;
                    tempFloatA = Mathf.CeilToInt(tempFloatA);
                    tempFloatA = tempFloatA / 100;
                    _accelTime.floatValue = tempFloatA;

                    tempForAccel = EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("" + tab + tab + tab));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.EndVertical();
                }
                
                // Display Area for Main Dash Time
                dashTimeDisplayArea = EditorGUILayout.BeginVertical();
                GUILayout.Label(new GUIContent("" + tab + tab + tab + tab + tab + tab));
                EditorGUILayout.EndVertical();

                // GUI Control for PlayerController.decelTime
                {
                    EditorGUILayout.BeginVertical();
                    GUILayout.Label(new GUIContent("Decel Time: " + t.decelTime,
                        "The amount that deceleration decrements every frame"));

                    // Time for deceleration is input as the percentage of time the dash takes to slow down
                    _decelTime.floatValue = GUILayout.HorizontalSlider(_decelTime.floatValue, 1f, 0f);
                    float tempFloatD = _decelTime.floatValue * 100;
                    tempFloatD = Mathf.CeilToInt(tempFloatD);
                    tempFloatD = tempFloatD / 100;
                    _decelTime.floatValue = tempFloatD;

                    tempForDecel = EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("" + tab + tab + tab));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.EndHorizontal();
            }

            CalculateAccelerationAndDeceleration();

            GUI.Label(dashTimeDisplayArea, new GUIContent("" + tab + t.dashTime,
                "The amount of time that the dash spends at full speed"));

            EditorGUILayout.BeginHorizontal();
            GUI.Label(tempForAccel, new GUIContent("Accel Amount: " + t.accelAmount + tab + tab,
                "The amount that Acceleration increments every frame"));
            GUI.Label(tempForDecel, new GUIContent("Decel Amount: " + t.decelAmount + tab + tab,
                "The amount that Deceleration decrements every frame"));
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        #endregion
        
        serializedObject.ApplyModifiedProperties();
    }

    void CalculateAccelerationAndDeceleration()
    {
        float fixedTimestep = Time.fixedDeltaTime;
        float totalDashTime = _totalDashTime.floatValue;
        float dashAccelTime = totalDashTime * _accelTime.floatValue;
        float dashDecelTime = totalDashTime * _decelTime.floatValue;
        float mainDashTime = (totalDashTime - dashAccelTime) - dashDecelTime;

        // There should be no need to check total dash time with a net time value
        //  the main dash time will already change to reflect changes in
        //  acceleration time or deceleration time

        float baseMoveSpeed = _speed.floatValue;
        float dashMoveSpeed = _dashSpeed.floatValue;
        float changeInSpeed = dashMoveSpeed - baseMoveSpeed;

        float newAccel = (changeInSpeed * fixedTimestep) / dashAccelTime;
        float newDecel = (changeInSpeed * fixedTimestep) / dashDecelTime;
        
        _dashTime.floatValue = mainDashTime;
        _accelAmount.floatValue = newAccel;
        _decelAmount.floatValue = newDecel;
    }
}
