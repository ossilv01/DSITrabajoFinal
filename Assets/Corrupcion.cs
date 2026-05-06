using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Corrupcion : VisualElement
{

    const int MAX = 4;

    int valor = 3;
    string iconFull = "barred";
    string iconEmpty = "bargrey";

    VisualElement container = new VisualElement();

    public int Valor
    {
        get => valor;
        set
        {
            valor = Mathf.Clamp(value, 0, MAX);
            Actualizar();
        }
    }

    public string IconFull
    {
        get => iconFull;
        set
        {
            iconFull = value;
            Actualizar();
        }
    }

    public string IconEmpty
    {
        get => iconEmpty;
        set
        {
            iconEmpty = value;
            Actualizar();
        }
    }

    public new class UxmlFactory : UxmlFactory<Corrupcion, UxmlTraits> { }

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlIntAttributeDescription valorAttr =
            new UxmlIntAttributeDescription { name = "valor", defaultValue = 3 };

        UxmlStringAttributeDescription fullAttr =
            new UxmlStringAttributeDescription { name = "full", defaultValue = "barred" };

        UxmlStringAttributeDescription emptyAttr =
            new UxmlStringAttributeDescription { name = "empty", defaultValue = "bargrey" };

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            var hc = ve as Corrupcion;

            hc.Valor = valorAttr.GetValueFromBag(bag, cc);
            hc.IconFull = fullAttr.GetValueFromBag(bag, cc);
            hc.IconEmpty = emptyAttr.GetValueFromBag(bag, cc);
        }
    }

    public Corrupcion()
    {
        container.style.flexDirection = FlexDirection.Row;

         var minusBtn = new Button(() => { Valor--; });
        minusBtn.text = "";
        hierarchy.Add(minusBtn);

        minusBtn.style.backgroundImage =
    new StyleBackground(Resources.Load<Texture2D>("minuscorr"));
    minusBtn.style.backgroundColor = new Color(0,0,0,0);
        // crear los 5 iconos UNA VEZ
        for (int i = 0; i < MAX; i++)
        {
            var icon = new VisualElement();
            icon.style.width = 32;
            icon.style.height = 32;
            container.Add(icon);
        }

        hierarchy.Add(container);
        // Botón +
        var plusBtn = new Button(() => { Valor++; });
        plusBtn.text = "";
        plusBtn.style.backgroundImage =
    new StyleBackground(Resources.Load<Texture2D>("pluscorr"));
    plusBtn.style.backgroundColor = new Color(0,0,0,0);

        hierarchy.Add(plusBtn);
        Actualizar();
    }

    void Actualizar()
    {
        var fullTex = Resources.Load<Texture2D>(iconFull);
        var emptyTex = Resources.Load<Texture2D>(iconEmpty);

        List<VisualElement> icons = container.Children().ToList();

        for (int i = 0; i < MAX; i++)
        {
            icons[i].style.backgroundImage =
                new StyleBackground(i < valor ? fullTex : emptyTex);
        }
    }
}
