using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdControl : MonoBehaviour
{
    [SerializeField] private float _lauchforce;
    [SerializeField] private float _maxDragDistance;
    [SerializeField] Sprite _lauchSprite;
    [SerializeField] Sprite _idleSprite;

    private SpriteRenderer _sprite;
    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider2D;
    [SerializeField] TrailRenderer _trailRenderer; // Adiciona referência ao TrailRenderer
    private Vector2 _startPosition;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
      //  _trailRenderer = GetComponent<TrailRenderer>(); // Inicializa o TrailRenderer
    }

    void Start()
    {
        _startPosition = _rigidbody2D.position;
        _rigidbody2D.isKinematic = true;
    }

    private void OnMouseDown()
    {
        _sprite.color = Color.red;  
    }

    private void OnMouseUp()
    {
        Vector2 currentPosition = _rigidbody2D.position;
        Vector2 direction = _startPosition - currentPosition;
        direction.Normalize();

        _rigidbody2D.isKinematic = false;
        _rigidbody2D.AddForce(direction * _lauchforce);

        _sprite.color = Color.white;
        GetComponent<SpriteRenderer>().sprite = _lauchSprite;
        _rigidbody2D.constraints = RigidbodyConstraints2D.None;

        // Ativa o TrailRenderer ao lançar o pássaro
        _trailRenderer.enabled = true;
    }

    private void OnMouseDrag()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 desiredPosition = mousePosition;

        float distance = Vector2.Distance(desiredPosition, _startPosition);

        if (distance > _maxDragDistance)
        {
            Vector2 direction = desiredPosition - _startPosition;
            direction.Normalize();
            desiredPosition = _startPosition + (direction * _maxDragDistance);
        }

        if (desiredPosition.x > _startPosition.x)
        {
            desiredPosition.x = _startPosition.x;
        }

        _rigidbody2D.position = desiredPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Colidiu");

        // Esconde o pássaro, desativa o collider e desativa o trail
        _sprite.enabled = false;
        _collider2D.enabled = false;
        _trailRenderer.enabled = false; // Desativa o TrailRenderer

        StartCoroutine(ResetAfterDelay());
    }

    private IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(3);

        // Reaparece o pássaro, reativa o collider e o trail ao voltar para o início
        _sprite.enabled = true;
        _collider2D.enabled = true;
        _trailRenderer.enabled = false; // TrailRenderer começa desativado até o próximo lançamento

        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        _rigidbody2D.rotation = 0f;
        GetComponent<SpriteRenderer>().sprite = _idleSprite;
        _rigidbody2D.position = _startPosition;
        _rigidbody2D.isKinematic = true;
        _rigidbody2D.velocity = Vector2.zero;
    }
}
