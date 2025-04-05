using UnityEngine;
using System.Collections;

namespace FIMSpace.FProceduralAnimation
{
    [AddComponentMenu("FImpossible Creations/Ragdoll Animator")]
    [DefaultExecutionOrder(-1)]
    public partial class RagdollAnimator : MonoBehaviour
    {
        //[HideInInspector] public bool _EditorDrawSetup = true;

        [SerializeField]
        private RagdollProcessor Processor;

        [Tooltip("! REQUIRED ! Just object with Animator and skeleton as child transforms")]
        public Transform ObjectWithAnimator;
        [Tooltip("If null then it will be found automatically - do manual if you encounter some errors after entering playmode")]
        public Transform RootBone;

        [Space(2)]
        [Tooltip("! OPTIONAL ! Leave here nothing to not use the feature! \n\nObject with bones structure to which ragdoll should try fit with it's pose.\nUseful only if you want to animate ragdoll with other animations than the model body animator.")]
        public Transform CustomRagdollAnimator;

        //[Tooltip("Toggle it if you want to drive ragdoll animator with some custom procedural motion done on the bones, like Tail Animator or some other procedural animation plugin")]
        //public bool CaptureLateUpdate = false;

        [Tooltip("If generated ragdoll should be destroyed when main skeleton root object stops existing")]
        public bool AutoDestroy = true;

        [HideInInspector]
        [Tooltip("When false, then ragdoll dummy skeleton will be generated in playmode, when true, it will be generated in edit mode")]
        public bool PreGenerateDummy = false;

        [Tooltip("Generated ragdoll dummy will be put inside this transform as child object.\n\nAssign main character object for ragdoll to react with character movement rigidbody motion, set other for no motion reaction.")]
        public Transform TargetParentForRagdollDummy;


        public RagdollProcessor Parameters { get { return Processor; } }


        public void Reset()
        {
            if (Processor == null) Processor = new RagdollProcessor();
            Processor.TryAutoFindReferences(transform);
            Animator an = GetComponentInChildren<Animator>();
            if (an) ObjectWithAnimator = an.transform;
        }


        public void Start()
        {
            if (Processor.Initialized) return;

            Processor.BackCompabilityCheck();

            Processor.Initialize(this, ObjectWithAnimator, CustomRagdollAnimator, RootBone, TargetParentForRagdollDummy);

            if (AutoDestroy)
            {
                if (!Processor.StartAfterTPose) SetAutoDestroy();
                else StartCoroutine(IEAutoDestroyAfterTPose());
            }

            _initialReposeMode = Parameters.ReposeMode;
        }


        #region Auto Destroy helpers

        IEnumerator IEAutoDestroyAfterTPose()
        {
            while (Parameters.Initialized == false)
            {
                yield return null;
            }

            SetAutoDestroy();
            yield break;
        }

        void SetAutoDestroy()
        {
            autoDestroy = Processor.RagdollDummyBase.gameObject.AddComponent<RagdollAutoDestroy>();
            autoDestroy.Parent = Processor.Pelvis.gameObject;
        }

        #endregion


        private void FixedUpdate()
        {
            #region Debug Performance Measure Start
#if UNITY_EDITOR
            _Debug_Perf_MeasureFixedUpdate(true);
#endif
            #endregion

            Processor.FixedUpdate();

            #region Debug Performance Measure End
#if UNITY_EDITOR
            _Debug_Perf_MeasureFixedUpdate(false);
#endif
            #endregion
        }


        private void Update()
        {
            #region Debug Performance Measure Start
#if UNITY_EDITOR
            _Debug_Perf_MeasureUpdate(true);
#endif
            #endregion

            Processor.Update();

            #region Debug Performance Measure End
#if UNITY_EDITOR
            _Debug_Perf_MeasureUpdate(false);
#endif
            #endregion
        }


