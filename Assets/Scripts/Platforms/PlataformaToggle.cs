using UnityEngine;

public class PlataformaToggle : MonoBehaviour
{
    [SerializeField] private string grupo = "A"; 

    private Renderer platformRenderer;
    private Collider2D platformCollider; 
    private static bool grupoAVisible = true; 
    private bool isVisible; 

    void Start()
    {
        platformRenderer = GetComponent<Renderer>();
        platformCollider = GetComponent<Collider2D>();

        isVisible = grupo == "A" ? grupoAVisible : !grupoAVisible;

        UpdatePlatformState();
    }

    public static void AlternarGrupos()
    {
        grupoAVisible = !grupoAVisible;

        PlataformaToggle[] plataformas = FindObjectsOfType<PlataformaToggle>();
        foreach (var plataforma in plataformas)
        {
            plataforma.UpdateGroupState();
        }
    }

    private void UpdateGroupState()
    {

        isVisible = grupo == "A" ? grupoAVisible : !grupoAVisible;
        UpdatePlatformState();
    }

    private void UpdatePlatformState()
    {
        platformRenderer.enabled = isVisible;
        platformCollider.enabled = isVisible;
    }
}
