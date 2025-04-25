using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UIBox
{
    public class UIBoxUIController : MonoBehaviour {

        [SerializeField] private UIBoxController _boxController;
        [SerializeField] private List<ScriptableObject> _uiBehaviours;
        [SerializeField] private List<ScriptableObject> _uiConfigs;

        [SerializeField] private Canvas _UICanvas;
        
      
        private List<UIBehaviour> _activeBehaviours;
        private static EventSystem eventSystem;

        private GameObject _uiElement;

        private UIBoxUITypeState _currentTypeState;

        private void Awake()
        {
            // Initialize the list of active behaviours
            _activeBehaviours = _uiBehaviours.OfType<UIBehaviour>().ToList();

            eventSystem = EventSystem.current;
            _currentTypeState = new EmptyTypeState(); // Default state
        }

        private void Start()
        {
            foreach (var behaviour in _activeBehaviours)
            {
                behaviour.Initialize(this);
            }
            _currentTypeState.Enter(this);
        }

        private void Update()
        {
            foreach (var behaviour in _activeBehaviours)
            {
                behaviour.Process(this);
            }
        }

        public void AddButtonToCanvas()
        { 

        }

        public void ChangeTypeState(UIBoxUITypeState newState)
        {
            _currentTypeState?.Exit(this);
            _currentTypeState = newState;
            _currentTypeState.Enter(this);
        }
    }

    #region UIBox Type States
    public interface UIBoxUITypeState
    { 
        void Enter(UIBoxUIController controller);
        void Update(UIBoxUIController controller);
        void Exit(UIBoxUIController controller);
    }

    #region Button Type State
    public class ButtonTypeState : UIBoxUITypeState
    {

        public void Enter(UIBoxUIController controller)
        {
            // Initialize button state
            
        }
        public void Update(UIBoxUIController controller)
        {
            // Update button state
        }
        public void Exit(UIBoxUIController controller)
        {
            // Cleanup button state
        }
    }
    #endregion

    #region Empty Type State
    public class EmptyTypeState : UIBoxUITypeState
    { // An empty "dummy" state 
        public void Enter(UIBoxUIController controller) { }
        public void Update(UIBoxUIController controller) { }
        public void Exit(UIBoxUIController controller) { }
    }
    #endregion

    #region Slider Type State
    public class SliderTypeState : UIBoxUITypeState
    {
        public void Enter(UIBoxUIController controller)
        {
            // Initialize slider state
        }
        public void Update(UIBoxUIController controller)
        {
            // Update slider state
        }
        public void Exit(UIBoxUIController controller)
        {
            // Cleanup slider state
        }
    }
    #endregion

    #region Scrollbar Type State
    public class ScrollbarTypeState : UIBoxUITypeState
    {
        public void Enter(UIBoxUIController controller)
        {
            // Initialize scrollbar state
        }
        public void Update(UIBoxUIController controller)
        {
            // Update scrollbar state
        }
        public void Exit(UIBoxUIController controller)
        {
            // Cleanup scrollbar state
        }
    }
    #endregion

    #endregion

}