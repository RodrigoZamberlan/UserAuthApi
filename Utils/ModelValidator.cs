using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace UserAuthApi.Utils;

public static class ModelValidator {
    public static bool Validate<T>(T model, out string errorMessage) {
        if (model == null) {
            errorMessage = "Data model is required";
            return false;
        }
        
        var requiredProperties = typeof(T).GetProperties().Where(property => property.GetCustomAttributes<RequiredAttribute>() != null);

        foreach (var property in requiredProperties) {
            var value = property.GetValue(model);
            
            if (value == null || (value is string str && string.IsNullOrWhiteSpace(str))) {
                errorMessage = $"{property.Name} is required";
                return false;
            }
        }

        errorMessage = string.Empty;
        return true;
    }
}