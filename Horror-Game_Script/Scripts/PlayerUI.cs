using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Text nameText;

    //  Slot�֐�  //
    public Sprite blankImage;
    public List<Sprite> imageList = new List<Sprite>();
    public List<Image> slotList = new List<Image>();
    public Text slotName;
    private int imageCount;
    private int selectCount;
    private int itemNumber;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        KeyHandler();
    }
    public void KeyHandler()  //�L�[���͊֐�
    {
        //Slot�֐�//
        if (Input.GetKeyDown(KeyCode.E))
        {
            selectCount += 1;
            if (selectCount > imageList.Count)
            {
                selectCount = 1;
            }
            Slot();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            selectCount -= 1;
            if (selectCount < -imageList.Count)
            {
                selectCount = -1;
            }
            Slot();
        }
    }
    public void Slot()  //�A�C�e���X���b�g�֐�
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            itemNumber = i + selectCount;
            if (imageList.Count > 2)
            {
                //itemNumber���摜�̐���葽�������ꍇ
                if (itemNumber > imageList.Count - 1)
                {
                    int result = itemNumber - imageList.Count;
                    itemNumber = result;
                }
                //itemNumber��0��菬���������ꍇ
                if (itemNumber < 0)
                {
                    int result = imageList.Count + itemNumber;
                    itemNumber = result;
                }

            }
            //slotList[i].sprite = itemList[itemNumber].GetComponent<ItemController>().image;
            slotList[i].sprite = imageList[itemNumber];
            if (slotList[0].sprite.name == "Blank") slotName.text = null;
            else slotName.text = slotList[0].sprite.name;

        }
    }
    public void ObjectName(GameObject target)
    {
        nameText.text = target.name;
        Debug.Log("OK");
        if (Input.GetMouseButtonDown(0))
        {
            if(target.tag == "Battery" || target.tag == "Key")
            {
                Destroy(target);
            }
        }
    }
}
