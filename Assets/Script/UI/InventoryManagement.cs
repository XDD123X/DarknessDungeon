using Assets.Scripts.Examples.Interface.Elements;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryManagement : MonoBehaviour
{

    // Control everything of inventory or character
    [SerializeField] private GameObject EquipmentSystem;
    // ===========================================
    [SerializeField] private Vector2 offset = new Vector2(150f, 180f);
    [SerializeField] private Vector2 offsetLeftClick = new Vector2(120, -70);
    [SerializeField] private Vector2 offsetBetweenStatusIcon = new Vector2(80.0f, 100.0f);
    [SerializeField] private float distance = 64;
    [SerializeField] private GameObject ItemCursor;
    [SerializeField] private GameObject ItemInformation;
    [SerializeField] private GameObject HotbarSlotHolder;
    [SerializeField] private GameObject AttributeContainer;
    [SerializeField] private GameObject EquipmentContainer;
    [SerializeField] private GameObject HP;
    [SerializeField] private GameObject ClickContainer;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject DialogPanel;
    [SerializeField] private GameObject CharacterIcon;
    [SerializeField] private GameObject StatusIcon;
    [SerializeField] private GameObject HP_BAR;
    [SerializeField] private GameObject SlotHolder;
    // Settings panel
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private Toggle tgMusic;
    [SerializeField] private Slider slMusic;
    // Loading UI
    [SerializeField] private GameObject loadingUI;
    [SerializeField] private Image loadingBar;
    // Dead UI
    [SerializeField] private GameObject deadUI;
    // Congratulation UI
    [SerializeField] private GameObject winUI;
    // Timer UI 
    [SerializeField] private TextMeshProUGUI timerUI;
 
    // This is the actual item slot holder in inventory
    private List<SlotClass> Inventory;
    // This is the item init with player from start
    public List<SlotClass> InventoryList;

    private List<SlotClass> Equipment;

    [SerializeField] private GameObject hotbarSelector;
    [SerializeField] private int selectedIndex = 0;
    public ItemClass selectedItem;

    // This control all the sprite and gameobject
    private GameObject[] slots;
    private GameObject[] hotbarSlots;

    private TextMeshProUGUI[] listAttribute;

    private GameObject[] equipmentSlot;

    private SlotClass movingSlot;
    private SlotClass tempSlot;
    private SlotClass originalSlot;

    private SlotClass hoverSlot;
    private SlotClass hoverSlotCharacterUI;

    private bool isMovingItem;

    private PlayerController playerScript;
    private GameObject player;
    private BuffHolder buffHolder;

    // Display status information
    [SerializeField] private GameObject prefabSlot;
    private Buff hoverBuff;
    [SerializeField] private GameObject statusInformation;

    // SFX
    private AudioSource playerAudioSource;
    private AudioSource settingAudioSource;
    private SoundSettings soundLibrary;

    private RectTransform buttonUseRect;
    private RectTransform buttonRemoveRect;

    private bool isChoosingHand;

    private Button Left, Right;

    private ItemClass previousWeapon = null;
    private Weapon weaponScript;

    private SlotClass lastHoverSlot;

    private Image hp_display;
    public void TurnOff()
    {
        isChoosingHand = false;
        isMovingItem = false;
        EquipmentSystem.SetActive(false);

        ItemInformation.SetActive(false);
        statusInformation.SetActive(false);
        ClickContainer.SetActive(false);
        DialogPanel.SetActive(false);
        CharacterIcon.SetActive(true);
        lastHoverSlot = null;

        Time.timeScale = 1;
    }

    public void TurnOn()
    {
        EquipmentSystem.SetActive(true);

        Time.timeScale = 0;
        CharacterIcon.SetActive(false);

        playerAudioSource.PlayOneShot(soundLibrary.OpenCharacter, 0.65f);
    }

    private void RefreshStatusUI()
    {
        foreach (Transform child in StatusIcon.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < buffHolder.buffs.Count; i++)
        {
            GameObject newSlot = Instantiate(prefabSlot, StatusIcon.transform);

            newSlot.transform.localPosition = new Vector3(
              prefabSlot.transform.localPosition.x + offsetBetweenStatusIcon.x * (i % 7),
              prefabSlot.transform.localPosition.y + offsetBetweenStatusIcon.y * (i / 7),
              prefabSlot.transform.localPosition.z);
            newSlot.GetComponent<Image>().sprite = buffHolder.buffs[i].icon;
            newSlot.GetComponent<SlotStatus>().buff = buffHolder.buffs[i];
        }
    }

    private void Awake()
    {
        SlotClass.INIT = 0;
    }
    private void Start()
    {
        hp_display = HP_BAR.GetComponent<Image>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        buffHolder = player.GetComponent<BuffHolder>();

        slots = new GameObject[SlotHolder.transform.childCount];
        hotbarSlots = new GameObject[HotbarSlotHolder.transform.childCount];
        equipmentSlot = new GameObject[EquipmentContainer.transform.childCount];

        playerAudioSource = player.GetComponent<AudioSource>();
        settingAudioSource = GameObject.FindGameObjectWithTag("Setting").GetComponent<AudioSource>();
        soundLibrary = GameObject.FindGameObjectWithTag("Setting").GetComponent<SoundSettings>();

        TurnOff();
        HideSettingPanel();
        loadingUI.SetActive(false);
        deadUI.SetActive(false);
        winUI.SetActive(false);

        buttonUseRect = ClickContainer.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        buttonRemoveRect = ClickContainer.transform.GetChild(1).gameObject.GetComponent<RectTransform>();

        // Find the button left and right in choosing hand panel
        Left = DialogPanel.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Button>();
        Right = DialogPanel.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<Button>();

        Inventory = new List<SlotClass>();
        Equipment = new List<SlotClass>();

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < InventoryList.Count && InventoryList[i] != null && InventoryList[i].GetItem() != null)
            {
                Inventory.Add(new SlotClass(InventoryList[i].GetItem(), InventoryList[i].GetQuantity()));

            }
            else
            {
                Inventory.Add(new SlotClass());
            }
        }

        for (int i = 0; i < SlotHolder.transform.childCount; i++)
        {
            slots[i] = SlotHolder.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < HotbarSlotHolder.transform.childCount; i++)
        {
            hotbarSlots[i] = HotbarSlotHolder.transform.GetChild(i).gameObject;
        }

        listAttribute = new TextMeshProUGUI[AttributeContainer.transform.childCount];
        for (int i = 0; i < AttributeContainer.transform.childCount; i++)
        {
            listAttribute[i] = AttributeContainer.transform.GetChild(i).gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }

        for (int i = 0; i < EquipmentContainer.transform.childCount; i++)
        {
            equipmentSlot[i] = EquipmentContainer.transform.GetChild(i).gameObject;
            //equipmentSlot[i].transform.GetChild(1).gameObject.GetComponent<Image>().SetActive(false);
            Equipment.Add(new SlotClass());
        }

        weaponScript = GameObject.FindGameObjectWithTag("Weapon").GetComponent<Weapon>();

        RefreshUI();
    }

    private void Update()
    {
        hp_display.fillAmount = playerScript.CurrentHP / playerScript.MaxHP;
        playerAudioSource.mute = !tgMusic.isOn; 
        settingAudioSource.mute = !tgMusic.isOn;
        settingAudioSource.volume = slMusic.value;
        playerAudioSource.volume = slMusic.value;
        if (isChoosingHand) return;

        hoverSlot = GetClosestSlot();
        hoverSlotCharacterUI = GetClosetEquipmentSlot();
        hoverBuff = GetClosestStatusIcon();

        if (hoverBuff != null && !ClickContainer.active)
        {
            statusInformation.SetActive(true);
            statusInformation.transform.position = new Vector3(Input.mousePosition.x + offset.x, Input.mousePosition.y - offset.y, Input.mousePosition.z);
            UpdateStatusInformation();
        }
        else
        {
            statusInformation.SetActive(false);
        }

        // Using for sound
        //if (EquipmentSystem.active && !DialogPanel.active && !ClickContainer.active
        //    && ((hoverSlot != null && hoverSlot.GetItem() != null && hoverSlot != lastHoverSlot) 
        //    || (hoverSlotCharacterUI != null && hoverSlotCharacterUI.GetItem() != null && hoverSlotCharacterUI != lastHoverSlot))) {
        //    playerAudioSource.PlayOneShot(soundLibrary.HoverSlot, 0.3f);
        //    if (hoverSlot != null && hoverSlot.GetItem() != null)
        //        lastHoverSlot = hoverSlot;
        //    else
        //        lastHoverSlot = hoverSlotCharacterUI;
        //}
        //=====================================================

        if (hoverSlot != null && hoverSlot.GetItem() != null && !ClickContainer.active)
        {
            ItemInformation.SetActive(true);
            ItemInformation.transform.position = new Vector3(Input.mousePosition.x + offset.x, Input.mousePosition.y - offset.y, Input.mousePosition.z);
            UpdateInformation(hoverSlot.GetItem());
        }
        else if (hoverSlotCharacterUI != null && hoverSlotCharacterUI.GetItem() != null)
        {
            ItemInformation.SetActive(true);
            ItemInformation.transform.position = new Vector3(Input.mousePosition.x + offset.x, Input.mousePosition.y - offset.y, Input.mousePosition.z);
            UpdateInformation(hoverSlotCharacterUI.GetItem());
        }
        else
        {
            ItemInformation.SetActive(false);
        }
        ItemCursor.SetActive(isMovingItem);
        ItemCursor.transform.position = Input.mousePosition;
        if (isMovingItem)
        {
            ItemCursor.GetComponent<Image>().sprite = movingSlot.GetItem().itemIcon;
        }

        if (Input.GetMouseButtonDown(1))
        {
            SlotClass item = GetClosestSlot();
            SlotClass equipment = GetClosetEquipmentSlot();
            if (item != null && item.GetItem() != null)
            {
                isMovingItem = false;
                RefreshUI();
            }
            if (equipment != null && equipment.GetItem() != null)
            {
                isMovingItem = false;
                RefreshUI();
            }
            // Process for item in the inventory
            if (item != null && item.GetItem() != null && item.GetItem() is not WeaponClass)
            {

                ClickContainer.SetActive(true);
                ClickContainer.transform.position = new Vector3(Input.mousePosition.x + offsetLeftClick.x, Input.mousePosition.y + offsetLeftClick.y, Input.mousePosition.z);

                Button useButton = ClickContainer.transform.GetChild(0).gameObject.GetComponent<Button>();
                useButton.onClick.RemoveAllListeners();
                useButton.onClick.AddListener(() => InventoryUseItem(item));


                Button removeButton = ClickContainer.transform.GetChild(1).gameObject.GetComponent<Button>();
                removeButton.onClick.RemoveAllListeners();
                removeButton.onClick.AddListener(() => RemoveItem(item));
            }
            else
            {
                if (ClickContainer.active) { ClickContainer.SetActive(false); }
                if (equipment != null)
                {
                    SlotClass empty = null;
                    // Take the empty slot from inventory
                    for (int i = 0; i < slots.Length; i++)
                    {
                        if (Inventory[i].GetItem() == null)
                        {
                            empty = Inventory[i];
                            break;
                        }
                    }
                    // Check if inventory is full or not
                    if (empty != null)
                    {
                        // Play sound
                        playerAudioSource.PlayOneShot(soundLibrary.UnequipItemSFX, 0.65f);
                        // Remove attribute 
                        ArmorClass armor = equipment.GetItem().GetArmor();
                        playerScript.E_MaxHP -= armor.GetHPAttr();
                        if (playerScript.CurrentHP > playerScript.MaxHP)
                        {
                            playerScript.CurrentHP = playerScript.MaxHP;
                        }
                        playerScript.E_Def -= armor.GetArmorAttr();
                        buffHolder.AllCalculate();
                        // Add to inventory UI
                        empty.SetSlot(equipment.GetItem(), equipment.GetQuantity());
                        // Remove from character UI 
                        equipment.Clear();
                        RefreshUI();
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Input.mousePosition;

            Vector2 localMousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, mousePosition, canvas.worldCamera, out localMousePosition);

            if (!RectTransformUtility.RectangleContainsScreenPoint(buttonUseRect, mousePosition, canvas.worldCamera)
                && !RectTransformUtility.RectangleContainsScreenPoint(buttonRemoveRect, mousePosition, canvas.worldCamera))
            {
                if (ClickContainer.active)
                {
                    ClickContainer.SetActive(false);
                }
            }


            if (!ClickContainer.active)
            {
                if (isMovingItem)
                {
                    EndMovingItem();
                }
                else
                    BeginMovingItem();
            }

        }

        if (Input.GetKey(KeyCode.Alpha1) && !playerScript.IsAttacking)
        {
            selectedIndex = 2;
        }
        else if (Input.GetKey(KeyCode.Alpha2) && !playerScript.IsAttacking)
        {
            selectedIndex = 1;
        }
        else if (Input.GetKey(KeyCode.Alpha3) && !playerScript.IsAttacking)
        {
            selectedIndex = 0;
        }

        hotbarSelector.transform.position = hotbarSlots[selectedIndex].transform.position;
        selectedItem = Inventory[Inventory.Count - selectedIndex - 1].GetItem();
        if (previousWeapon != selectedItem)
        {
            weaponScript.ChangeWeapon((selectedItem == null ? null : selectedItem.GetWeapon()));

            if (previousWeapon != null)
            {
                WeaponClass wp = previousWeapon.GetWeapon();

                if (wp != null)
                {
                    playerScript.E_Atk -= wp.GetDamage();
                    playerScript.E_CritChance -= wp.GetCritChance();
                    playerScript.E_CritDMG -= wp.GetCritDamage();
                    playerScript.E_Penetration -= wp.GetPenetration();
                    playerScript.E_AttackSpeed -= wp.getAttackSpeed();
                    playerScript.E_MagicAtk -= wp.GetMagicDamage();

                    buffHolder.AllCalculate();
                }
            }

            previousWeapon = selectedItem;

            if (selectedItem != null)
            {
                WeaponClass myWeapon = selectedItem.GetWeapon();
                playerScript.E_Atk += myWeapon.GetDamage();
                playerScript.E_MagicAtk += myWeapon.GetMagicDamage();
                playerScript.E_CritChance += myWeapon.GetCritChance();
                playerScript.E_CritDMG += myWeapon.GetCritDamage();
                playerScript.E_Penetration += myWeapon.GetPenetration();
                playerScript.E_AttackSpeed += myWeapon.getAttackSpeed();

                buffHolder.AllCalculate();
            }
        }
        RefreshAttributeUI();
        RefreshStatusUI();

    }

    #region Ultis
    public string GetAllItemToString()
    {
        string result = "";
        foreach (SlotClass slot in Inventory) { 
            if (slot.GetItem() == null) {
                result += "-1;";
            }
            else
            {
                result += slot.GetItem().itemID + "." + slot.GetQuantity() + ";";
            }
        }
        return result.Remove(result.Length - 1);
    }

    public string GetStarterPack()
    {
        return "35.1;79.1;67.1";
    }

    public void ClearAll()
    {
        if (Inventory == null) return;
        foreach (SlotClass slot in Inventory)
        {
            slot.Clear();
        }

        RefreshUI();
    }
    #endregion

    #region Using Item
    public void InventoryUseItem(SlotClass item)
    {
        if (item.GetItem() is ConsumableClass)
        {
            if (item.GetItem().GetConsumable().Use(player))
            {
                if (item.GetQuantity() == 1)
                {
                    item.Clear();
                }
                else
                {
                    item.SetQuantity(item.GetQuantity() - 1);
                }
            }
            else
            {
                // DO NOTHING
            }
        }
        else if (item.GetItem() is ArmorClass)
        {

            ArmorClass actualItem = item.GetItem().GetArmor();
            // Boots
            if (actualItem.armorType == ArmorClass.ArmorType.Boots)
            {
                playerAudioSource.PlayOneShot(soundLibrary.EquipItemSFX, 0.65f);

                if (Equipment[0].GetItem() == null)
                {
                    Equipment[0].SetSlot(item.GetItem(), item.GetQuantity());
                    item.Clear();
                }
                else
                {
                    tempSlot = new SlotClass(Equipment[0]);

                    playerScript.E_Def -= Equipment[0].GetItem().GetArmor().GetArmorAttr();
                    playerScript.E_MaxHP -= Equipment[0].GetItem().GetArmor().GetHPAttr();


                    Equipment[0].SetSlot(item.GetItem(), item.GetQuantity());
                    item.SetSlot(tempSlot.GetItem(), tempSlot.GetQuantity());
                }
                playerScript.E_Def += actualItem.GetArmorAttr();
                playerScript.E_MaxHP += actualItem.GetHPAttr();

                if (playerScript.CurrentHP > playerScript.MaxHP)
                {
                    playerScript.CurrentHP = playerScript.MaxHP;
                }

                buffHolder.AllCalculate();
            }
            // Clothes
            else if (actualItem.armorType == ArmorClass.ArmorType.Clothes)
            {
                playerAudioSource.PlayOneShot(soundLibrary.EquipItemSFX, 0.65f);

                if (Equipment[3].GetItem() == null)
                {
                    Equipment[3].SetSlot(item.GetItem(), item.GetQuantity());
                    item.Clear();
                }
                else
                {
                    tempSlot = new SlotClass(Equipment[3]);

                    playerScript.E_Def -= Equipment[3].GetItem().GetArmor().GetArmorAttr();
                    playerScript.E_MaxHP -= Equipment[3].GetItem().GetArmor().GetHPAttr();


                    Equipment[3].SetSlot(item.GetItem(), item.GetQuantity());
                    item.SetSlot(tempSlot.GetItem(), tempSlot.GetQuantity());
                }
                playerScript.E_Def += actualItem.GetArmorAttr();
                playerScript.E_MaxHP += actualItem.GetHPAttr();

                if (playerScript.CurrentHP > playerScript.MaxHP)
                {
                    playerScript.CurrentHP = playerScript.MaxHP;
                }


                buffHolder.AllCalculate();
            }
            // Cloves
            else
            {
                isChoosingHand = true;
                DialogPanel.SetActive(true);
                Left.onClick.RemoveAllListeners();
                Left.onClick.AddListener(() => ChoosingHand(item, 0));
                Right.onClick.RemoveAllListeners();
                Right.onClick.AddListener(() => ChoosingHand(item, 1));
            }
        }

        RefreshUI();
        ClickContainer.SetActive(false);
    }

    public void RemoveItem(SlotClass item)
    {
        if (item != null)
        {
            item.Clear();

            RefreshUI();
            ClickContainer.SetActive(false);
        }
    }

    // 0 - Left 
    // 1 - Right
    private void ChoosingHand(SlotClass item, int hand)
    {
        playerAudioSource.PlayOneShot(soundLibrary.EquipItemSFX, 0.65f);    
        ArmorClass actualItem = item.GetItem().GetArmor();
        switch (hand)
        {
            case 0:
                {
                    if (Equipment[1].GetItem() == null)
                    {
                        Equipment[1].SetSlot(item.GetItem(), item.GetQuantity());
                        item.Clear();
                    }
                    else
                    {
                        tempSlot = new SlotClass(Equipment[1]);

                        playerScript.E_Def -= Equipment[1].GetItem().GetArmor().GetArmorAttr();
                        playerScript.E_MaxHP -= Equipment[1].GetItem().GetArmor().GetHPAttr();


                        Equipment[1].SetSlot(item.GetItem(), item.GetQuantity());
                        item.SetSlot(tempSlot.GetItem(), tempSlot.GetQuantity());
                    }


                    playerScript.E_Def += actualItem.GetArmorAttr();
                    playerScript.E_MaxHP += actualItem.GetHPAttr();
                    if (playerScript.CurrentHP > playerScript.MaxHP)
                    {
                        playerScript.CurrentHP = playerScript.MaxHP;
                    }

                    buffHolder.AllCalculate();

                    break;
                }
            case 1:
                {
                    if (Equipment[2].GetItem() == null)
                    {
                        Equipment[2].SetSlot(item.GetItem(), item.GetQuantity());
                        item.Clear();
                    }
                    else
                    {
                        tempSlot = new SlotClass(Equipment[2]);

                        playerScript.E_Def -= Equipment[2].GetItem().GetArmor().GetArmorAttr();
                        playerScript.E_MaxHP -= Equipment[2].GetItem().GetArmor().GetHPAttr();


                        Equipment[2].SetSlot(item.GetItem(), item.GetQuantity());
                        item.SetSlot(tempSlot.GetItem(), tempSlot.GetQuantity());
                    }

                    playerScript.E_Def += actualItem.GetArmorAttr();
                    playerScript.E_MaxHP += actualItem.GetHPAttr();
                    if (playerScript.CurrentHP > playerScript.MaxHP)
                    {
                        playerScript.CurrentHP = playerScript.MaxHP;
                    }

                    buffHolder.AllCalculate();

                    break;
                }
        }
        // End of the process
        isChoosingHand = false;
        DialogPanel.SetActive(false);
        RefreshUI();
    }
    #endregion

    #region UI Part
    private void RefreshEquipmentUI()
    {
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            try
            {
                equipmentSlot[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                equipmentSlot[i].transform.GetChild(1).GetComponent<Image>().enabled = true;
                equipmentSlot[i].transform.GetChild(1).GetComponent<Image>().sprite = Equipment[i].GetItem().itemIcon;
            }
            catch
            {
                equipmentSlot[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                equipmentSlot[i].transform.GetChild(1).GetComponent<Image>().enabled = false;
                equipmentSlot[i].transform.GetChild(1).GetComponent<Image>().sprite = null;

            }
        }
    }
    private void RefreshAttributeUI()
    {
        HP.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = playerScript.CurrentHP + "/" + playerScript.MaxHP;
        listAttribute[0].text = "Saùt thöông vaät lyù: " + playerScript.Atk.ToString("0.00");
        listAttribute[1].text = "Saùt thöông pheùp: " + playerScript.MagicAtk.ToString("0.00");
        listAttribute[2].text = "Phoøng ngöï: " + playerScript.Def;
        listAttribute[3].text = "Xuyeân giaùp: " + playerScript.Penetration + "%";
        listAttribute[4].text = "Tæ leä baïo kích: " + playerScript.CritChance + "%";
        listAttribute[5].text = "Saùt thöông baïo kích: " + playerScript.CritDmg + "%";
    }

    public void RefreshUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            try
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = Inventory[i].GetItem().itemIcon;
                if (Inventory[i].GetItem().isStackable)
                {
                    slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "x" + Inventory[i].GetQuantity();
                }
                else
                {
                    slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                }
            }
            catch
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            }
        }

        RefreshHotbar();
        RefreshEquipmentUI();
    }

    private void RefreshHotbar()
    {
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            try
            {
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = Inventory[Inventory.Count - i - 1].GetItem().itemIcon;
            }
            catch
            {
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
            }
        }
    }
    #endregion

    public bool CanAdd(ItemClass items)
    {
        SlotClass slot = Contains(items);
        if (slot != null && items.isStackable) return true;
        if (slot == null)
        {
            for (int i = 0; i < slots.Length - 3; i++)
            {
                if (Inventory[i].GetItem() == null)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool Add(ItemClass itemClass, int quantity)
    {
        if (!CanAdd(itemClass)) return false;

        SlotClass slot = Contains(itemClass);
        if (slot != null && itemClass.isStackable)
        {
            slot.SetQuantity(slot.GetQuantity() + quantity);
        }
        else
        {
            for (int i = 0;i<Inventory.Count;i++)
            {
                if (Inventory[i].GetItem() == null)
                {
                    Inventory[i].SetItem(itemClass);
                    Inventory[i].SetQuantity(quantity);
                    break;
                }
            }
        }

        RefreshUI();
        return true;
    }

    public bool Remove(ItemClass itemClass)
    {
        SlotClass slotToRemove = Contains(itemClass);
        if (slotToRemove != null)
        {
            if (slotToRemove.GetQuantity() > 1)
            {
                slotToRemove.SetQuantity(slotToRemove.GetQuantity() - 1);
            }
            else
                Inventory.Remove(slotToRemove);
        }
        else
        {
            return false;
        }

        RefreshUI();
        return true;
    }

    public SlotClass Contains(ItemClass item)
    { 

        if (item == null) return null;

        foreach (SlotClass slot in Inventory)
        {
            if (slot.GetItem() != null && slot.GetItem().Equals(item))
            {
                return slot;
            }
        }
        return null;
    }

    #region Equip item information
    private SlotClass GetClosetEquipmentSlot()
    {
        for (int i = 0; i < Equipment.Count; i++)
        {
            if (Vector2.Distance(equipmentSlot[i].transform.position, Input.mousePosition) <= distance)
            {
                return Equipment[i];
            }
        }
        return null;
    }
    #endregion

    #region Moving Item
    private bool BeginMovingItem()
    {
        originalSlot = GetClosestSlot();

        if (originalSlot == null || originalSlot.GetItem() == null) return false;
        else
        {
            isMovingItem = true;
            movingSlot = originalSlot;
            RefreshUI();
            return true;
        }
    }

    private bool EndMovingItem()
    {
        originalSlot = GetClosestSlot();

        if (originalSlot == null)
        {
            isMovingItem = false;
            RefreshUI();
            return false;
        }

        if (originalSlot.GetID() == movingSlot.GetID())
        {
            isMovingItem = false;
            RefreshUI();
            return false;
        }

        if (originalSlot.GetID() == 21 || originalSlot.GetID() == 22 || originalSlot.GetID() == 23)
        {
            if (movingSlot.GetItem() is not WeaponClass)
            {
                isMovingItem = false;
                RefreshUI();
                return false;
            }
        }

        if (movingSlot.GetID() == 21 || movingSlot.GetID() == 22 || movingSlot.GetID() == 23)
        {
            if (originalSlot.GetItem() != null && originalSlot.GetItem() is not WeaponClass)
            {
                isMovingItem = false;
                RefreshUI();
                return false;
            }
        }

        if (originalSlot != null && originalSlot.GetItem() != null)
        {
            if (originalSlot.GetItem() == movingSlot.GetItem())
            {
                if (originalSlot.GetItem().isStackable)
                {
                    originalSlot.AddQuantity(movingSlot.GetQuantity());
                    movingSlot.Clear();
                }
                else
                {
                    return false;
                }
            }
            else
            {
                tempSlot = new SlotClass(originalSlot);
                originalSlot.SetSlot(movingSlot.GetItem(), movingSlot.GetQuantity());
                movingSlot.SetSlot(tempSlot.GetItem(), tempSlot.GetQuantity());
            }
        }
        else
        {
            originalSlot.SetSlot(movingSlot.GetItem(), movingSlot.GetQuantity());
            movingSlot.Clear();
        }
        isMovingItem = false;
        RefreshUI();

        return true;
    }

    private SlotClass GetClosestSlot()
    {
        for (int i = 0; i < Inventory.Count; i++)
        {
            if (Vector2.Distance(slots[i].transform.position, Input.mousePosition) <= distance)
            {
                return Inventory[i];
            }
        }
        return null;
    }

    #endregion

    #region Display item information
    private void UpdateInformation(ItemClass item)
    {
        ItemInformation.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.itemName;
        if (item is ArmorClass)
        {
            ItemInformation.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Giaùp";
        }
        else if (item is WeaponClass)
        {
            ItemInformation.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Vuù Khí";
        }
        else
        {
            ItemInformation.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Vaät phaåm tieâu hao";
        }
        ItemInformation.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = item.ToString();
    }
    #endregion

    #region Display status information
    private Buff GetClosestStatusIcon()
    {
        for (int i = 0; i < StatusIcon.transform.childCount; i++)
        {
            if (Vector2.Distance(StatusIcon.transform.GetChild(i).transform.position, Input.mousePosition) <= prefabSlot.transform.GetComponent<RectTransform>().rect.width)
            {
                return StatusIcon.transform.GetChild(i).gameObject.GetComponent<SlotStatus>().buff;
            }
        }
        return null;
    }
    private void UpdateStatusInformation()
    {
        statusInformation.transform.GetChild(0).GetComponent<Image>().sprite = hoverBuff.icon;
        statusInformation.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = hoverBuff.name + " - " + hoverBuff.GetLevel();
        statusInformation.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = hoverBuff.ToString();
    }
    #endregion

    #region Setting panel
    public void DisplaySettingPanel()
    {
        Time.timeScale = 0;
        CharacterIcon.SetActive(false);
        tgMusic.isOn = PlayerPrefs.GetInt("EnableMusic") == 1;
        slMusic.value = PlayerPrefs.GetFloat("Music");
        settingPanel.SetActive(true);
    }

    public void HideSettingPanel()
    {
        CharacterIcon.SetActive(true);

        Time.timeScale = 1;
        PlayerPrefs.SetInt("EnableMusic", tgMusic.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("Music", slMusic.value);
        PlayerPrefs.Save();
        settingPanel.SetActive(false);
    }
    #endregion

    #region Return lobby 
    public void ReturnLobby()
    {
        loadingUI.SetActive(true);
        StartCoroutine(onLoading(1));
    }

    public void GoToMap(int index)
    {
        loadingUI.SetActive(true);
        StartCoroutine(onLoading(index));
    }
    IEnumerator onLoading(int index)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(index);

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);

            loadingBar.fillAmount = progressValue;
            yield return null;
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion

    #region Wave Control Part
    public void SetTimer(float timer)
    {
        timerUI.text = "Thời gian của đợt kế tiếp : " + timer.ToString("0.0") + " giây";
    }
    public void DisplayTimer()
    {
        timerUI.gameObject.SetActive(true);
    }
    public void HideTimer()
    {
        timerUI.gameObject.SetActive(false);
    }
    #endregion

    #region DEAD OR WIN
    public void YouDead()
    {
        Time.timeScale = 1;
        deadUI.SetActive(true);
        deadUI.transform.GetChild(0).GetComponent<Display>().ActiveDisplay();
    }

    public void FinishMap()
    {
        Time.timeScale = 1;
        winUI.SetActive(true);
        winUI.transform.GetChild(0).GetComponent<Display>().ActiveDisplay();
        ItemManagement.SaveRoundNumber(SceneManager.GetActiveScene().buildIndex - 2);
    }
    #endregion
}
