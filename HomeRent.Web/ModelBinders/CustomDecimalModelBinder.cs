using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;
using System.Threading.Tasks;

public class CustomDecimalModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        if (valueProviderResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);
        var value = valueProviderResult.FirstValue;

        if (string.IsNullOrWhiteSpace(value))
        {
            return Task.CompletedTask;
        }

        // Replace a period with a comma (since Bulgarian culture uses comma)
        if (value.Contains('.'))
        {
            value = value.Replace('.', ',');
        }

        if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal result))
        {
            bindingContext.Result = ModelBindingResult.Success(result);
        }
        else
        {
            bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Невалиден формат за десетично число.");
        }

        return Task.CompletedTask;
    }
}