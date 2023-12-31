﻿namespace ASP111.Services.Validation
{
    public interface IValidationService
    {
        bool IsValid(object model);
        Dictionary<String, String> ErrorMessages(object model);
    }
}

/* Array, List ==> [ 0 => "val1", 1 => "val2"]
 * Dictionary<String key, String value> ==> [ "key1" => "val1", "key2" => "val2"]  dict["key1"] == "val1"
 */
