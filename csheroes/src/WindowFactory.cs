using View;
using WindowsFormsView;

namespace csheroes.src
{
    public static class WindowFactory
    {
        public static Window GetWindow()
        {
            return WFWindowFactory.GetWindow();
        }
    }
}
