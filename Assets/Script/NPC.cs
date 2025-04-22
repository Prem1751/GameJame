using UnityEngine;
using UnityEngine.SceneManagement;

public class NPC : MonoBehaviour
{
    public string conversationSceneName = "ConversationScene"; // ชื่อ Scene สำหรับการพูดคุย
    public string npcName = "NPC ตัวอย่าง"; // ชื่อ NPC (ใช้ส่งข้อมูลถ้าจำเป็น)
    public float interactionDistance = 2f; // ระยะที่ผู้เล่นต้องเข้าใกล้เพื่อคุย
    public string interactionPrompt = "กด E เพื่อคุย"; // ข้อความ提示การกดปุ่ม

    private GameObject player;
    private bool isInRange = false;
    private GameObject promptTextObject; // GameObject ที่แสดงข้อความ提示

    void Start()
    {
        // สร้าง GameObject สำหรับแสดงข้อความ提示 (ถ้ายังไม่มี)
        promptTextObject = new GameObject("InteractionPrompt");
        TextMeshProUGUI tmp = promptTextObject.AddComponent<TextMeshProUGUI>();
        tmp.text = interactionPrompt;
        tmp.fontSize = 16;
        tmp.alignment = TMPro.TextAlignmentOptions.Center;
        promptTextObject.transform.SetParent(transform, false); // ให้ข้อความอยู่เหนือ NPC
        promptTextObject.transform.localPosition = Vector3.up * 2f; // ปรับตำแหน่ง
        promptTextObject.SetActive(false); // ซ่อนไว้ตอนแรก

        player = GameObject.FindGameObjectWithTag("Player"); // หา Player ด้วย Tag
        if (player == null)
        {
            Debug.LogError("Player GameObject with tag 'Player' not found!");
        }
    }

    void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            StartConversation();
        }

        // ตรวจสอบระยะห่างจากผู้เล่นเพื่อแสดง/ซ่อนข้อความ提示
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= interactionDistance)
            {
                if (!isInRange)
                {
                    EnterRange();
                }
            }
            else
            {
                if (isInRange)
                {
                    ExitRange();
                }
            }
        }
    }

    void EnterRange()
    {
        isInRange = true;
        if (promptTextObject != null)
        {
            promptTextObject.SetActive(true);
        }
    }

    void ExitRange()
    {
        isInRange = false;
        if (promptTextObject != null)
        {
            promptTextObject.SetActive(false);
        }
    }

    void StartConversation()
    {
        // โหลด Scene สำหรับการพูดคุย
        SceneManager.LoadScene(conversationSceneName);

        // (Optional) ส่งข้อมูล NPC ไปยัง Scene การพูดคุย
        // สามารถใช้ PlayerPrefs, Singleton Manager หรือวิธีอื่นๆ ในการส่งข้อมูล
        PlayerPrefs.SetString("Current собеседник", npcName);
        PlayerPrefs.Save();
    }

    // Collider Trigger Event
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            EnterRange();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ExitRange();
        }
    }
}