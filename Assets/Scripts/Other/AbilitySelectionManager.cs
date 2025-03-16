using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySelection : MonoBehaviour
{
    public List<Button> abilityButtons; 

    private static List<string> selectedAbilities = new List<string>(); 
    private const int maxAbilities = 2; 

    void Start()
    {
        selectedAbilities.Clear();

        for (int i = 0; i < abilityButtons.Count; i++)
        {
            int index = i;
            abilityButtons[i].onClick.AddListener(() => ToggleAbility(index));
        }
    }

    void ToggleAbility(int index)
    {
        string abilityName = "";

        switch (index)
        {
            case 0: abilityName = "ExtraHealth"; break;
            case 1: abilityName = "LongerParry"; break;
            case 2: abilityName = "SpeedBoost"; break;
            case 3: abilityName = "Shoot"; break;
        }

        if (selectedAbilities.Contains(abilityName))
        {
            selectedAbilities.Remove(abilityName);
            abilityButtons[index].image.color = Color.white; 
        }
        else
        {
            if (selectedAbilities.Count < maxAbilities)
            {
                selectedAbilities.Add(abilityName);
                abilityButtons[index].image.color = Color.gray; 
            }
        }
    }

    public static List<string> GetSelectedAbilities()
    {
        return new List<string>(selectedAbilities);
    }
}
