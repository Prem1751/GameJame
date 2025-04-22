// DialogueManager.cs
// ติดตั้งที่ GameObject ใดก็ได้ในฉาก (แนะนำให้สร้าง GameObject ชื่อ "Managers")

using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    // อ้างอิงไปยังสคริปต์ควบคุมผู้เล่นของคุณ
    public MonoBehaviour playerControlScript;

    private void OnEnable()
    {
        // ลงทะเบียนรับอีเวนต์จาก NPCInteraction
        NPCInteraction.OnDialogueStateChanged += HandleDialogueStateChanged;
    }

    private void OnDisable()
    {
        // ยกเลิกการลงทะเบียน
        NPCInteraction.OnDialogueStateChanged -= HandleDialogueStateChanged;
    }

    // จัดการกับอีเวนต์เมื่อสถานะบทสนทนาเปลี่ยน
    private void HandleDialogueStateChanged(bool isDialogueActive)
    {
        if (playerControlScript != null)
        {
            // เปิด/ปิดการทำงานของสคริปต์ควบคุมผู้เล่น
            playerControlScript.enabled = !isDialogueActive;
        }
    }
}