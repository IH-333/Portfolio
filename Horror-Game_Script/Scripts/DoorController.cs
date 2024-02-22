using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool lockType = true;  //�����������Ă��邩�ǂ��� true:�����������Ă��Ȃ� false:�����������Ă���
    public bool doorMove = false;  //�h�A���J���Ă��邩�ǂ����𔻒肷��
    private Animator anime;  //�h�A�̊J�A�j���[�V����

    public bool canF;
    public bool canB;
    private bool f_Point;  //�\����J������true,�߂��false
    private bool b_Point;  //������J������true,�߂��false

    public AudioSource doorAudio;  //�h�A�̊J��
    public AudioSource lockAudio;  //�h�A�̉����E�{����
    public AudioSource failAudio;  //�����������Ă��鎞�̉�


    // Start is called before the first frame update
    void Start()
    {
        anime = GetComponent<Animator>();

    }
    private void Update()
    {

    }
    public void DoorMove()
    {
        if (doorMove == false)  //��ԁF���Ă���@����F�J����
        {
            if (canF)
            {
                anime.SetBool("Close_f", false);
                anime.SetBool("Open_f", true);
                f_Point = true;
            }
            if (canB)
            {
                anime.SetBool("Close_b", false);
                anime.SetBool("Open_b", true);
                b_Point = true;
            }
        }
        else if(doorMove)  //��ԁF�J���Ă���@����F����
        {
            if (f_Point)
            {
                anime.SetBool("Open_f", false);
                anime.SetBool("Close_f", true);
                f_Point = false;
            }
            if (b_Point)
            {
                anime.SetBool("Open_b", false);
                anime.SetBool("Close_b", true);
                b_Point = false;
            }
        }
        doorMove = !doorMove;  //�h�A���J���Ă��邩�ǂ����𔻒�
        doorAudio.Play();  //�J�����Đ�
    }

}
