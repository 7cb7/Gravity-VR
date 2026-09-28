using UnityEngine;
using UnityEngine.UI;
using TMPro;

// This shall mark said quiz when CHECK ANSWERS is pressed.
// Correct answers go green whilst wrong picks go red and the score shows at the top.
public class QuizManager : MonoBehaviour
{
    // one of these per question filled in through the Inspector
    [System.Serializable]

    public class QuizQuestion
    {
        public ToggleGroup toggleGroup;  // this makes sure only one answer can be ticked
        public Toggle[] optionToggles = new Toggle[4];
        public Text[] optionLabels = new Text[4];

        [Tooltip("Which option is correct: 0 = first (A), 1 = second (B), 2 = third (C), 3 = fourth (D).")]
        public int correctOptionIndex;
    }

    [Header("Questions")]
    public QuizQuestion[] questions;

    [Header("Result")]
    public TMP_Text resultText;

    [Header("Scrolling")]
    public ScrollRect scrollRect;

    [Header("Colors")]
    public Color correctColor = Color.green;
    public Color incorrectColor = Color.red;
    public Color defaultColor = Color.white;

    // hooked up to the CHECK ANSWERS button
    public void CheckAnswers()
    {
        int score = 0;

        foreach (QuizQuestion q in questions)
        {
            // -1 means they didn't pick anything for this one
            int selectedIndex = -1;

            for (int i = 0; i < q.optionToggles.Length; i++)


            {
                if (q.optionToggles[i] != null && q.optionToggles[i].isOn)
                    selectedIndex = i;


                // wipe any colours from last time they pressed check
                if (q.optionLabels[i] != null)
                    q.optionLabels[i].color = defaultColor;
            }

            // always show the right answer in green, whatever they picked
            if (q.optionLabels[q.correctOptionIndex] != null)
                q.optionLabels[q.correctOptionIndex].color = correctColor;

            if (selectedIndex == q.correctOptionIndex)
            {
                score++;
            }
            else if (selectedIndex != -1 && q.optionLabels[selectedIndex] != null)
            {
                // they picked the wrong one, so that one goes red
                q.optionLabels[selectedIndex].color = incorrectColor;
            }

        }

        if (resultText != null)
            resultText.text = "Your Score: " + score + " / " + questions.Length;

        // kjump back up to the top so they actually see their score
        if (scrollRect != null)
            scrollRect.verticalNormalizedPosition = 1f;
    }
}