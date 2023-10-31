using System.Drawing;

namespace csheroes.src.UI
{
    public static class ButtonFactory
    {
        public static Button InstantiateButton(ButtonTemplate template)
        {
            return template switch
            {
                ButtonTemplate.MainMenuButton => new()
                {
                    Size = new Size(250, 100)
                },
                ButtonTemplate.CampMenuButton => new()
                {
                    Size = new Size(25, 25)
                },
                _ => throw new ButtonTemplateNotFoundException($"Button template for {template} not found")
            };
        }
    }
}
