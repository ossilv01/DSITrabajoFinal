using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements; 
public class Final : MonoBehaviour
{
   
    VisualElement contenido_estado;
    VisualElement contenido_inventario;
    VisualElement contenido_notas;
    
    VisualElement contenido_decisiones;


    VisualElement pestanya_estado;
    VisualElement pestanya_inventario;
    VisualElement pestanya_notas;

    VisualElement pestanya_decisiones;

    List<VisualElement> leftItems = new List<VisualElement>();
    List<VisualElement> leftLocks = new List<VisualElement>();
    List<VisualElement> rightItems = new List<VisualElement>();

    List<Texture2D> itemTextures = new List<Texture2D>();
    Texture2D lockOpen;
    Texture2D lockClosed;

    TextField notaField;
    string notaGuardada = "";
    public AudioSource audioSource;
    AudioClip vozKara;
    AudioClip vozC;
    AudioClip vozM;



    private void NoContenido()
    {
        contenido_estado.style.display = DisplayStyle.None;
        contenido_inventario.style.display = DisplayStyle.None;
        contenido_notas.style.display = DisplayStyle.None; 
        contenido_decisiones.style.display = DisplayStyle.None;
    }

    private void OnEnable()
    {
        //Logica pestañas
        UIDocument uidoc = GetComponent<UIDocument>();
        VisualElement rootve = uidoc.rootVisualElement;

        VisualElement contenido = rootve.Q("Contenido");
        VisualElement pestanyas = rootve.Q("Pestanyas");

         Label texto1 = rootve.Q<Label>("Obe");
         texto1.text = @"<gradient=""blue"">OBEDIENCIA";

         Label texto2 = rootve.Q<Label>("Cor");
         texto2.text =  @"<gradient=""red"">CORRUPCION";

         Label texto3 = rootve.Q<Label>("Obe1");
         texto3.text = @"<gradient=""blue"">OBEDIENCIA";

         Label Texto = rootve.Q<Label>("Cor1");
         Texto.text =  @"<gradient=""red"">CORRUPCION";

         Label Textov = rootve.Q<Label>("Obe2");
         Textov.text = @"<gradient=""blue"">OBEDIENCIA";

         Label Textoa = rootve.Q<Label>("Cor2");
         Textoa.text =  @"<gradient=""red"">CORRUPCION";

        
        pestanya_estado = pestanyas.Q("Estado");
        pestanya_inventario = pestanyas.Q("Inventario");
        pestanya_notas = pestanyas.Q("Notas");
        pestanya_decisiones = pestanyas.Q("Decisiones");

        contenido_estado = contenido.Q("Estado");
        contenido_inventario = contenido.Q("Inventario");
        contenido_notas = contenido.Q("Notas");
        contenido_decisiones = contenido.Q("Decisiones");

        pestanya_estado.RegisterCallback<MouseDownEvent>(evt =>
        {
            Debug.Log("Pestaña estado");
            NoContenido();
            contenido_estado.style.display = DisplayStyle.Flex;
        });

        pestanya_inventario.RegisterCallback<MouseDownEvent>(evt =>
        {
            Debug.Log("Pestaña inventario");
            NoContenido();
            contenido_inventario.style.display = DisplayStyle.Flex;
        });

        pestanya_notas.RegisterCallback<MouseDownEvent>(evt =>
        {
            Debug.Log("Pestaña notas");
            NoContenido();
            contenido_notas.style.display = DisplayStyle.Flex;
        });

        pestanya_decisiones.RegisterCallback<MouseDownEvent>(evt =>
        {
            Debug.Log("Pestaña decisiones");
            NoContenido();
            contenido_decisiones.style.display = DisplayStyle.Flex;
        });

        //logica inventario
        lockOpen = Resources.Load<Texture2D>("unlock");
        lockClosed = Resources.Load<Texture2D>("lock");
        var inventario = contenido.Q<VisualElement>("Inventario");
        var dcha = inventario.Q<VisualElement>("Dcha");
        var llevar = dcha.Q<VisualElement>("Llevar");
        // LEFT (objetos disponibles)
        for (int i = 1; i <= 5; i++)
        {
        var slot = rootve.Q<VisualElement>($"Slot{i}");
        var lockVe = slot.Q<VisualElement>("lock");
        var itemVe = slot.Q<VisualElement>("item");

        Texture2D tex = Resources.Load<Texture2D>($"item{i}");

        if (tex == null)
        Debug.LogError($"No se encontró item{i} en Resources");

        leftLocks.Add(lockVe);
        leftItems.Add(itemVe);
        itemTextures.Add(tex);

        int index = i - 1;

        lockVe.RegisterCallback<ClickEvent>(e => OnClickLeft(index));
        }

        // RIGHT (slots misión)
         for (int i = 1; i <= 3; i++)
        {
        var take = dcha.Q<VisualElement>($"Take{i}");
        var item = take.Q<VisualElement>("item");

        if (take == null) Debug.LogError($"Take{i} NULL");
        if (item == null) Debug.LogError($"Item de Take{i} NULL");

        item.style.backgroundImage = null;
        item.userData = null;

        rightItems.Add(item);
        }

        //NOTAS
        notaField = rootve.Q<TextField>();
        //Guardar
        var guardar = rootve.Q<VisualElement>("Guardar");
        guardar.RegisterCallback<ClickEvent>(evt =>
        {
            notaGuardada = notaField.value;
            Debug.Log("Nota guardada: " + notaGuardada);
        });

        //Restaurar
        var restaurar = rootve.Q<VisualElement>("Restart");
        restaurar.RegisterCallback<ClickEvent>(evt =>
        {
            notaField.value = notaGuardada;
            Debug.Log("Nota restaurada");
        });

        //Voces
       audioSource = GetComponentInChildren<AudioSource>();

        vozKara = Resources.Load<AudioClip>("vozKara");
        vozC = Resources.Load<AudioClip>("vozC");
        vozM = Resources.Load<AudioClip>("vozM");

        var eye1 = rootve.Q<VisualElement>("Eye");
        var eye2 = rootve.Q<VisualElement>("Eye2");
        var eye3 = rootve.Q<VisualElement>("Eye3");

        eye1.RegisterCallback<ClickEvent>(evt =>
        {
            audioSource.PlayOneShot(vozC);
        });

        eye2.RegisterCallback<ClickEvent>(evt =>
        {
            audioSource.PlayOneShot(vozKara);
        });

        eye3.RegisterCallback<ClickEvent>(evt =>
        {
            audioSource.PlayOneShot(vozM);
        });
    }

    void OnClickLeft(int index)
    {
    var lockVe = leftLocks[index];
    var texture = itemTextures[index];

    bool isLocked = lockVe.userData as bool? ?? false;

    if (!isLocked)
    {
        lockVe.style.backgroundImage = new StyleBackground(lockClosed);
        lockVe.userData = true;

        AddToRight(texture);
    }
    else
    {
        lockVe.style.backgroundImage = new StyleBackground(lockOpen);
        lockVe.userData = false;

        RemoveFromRight(texture);
    }
    }
    void AddToRight(Texture2D texture)
    {
            foreach (var slot in rightItems)
        {
            if (slot.userData == null)
            {
                slot.style.backgroundImage = new StyleBackground(texture);
                slot.userData = texture;
                return;
            }
        }

        Debug.Log("No hay hueco libre");
    }

    void RemoveFromRight(Texture2D texture)
    {
    foreach (var slot in rightItems)
    {
        if (slot.userData == texture)
        {
            slot.style.backgroundImage = null;
            slot.userData = null;
            return;
        }
    }
    }
}
