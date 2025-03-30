using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HomeRent.Web.ModelBinders
{
    public class CustomDecimalModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(decimal) ||
                context.Metadata.ModelType == typeof(decimal?))
            {
                return new CustomDecimalModelBinder();
            }

            return null;
        }
    }
}
