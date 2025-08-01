using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCarsDriver.Helpers;
using ZhooCars.Common;

namespace ZCarsDriver.Converters
{
    public class ApprovalStatusToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "Not Submitted";
            if (value is ApprovalStatus status)
            {
                var result = AppHelper.ApprovalStatusStyle.GetStyle(status);

                return result.LabelText;
            }

            return "unknown";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
