using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //  �X�e�[�^�X  //
    public float movespeed = 10;  //�������x
    public float dashSpeed = 20;  //���鑬�x
    public float jumpForce = 5;  //������
    public float maxSpeed;  //�ړ����x�̍ő呬�x
    //  ��������  //
    private Rigidbody rb;   //�v���C���[�̕�������
    public bool canMove;  //ture:�v���C���[�̑��삪�\�@false:�v���C���[�̑��삪�s�\
    public Animator cameraAnime;  //�J�����̃A�j���[�V����
    //  Move�֐�  //
    private AudioSource moveAudio;  //�ړ����̌��ʉ�
    private bool useMoveAudio;  
    private Vector3 moveForward_x;    //�J�����̐��ʂ����ɑO��̈ړ����s��
    private Vector3 moveForward_z;    //�J�����̐��ʂ����ɍ��E�̈ړ����s��
    private float speed;  //�v���C���[�̈ړ����x���i�[����

    //[SerializeField] private float limitSlope = 45;  //�ō��p�x
    private bool walkKey1;  //W�L�[��S�L�[�������ꂽ���utrue�v�A�����ꂽ���ufalse�v
    private bool walkKey2;  //A�L�[��D�L�[�������ꂽ���utrue�v�A�����ꂽ���ufalse�v
    //  Rotate�֐�  //
    private float mouse_x;    //�}�E�X�̉��̈ړ��ʂ��i�[
    private float mouse_y;    //�}�E�X�̏c�̈ړ��ʂ��i�[
    [SerializeField] private float rotateSpeed = 2;    //�̂̉�]�̑��x

    //  Dash�֐�  //
    private bool canDash;  //true:�_�b�V�����\�@false:�_�b�V�����s�\
    public bool useDash = true;  //true:�_�b�V���X���C�_�[�������@false:�_�b�V���X���C�_�[���������Ȃ�
    public Slider dashSlider;  //�_�b�V���ł��鎞�Ԃ�����UI���i�[
    //  Jump�֐�  //
    private bool canJump;  //true:�W�����v���\�@false:�W�����v���s�\
    private Ray jumpRay;  //�n�ʂƐڐG���Ă���Ԃ̂݁ucanJump�v��true�ɂ���
    //  Crouch�֐�  //
    private bool canCrouch;  //true:���Ⴊ�݂��\�@false:���Ⴊ�݂��s�\
    public bool crouchFlag;  //true:���Ⴊ��ł����ԁ@false:���Ⴊ��ł��Ȃ����
    //  Slope�֐�  //
    private float getSlope;  //�X�΂̊p�x
    private bool canSlope;  //true:���o������A�������肷��@false:��̏�ɗ��܂�
    private Ray slopeRay;  //��̊p�x���擾����
    float posi_be;  //�O�t���[���ł̃v���C���[�̈ʒu
    float posi_af;  //���t���[���ł̃v���C���[�̈ʒu

    //  RoomName�֐�  //
    public Text roomNameText;
    //�̗̓o�[//
    public Slider lifeSlider;  //�v���C���[�̗̑͂�����UI���i�[

    void Start()
    {
        moveAudio = GetComponent<AudioSource>();
        speed = movespeed * 3;
        maxSpeed = movespeed;
        rb = GetComponent<Rigidbody>();
        canMove = false;  
    }

    void Update()
    {
        if (canMove)
        {
            Rotate();
            Dash();
            Jump();
            Crouch();
            Slope();
        }
        //Room();
    }
    private void FixedUpdate()
    {
        if (canMove) Move();  //�ړ��֐�
    }
    public void Rotate()  //��]�֐�
    {
        mouse_x = Input.GetAxis("Mouse X");
        Vector3 playerRotate = transform.localEulerAngles;
        playerRotate.y -= mouse_x * -rotateSpeed;
        transform.localEulerAngles = playerRotate;

    }
    public void Move()  //�ړ��֐�
    {

        if (canMove)
        {
            moveForward_z = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            moveForward_x = Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)).normalized;
        }

        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(moveForward_z * speed);  //�O���Ɉړ�
        }
        if (Input.GetKey(KeyCode.S)) rb.AddForce(moveForward_z * -speed);  //����Ɉړ�

        if (Input.GetKey(KeyCode.D)) rb.AddForce(moveForward_x * speed);  //�E���Ɉړ�

        if (Input.GetKey(KeyCode.A)) rb.AddForce(moveForward_x * -speed);  //�����Ɉړ�

        if (rb.velocity.magnitude > maxSpeed) rb.velocity = rb.velocity.normalized * maxSpeed;  //�ړ����x�̍ő呬�x�𒴂��Ȃ��悤�ɂ���

    }
    public void Dash()  //�_�b�V���֐�
    {
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) && Input.GetKey(KeyCode.LeftShift))
        {
            canDash = true;
            cameraAnime.speed = 2;
            if (Input.GetKey(KeyCode.W) && useDash) dashSlider.value -= 0.125f;
            if (Input.GetKey(KeyCode.S) && useDash) dashSlider.value -= 0.25f;
            if (dashSlider.value == 0)
            {
                canDash = false;
                cameraAnime.speed = 1;
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            canDash = false;
            cameraAnime.speed = 1;
        }
        if (canDash)
        {
            speed = dashSpeed * 2;
            maxSpeed = dashSpeed;

        }
        else if (canDash == false)
        {
            speed = movespeed * 2;
            maxSpeed = movespeed;
            dashSlider.value += 0.4f;
        }
    }
    public void Jump()  //�W�����v�֐�
    {
        jumpRay = new Ray(transform.position, Vector3.down);
        RaycastHit jumpHit;
        Physics.Raycast(jumpRay, out jumpHit, 1.5f);  //()�̑�������Ray�̖��O�A�������ɏ�������ϐ����A��O������Ray�̒����@�@�@���uout�v��t���邱�Ƃ�hit�ɖ߂�l���擾�ł���
        if (jumpHit.collider) canJump = true;
        else canJump = false;

        if (canJump && canCrouch == false && Input.GetKeyDown(KeyCode.Space))
        {
            canJump = false;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    public void Crouch()  //���Ⴊ�ފ֐�
    {

        if (Input.GetKeyDown(KeyCode.F))
        {
            canCrouch = !canCrouch;
        }
        if (canCrouch)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 0.5f, Camera.main.transform.position.z);
            GetComponent<CapsuleCollider>().height = 1;
            rb.drag = 5;
            crouchFlag = true;
            canJump = false;
        }
        else
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 2.5f, Camera.main.transform.position.z);
            GetComponent<CapsuleCollider>().height = 1.7f;
            rb.drag = 2;
            crouchFlag = false;
            canJump = true;
        }

    }
    public void Slope()  //�Ζʊ֐�
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            canSlope = true;
            walkKey1 = true;
            cameraAnime.SetBool("Move", walkKey1);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            canSlope = true;
            walkKey2 = true;
            cameraAnime.SetBool("Move", walkKey2);
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            canSlope = false;
            walkKey1 = false;
            cameraAnime.SetBool("Move", walkKey1);
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            canSlope = false;
            walkKey2 = false;
            cameraAnime.SetBool("Move", walkKey2);
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {
            if (canJump && walkKey1 == false && walkKey2 == false) rb.velocity = Vector3.zero;
        }
        posi_af = transform.position.y;

        slopeRay = new Ray(transform.position, Vector3.down);
        RaycastHit slopeHit;
        Physics.Raycast(slopeRay, out slopeHit, 1.25f);  //()�̑�������Ray�̖��O�A�������ɏ�������ϐ����A��O������Ray�̒����@�@�@���uout�v��t���邱�Ƃ�hit�ɖ߂�l���擾�ł���
        getSlope = Vector3.Angle(Vector3.up, slopeHit.normal);
        if (getSlope > 0 && canSlope == false)
        {
            rb.velocity = Vector3.zero;
        }
        else if (getSlope > 0 && canSlope)
        {
            if (posi_af < posi_be)
            {
                rb.AddForce(Vector3.up * -12f);
            }
            else rb.AddForce(Vector3.up * 6f);

        }
        posi_be = transform.position.y;
    }
}