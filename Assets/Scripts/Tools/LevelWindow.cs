using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelWindow : MonoBehaviour
{

    private Text levelText;
    private Transform nextLevelBox;
    private Transform descriptionBox;
    private Image experienceBarImage;
    private Image instantExperienceBarImage;
    private LevelSystem levelSystem;
    private LevelSystemAnimated levelSystemAnimated;

    private void Awake()
    {
        levelText = transform.Find("BarBox/LevelText").GetComponent<Text>();
        nextLevelBox = transform.Find("NewLevelBox");
        descriptionBox = transform.Find("BarBox/DescriptionBox");
        experienceBarImage = transform.Find("BarBox/ExperienceBar/Bar").GetComponent<Image>();
        instantExperienceBarImage = transform.Find("BarBox/ExperienceBar/InstantBar").GetComponent<Image>();
    }

    private void SetInstantExperienceBarSize(float experienceNormalized)
    {
        instantExperienceBarImage.fillAmount = experienceNormalized;
    }

    private void SetExperienceBarSize(float experienceNormalized)
    {
        experienceBarImage.fillAmount = experienceNormalized;
    }

    private void SetLevelNumber(int levelNumber)
    {
        levelText.text = "LEVEL\n" + (levelNumber + 1);
        
    }

    private void SetNewLevelText(int levelNumber)
    {
        Color lvlColor; 
        ColorUtility.TryParseHtmlString("#FDC900", out lvlColor);
        InformationPopup.Create(nextLevelBox, "NEW LEVEL\n" + (levelNumber + 1), lvlColor, 80, 0.2f);
    }

    private void SetDescriptionText(int experienceAmount, string reason)
    {
        InformationPopup.Create(descriptionBox, "+" + experienceAmount + " EXP " + reason, Color.white, 40, 0.6f);
    }

    public void SetLevelSystem(LevelSystem levelSystem)
    {
        this.levelSystem = levelSystem;

        SetInstantExperienceBarSize(levelSystem.GetExperienceNormalized());

        levelSystem.OnInstantExperienceChanged += LevelSystem_OnInstantExperienceChanged;
    }

    public void SetLevelSystemAnimated(LevelSystemAnimated levelSystemAnimated)
    {
        // Set the LevelSystemAnimated object
        this.levelSystemAnimated = levelSystemAnimated;

        // Update the starting values
        SetLevelNumber(levelSystemAnimated.GetLevelNumber());
        SetExperienceBarSize(levelSystemAnimated.GetExperienceNormalized());

        // Surbscribe to the changed events
        levelSystemAnimated.OnExperienceChanged += LevelSystemAnimated_OnExperienceChanged;
        levelSystemAnimated.OnLevelChanged += LevelSystemAnimated_OnLevelChanged;
    }

    private void LevelSystemAnimated_OnLevelChanged(object sender, System.EventArgs e)
    {
        // Level changed, update text
        SetLevelNumber(levelSystemAnimated.GetLevelNumber());
        SetNewLevelText(levelSystemAnimated.GetLevelNumber());

        // Update Instant Experience
        if (levelSystem.GetLevelNumber() <= levelSystemAnimated.GetLevelNumber())
            SetInstantExperienceBarSize(levelSystem.GetExperienceNormalized());
    }

    private void LevelSystemAnimated_OnExperienceChanged(object sender, System.EventArgs e)
    {
        // Experience changed, update bar size
        SetExperienceBarSize(levelSystemAnimated.GetExperienceNormalized());
    }

    private void LevelSystem_OnInstantExperienceChanged(object sender, System.EventArgs e)
    {
        // Instant Experience changed, update bar size
        if(levelSystem.GetLevelNumber() <= levelSystemAnimated.GetLevelNumber())
            SetInstantExperienceBarSize(levelSystem.GetExperienceNormalized());

        SetDescriptionText(levelSystem.GetExperienceAmount(), levelSystem.GetReasonText());
    }
}
