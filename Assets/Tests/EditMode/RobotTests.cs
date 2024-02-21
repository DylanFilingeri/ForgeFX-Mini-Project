using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class RobotTests
{
    [Test]
    public void Check_Detached_Text_Is_Correct()
    {
        // Arrange
        var highlightAndMoveRobot = new GameObject().AddComponent<HighlightAndMoveRobot>();
        highlightAndMoveRobot.text = new GameObject().AddComponent<Text>();
        highlightAndMoveRobot.text.text = "";
        highlightAndMoveRobot.SetAttached(false);

        // Act
        highlightAndMoveRobot.ChangeText();

        // Assert
        Assert.AreEqual("Detached", highlightAndMoveRobot.text.text);
    }

    [Test]
    public void Check_Attached_Text_Is_Correct()
    {
        // Arrange
        var highlightAndMoveRobot = new GameObject().AddComponent<HighlightAndMoveRobot>();
        highlightAndMoveRobot.text = new GameObject().AddComponent<Text>();
        highlightAndMoveRobot.text.text = "";
        highlightAndMoveRobot.SetAttached(true);

        // Act
        highlightAndMoveRobot.ChangeText();

        // Assert
        Assert.AreEqual("Attached", highlightAndMoveRobot.text.text);
    }
}
