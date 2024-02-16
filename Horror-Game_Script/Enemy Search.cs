using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySearch : MonoBehaviour
{
    [SerializeField] private float searchAngle = 130f;  //���E�͈̔�
    [SerializeField] private GameObject enemy;
    [SerializeField] private SphereCollider searchErea;
    public LastBossController lastBossController;
    private bool canRun;
    private int count;
    private GameObject target;  //�v���C���[���i�[
    private bool isChase;
    private GameObject player;
    private float countDown;
    private bool chaseFlag;  //true:�ǐՒ��@false:�ǐՂ𒆎~
    private void Update()
    {
        if (isChase)
        {
            enemy.GetComponent<EnemyController>().Dash(target);
            searchErea.radius = 5f;
            if (chaseFlag)
            {
                countDown += Time.deltaTime;
                if (countDown > 5)
                {
                    enemy.GetComponent<EnemyController>().canWalk = true;
                    searchErea.radius = 3.5f;
                    countDown = 0;
                    isChase = false;
                    chaseFlag = false;
                }
            }
            //searchErea.radius = 5;
        }
        if (canRun && count == 0)
        {
            //Camera.main.GetComponent<CameraController>().runAudio.Play();
            count = 1;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            /*
            Camera.main.GetComponent<CameraController>().searchAudio.Stop();
            Camera.main.GetComponent<CameraController>().searchAudio.volume = 0.05f;
            Camera.main.GetComponent<CameraController>().heartAudio.Play();
            */
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            var playerDirection = other.transform.position - transform.position;
            var angle = Vector3.Angle(transform.forward, playerDirection);
            target = other.gameObject;
            if(angle <= searchAngle)
            {
                enemy.GetComponent<EnemyController>().canWalk = false;
                isChase = true;  //�ǐՂ��n�߂�
                //�v���C���[�̕���������
                Quaternion lookRotation = Quaternion.LookRotation(other.transform.position - transform.position, Vector3.up);
                lookRotation.z = 0;
                lookRotation.x = 0;
                enemy.transform.rotation = Quaternion.Lerp(enemy.transform.rotation, lookRotation, 0.05f);
                //Camera.main.GetComponent<CameraController>().searchAudio.Stop();
                canRun = true;
                if (enemy.tag == "Boss")
                {
                    float position = other.transform.position.magnitude - transform.position.magnitude;
                    enemy.GetComponent<LastBossController>().Attack(position);
                    //Debug.Log(position);
                }
            }
            if (target.GetComponent<PlayerController>().crouchFlag)
            {
                ;
            }
            else if(target.GetComponent<PlayerController>().crouchFlag == false)
            {
                //�v���C���[�̕���������
                Quaternion lookRotation = Quaternion.LookRotation(other.transform.position - transform.position, Vector3.up);
                lookRotation.z = 0;
                lookRotation.x = 0;
                enemy.transform.rotation = Quaternion.Lerp(enemy.transform.rotation, lookRotation, 0.005f);

            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            chaseFlag = true;
            player = other.gameObject;

        }


        //isChase = false;  //�ǐՂ���߂�
        //Camera.main.GetComponent<CameraController>().runAudio.Stop();
        //Camera.main.GetComponent<CameraController>().searchAudio.Play();
        /*
        Camera.main.GetComponent<CameraController>().runAudio.Stop();
        */
    }
    /*
    [SerializeField] private GameObject enemy;
    [SerializeField] private SphereCollider searchErea;
    private EnemyController enemyController;

    // Start is called before the first frame update
    void Start()
    {
        enemyController = enemy.GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            //�@��l���̕���
            var playerDirection = other.transform.position - transform.position;
            //�@�G�̑O������̎�l���̕���
            var angle = Vector3.Angle(transform.forward, playerDirection);
            //�@�T�[�`����p�x���������甭��
            if (angle <= searchAngle)
            {
                Debug.Log("OK");
                Quaternion lookRotation = Quaternion.LookRotation(other.transform.position - enemy.transform.position, Vector3.up);
                lookRotation.z = 0;
                lookRotation.x = 0;
                enemy.transform.rotation = Quaternion.Lerp(enemy.transform.rotation, lookRotation, 0.05f);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            ;
        }
    }
    */
}