        private void LateUpdate()
        {
            #region Debug Performance Measure Start
#if UNITY_EDITOR
            _Debug_Perf_MeasureLateUpdate(true);
#endif
            #endregion

            Processor.LateUpdate();

            #region Debug Performance Measure End
#if UNITY_EDITOR
            _Debug_Perf_MeasureLateUpdate(false);
#endif
            #endregion
        }


        #region Utility and Editor Code


#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (Application.isPlaying == false)
            {
                if (Processor != null)
                    if (Processor._EditorDrawBones)
                        Processor.DrawSetupGizmos();
            }

            Processor.DrawGizmos();
        }
#endif


        bool wasDisabled = false;
        private void OnDisable()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
#endif
                wasDisabled = true;

                if (Parameters.RagdollDummyBase)
                {
                    Parameters.RagdollDummyBase.gameObject.SetActive(false);
                }

#if UNITY_EDITOR
            }
#endif
        }

        int onEnableCount = 0;
        private void OnEnable()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
#endif
            {
                onEnableCount += 1;
                if (onEnableCount > 1) Start(); // When called always onEnable, it was breaking dismemberement feature!

                if( wasDisabled )
                {
                    wasDisabled = false;

                    if( Parameters.RagdollDummyBase )
                    {
                        var c = Parameters.GetPelvisBone();

                        while( c != null )
                        {
                            if( c.visibleBone ) c.visibleBone.localRotation = c.initialLocalRotation;
                            if( c.transform ) c.transform.localRotation = c.initialLocalRotation;
                            c = c.child;
                        }

                        Parameters.RagdollDummyBase.gameObject.SetActive( true );
                    }

                    if( Parameters.Initialized )
                    {
                        //if (rag.enabled)
                        //{
                        //    rag.enabled = false;
                        //    rag.Parameters.RagdollDummyRoot.gameObject.SetActive(false);
                        //}
                        Parameters.User_PoseAsInitalPose();

                        //rag.enabled = true;
                        Parameters.RagdollDummyBase.gameObject.SetActive( true );
                        //rag.Parameters.User_PoseAsAnimator();
                        Parameters.AlignRagdollBonesWithAnimatorBones();
                    }
                
                }
            }
        }

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                if (Processor == null) Processor = new RagdollProcessor();
                Parameters.SwitchAllExtendedAnimatorSync(Parameters.ExtendedAnimatorSync);
            }
        }


        public RagdollProcessor.EBaseTransformRepose _initialReposeMode { get; set; }


        #endregion


        #region Auto Destroy Reference

        private void OnDestroy()
        {
            if (autoDestroy != null) autoDestroy.StartChecking();
        }

        private RagdollAutoDestroy autoDestroy = null;
        private class RagdollAutoDestroy : MonoBehaviour
        {
            public GameObject Parent;
            public void StartChecking() { Check(); if (Parent != null) InvokeRepeating("Check", 0.05f, 0.5f); }
            void Check() { if (Parent == null) Destroy(gameObject); }
        }

        #endregion


        #region Extra Debugging Classes

#if UNITY_EDITOR

        public FDebug_PerformanceTest _Editor_Perf_Update = new FDebug_PerformanceTest();
        public FDebug_PerformanceTest _Editor_Perf_LateUpdate = new FDebug_PerformanceTest();
        public FDebug_PerformanceTest _Editor_Perf_FixedUpdate = new FDebug_PerformanceTest();

        void _Debug_Perf_MeasureUpdate(bool start) { _Debug_Perf_DoMeasure(_Editor_Perf_Update, start); }
        void _Debug_Perf_MeasureLateUpdate(bool start) { _Debug_Perf_DoMeasure(_Editor_Perf_LateUpdate, start); }
        void _Debug_Perf_MeasureFixedUpdate(bool start) { _Debug_Perf_DoMeasure(_Editor_Perf_FixedUpdate, start); }
        void _Debug_Perf_DoMeasure(FDebug_PerformanceTest test, bool start) { if (start) test.Start(gameObject, false); else test.Finish(); }

#endif

        #endregion


    }
}