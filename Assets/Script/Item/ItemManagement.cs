using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class ItemManagement : MonoBehaviour
{
    private static readonly string EncryptionKey = "GameOfTheYearBWQ";
    private static InventoryManagement inventoryManagement;

    public static List<ConsumableClass> consumables;
    public static List<WeaponClass> weapons;
    public static List<ArmorClass> armors;

    public static readonly string PREVIOUS_DATA = "Assets/Resources/PREVIOUS_DATA.bytes";
    public static readonly string USER_DATA = "Assets/Resources/User_data.bytes";
    public static readonly string ADMIN_DATA = "Assets/Resources/Admin_data.bytes";
    private static readonly string pathRoundNumber = "Assets/Resources/RoundNum.bytes";

    void Start()
    {
        inventoryManagement = GameObject.FindGameObjectWithTag("UI").GetComponent<InventoryManagement>();

        LoadAllItem();
        Invoke("LOADUSER", 0.1f);
    }
    public void LOADUSER()
    {
        Load(USER_DATA);
    }
    public static ItemClass GetItem(int id)
    {
        ItemClass misc = consumables.FirstOrDefault(p => p.itemID == id);
        ItemClass weapon = weapons.FirstOrDefault(p => p.itemID == id);
        ItemClass armor = armors.FirstOrDefault(p => p.itemID == id);
        if (misc != null) return misc.GetItem();
        else if (weapon != null) return weapon.GetItem();
        else if (armor != null) return armor.GetItem();
        return null;
    }

    void LoadAllItem()
    {
        try
        {
            consumables = new List<ConsumableClass>(Resources.LoadAll<ConsumableClass>("Items/Misc"));
            weapons = new List<WeaponClass>(Resources.LoadAll<WeaponClass>("Items/Weapons"));
            armors = new List<ArmorClass>(Resources.LoadAll<ArmorClass>("Items/Armors"));
        }
        catch
        {
            Debug.Log("CANNOT LOADING ITEM DATABASE");
        }
    }
    public static void LoadRoundNumber()
    {
        if (!File.Exists(pathRoundNumber))
        {
            File.Create(pathRoundNumber).Dispose();
            string data = "1";
            byte[] inputEncrypt = EncryptStringToBytes_Aes(data, EncryptionKey);
            File.WriteAllBytes(pathRoundNumber, inputEncrypt);
        }
        byte[] encryptedData = File.ReadAllBytes(pathRoundNumber);
        string result = DecryptStringFromBytes_Aes(encryptedData, EncryptionKey);
        if (result == null)
        {
            PlayerPrefs.SetInt("Map", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Map", int.Parse(result));
        }
    }

    public static void SaveRoundNumber(int roundNumber)
    {
        string data = roundNumber.ToString();
        byte[] encryptedData = EncryptStringToBytes_Aes(data, EncryptionKey);
        File.WriteAllBytes(pathRoundNumber, encryptedData);
    }

    public static void Load(string filePath)
    {

        inventoryManagement.ClearAll();

        if (!File.Exists(filePath))
        {
            File.Create(filePath).Dispose();
            string data = inventoryManagement.GetStarterPack();
            byte[] inputEncrypt = EncryptStringToBytes_Aes(data, EncryptionKey);
            File.WriteAllBytes(filePath, inputEncrypt);
        }

        byte[] encryptedData = File.ReadAllBytes(filePath);
        string result = DecryptStringFromBytes_Aes(encryptedData, EncryptionKey);
        string[] listItem = result.Split(";");
        foreach (string item in listItem)
        {
            if (item == "-1")
            {
                inventoryManagement.Add(null, 0);
            }
            else
            {
                string[] divide = item.Split(".");

                inventoryManagement.Add(GetItem(int.Parse(divide[0])), int.Parse(divide[1]));

            }
        }
        inventoryManagement.RefreshUI();
    }
    public static void Save(string filePath)
    {
        string data = inventoryManagement.GetAllItemToString();
        byte[] encryptedData = EncryptStringToBytes_Aes(data, EncryptionKey);
        File.WriteAllBytes(filePath, encryptedData);
    }

    private static byte[] EncryptStringToBytes_Aes(string plainText, string key)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] iv = new byte[16];
        byte[] encrypted;

        using (Aes aesAlg = Aes.Create())
        {
            using (var encryptor = aesAlg.CreateEncryptor(keyBytes, iv))
            {
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
        }

        return encrypted;
    }

    private static string DecryptStringFromBytes_Aes(byte[] cipherText, string key)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] iv = new byte[16];
        string plaintext = null;

        using (Aes aesAlg = Aes.Create())
        {
            using (var decryptor = aesAlg.CreateDecryptor(keyBytes, iv))
            {
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        return plaintext;
    }
}
