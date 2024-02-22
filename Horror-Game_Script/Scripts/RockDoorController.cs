using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockDoorController : MonoBehaviour
{
    public bool lockType = true;  //�����������Ă��邩�ǂ���
    public bool doorMove = false;  //�h�A���J���Ă��邩�ǂ����𔻒肷��
    public string keyName;  //�Ή����錮�̖��O���i�[����
    private Animator anime;
    public AudioSource rockAudio;
    public AudioSource doorAudio;
    public AudioSource rockOpenAudio;

    // Start is called before the first frame update
    void Start()
    {
        anime = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DoorMove()
    {
        if (doorMove == false)  //�J����
        {
            anime.SetBool("Rock Close", false);
            anime.SetBool("Rock Open", true);
        }
        else if (doorMove)  //����
        {
            anime.SetBool("Rock Open", false);
            anime.SetBool("Rock Close", true);
        }
        doorMove = !doorMove;
        doorAudio.Play();
    }

}
