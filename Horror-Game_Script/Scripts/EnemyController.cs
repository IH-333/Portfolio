using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public class EnemyController : MonoBehaviour
{
    private NavMeshAgent nav;
    
    //  �p�����[�^�[  //
    public int walkSpeed = 3;  //�p�j���̃X�s�[�h
    public int dashSpeed = 6;  //�ǐՎ��̃X�s�[�h

    //  Walk�֐�  //
    public bool canWalk = true;  //true:Walk�֐������s����Ă���@false:Walk�֐������s����Ă��Ȃ�
    public List<GameObject> enemyPoint = new List<GameObject>();   //�p�j�n�_
    public int pointValue;  //�p�j�n�_���w�肷��ϐ�

    //  Door�֐�  //
    private DoorController doorController;  //�h�A�̃X�N���v�g���i�[
    private LockController lockController;  //���t���h�A�̃X�N���v�g���i�[
    private bool stopFlag;  //�G�̑��x��0�ɂ��Ē�~������
    public bool canDoor;  //ture:DoorMove�����s�\�@false:DoorMove�����s�s�\
    private float countDwon;  //���Ԍv���֐�

    /*
    private NavMeshAgent nav;  //AI Naigation���i�[
    private Animator anime;

    //  �p�j�ϐ�  //
    //�p�j����n�_���i�[
    public List<GameObject> enemyPoint = new List<GameObject>();
    //�p�j����n�_���w�肷��ϐ�
    public int pointValue;

    public bool canWalk;
    private bool canDoor;
    private float countDown;

    public int hitPoint;  //Colony�̓G�݂̗̂v�f

    */

    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();

        pointValue = Random.Range(0, enemyPoint.Count);  //�n�_�������_���őI�o
    }
    private void Update()
    {
        if (canWalk) Walk();
        Door();
    }
    public void Walk()  //�p�j�֐�
    {
        nav.speed = walkSpeed;  //�ړ����鑬����ݒ�
        //�n�_�̈ʒu���擾
        Vector3 targetPoint = enemyPoint[pointValue].transform.position;
        //�n�_�Ɍ������Ĉړ�����
        nav.destination = new Vector3(targetPoint.x, targetPoint.y, targetPoint.z);
    }
    public void Dash(GameObject target)
    {
        nav.speed = dashSpeed;  //�ړ����鑁����ݒ�
        //�v���C���[�Ɍ������Ĉړ�����
        nav.destination = target.transform.position;

    }
    public void Door()  //�h�A�֐�
    {
        //�h�A�ɑ΂��鏈��
        Vector3 MoveForward = Vector3.Scale(transform.forward, new Vector3(1, 1, 1)).normalized;  //�O�����擾
        Ray enemyRay = new Ray(transform.position + new Vector3(0, 2, 0), MoveForward);  //Ray�𔭐�
        //Debug.DrawRay(enemyRay.origin, enemyRay.direction * 3f, Color.red, 3);  //Ray������
        RaycastHit enemyHit;
        Physics.Raycast(enemyRay, out enemyHit, 2f);  //Ray�ɔ����t�^
        if(enemyHit.collider != null)
        {
            if(enemyHit.collider.tag == "Door")
            {
                doorController = enemyHit.collider.GetComponent<DoorController>();  //�h�A�̃X�N���v�g���擾
                if (doorController.lockType == false) pointValue = Random.Range(0, enemyPoint.Count);
                else if (doorController.lockType && stopFlag == false)
                {
                    //doorController.doorMove = true;
                    doorController.canF = true;
                    doorController.DoorMove();
                    stopFlag = true;
                }
            }
            if(enemyHit.collider.tag == "Lock")
            {
                lockController = enemyHit.collider.GetComponent<LockController>();  //���t���h�A�̃X�N���v�g���擾
                if (lockController.lockType == false) pointValue = Random.Range(0, enemyPoint.Count);
                else if (lockController.lockType && canDoor)
                {
                    doorController.canF = true;
                    //lockController.doorMove = true;
                    lockController.DoorMove();
                    stopFlag = true;
                }
            }
        }
        if (stopFlag)
        {
            nav.speed = 0;  //�ړ����x��0��
            countDwon += Time.deltaTime;  //���Ԍv���J�n
            if(countDwon > 3)  //3�b��Ɉړ��J�n
            {
                nav.speed = walkSpeed;
                countDwon = 0;
                if (doorController) doorController.canF = false;
                if (lockController) lockController.canF = false;
                stopFlag = false;
            }
        }
        

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<PlayerController>().lifeSlider.value = 0;
        }
    }
    /*
    private void Start()
    {
        if (gameObject.tag == "Boss"  || gameObject.tag == "Colony") anime = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        canWalk = true;
        nav.speed = walkSpeed;  //�ړ��X�s�[�h��p�j���̃X�s�[�h�ɂ���
        pointValue = Random.Range(0, enemyPoint.Count);  //�n�_�������_���őI�o
    }
    private void Update()
    {
        if (canWalk)
        {
            if (gameObject.tag == "Boss")
            {
                anime.SetFloat("Dash", 1);
                anime.SetBool("Walk", true);
            }
            Walk();
        }
        if(gameObject.tag == "Colony")
        {
            int num = Random.Range(0, 10) + 1;
            if (num < 5) anime.SetBool("Jump", true);
            else anime.SetBool("Jump", false);
            if (hitPoint == 20) Destroy(gameObject);

        }
    }
    public void Walk()  //�p�j�֐�
    {
        nav.speed = walkSpeed;
        if(gameObject.tag == "Enemy")
        {
            //�n�_�̈ʒu���擾
            Vector3 targetPoint = enemyPoint[pointValue].transform.position;
            //�n�_�Ɍ������Ĉړ�����
            nav.destination = new Vector3(targetPoint.x, targetPoint.y, targetPoint.z);

        }

        //�h�A�ɑ΂��鏈��
        Vector3 MoveForward = Vector3.Scale(transform.forward, new Vector3(1, 1, 1)).normalized;
        Ray enemyRay = new Ray(transform.position + new Vector3(0, 2, 0), MoveForward);
        //Debug.DrawRay(enemyRay.origin, enemyRay.direction * 3f, Color.red, 3);
        RaycastHit enemyHit;
        Physics.Raycast(enemyRay, out enemyHit, 3f);
        if(enemyHit.collider != null && (enemyHit.collider.tag == "Door" || enemyHit.collider.tag == "Lock"))
        {
            if(enemyHit.collider.tag == "Door") doorController = enemyHit.collider.GetComponent<DoorController>();  //�h�A�̃X�N���v�g���擾
            if(enemyHit.collider.tag == "Lock") lockController = enemyHit.collider.GetComponent<LockController>();  //���t���h�A�̃X�N���v�g���擾
            //  �h�A�Ɍ����������Ă�����ʂ̏ꏊ�Ɉړ�
            if (doorController.lockType == false || lockController.lockType == false) pointValue = Random.Range(0, enemyPoint.Count);
            //  �h�A�Ɍ����������Ă��Ȃ������Ƃ��̏���
            else if(doorController.lockType || lockController.lockType)
            {
                nav.speed = 0;  //�ړ����x��0�ɂ���
                countDown += Time.deltaTime;  //���Ԍv��
                //3�b��Ƀh�A���J����
                if(countDown > 3)
                {
                    //�h�A���J���鏈��
                    if (doorController.doorMove == false)
                    {
                        doorController.doorMove = true;
                        doorController.DoorMove();
                    }
                    //���t���h�A���J���鏈��
                    if (lockController.doorMove == false)
                    {
                        lockController.doorMove = true;
                        lockController.DoorMove();
                    }
                    nav.speed = walkSpeed;
                }
            }
            //  �h�A�����Ă����班���~�܂��ăh�A���J���Ă���ړ�
        }



















        /*
        if (enemyHit.collider != null && enemyHit.collider.tag == "Door" && enemyHit.collider.GetComponent<DoorController>().doorMove == false)
        {
            nav.speed = 0;
            countDown += Time.deltaTime;
            if (countDown > 3)
            {
                nav.speed = walkSpeed;
                enemyHit.collider.GetComponent<DoorController>().DoorMove();
            }
        }

        if (enemyHit.collider != null && enemyHit.collider.tag == "Lock")
        {
            if (enemyHit.collider.GetComponent<LockController>().lockType == false)
            {
                pointValue = Random.Range(0, enemyPoint.Count);
            }
            else
            {
                enemyHit.collider.GetComponent<LockController>().DoorMove();
            }

        }
    }
    public void Dash(GameObject target)  
    {
        if(gameObject.tag == "Boss")
        {
            anime.SetFloat("Dash", 10);
        }
        nav.destination = target.transform.position;
        nav.speed = dashSpeed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (gameObject.tag == "Enemy") other.GetComponent<PlayerController>().lifeSlider.value = 0;
            else if (gameObject.tag == "Colony")
            {
                other.GetComponent<PlayerController>().lifeSlider.value -= 5;
                other.GetComponent<Rigidbody>().AddForce(transform.forward * -2, ForceMode.Impulse);
            }
            else if(gameObject.tag == "Boss")
            {
                other.GetComponent<PlayerController>().lifeSlider.value -= 10;
            }

        }
    }



    /*
    [SerializeField] private int walkSpeed = 5;  //�p�j���̃X�s�[�h
    [SerializeField] private int dashSpeed = 10;  //�ǐՎ��̃X�s�[�h
    [SerializeField] private float searchAngle = 130f;  //���E�͈̔�
    private NavMeshAgent nav;  //AI Navigation���i�[
    private Ray slopeRay;
    private float getSlope;

    //EnemyWalk�֐�//
    private bool canWalk;
    public int countDown;
    public int pointValue;
    //�p�j����n�_���i�[
    public List<GameObject> pointList = new List<GameObject>();

    //Boss�̃A�j���[�V����//
    private Animator anime;


    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        canWalk = true;
        //�|�C���g���w�肷�鐮���������_���ŎZ�o
        pointValue = Random.Range(0, pointList.Count);
        if(gameObject.tag == "Boss")
        {
            anime = GetComponent<Animator>();
        }
    }
    private void Update()
    {
        Vector3 MoveForward = Vector3.Scale(transform.forward, new Vector3(1, 1, 1)).normalized;
        Ray enemyRay = new Ray(transform.position + new Vector3(0, -1, 0), MoveForward);
        //Debug.DrawRay(enemyRay.origin, enemyRay.direction * 3.5f, Color.red, 100);
        RaycastHit enemyHit;
        Physics.Raycast(enemyRay, out enemyHit, 10f);
        if (enemyHit.collider.tag == "Door")
        {
            enemyHit.collider.GetComponent<DoorController>().DoorMove();
        }
        if(enemyHit.collider.tag == "Rock Door")
        {
            if(enemyHit.collider.GetComponent<RockDoorController>().rockType == 0)
            {
                pointValue = Random.Range(0, pointList.Count);
            }
            else
            {
                enemyHit.collider.GetComponent<RockDoorController>().DoorMove();
            }
            
        }
        if (canWalk) EnemyWalk();
    }
    public void EnemyWalk()  //�p�j�֐�
    {
        anime.SetBool("Walk", true);
        slopeRay = new Ray(transform.position, Vector3.down);
        RaycastHit slopeHit;
        Physics.Raycast(slopeRay, out slopeHit, 2.5f);  //()�̑�������Ray�̖��O�A�������ɏ�������ϐ����A��O������Ray�̒����@�@�@���uout�v��t���邱�Ƃ�hit�ɖ߂�l���擾�ł���
        getSlope = Vector3.Angle(Vector3.up, slopeHit.normal);
        if (getSlope > 0)
        {
            nav.speed = walkSpeed / 2;
        }
        else if(getSlope == 0)
        {
            nav.speed = walkSpeed;
        }

        //���Ԃ��v��
        countDown += 1;
        if (countDown % 1800 == 0)
        {
            //�|�C���g���w�肷�鐮���������_���ŎZ�o
            pointValue = Random.Range(0, pointList.Count);
        }
        //�|�C���g�̈ʒu���擾
        Vector3 targetPoint = pointList[pointValue].transform.position;
        //�|�C���g���w�肳��Ă���ꍇ�Ɏ��s�����
        if (pointList.Count > 0) nav.destination = new Vector3(targetPoint.x, targetPoint.y, targetPoint.z);

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            anime.SetBool("Attack1", true);
            //�@��l���̕���
            var playerDirection = other.transform.position - transform.position;
            //�@�G�̑O������̎�l���̕���
            var enemyAngle = Vector3.Angle(transform.forward, playerDirection);
            //�@�T�[�`����p�x���������甭��
            if (enemyAngle <= searchAngle)
            {
                //�v���C���[�̕���������
                Quaternion lookRotation = Quaternion.LookRotation(other.transform.position - transform.position, Vector3.up);
                lookRotation.z = 0;
                lookRotation.x = 0;
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.05f);
                if (getSlope > 0)
                {
                    nav.speed = walkSpeed / 1.5f;
                }
                else if(getSlope == 0)
                {
                    nav.speed = dashSpeed;
                }
                nav.destination = other.transform.position;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //�@��l���̕���
            var playerDirection = other.transform.position - transform.position;
            //�@�G�̑O������̎�l���̕���
            var enemyAngle = Vector3.Angle(transform.forward, playerDirection);
            //�@�T�[�`����p�x���������甭��
            if (enemyAngle <= searchAngle)
            {
                canWalk = false;
                Camera.main.GetComponent<CameraController>().searchAudio.Stop();
                Camera.main.GetComponent<CameraController>().runAudio.Play();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        anime.SetBool("Attack1", false);
        if(other.tag == "Player")
        {
            //�@��l���̕���
            var playerDirection = other.transform.position - transform.position;
            //�@�G�̑O������̎�l���̕���
            var enemyAngle = Vector3.Angle(transform.forward, playerDirection);
            //�@�T�[�`����p�x���������甭��
            if (enemyAngle <= searchAngle)
            {
                canWalk = true;
                Camera.main.GetComponent<CameraController>().runAudio.Stop();
                Camera.main.GetComponent<CameraController>().searchAudio.Play();
            }

        }
    }


    
    private Ray enemyRay;
    //�v���C���[���i�[
    public GameObject target;
    //�p�j����|�C���g���i�[
    public List<GameObject> pointList = new List<GameObject>();
    private int value;
    private float countDown;
    private int trun;
    //�G�̒ǐՕ��@���w��
    private int enemyType;

    // Start is called before the first frame update
    void Start()
    {
        //�|�C���g���w�肷�鐮���������_���ŎZ�o
        value = Random.Range(0, pointList.Count);
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //EnemyChase();
        //if(trun == 0) EnemyMove();
    }
    public void EnemyChase()  //�ǐՃR�[�h
    {
        Vector3 MoveForward = Vector3.Scale(transform.forward, new Vector3(1, 1, 1)).normalized;
        enemyRay = new Ray(transform.position + new Vector3(0, -1, 0), MoveForward);
        //Debug.DrawRay(enemyRay.origin, enemyRay.direction * 3.5f, Color.red, 100);
        RaycastHit enemyHit;
        Physics.Raycast(enemyRay, out enemyHit, 10f);
        if(enemyHit.collider.tag == "Player")
        {
            nav.destination = enemyHit.collider.transform.position;
        }
        if (enemyHit.collider.tag == "Door")
        {
            enemyHit.collider.GetComponent<DoorController>().DoorMove();
        }
    }

    //���͈͓��Ƀv���C���[������ƃv���C���[�̕��Ɍ���
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            nav.speed = chaseSpeed;
            trun = 1;
            Quaternion lookRotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);
            lookRotation.z = 0;
            lookRotation.x = 0;
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.05f);
            //nav.destination = other.transform.position;

        }
        
        if(other.tag == "Door")
        {
            trun = 2;
            other.GetComponent<DoorController>().DoorMove();
            trun = 0;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player") trun = 0;

    }
    */
}
