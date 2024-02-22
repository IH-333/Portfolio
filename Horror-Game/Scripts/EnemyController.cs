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
}
