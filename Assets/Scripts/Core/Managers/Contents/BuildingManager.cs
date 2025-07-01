using UnityEngine;

public class BuildingManager : Singleton<BuildingManager>
{
    [SerializeField] private Preview preview;
    [SerializeField] private BaseBuilding curBuild;  
    
    public BaseBuilding CurBuild
    {
        get { return curBuild; }
        set
        {
            curBuild = value;

           SpriteRenderer spriteRenderer = preview.GetComponent<SpriteRenderer>();
           spriteRenderer.sprite = curBuild.GetComponent<SpriteRenderer>().sprite;
        }
    }

    public void ChoiceBuild(BaseBuilding build) 
    {
        CurBuild = build;
        BuildingSetting();
    }

    private void BuildingSetting()
    {
        preview.gameObject.SetActive(true);         

        BoxCollider2D previewColl = preview.GetComponent<BoxCollider2D>();
        BoxCollider2D curColl = curBuild.GetComponent<BoxCollider2D>();
        previewColl.size = curColl.size;
        previewColl.offset = curColl.offset;
    }
}
