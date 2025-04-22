// NPCInteraction.cs
// ติดตั้งที่ตัว NPC

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPCInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionDistance = 3f;
    public KeyCode interactionKey = KeyCode.E;

    [Header("UI Elements")]
    public GameObject interactionPrompt;    // UI แสดง "กด E เพื่อคุย"
    public GameObject dialoguePanel;        // พาเนลแสดงบทสนทนา
    public Text dialogueText;               // ข้อความสนทนา
    public Text npcNameText;                // ชื่อ NPC

    [Header("Dialogue Content")]
    public string npcName = "NPC";
    public string[] dialogueLines;          // บรรทัดบทสนทนา

    private Transform player;
    private bool playerInRange = false;
    private bool dialogueActive = false;
    private int currentLine = 0;

    // ส่งอีเวนต์ออกไปเมื่อเริ่ม/จบบทสนทนา - คุณสามารถสร้างสคริปต์ DialogueManager 
    // มารับอีเวนต์นี้แล้วควบคุม Player ของคุณได้
    public delegate void DialogueEvent(bool isActive);
    public static event DialogueEvent OnDialogueStateChanged;

    void Start()
    {
        // หา Player ในฉาก
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        // ตั้งค่าเริ่มต้น ซ่อน UI ทั้งหมด
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    void Update()
    {
        // ตรวจสอบระยะห่างระหว่าง Player กับ NPC
        if (player != null)
        {
            float distance = Vector3.Distance(player.position, transform.position);

            // อยู่ในระยะโต้ตอบหรือไม่
            if (distance <= interactionDistance)
            {
                if (!playerInRange)
                {
                    playerInRange = true;
                    if (interactionPrompt != null)
                        interactionPrompt.SetActive(true);
                }

                // กดปุ่มโต้ตอบหรือไม่
                if (Input.GetKeyDown(interactionKey) && !dialogueActive)
                {
                    StartDialogue();
                }
                else if (Input.GetKeyDown(interactionKey) && dialogueActive)
                {
                    ContinueDialogue();
                }
            }
            else
            {
                if (playerInRange)
                {
                    playerInRange = false;
                    if (interactionPrompt != null)
                        interactionPrompt.SetActive(false);
                }

                // ถ้าออกจากระยะแล้วกำลังคุยอยู่ ให้ปิดบทสนทนา
                if (dialogueActive)
                {
                    EndDialogue();
                }
            }
        }
    }

    void StartDialogue()
    {
        dialogueActive = true;
        currentLine = 0;

        // แสดงพาเนลบทสนทนา
        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        // ซ่อนปุ่มกด E
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        // แสดงชื่อ NPC
        if (npcNameText != null)
            npcNameText.text = npcName;

        // แสดงข้อความบรรทัดแรก
        DisplayDialogueLine();

        // ส่งอีเวนต์ว่าเริ่มบทสนทนาแล้ว
        if (OnDialogueStateChanged != null)
            OnDialogueStateChanged(true);
    }

    void ContinueDialogue()
    {
        currentLine++;

        if (currentLine < dialogueLines.Length)
        {
            DisplayDialogueLine();
        }
        else
        {
            EndDialogue();
        }
    }

    void DisplayDialogueLine()
    {
        if (dialogueText != null && currentLine < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLine];
        }
    }

    void EndDialogue()
    {
        dialogueActive = false;

        // ซ่อนพาเนลบทสนทนา
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        // แสดงปุ่มกด E ถ้ายังอยู่ในระยะ
        if (interactionPrompt != null && playerInRange)
            interactionPrompt.SetActive(true);

        // ส่งอีเวนต์ว่าจบบทสนทนาแล้ว
        if (OnDialogueStateChanged != null)
            OnDialogueStateChanged(false);
    }

    // วาดรัศมีการโต้ตอบในหน้า Editor (ช่วยในการปรับแต่ง)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}