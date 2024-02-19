using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HighlightAndMoveRobot : MonoBehaviour
{
    public Material highlightMaterial;
    public GameObject leftArmRoot;
    private enum Selection{Torso, LeftArm, None };
    private Selection selection;
    private Renderer[] renderers;
    private Transform highlighted;
    private readonly float zMovementOffset = .45f;
    private bool moving = false;
    private Vector3 leftArmStartPos;
    private Vector3 leftArmDetachedPos;
    private readonly float armOffset = .025f;
    private readonly float lerpMovementSpeed = 50f;
    public Text text;
    private bool attached = true;

    void Awake()
    {
        selection = Selection.None;
        leftArmStartPos = leftArmRoot.transform.localPosition;
        ChangeText();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && !moving)
        {
            Transform mouseSelection = hit.transform;

            if (mouseSelection.CompareTag("Torso") && selection != Selection.Torso)
            {
                Highlight(Selection.Torso, mouseSelection);
            }
            else if (mouseSelection.CompareTag("Left Arm") && selection != Selection.LeftArm)
            {
                Highlight(Selection.LeftArm, leftArmRoot.transform);
            }
            else if (!mouseSelection.CompareTag("Left Arm") && !mouseSelection.CompareTag("Torso") && selection != Selection.None)
            {
                DisableHighlight();
                selection = Selection.None;
            }   
        }
        else if(selection != Selection.None && !moving)
        {
            DisableHighlight();
            selection = Selection.None;
        }

        //Change text when detached and store detached arm position
        if (Input.GetMouseButtonDown(0))
        {
            if (highlighted != null)
            {
                if (highlighted == leftArmRoot.transform)
                {
                    attached = false;
                    ChangeText();
                }
                else if (selection == Selection.Torso && attached == false)
                {
                    leftArmDetachedPos = leftArmRoot.transform.position;
                }
            }
        }
        // Move Robot when holding left mouse button
        if (Input.GetMouseButton(0))
        {
            if(highlighted != null)
            {
                moving = true;
                Vector3 position = Input.mousePosition;
                position.z = zMovementOffset;
                Vector3 endPosition = Camera.main.ScreenToWorldPoint(position);
                highlighted.position = Vector3.Lerp(highlighted.position, endPosition, Time.deltaTime * lerpMovementSpeed);

                if (selection == Selection.Torso && attached == false)
                {
                    leftArmRoot.transform.position = leftArmDetachedPos;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            moving = false;
            // Check that the arm is close enough to starting position to snap back into place
            if(selection == Selection.LeftArm)
            {
                if ((leftArmRoot.transform.localPosition.x > leftArmStartPos.x - armOffset) && leftArmRoot.transform.localPosition.x < leftArmStartPos.x + armOffset)
                {
                    if (leftArmRoot.transform.localPosition.y < leftArmStartPos.y + armOffset && leftArmRoot.transform.localPosition.y > leftArmStartPos.y - armOffset)
                    {
                        leftArmRoot.transform.localPosition = leftArmStartPos;
                        attached = true;
                        ChangeText();
                    }
                }
            }     
        }
    }
    private void Highlight(Selection enumSelection, Transform transformSelection)
    {
        DisableHighlight();
        selection = enumSelection;
        renderers = transformSelection.GetComponentsInChildren<Renderer>();
        highlighted = transformSelection;
        EnableHighlight();
    }
    private void EnableHighlight()
    {
        foreach (Renderer renderer in renderers)
        {
            //Add highlight material
            List<Material> materials = renderer.sharedMaterials.ToList();
            materials.Add(highlightMaterial);
            renderer.materials = materials.ToArray();
        }
    }
    private void DisableHighlight()
    {
        if (highlighted != null)
        {
            foreach (Renderer renderer in renderers)
            {
                //Remove highlight material  
                List<Material> materials = renderer.sharedMaterials.ToList();
                materials.Remove(highlightMaterial);
                renderer.materials = materials.ToArray();
            }
            highlighted = null;
        }
    }
    private void ChangeText()
    {
        if (text != null)
        {
            if (attached)
            {
                text.text = "Attached";
            }
            else if (!attached)
            {
                text.text = "Detached";
            }
        } 
    }
}
