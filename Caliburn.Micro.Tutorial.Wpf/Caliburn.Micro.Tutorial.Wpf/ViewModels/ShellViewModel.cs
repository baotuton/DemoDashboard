using System.Threading;
using System.Threading.Tasks;

namespace Caliburn.Micro.Tutorial.Wpf.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        public Task EditCategories()
        {
            var viewmodel = IoC.Get<CategoryViewModel>();
            return ActivateItemAsync(viewmodel, new CancellationToken());
        }

        protected async override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await EditCategories();
        }
    }
}
