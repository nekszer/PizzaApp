using Light.Maui;

namespace PizzaApp
{
    internal class AppServices
    {
        private ICrossContainer container;

        public AppServices(ICrossContainer container)
        {
            this.container = container;
        }
    }
}