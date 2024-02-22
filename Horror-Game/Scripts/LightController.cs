using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashLight : MonoBehaviour
{
    //private CameraController cameraController;
    public ItemControl itemControl;
    [SerializeField] private Light spotLight;
    [SerializeField] private GameObject lightColider;

    public bool useLight = false;
    private bool canLight = false;
    [SerializeField] private Light whiteLight;
    [SerializeField] private Image batteryImage;
    [SerializeField] private Slider battery;
    [SerializeField] private Image fillImage;
    [SerializeField] private Text batteryText;  //�o�b�e���[�̎c���ʂ�\��
    [SerializeField] private Image gear;
    [SerializeField] private Image gearMemory;
    [SerializeField] private float lostSpeed = 0.5f;  //�o�b�e���[�̏��Ց��x
    public List<Sprite> batteryList = new List<Sprite>();
    private int gearCount;
    private Vector3 gearMemoryPosi;
    [SerializeField] private List<Image> gearMemoryList = new List<Image>();

    void Start()
    {
        
        gearMemoryPosi = gearMemory.rectTransform.position;
    }

    void Update()
    {
        if (battery.value == 0)
        {
            if (itemControl.batteryCount > 0)  //�o�b�e���[�̐؂�ւ�
            {
                battery.value = 100;
                itemControl.batteryCount -= 1;
                itemControl.batteryList[itemControl.batteryCount].gameObject.SetActive(false);
            }
            else
            {
                canLight = false;
            }
        }
        if (useLight)
        {
            LightControll();
        }
        batteryImage.gameObject.SetActive(useLight);

    }
    public void LightControll()
    {
        if (battery.value > 0 && Input.GetMouseButtonDown(1))
        {
            canLight = !canLight;
        }
        spotLight.gameObject.SetActive(canLight);
        lightColider.gameObject.SetActive(canLight);
        
        if (canLight)
        {
            battery.value -= Time.deltaTime * lostSpeed;  //�o�b�e���[�����炷
            batteryText.text = $"{Mathf.Floor(battery.value).ToString()}%";  //�o�b�e���[�c�ʂ�\��
            if (battery.value <= 20)  //�o�b�e���[��20���ȏゾ�ƍ��F�ɂ���
            {
                fillImage.color = Color.red;
                batteryText.color = Color.red;
            } 
            else  //�o�b�e���[��20���ȉ����ƐԐF�ɂ���
            {
                fillImage.color = Color.black;
                batteryText.color = Color.white;

            }
        }


        float wheel = Input.GetAxis("Mouse ScrollWheel") * 10;
        transform.localEulerAngles += new Vector3(wheel, 0, 0);
        if (Input.GetMouseButtonDown(2))
        {
            gearCount += 1;
            whiteLight.range += 10;
            whiteLight.spotAngle += 10;
            lostSpeed += 0.5f;
            if(gearCount > 4)
            {
                gearCount = 0;
                whiteLight.range = 15;
                whiteLight.spotAngle = 60;
                lostSpeed = 0.5f;
            }
            gearMemory.rectTransform.position = gearMemoryList[gearCount].rectTransform.position;
        }

    }
}
