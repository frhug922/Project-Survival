using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractionHandler : MonoBehaviour
{
    [SerializeField] private float _interactRange = 1.5f;
    [SerializeField] private LayerMask _interactableLayer;
    [SerializeField] private FarmingUIController _uiController;

    private IInteractable _currentInteractable;
    private Collider2D[] _currentHits;
    private float _holdTime;
    private float _holdThreshold = 1.0f;
    private Transform _nearestTransform;
    private IInteractable _previousInteractable;

    private void Update()
    {
        DetectInteractable();

        if (_currentInteractable != null)
        {
            if (_currentInteractable != _previousInteractable)
            {
                if (_previousInteractable != null)
                    (_previousInteractable as InteractableObjectAdapter)?.SetNameLabelVisible(false);

                (_currentInteractable as InteractableObjectAdapter)?.SetNameLabelVisible(true);
                _previousInteractable = _currentInteractable;
            }

            if (_currentInteractable is InteractableObjectAdapter adapter)
                adapter.SetNameLabelVisible(true);

            _uiController.Show(_currentInteractable.GetDescription(), _nearestTransform);

            InteractableObjectAdapter currentAdapter = _currentInteractable as InteractableObjectAdapter;

            // Terminal은 키 누른 순간에만 실행 (중복 방지)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (currentAdapter != null &&
                    currentAdapter.InteractionTypeValue == InteractableObjectAdapter.InteractionType.Terminal)
                {
                    _currentInteractable.Interact();
                    return;
                }
                else if (currentAdapter != null && currentAdapter.InteractionTypeValue == InteractableObjectAdapter.InteractionType.Container && GameManager.Instance.IsUIOpen == true)
                {
                    _currentInteractable.Interact();
                }
            }



            // 나머지는 Hold로 진행
            if (Input.GetKey(KeyCode.Space))
            {
                _holdTime += Time.deltaTime;
                currentAdapter?.ShowProgressBar(_holdTime / _holdThreshold);

                if (_holdTime >= _holdThreshold)
                {
                    _currentInteractable?.Interact();
                    _holdTime = 0f;
                    currentAdapter?.HideProgressBar();
                }
            }
            else
            {
                _holdTime = 0f;
                currentAdapter?.HideProgressBar();
            }
        }
        else
        {
            _uiController.Hide();
            _holdTime = 0f;
            (_previousInteractable as InteractableObjectAdapter)?.HideProgressBar();
        }
    }

    private void DetectInteractable()
    {
        _currentHits = Physics2D.OverlapCircleAll(transform.position, _interactRange, _interactableLayer);

        if (_currentHits.Length == 0)
        {
            if (_currentInteractable is InteractableObjectAdapter oldAdapter)
                oldAdapter.SetNameLabelVisible(false);

            foreach (var obj in FindObjectsOfType<InteractableObjectAdapter>())
                obj.SetOutline(false);

            _currentInteractable = null;
            _nearestTransform = null;
            return;
        }

        var sortedHits = _currentHits
            .OrderBy(hit => Vector2.Distance(transform.position, hit.transform.position))
            .ToList();

        foreach (var obj in FindObjectsOfType<InteractableObjectAdapter>())
            obj.SetOutline(false);

        foreach (var hit in sortedHits)
        {
            var adapter = hit.GetComponent<InteractableObjectAdapter>();
            if (adapter != null && adapter.InteractionTypeValue == InteractableObjectAdapter.InteractionType.FieldDrop)
                adapter.SetOutline(true);
        }

        foreach (var hit in sortedHits)
        {
            var adapter = hit.GetComponent<InteractableObjectAdapter>();
            if (adapter != null &&
                adapter.InteractionTypeValue != InteractableObjectAdapter.InteractionType.FieldDrop)
            {
                adapter.SetOutline(true);
                break;
            }
        }

        var nearest = sortedHits[0];
        _currentInteractable = nearest.GetComponent<IInteractable>();
        _nearestTransform = nearest.transform;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _interactRange);
    }
}
