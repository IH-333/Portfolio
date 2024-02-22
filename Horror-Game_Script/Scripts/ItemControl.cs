using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemControl : MonoBehaviour
{
    private Ray itemRay;
    private ItemScript itemScript;

    //Item�֐�//
    public List<string> itemTypeList = new List<string>();  //�A�C�e���̎�ނ��i�[
    public List<string> itemNameList = new List<string>();  //�A�C�e���̖��O���i�[
    public List<Sprite> itemImageList = new List<Sprite>();  //�A�C�e���̉摜���i�[
    public int listNumber = 0;  //���X�g�̏��Ԃ��w�肷��
    private int loop;  //�J��Ԃ���
    //crystal �ϐ�//
    public Image crystalImage;
    //medicine drug�ϐ�//
    public int medicineCount;
    public int drugCount;
    //battry�ϐ�//
    public int batteryCount;
    public List<Image> batteryList = new List<Image>();
    //light�ϐ�//
    public GameObject playerArm;  //�v���C���[�̘r���i�[
    public Image lightImage;  //�����d����UI���i�[
    //Slot�֐�//
    public List<Image> slotImageList = new List<Image>();
    public Text slotText;  //���C���X���b�g�ɑ��U����Ă���A�C�e���̖��O���i�[
    //SlotMove�֐�//
    public bool canSlotMove;  //�X���b�g�̑��삪�\���ǂ����𔻒f����
    //�����摜�X�N���v�g//
    public ExplainScript explainScript;

    public kaiten kaiten;  //�p�`�X���̃X�N���v�g���i�[
    public Sprite blankImage;  //�A�C�e���g�p��̋󔒉摜


    // Update is called once per frame
    void Update()
    {
        if (canSlotMove) SlotMove();
        if(itemNameList.Count > 0) slotText.text = itemNameList[listNumber];  //���C���X���b�g�őI�����Ă���A�C�e���̖��O��\��
    }
    public void Item(GameObject item)  //�A�C�e�����擾�������̊֐�
    {
        itemScript = item.GetComponent<ItemScript>();   //�A�C�e���̃X�N���v�g���擾
        //�d�r���擾�������̏���
        if (itemScript.itemType == "battery")
        {
            batteryList[batteryCount].gameObject.SetActive(true);  //�d�rUI��\��
            batteryCount += 1;  //�d�r�̌��ݐ���1���₷
            if (batteryCount == 4) batteryCount = 0;
            Destroy(item);  //�d�r���폜
        }
        //�����d�����擾�������̏���
        else if (itemScript.itemType == "light" || itemScript.itemType == "saber")
        {
            lightImage.gameObject.SetActive(true);  //�����d����UI��\��������
            item.transform.parent = Camera.main.gameObject.transform;  //�v���C���[�̘r�Ɛe�q�֌W��t����
            item.transform.localEulerAngles = playerArm.transform.localEulerAngles;  //�����d���𐳖ʂɌ���������
            item.transform.position = playerArm.transform.position;  //�����d�����v���C���[�̖T�Ɉړ�������
            explainScript.LightExplain();  //�����d���̐����摜��\��
        }
        //�����擾�������̏���
        else if (itemScript.itemType == "key")
        {
            itemTypeList.Add(itemScript.itemType);  //�A�C�e���̎�ނ�ǉ�
            itemNameList.Add(itemScript.itemName);  //�A�C�e���̖��O��ǉ�
            itemImageList.Add(itemScript.itemImage);  //�A�C�e���̉摜��ǉ�
            if (itemImageList.Count == 2) canSlotMove = true;  //�A�C�e���̐����P�̎��̓X���b�g�𑀍�ł��Ȃ�
            Slot();  //�A�C�e���摜���X���b�g�Ɋi�[
            explainScript.KeyExplain(itemScript);  //���̐����摜��\��
            Destroy(item);  //�A�C�e�����폜

        }
        //�񕜖���擾�������̏���
        else if (itemScript.itemType == "medicine")
        {
            if(medicineCount == 0)
            {
                itemTypeList.Add(itemScript.itemType);  //�A�C�e���̎�ނ�ǉ�
                itemNameList.Add(itemScript.itemName);  //�A�C�e���̖��O��ǉ�
                itemImageList.Add(itemScript.itemImage);  //�A�C�e���̉摜��ǉ�
            }
            medicineCount += 1;
            if (itemImageList.Count == 2) canSlotMove = true;  //�A�C�e���̐����P�̎��̓X���b�g�𑀍�ł��Ȃ�
            Slot();
            Destroy(item);
            if (explainScript.medicineCount == false) explainScript.MedicineExplain();  //�񕜖�̐����摜��\��
        }
        //��������擾�������̏���
        else if (itemScript.itemType == "drug")
        {
            if(drugCount == 0)
            {
                itemTypeList.Add(itemScript.itemType);  //�A�C�e���̎�ނ�ǉ�
                itemNameList.Add(itemScript.itemName);  //�A�C�e���̖��O��ǉ�
                itemImageList.Add(itemScript.itemImage);  //�A�C�e���̉摜��ǉ�
            }
            drugCount += 1;
            if (itemImageList.Count == 2) canSlotMove = true;  //�A�C�e���̐����P�̎��̓X���b�g�𑀍�ł��Ȃ�
            Slot();
            Destroy(item);
            if (explainScript.drugCount == false) explainScript.DrugExplain();  //�񕜖�̐����摜��\��
        }
        //�R�C�����擾�������̏���
        else if(itemScript.itemType == "coin")
        {
            kaiten.medal += item.GetComponent<CoinScript>().value;
            Destroy(item);
        }
        //�I�[�o�[�z�΂��擾�������̏���
        else if (itemScript.itemType == "crystal")
        {
            explainScript.CrystalExplain();  //���̐����摜��\��
            Destroy(item);  //�A�C�e�����폜
        }
        //�I�[�p�[�c�݌v�����擾�������̏���
        else if (itemScript.itemType == "build")
        {
            explainScript.BuildExplain();  //���̐����摜��\��
            Destroy(item);  //�A�C�e�����폜
        }
    }
    public void Slot()  //�A�C�e���X���b�g�ɃA�C�e���𑕓U����
    {
        int count = listNumber;
        if (itemImageList.Count < 5) loop = itemImageList.Count;
        else if (itemImageList.Count >= 5) loop = slotImageList.Count;

        for (int i = 0; i < loop; i++)  //5��J��Ԃ�
        {
            slotImageList[i].sprite = itemImageList[count];
            count += 1;
            if (count == itemImageList.Count) count = 0;

        }
    }
    public void SlotMove()  //�A�C�e���X���b�g���̃A�C�e���̔z�u�𓮂���
    {
        if (Input.GetKeyDown(KeyCode.E))  //�A�C�e�����E�Ɉړ�
        {
            listNumber += 1;
            if (listNumber == itemImageList.Count) listNumber = 0;
            Slot();
        }
        if (Input.GetKeyDown(KeyCode.Q))  //�A�C�e�������Ɉړ�
        {
            listNumber -= 1;
            if (listNumber < 0) listNumber = itemImageList.Count - 1;
            Slot();
        }
    }
    public void SlotSort()  //�A�C�e���X���b�g�̕��ёւ��֐�
    {
        listNumber = 0;
        for(int i = 0; i < slotImageList.Count; i++)
        {
            //�Ώۂ̃X���b�g�ɉ������U����Ă��炸�A�ƂȂ�̃X���b�g�ɑ��U����Ă��鎞
            if(slotImageList[i].sprite == blankImage && i < slotImageList.Count)
            {
                //�Ώۂ̃X���b�g�ׂ̗̃X���b�g�����݂��Ă��鎞
                if (slotImageList[i + 1].sprite != blankImage)
                {
                    slotImageList[i].sprite = slotImageList[i + 1].sprite;
                    slotImageList[i + 1].sprite = blankImage;
                }
            }
        }
    }
}
