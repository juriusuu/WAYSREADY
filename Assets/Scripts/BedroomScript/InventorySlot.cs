using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BedroomScriptS3;
public class InventorySlot : MonoBehaviour
{
    public Image itemImage; // Reference to the Image component for the item's sprite
    public Text itemQuantityText; // Reference to the Text component for the item's quantity

    private string itemName; // The name of the item in this slot
    private int itemQuantity; // The quantity of the item in this slot
    private PickupItems pickupItemRef;
    // Setup the slot with item data

    private void Awake()
    {
        // Ensure there is always an AudioSource attached
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }
    public void Setup(string itemName, int itemQuantity, Sprite itemSprite, PickupItems pickupItemRef)
    {
        this.itemName = itemName;
        this.itemQuantity = itemQuantity;
        this.pickupItemRef = pickupItemRef;
        // Assign the sprite to the Image component
        if (itemImage != null)
        {
            if (itemSprite != null)
            {
                itemImage.sprite = itemSprite; // Assign the sprite to the Image
                itemImage.enabled = true; // Ensure the Image is enabled
                Debug.Log($"Setting up slot for {itemName} with sprite: {itemSprite.name}");
            }
            else
            {
                itemImage.sprite = null; // Clear the sprite
                itemImage.enabled = false; // Disable the Image if no sprite is provided
                Debug.LogWarning($"No sprite provided for {itemName}");
            }
        }
        else
        {
            Debug.LogError($"Image component is not assigned on {gameObject.name}. Cannot display sprite.");
        }

        // Update the quantity text
        if (itemQuantityText != null)
        {
            itemQuantityText.text = itemQuantity.ToString();
        }
        else
        {
            Debug.LogWarning($"Text component for quantity is not assigned on {gameObject.name}.");
        }
    }
    public static bool IsTowelActive = false; // <-- Add this line
    public static bool IsClothesActive = false; // <-- Add this line
    /*     [Header("Towel Audio")]
        public AudioClip towelActivatedClip;
        public AudioClip towelDeactivatedClip; */

    [Header("Item Audio")]
    public AudioClip waterbottleClip;
    public AudioClip cannedGoodsClip;
    public AudioClip toiletriesClip;

    public AudioClip flashlightClipActivated;

