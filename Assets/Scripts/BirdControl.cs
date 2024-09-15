using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdControl : MonoBehaviour
{
    [SerializeField] private float _lauchforce;
    [SerializeField] private float _maxDragDistance;
    [SerializeField] Sprite _lauchSprite;
    [SerializeField] Sprite _idleSprite;
    //[SerializeField] private ParticleSystem _particleSystem;

    private SpriteRenderer _sprite;
    private Rigidbody2D _rigidbody2D;
    private Vector2 _startPosition;



    // Start is called before the first frame update
    private void Awake(){
        _sprite = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    
    void Start()
    {
        _startPosition = _rigidbody2D.position;
        _rigidbody2D.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnMouseDown(){
        _sprite.color = Color.red;  //Mudanca de cor do passaro, feedback visual
    }

    private void OnMouseUp(){
        Vector2 currentPosition = _rigidbody2D.position;
        Vector2 direction = _startPosition - currentPosition;
        direction.Normalize();

        _rigidbody2D.isKinematic = false;
        _rigidbody2D.AddForce(direction * _lauchforce);

        //_particleSystem.Play();
        _sprite.color = Color.white;
         GetComponent<SpriteRenderer>().sprite = _lauchSprite;
        _rigidbody2D.constraints = RigidbodyConstraints2D.None;
    }

    private void OnMouseDrag(){
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

        var guideDirection = _startPosition - desiredPosition;
        guideDirection.Normalize();

        _rigidbody2D.position = desiredPosition;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Colidiu");

        StartCoroutine(ResetAfterDelay());
        //_particleSystem.Pause();

    }

    private IEnumerator ResetAfterDelay()
    {

        yield return new WaitForSeconds(3);
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        _rigidbody2D.rotation = 0f;
        GetComponent<SpriteRenderer>().sprite = _idleSprite;
        _rigidbody2D.position = _startPosition;
        _rigidbody2D.isKinematic = true;
        _rigidbody2D.velocity = Vector2.zero;
        //_particleSystem.Clear();
    }
}
