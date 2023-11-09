using View;

namespace WindowsFormsView
{
    public static class WFWindowFactory
    {
        public static Window GetWindow()
        {
            return new WindowsFormsWindow();
        }
    }
}
