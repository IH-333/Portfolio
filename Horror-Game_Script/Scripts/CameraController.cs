using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CameraController : MonoBehaviour
{

    public PlayerUI playerUI;
    //�}�E�X�̉��̈ړ��ʂ��i�[
    private float mouse_x;
    //�}�E�X�̏c�̈ړ��ʂ��i�[
    private float mouse_y;
    //��]�̑��x
    [SerializeField] private float rotateSpeed = 2;
    //�J�������Ǐ]����Ώ�
    private GameObject player;
    //�J�����ƃv���C���[�Ƃ̋����̍����i�[
    private Vector3 offset;
    //�A�C�e���̐�������\������
    public Image lightImage;
    public Image keyImage;
    private bool canImage;

    //������̕ϐ�//
    private float countDown;  //�o�ߎ��Ԃ��i�[����
    private bool useDrug;  //��������g�p����Ɓuture�v�ɂȂ�
    private float dashSpeed_be;  //�����O�̑��x���i�[
    public float dashSpeed_af = 20;  //������̑��x���i�[
    private Color dashSliderColor;  //�����O�̃_�b�V���X���C�_�[�̐F���i�[
    public GameObject dashSliderFill;  //�_�b�V���X���C�_�[�̃X���C�h���i�[

    //Rotate�֐�//
    public bool canRotate = true;

    //  Name�֐�  //
    public Text nameText;  //�I�����Ă���I�u�W�F�N�g�̖��O��\��
    public Text labelText;  //�I�����Ă���I�u�W�F�N�g�̏�ԁE������@��\��
    public Text itemText;  //�X���b�g�ɑ��U���Ă���A�C�e���̎g�p���@��\��
    private Ray nameRay;
    //�h�A�̕ϐ�//
    private int canOpen;  //�Ή����錮�������Ă���ꍇture�ɂȂ�
    private string doorText;  //�h�A�̑�����@��\��
    private string openText;  //�h�A���{��������@���i�[
    private string lockText;  //�h�A������������@���i�[
    private string lockOpenText;  //���t���h�A������������@���i�[
    private string lockLockText;  //���t���h�A���{��������@���i�[
    private string failText; //�h�A�Ɍ����������Ă��鎞�̃e�L�X�g���i�[
    //�����d���̕ϐ�//
    [SerializeField] private GameObject playerArm;
    [SerializeField] GameObject Light;
    public int batteryCount;
    public List<Image> batteryList = new List<Image>();
    //���폤�l�̕ϐ�//
    public GameObject murabitoPanel;  //�A�C�e���w����UI
    public GameObject gameController;
    //���C�g�Z�C�o�[�̕ϐ�//
    public GameObject saberPanel;  //�u���C�g�Z�C�o�[�v��UI���i�[����
    public int crystalCount;  //�u�I�[�o�[���^���v�̌����i�[����
    public int buildCount;  //�u�I�[�p�[�c�݌v���v�̌����i�[����
    public int overCount;  //�u�I�[�o�[�h���C�u�d�r�v�̌����i�[����
    public Text crystalText;  //�u�I�[�o�[���^���v�̕���
    public Text buildText;  //�u�I�[�p�[�c�݌v���v�̕���
    public Text overText;  //�u�I�[�o�[�h���C�u�d�r�v�̕���
    public Text makeText;  //���C�g�Z�C�o�[���쐬����{�^��

    //  Audio�֐�  //
    /*
    public AudioSource searchAudio;  //�T����BGM
    public AudioSource runAudio;  //������BGM
    public AudioSource heartAudio;  //�S��
    */

    private ItemControl itemControl;


    void Start()
    {
        //���O���uPlayer�v�̃I�u�W�F�N�g���擾
        player = GameObject.Find("Player");
        player.GetComponent<PlayerController>().canMove = true;
        //�J�����ƃv���C���[�Ƃ̋����̍����擾
        offset = transform.position - player.transform.position;
        //�h�A�ɃJ�[�\�������킹�����̕���
        openText = "[�}�E�X���N���b�N]�ŊJ��\n[R�L�[]�Ŏ{������";
        lockText = "[R�L�[]�ŉ�������";
        lockOpenText = "[C�L�[]�Ō����g�p���ĉ�������";
        lockLockText = "[C�L�[]�Ō����g�p���Ď{������";
        failText = "��p�̌����K�v������";
        itemControl = GetComponent<ItemControl>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (canRotate)
        {
            Rotate();
        }
        Act();
        ExtraItems();
        if (useDrug)
        {
            countDown += Time.deltaTime;
            Debug.Log(countDown);
            player.GetComponent<PlayerController>().dashSpeed = dashSpeed_af;  //�v���C���[�̃_�b�V���X�s�[�h���A�b�v������
            if(countDown > 10)
            {
                useDrug = false;
                player.GetComponent<PlayerController>().dashSpeed = dashSpeed_be;  //�v���C���[�̃_�b�V���X�s�[�h����ɖ߂�
                player.GetComponent<PlayerController>().useDash = true;
                dashSliderFill.GetComponent<Image>().color = dashSliderColor;
                countDown = 0;
            }
        }
    }
    public void Rotate()  //��]�֐�
    {
        //�v���C���[�ƃJ�����̑��΋��������߁A���̋�����ۂ��Ȃ���v���C���[�ɍ��킹�ăJ�������ړ�����
        GetComponent<Transform>().position = player.transform.position + offset;
        mouse_x = Input.GetAxis("Mouse X");
        mouse_y = Input.GetAxis("Mouse Y");
        Vector3 cameraRotate = transform.localEulerAngles;
        cameraRotate.x -= mouse_y * rotateSpeed;
        cameraRotate.y += mouse_x * rotateSpeed;
        transform.localEulerAngles = cameraRotate;
        //�v���C���[�̘r�����_�ɍ��킹�ē�����
        mouse_y = Input.GetAxis("Mouse Y");
        Vector3 armRotate = playerArm.transform.localEulerAngles;
        armRotate.x += mouse_y * rotateSpeed;
        playerArm.transform.localEulerAngles = armRotate;
    }
    public void Act()  //�A�C�e���̎擾�E�g�p�A�h�A�̊J�A�h�A�̉����E�{���̊֐��A�A�C�e���̎g�p
    {
        //Ray�𐶐��E�ˏo����//
        Vector3 MoveForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 1, 1)).normalized;
        nameRay = new Ray(Camera.main.transform.position, MoveForward);
        RaycastHit hit;
        Physics.Raycast(nameRay, out hit, 3.5f);
        if (hit.collider != null)
        {
            //  �A�C�e�����E���Ƃ��̏���  //
            if (hit.collider.gameObject.tag == "Item")
            {
                ItemScript itemScript = hit.collider.gameObject.GetComponent<ItemScript>();  //�A�C�e���̃X�N���v�g���擾
                nameText.text = itemScript.itemName;  //�A�C�e���̖��O��\��
                if (Input.GetMouseButtonDown(0))
                {
                    itemControl.Item(hit.collider.gameObject);
                }
            }
            if(hit.collider.gameObject.tag == "BedLight")  //�x�b�h�̃��C�g
            {
                if (hit.collider.GetComponent<BedLightScript>().canBedLight == true) nameText.text = "[���N���b�N]�Ŗ����������";
                else if (hit.collider.GetComponent<BedLightScript>().canBedLight == false) nameText.text = "[���N���b�N]�Ŗ������t����";
                if (Input.GetMouseButtonDown(0))
                {
                    hit.collider.GetComponent<BedLightScript>().canBedLight = !hit.collider.GetComponent<BedLightScript>().canBedLight;
                }

            }
            //  ���폤�l�ɘb�������鎞�̏���  //
            else if(hit.collider.gameObject.tag == "Murabito")
            {
                nameText.text = "[���N���b�N]�Řb��������";
                if (Input.GetMouseButtonDown(0))
                {
                    murabitoPanel.SetActive(true);  //���폤�l��UI��\��������
                    canRotate = false;  //�J�����̑���𖳌�������
                    player.GetComponent<PlayerController>().canMove = false;  //�v���C���[�̑���𖳌�������
                    //gameController.GetComponent<GameController>().canCursor = true;
                    nameText.text = "";
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    murabitoPanel.SetActive(false);  //���폤�l��UI���\���ɂ�����
                    canRotate = true;  //�J�����̑����L��������
                    player.GetComponent<PlayerController>().canMove = true;  //�v���C���[�̑����L��������
                    Camera.main.GetComponent<CameraController>().canRotate = true;
                    //gameController.GetComponent<GameController>().canCursor = false;
                }
            }
            //  �h�A���J���鏈��  //
            else if (hit.collider.gameObject.tag == "Door")
            {
                //�h�A�̃X�N���v�g���擾
                DoorController doorController = hit.collider.gameObject.GetComponent<DoorController>();

                //�h�A�̏�Ԃɂ���đ�����@��\��
                if (doorController.lockType) doorText = openText;
                else if (doorController.lockType == false) doorText = lockText;

                //�����������Ă��Ȃ��ꍇ
                if (doorController.lockType && Input.GetMouseButtonDown(0))
                {
                    doorController.DoorMove();
                }
                //�����������Ă���ꍇ
                else if (doorController.lockType == false && Input.GetMouseButtonDown(0))
                {
                    lockText = "�����������Ă���\n[R�L�[]�ŉ�������";
                    doorController.failAudio.Play();
                }
                labelText.text = doorText;
                //�h�A���܂��Ă��鎞
                if (doorController.doorMove == false && Input.GetKeyDown(KeyCode.R))
                {
                    doorController.lockType = !doorController.lockType;
                    doorController.lockAudio.Play();
                    lockText = "[R�L�[]�ŉ�������";
                }
            }
            //  ���t���h�A���J���鏈��  //
            else if (hit.collider.gameObject.tag == "Lock")
            {
                //���t���h�A�̃X�N���v�g���擾
                LockController lockController = hit.collider.gameObject.GetComponent<LockController>();
                nameText.text = lockController.doorName;
                //���C���X���b�g�Ɍ��𑕓U���Ă��鎞
                if (itemControl.itemTypeList.Count > 0 && itemControl.itemTypeList[itemControl.listNumber] == "key")
                {
                    //�h�A�̏�Ԃɂ���đ�����@��\��
                    if (lockController.lockType) doorText = lockLockText;  //�����J���Ă��鎞�̃e�L�X�g
                    else if (lockController.lockType == false) doorText = lockOpenText;  //�����܂��Ă��鎞�̃e�L�X�g
                    labelText.text = doorText;  
                    itemText.text = lockOpenText;  //������@��\��
                    //�����g�p������
                    if (Input.GetKeyDown(KeyCode.C))
                    {
                        for (int i = 0; i < itemControl.itemNameList.Count; i++)
                        {
                            //�Ή����錮�������Ă��āA���C���X���b�g�ɑ��U���Ă��鎞
                            if (lockController.keyName == itemControl.itemNameList[i] && itemControl.listNumber == i)
                            {
                                canOpen = 1;
                            }
                        }
                        if(canOpen == 1)  //�Ή����錮�������Ă��āA���C���X���b�g�ɑ��U���Ă��鎞
                        {
                            if (lockController.lockType == false)  //�����J����
                            {
                                lockController.lockType = true;
                                lockLockText = "�����J����\n[C�L�[]�Ō����g�p���Ď{������";
                            }
                            else if (lockController.lockType && lockController.doorMove == false)  //����߂�i�h�A��߂Ă��鎞�̂ݗL���j
                            {
                                lockController.lockType = false;
                                lockOpenText = "����߂�\n[C�L�[]�Ō����g�p���ĉ�������";
                            }
                            canOpen = 0;  //�ϐ���������
                        }
                        else if(canOpen == 0)  //�Ή����錮�����C���X���b�g�ɑ��U���Ă��Ȃ���
                        {
                            lockOpenText = "�ǂ���献���Ⴄ�悤��";
                            lockLockText = "�ǂ���献���Ⴄ�悤��";
                        }
                    }
                }
                //���C���X���b�g�Ɍ��𑕓U���Ă��Ȃ���
                else
                {
                    labelText.text = failText;
                    if (Input.GetMouseButtonDown(0))
                    {
                        failText = "��p�̌����K�v������\n�����������Ă���";
                    }
                }
                //�h�A���J���鏈��
                if (Input.GetMouseButtonDown(0))
                {
                    //�����܂��Ă��鎞
                    if(lockController.lockType == false) lockOpenText = "�����������Ă���";
                    //�����J���Ă��鎞
                    else if(lockController.lockType) lockController.DoorMove();
                }
            }
            else if(hit.collider.gameObject.tag == "LastDoor")  //�{�X�����O�̃h�A
            {
                LastDoorScript lastDoorScript = hit.collider.GetComponent<LastDoorScript>();
                if (lastDoorScript.doorMove == false)  //�܂��Ă��鎞
                {
                    //�u�{�X��ɒ��݂܂����H�i��x����Ɩ߂邱�Ƃ͂ł��܂���j�v�̕\�L���o��
                    //labelText.text = "�{�X��ɒ��݂܂����H\n����x����Ɩ߂邱�Ƃ͂ł��܂���";
                    if (Input.GetMouseButtonDown(0))
                    {
                        //lastDoorScript.DoorMove();
                        SceneManager.LoadScene("EndScene");

                    }
                }
            }
            else if(hit.collider.gameObject.tag == "Slot")
            {
                kaiten slot = hit.collider.gameObject.GetComponent<kaiten>();
                nameText.text = "[���N���b�N]�Ńv���C����";
                if (Input.GetMouseButtonDown(0))
                {
                    slot.canCamera = true;
                    slot.canSlot = true;
                    slot.patiPanel.SetActive(true);
                    nameText.text = "";
                    player.GetComponent<PlayerController>().enabled = false;
                    Light.GetComponent<FlashLight>().enabled = false;
                    Camera.main.GetComponent<CameraController>().enabled = false;
                }
            }
        }
        else//�e�L�X�g�̏�����
        {
            nameText.text = "";
            labelText.text = "";
            openText = "[�}�E�X���N���b�N]�ŊJ��\n[R�L�[]�Ŏ{������";
            lockText = "[R�L�[]�ŉ�������";
            lockOpenText = "[C�L�[]�Ō����g�p���ĉ�������";
            lockLockText = "[C�L�[]�Ō����g�p���Ď{������";
            failText = "��p�̌����K�v������";
        }
        //���X�g�̗v�f��1�ȏ�ŁA���C���X���b�g�ɑ��U����Ă���A�C�e�����񕜖򂾂����ꍇ
        if (itemControl.itemTypeList.Count > 0 && itemControl.itemTypeList[itemControl.listNumber] == "medicine")
        {
            itemControl.slotText.text = $"{itemControl.itemNameList[itemControl.listNumber]} �~ {itemControl.medicineCount}";
            itemText.text = "[C�L�[]�ŉ񕜖���g�p����";

            if (Input.GetKeyDown(KeyCode.C))
            {
                if (player.GetComponent<PlayerController>().lifeSlider.value < 100)
                {
                    player.GetComponent<PlayerController>().lifeSlider.value += 20;
                    itemControl.medicineCount -= 1;
                    if (itemControl.medicineCount == 0)
                    {
                        labelText.text = "";
                        itemControl.itemTypeList.RemoveAt(itemControl.listNumber);
                        itemControl.itemNameList.RemoveAt(itemControl.listNumber);
                        itemControl.itemImageList.RemoveAt(itemControl.listNumber);
                        itemControl.slotImageList[0].sprite = itemControl.blankImage;
                        nameText.text = "";  //�A�C�e���̑�����@��������
                        itemControl.slotText.text = "";  //���C���X���b�g�̃A�C�e������������
                        itemControl.SlotSort();
                    }
                }
                else if (player.GetComponent<PlayerController>().lifeSlider.value == 100) labelText.text = "�̗͂����^�����B";

            }
        }
        //���X�g�̗v�f��1�ȏ�ŁA���C���X���b�g�ɑ��U����Ă���A�C�e���������򂾂����ꍇ
        else if (itemControl.itemTypeList.Count > 0 && itemControl.itemTypeList[itemControl.listNumber] == "drug")
        {
            itemControl.slotText.text = $"{itemControl.itemNameList[itemControl.listNumber]} �~ {itemControl.drugCount}";
            itemText.text = "[C�L�[]�ŋ�������g�p����";
            if (Input.GetKeyDown(KeyCode.C))
            {
                useDrug = true;
                dashSpeed_be = player.GetComponent<PlayerController>().dashSpeed;  //�v���C���[�̃_�b�V�����̑��x���擾
                player.GetComponent<PlayerController>().useDash = false;  //�_�b�V���X���C�_�[������Ȃ��悤�ɂ���
                dashSliderColor = dashSliderFill.GetComponent<Image>().color;
                dashSliderFill.GetComponent<Image>().color = Color.red;
                itemControl.drugCount -= 1;
                if (itemControl.drugCount == 0)
                {
                    labelText.text = "";
                    itemControl.itemTypeList.RemoveAt(itemControl.listNumber);
                    itemControl.itemNameList.RemoveAt(itemControl.listNumber);
                    itemControl.itemImageList.RemoveAt(itemControl.listNumber);
                    itemControl.slotImageList[0].sprite = itemControl.blankImage;
                    nameText.text = "";  //�A�C�e���̑�����@��������
                    itemControl.slotText.text = "";  //���C���X���b�g�̃A�C�e������������
                    itemControl.SlotSort();
                }
            }
        }
        else
        {
            itemText.text = "";
        }
    }
    public void ExtraItems()  //���C�g�Z�C�o�[�����̂ɕK�v�ȃA�C�e����UI����
    {
        if (crystalCount > 0 || buildCount > 0 || overCount > 0) saberPanel.gameObject.SetActive(true);
        crystalText.text = $"�I�[�o�[���^���F{crystalCount}/3��";
        buildText.text = $"�I�[�p�[�c�݌v���F{buildCount}/3��";
        overText.text = $"�I�[�o�[�h���C�u�d�r�F{overCount}/3��";
        if(crystalCount == 3 && buildCount == 3 || overCount == 3)
        {
            makeText.gameObject.SetActive(true);
        }
    }
    
}