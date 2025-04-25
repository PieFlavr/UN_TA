using System.Collections.Generic;

using UnityEngine;
using TMPro;
using System.Linq;

namespace Assets.Scripts.UIBox
{
    public class UIBoxController : MonoBehaviour
    {
        public enum UIBoxStateType { Idle, Morphing, Kinematic }

        #region Inspector References
        [SerializeField] private Canvas _UICanvas;
        [SerializeField] private GameObject _box;
        [SerializeField] private List<ScriptableObject> _scriptableObjects;
        [SerializeField] private UIBoxStateType _initialState = UIBoxStateType.Idle;
        #endregion

        #region Component References
        private Transform _thisTransform;
        private Transform _canvasTransform;
        private TextMeshProUGUI _label;
        private Transform _boxTransform;
        private Rigidbody _boxRb;
        private Collider _boxCollider;
        private RectTransform _labelRectTransform;
        #endregion 

        #region Internal State
        private List<UIBoxEffect> _effects;
        [SerializeField] private float _morphSpeed = 1.0f;
        [SerializeField] private string _text = "";
        [SerializeField] private Vector3 _textOffset = Vector3.zero;
        private UIBoxState _currentState;
        #endregion

        #region Morphing Properties
        [SerializeField] private Vector3 _targetPosition = Vector3.zero;
        [SerializeField] private Vector3 _targetScale = Vector3.one;
        #endregion

        #region Initialization Functions
        public void Init(Vector3 position, Vector3 scale, float morphSpeed, string text = "", UIBoxState initialState = null)
        {
            _targetPosition = position;
            _targetScale = scale;
            _morphSpeed = morphSpeed;
            _text = text;
            _currentState = initialState ?? new IdleState();
            _currentState.Enter(this);
        }

        #endregion

        void Awake()
        {
            _thisTransform = this.GetComponent<Transform>();

            #region Box Initialization
            if (_box == null)
            {
                Debug.LogError("Box GameObject is not assigned in the inspector.");
            }
            else
            {
                _boxTransform = _box.GetComponent<Transform>();
                _boxRb = _box.GetComponent<Rigidbody>();
                _boxCollider = _box.GetComponent<Collider>();
            }
            #endregion

            #region Canvas Initialization
            if (_UICanvas == null)
            {
                Debug.LogError("UI Canvas is not assigned in the inspector.");
            }
            else
            {
                _label = _UICanvas.GetComponentInChildren<TextMeshProUGUI>();
                _labelRectTransform = _label.GetComponent<RectTransform>();
                _canvasTransform = _UICanvas.GetComponent<Transform>();
            }
            #endregion

            #region Effect Initialization
            _effects = _scriptableObjects.OfType<UIBoxEffect>().ToList();
            #endregion

            #region State Initialization
            switch (_initialState)
            {
                case UIBoxStateType.Idle:
                    _currentState = new IdleState();
                    break;
                case UIBoxStateType.Morphing:
                    _currentState = new MorphingState();
                    break;
                case UIBoxStateType.Kinematic:
                    _currentState = new KinematicState();
                    break;
            }
            _currentState.Enter(this);
            #endregion
        }

        void Update()
        {
            _currentState.Update(this);

            if (_effects != null)
            {
                foreach (var effect in _effects)
                {
                    effect.Apply(this);
                    effect.Process(this);
                }
            }
            if (_label != null)
            {
                _label.text = _text;
            }

            SyncBoxState();
        }

        #region Internal Functions
        private void SyncBoxState()
        {
            if (_boxRb == null || _boxCollider == null)
            {
                Debug.LogError("Box Rigidbody or Collider is not assigned in the inspector.");
                return;
            }
            _labelRectTransform.localPosition = _textOffset - new Vector3(0, 0, _boxTransform.localScale.z);
            _labelRectTransform.sizeDelta = new Vector2(_boxTransform.localScale.x, _boxTransform.localScale.y);
        }
        #endregion

        #region External Interface
        public void ChangeState(UIBoxState newState)
        {
            _currentState.Exit(this);
            _currentState = newState;
            _currentState.Enter(this);
        }

        public void AddEffect(UIBoxEffect effect)
        {
            if (effect != null)
            {
                _effects.Add(effect);
            }
            else
            {
                Debug.LogError("Effect is null.");
            }
        }

        public void LerpToTarget()
        {
            _thisTransform.position = Vector3.Lerp(_thisTransform.position, _targetPosition, Time.deltaTime * _morphSpeed);
            _boxTransform.localScale = Vector3.Lerp(_boxTransform.localScale, _targetScale, Time.deltaTime * _morphSpeed);
        }
        #endregion

        #region Getters and Setters
        public void setKinematic(bool isKinematic)
        {
            if (_boxRb != null)
            {
                _boxRb.isKinematic = isKinematic;
            }
            else
            {
                Debug.LogError("Rigidbody is not assigned in the inspector.");
            }
        }

        public void setCollision(bool isEnabled)
        {
            if (_boxCollider != null)
            {
                _boxCollider.enabled = isEnabled;
            }
            else
            {
                Debug.LogError("Collider is not assigned in the inspector.");
            }
        }

        public void SetText(string text)
        {
            _text = text;
            if (_label != null)
            {
                _label.text = _text;
            }
            else
            {
                Debug.LogError("Label is not assigned in the inspector.");
            }
        }

        public float MorphSpeed { get => _morphSpeed; set => _morphSpeed = value; }
        #endregion
    }

    #region UIBox State Implementations

    #region State Interface
    public interface UIBoxState
    {
        void Enter(UIBoxController controller);
        void Update(UIBoxController controller);
        void Exit(UIBoxController controller);
    }
    #endregion

    #region Idle State
    public class IdleState : UIBoxState
    {
        public void Enter(UIBoxController controller)
        {
            Debug.Log("Entering Idle State");
        }

        public void Update(UIBoxController controller)
        {
            // Idle state logic
        }

        public void Exit(UIBoxController controller)
        {
            Debug.Log("Exiting Idle State");
        }
    }
    #endregion

    #region Morphing State
    public class MorphingState : UIBoxState
    {
        public void Enter(UIBoxController controller)
        {
            Debug.Log("Entering Morphing State");
            controller.setKinematic(false);
            controller.setCollision(false);
        }

        public void Update(UIBoxController controller)
        {
            Debug.Log("Morphing State Update");
            controller.LerpToTarget();
        }

        public void Exit(UIBoxController controller)
        {
            Debug.Log("Exiting Morphing State");
        }
    }
    #endregion

    #region Kinematic State
    public class KinematicState : UIBoxState
    {
        public void Enter(UIBoxController controller)
        {
            Debug.Log("Entering Kinematic State");
            controller.setKinematic(true);
            controller.setCollision(true);

        }

        public void Update(UIBoxController controller)
        {
            // Kinematic state logic
        }

        public void Exit(UIBoxController controller)
        {
            Debug.Log("Exiting Kinematic State");
            controller.setKinematic(false);
            controller.setCollision(false);
        }
    }
    #endregion

    #endregion
}

