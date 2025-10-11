using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Errors
{
    public enum ErrorTypes
    {
        UniqueError,
        UniqueErrorWithParams,
        NullError,
        NullErrorWithParams
    }
    public static class ErrorsDictionary
    {
        private static Dictionary<ErrorTypes, string> Errors = new Dictionary<ErrorTypes, string>() 
        {
            { ErrorTypes.UniqueError, "Нарушение условия уникальности в одном из полей записи с уже имеющейся записью" },
            { ErrorTypes.UniqueErrorWithParams, "Нарушение условия уникальности в поле '{0}' с уже имеющейся записью" },
            { ErrorTypes.NullError, "Поле не может быть null" },
            { ErrorTypes.NullErrorWithParams, "Поле '{0}', не может быть null"}
        };

        public static string GetErrorString(ErrorTypes ErrorType, object?[]? ValuesToInsert = null)
        {
            if (ValuesToInsert == null)
            {
                return Errors[ErrorType];
            }
            else
            {
                return string.Format(Errors[ErrorType], ValuesToInsert);
            }
        }
    }
}
