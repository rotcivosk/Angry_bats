using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Monster : MonoBehaviour
{
    [SerializeField] Sprite _deadSprite;
    [SerializeField] ParticleSystem _particles;
    [SerializeField] GameObject _deadparticles;

    bool _hasDied;

    // Vari�veis para movimenta��o
    [SerializeField] float speed = 2f; // Velocidade do movimento
    private Vector2 direction; // Dire��o do movimento (pode ser ajustada no Inspector)
    [SerializeField] float distance = 5f; // Dist�ncia m�xima para o movimento
    [SerializeField] float angle = 0f;
    private float elapsedTime = 0f;
    [SerializeField] float moveTime = 2f;

    private Vector2 _startPosition; // Posi��o inicial do monstro
    private bool movingForward = true; // Indica se o monstro est� indo ou voltando

    void Start()
    {
        // Converte o �ngulo em radianos e calcula a dire��o
        float radians = angle * Mathf.Deg2Rad;
        direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
    }

    void Update()
    {
        if (!_hasDied)
        {
            elapsedTime += Time.deltaTime;

            // Muda de dire��o ap�s o tempo de movimento definido
            if (elapsedTime >= moveTime)
            {
                movingForward = !movingForward;
                elapsedTime = 0f; // Reseta o tempo para o ciclo de retorno
            }

            MoveMonster();
        }
    }

    void MoveMonster()
    {
        // Move o monstro na dire��o determinada ou na dire��o oposta ap�s o tempo
        Vector2 moveDir = movingForward ? direction : -direction;
        transform.Translate(moveDir * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (ShouldDieFromCollision(collision))
        {
            StartCoroutine(Die());
        }
    }

    bool ShouldDieFromCollision(Collision2D collision)
    {
        if (_hasDied)
            return false;

        BirdControl bird = collision.gameObject.GetComponent<BirdControl>();
        if (bird != null)
            return true;

        if (collision.contacts[0].normal.y < -0.5)
            return true;

        return false;
    }

    IEnumerator Die()
    {
        _hasDied = true;
        GetComponent<SpriteRenderer>().sprite = _deadSprite;
        GetComponent<Rigidbody2D>().gravityScale = 1;
        _particles.Play();
        yield return new WaitForSeconds(2);
        Instantiate(_deadparticles, this.transform.position, Quaternion.identity);
        gameObject.SetActive(false);

    }
}