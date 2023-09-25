using UnityEngine;
using DG.Tweening;
using KanKikuchi.AudioManager;

public class LifeUpItem : MonoBehaviour,GetItemIo
{
    public Rigidbody2D m_Rigidbody;

    [SerializeField]
    const int _liferPoint=1;

    [SerializeField]
    private float m_Thrust;

    private bool targetbool;
    private GameObject target;
    public float speed = 5f;

    STG_PlayerShip playerShip;

    STG_Score _score;

    [SerializeField]
    SEAssistant sEAssistant;

    public bool MoveItem { get; set; } = false;
    void Awake()
    {
        if (GameObject.FindWithTag("Player") != null)
        {
            target = GameObject.FindWithTag("Player");
        }
        playerShip = GameObject.FindWithTag("Player").GetComponent<STG_PlayerShip>();
    }
    void Start()
    {
        targetbool = false;
        transform.DORotate(new Vector3(0, 0, -360), 0.3f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLink(gameObject);
        int rnd = Random.Range(5, 15);
        m_Rigidbody.AddForce(transform.up * (m_Thrust + rnd));

    }

    private void FixedUpdate()
    {
        if (targetbool == true)
        {
            if (target != null)
            {
                transform.position = Vector3.MoveTowards(
              transform.position,
              target.transform.position,
              speed * Time.deltaTime);
            }
            else
            {
                targetbool = false;
            }
            
        }
        if (MoveItem == true)
        {
            transform.position = Vector3.MoveTowards(
              transform.position,
              target.transform.position,
              speed * 2f * Time.deltaTime);
        }

    }
    public void GetUseAbility()
    {
        sEAssistant.Play();
        //Debug.Log("ライフアップ");
        switch (playerShip._lifePoint)
        {
            case 3:
                _score = GameObject.Find("ScoreGroup").GetComponent<STG_Score>();
                _score.AddPoint(500);
                Debug.Log("ライフポイント :" + playerShip._lifePoint);
                Destroy(this.gameObject);
                break;
            case 2:
                playerShip._lifeObj[2].SetActive(true);
                playerShip._lifePoint += _liferPoint;
                Debug.Log("ライフポイント :" + playerShip._lifePoint);
                Destroy(this.gameObject);
                break;
            case 1:
                playerShip._lifeObj[1].SetActive(true);
                playerShip._lifePoint += _liferPoint;
                Debug.Log("ライフポイント :" + playerShip._lifePoint);
                Destroy(this.gameObject);
                break;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.CompareTag("ItemDestroy"))
        {
            //Debug.Log("ItemDes");
            Destroy(this.gameObject);
        }
        if (c.gameObject.CompareTag("CircleVacuume"))
        {
            if (targetbool == true) return;
            if (target != null)
            {
                target = GameObject.FindWithTag("Player");
                m_Rigidbody.gravityScale = 0;
                targetbool = true;
            }
        }
    }
}