    public AudioClip flashlightClipDeactivated;
    public AudioClip firstAidKitClip;
    public AudioClip phoneClip;
    public AudioClip powerbankClip;
    public AudioClip towelActivatedClip;
    public AudioClip towelDeactivatedClip;
    public AudioClip clothesActivatedClip;      // <-- Add this
    public AudioClip clothesDeactivatedClip;    // <-- Add this
    public AudioClip moneyClip;
    public AudioClip clothesClip;
    public AudioClip folderClip;
    private AudioSource audioSource;
    public void OnSlotClicked()
    {
        Debug.Log($"Clicked on {itemName} with quantity {itemQuantity}");

        // Use the item if pickupItemRef is assigned (for in-scene items)
        if (pickupItemRef != null)
        {
            pickupItemRef.Use();
            Debug.Log($"Used item: {itemName}");
            return;
        }

        // If pickupItemRef is null (after loading), use itemName to trigger logic
        switch (itemName.ToLower())
        {
            case "waterbottle":
                Debug.Log("Used waterbottle!");
                // Find the DrinkInteractionManager in the scene and trigger the drink logic
                DrinkInteractionManager drinkManager = GameObject.FindObjectOfType<DrinkInteractionManager>();
                if (drinkManager != null)
                {
                    drinkManager.TriggerDrinkFromInventory();
                }
                else
                {
                    Debug.LogWarning("DrinkInteractionManager not found in the scene!");
                }
                // Add your waterbottle logic here
                break;
            case "canned goods":
                Debug.Log("Used canned goods!");
                // Add your canned goods logic here
                break;
            case "toiletries":
                Debug.Log("Used toiletries!");
                // Add your toiletries logic here
                break;
            case "flashlight":
                Debug.Log("Used flashlight!");
                // Only allow in stage1normal
                if (SceneManager.GetActiveScene().name == "Stage2Easy")
                {
                    FlashlightController flashlightController = GameObject.FindObjectOfType<FlashlightController>();
                    if (flashlightController != null && flashlightController.flashlightLight != null)
                    {
                        bool isCurrentlyOn = flashlightController.flashlightLight.enabled;
                        flashlightController.ToggleFlashlight(!isCurrentlyOn); // Toggle state

                        // Play audio for flashlight activation/deactivation
                        if (audioSource != null)
                        {
                            if (!isCurrentlyOn && flashlightClipActivated != null)
                                audioSource.PlayOneShot(flashlightClipActivated);
                            else if (isCurrentlyOn && flashlightClipDeactivated != null)
                                audioSource.PlayOneShot(flashlightClipDeactivated);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("FlashlightController not found in the scene!");
                    }
                }
                else
                {
                    Debug.Log("Flashlight can only be used in stage1normal!");
                }
                break;
            case "first aid kit":
                Debug.Log("Used first aid kit!");
                // Add your first aid kit logic here
                break;
            case "phone":
                Debug.Log("Used phone!");
                /*           if (TelephoneInteractionManagerS3.CanUsePhoneFromInventory)
                          {
                              Debug.Log("Used phone from inventory!");
                              // Find the PhoneButtonManager in the scene and trigger the phone sequence
                              PhoneButtonManager phoneButtonManager = GameObject.FindObjectOfType<PhoneButtonManager>();
                              if (phoneButtonManager != null)
                              {
                                  phoneButtonManager.TriggerPhoneSequenceFromInventory();
                              }
                              else
                              {
                                  Debug.LogWarning("PhoneButtonManager not found in the scene!");
                              }
                          }
                          else
                          {
                              Debug.Log("You can't use the phone yet!");
                          } */
                if (TelephoneInteractionManagerS3.CanUsePhoneFromInventory)
                {
                    Debug.Log("Used phone from inventory!");
                    string sceneName = SceneManager.GetActiveScene().name;

                    if (sceneName == "Stage1Hard")
                    {
                        // Find the PhoneButtonManager in the scene and trigger the phone sequence
                        PhoneButtonManager phoneButtonManager = GameObject.FindObjectOfType<PhoneButtonManager>();
                        if (phoneButtonManager != null)
                        {
                            phoneButtonManager.TriggerPhoneSequenceFromInventory();
                        }
                        else
                        {
                            Debug.LogWarning("PhoneButtonManager not found in the scene!");
                        }
                    }
                    else if (sceneName == "Stage2Hard")
                    {
                        // Find the PhoneButtonManager1 in the scene and trigger the phone sequence
                        ClassroomS3.PhoneButtonManager1 phoneButtonManager1 = GameObject.FindObjectOfType<ClassroomS3.PhoneButtonManager1>();
                        if (phoneButtonManager1 != null)
                        {
                            phoneButtonManager1.TriggerPhoneSequenceFromInventory();
                        }
                        else
                        {
                            Debug.LogWarning("PhoneButtonManager1 not found in the scene!");
                        }
                    }
                    else
                    {
                        Debug.Log("Phone use is not available in this stage.");
                    }
                }
                else
                {
                    Debug.Log("You can't use the phone yet!");
                }
                break;
            case "powerbank":
                Debug.Log("Used powerbank!");
                // Add your powerbank logic here
                break;
            case "towel":
                IsTowelActive = !IsTowelActive; // Toggle towel state
                Debug.Log(IsTowelActive ? "Towel activated!" : "Towel deactivated!");
                Debug.Log("Used towel!");


                // Play audio
                if (audioSource != null)
                {
                    if (IsTowelActive && towelActivatedClip != null)
                        audioSource.PlayOneShot(towelActivatedClip);
                    else if (!IsTowelActive && towelDeactivatedClip != null)
                        audioSource.PlayOneShot(towelDeactivatedClip);
                }
                // Add your towel logic here
                break;
            case "money":
                Debug.Log("Used money!");
                // Add your money logic here
                break;
            case "clothes":
                IsClothesActive = !IsClothesActive; // Toggle clothes state
                Debug.Log(IsClothesActive ? "Clothes activated!" : "Clothes deactivated!");
                Debug.Log("Used clothes!");

                // Play clothes audio
                if (audioSource != null)
                {
                    if (IsClothesActive && clothesActivatedClip != null)
                        audioSource.PlayOneShot(clothesActivatedClip);
                    else if (!IsClothesActive && clothesDeactivatedClip != null)
                        audioSource.PlayOneShot(clothesDeactivatedClip);
                }
                break;
            case "folder":
                Debug.Log("Used folder!");
                // Add your folder logic here
                break;
            default:
                Debug.LogWarning("No use logic defined for this item.");
                break;
        }
    }
}