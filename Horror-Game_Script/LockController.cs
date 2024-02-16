using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockController : MonoBehaviour
{
    public bool lockType = true;  //�����������Ă��邩�ǂ���
    public bool doorMove = false;  //�h�A���J���Ă��邩�ǂ����𔻒肷��
    public string doorName;  //�h�A�̖��O���i�[����
    public string keyName;  //�Ή����錮�̖��O���i�[����
    private Animator anime;  //�h�A�̊J�A�j���[�V����
    public AudioSource doorAudio;  //�h�A�̊J��
    public AudioSource lockAudio;  //�h�A�̉����E�{����
    public AudioSource failAudio;  //�����������Ă��鎞�̉�

    public bool canF;
    public bool canB;
    private bool f_Point;  //�\����J������true,�߂��false
    private bool b_Point;  //������J������true,�߂��false

    // Start is called before the first frame update
    void Start()
    {
        anime = GetComponent<Animator>();
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
        else if (doorMove)  //��ԁF�J���Ă���@����F����
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
        doorAudio.Play();  //�J�����Đ�    }

    }
}

